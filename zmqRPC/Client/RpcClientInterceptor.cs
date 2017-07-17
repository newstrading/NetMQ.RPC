using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;

namespace Burrow.RPC
{
    internal class RpcClientInterceptor : IInterceptor
    {
        //private readonly IRpcClientCoordinator _clientCoordinator;
        private readonly List<IMethodFilter> _methodFilters;

        RequestSocket client;
		logDelegate log;
        
		public RpcClientInterceptor( string connectionStringCommands,  params IMethodFilter[] methodFilters)
			: this (connectionStringCommands, null, methodFilters)
        {
		}
		
        public RpcClientInterceptor( string connectionStringCommands, logDelegate log, params IMethodFilter[] methodFilters)
        {
			this.log = log;
        	client = new RequestSocket (connectionStringCommands);  // connect	
			

        	// _clientCoordinator = clientCoordinator;
            _methodFilters = new List<IMethodFilter>((methodFilters ?? new IMethodFilter[0]).Union(new [] {new DefaultMethodFilter()}));
            _methodFilters.RemoveAll(filter => filter == null);
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.Method;

            var attributes = method.GetCustomAttributes(true).Select(x => x as Attribute).ToArray();
            var isAsync = _methodFilters.Any(filter => filter.IsAsync(method, attributes));
            _methodFilters.ForEach(filter => filter.CheckValid(method, attributes, isAsync));

            var @params = method.GetParameters().ToList();

            var args = new Dictionary<string, object>();
            for(int i=0 ; i < invocation.Arguments.Length; i++)
            {
                args.Add(@params[i].Name, invocation.Arguments[i]);
            }

            var request = new RpcRequest
            {
                Params = args,
                MemberType = method.MemberType,
                MethodName = method.Name,
                MethodSignature = InternalDependencies.MethodMatcher.GetMethodSignature(method),
                DeclaringType = method.DeclaringType.FullName,
                DeclaringAssembly = method.DeclaringType.Assembly.FullName.Substring (0,method.DeclaringType.Assembly.FullName.IndexOf (","))
            };

            
            
            var timeToLiveAttribute = attributes.LastOrDefault(x => x is RpcTimeToLiveAttribute);
            if (timeToLiveAttribute != null)
            {
                var att = (RpcTimeToLiveAttribute) timeToLiveAttribute;
                if (att.Seconds > 0)
                {
                    request.UtcExpiryTime = DateTime.UtcNow.AddSeconds(att.Seconds);
                }
            }

            string jsonRequest = JsonConvert.SerializeObject(request);
            //Console.WriteLine("Client Sending Request: {0}", jsonRequest);
			if (log != null) log(Direction.Sent, jsonRequest);
			client.SendFrame(jsonRequest);

			string jsonResponse = client.ReceiveFrameString();
			if (log != null) log(Direction.Received, jsonResponse);
			//Console.WriteLine("Client Response Received: {0}", jsonResponse);
			
			
			RpcResponse response =  JSON.DeserializeResponse (request, jsonResponse); //     JsonConvert.DeserializeObject<RpcResponse>(jsonResponse);

            
            MapResponseResult(invocation, @params, response);
        }

        private static void MapResponseResult(IInvocation invocation, List<ParameterInfo> @params, RpcResponse response)
        {
            if (response.Exception != null)
            {
                throw response.Exception;
            }

            if (invocation.Method.ReturnType != typeof(void) && response.ReturnValue != null)
            {
                invocation.ReturnValue = response.ReturnValue.ConvertToCorrectTypeValue(invocation.Method.ReturnType);
            }

            var outParams = @params.Where(x => x.IsOut).Select(x => x.Name);
            var missingOutValue = outParams.FirstOrDefault(param => !(response.ChangedParams ?? new Dictionary<string, object>()).ContainsKey(param));
            if (missingOutValue != null)
            {
                throw new Exception(string.Format("RpcResponse does not contain the modified value for param {0} which is an 'out' param. Probably there's something wrong with the bloody Coordinator", missingOutValue));
            }

            for (var i = 0; i < @params.Count; i++)
            {
                if (@params[i].IsOut)
                {
                    invocation.SetArgumentValue(i, response.ChangedParams[@params[i].Name].ConvertToCorrectTypeValue(@params[i].ParameterType.GetElementType()));
                }
                else if (@params[i].ParameterType.IsByRef && response.ChangedParams.ContainsKey(@params[i].Name))
                {
                    invocation.SetArgumentValue(i, response.ChangedParams[@params[i].Name].ConvertToCorrectTypeValue(@params[i].ParameterType.GetElementType()));
                }
            }
        }
    }
}