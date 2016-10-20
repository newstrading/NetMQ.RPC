using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;

using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;

namespace Burrow.RPC
{

    public class BurrowRpcServerCoordinator<T> : IRpcServerCoordinator where T : class
    {
        private readonly T _realInstance;
        ResponseSocket server;
        
        public BurrowRpcServerCoordinator(T realInstance, string connectionStringCommands)
        {
            if (realInstance == null)
            {
                throw new ArgumentNullException("realInstance");
            }
            _realInstance = realInstance;
            server = new ResponseSocket(connectionStringCommands);
      
        }

        private void Init()
        {
        }

        Thread t;
        public void Start()
        {
            Init();
            //_tunnel.SubscribeAsync<RpcRequest>(_serverId ?? typeof(T).Name, HandleMesage);
            t = new Thread (Run);
            t.Start();
        }
        
        public void Stop () {
        	running = false;
        	try {
        		server.Dispose();
        		t.Abort ();
        	} catch (Exception ex) {
        		;
        	}
        }
        

        bool running = true;
        void Run () {
        	while (running) {
					// Receive the message from the server socket
					string jsonRequest = server.ReceiveFrameString ();
					Console.WriteLine ("Server Rcvd: {0}", jsonRequest);

					//object JsonDe = JsonConvert.DeserializeObject(Json); 
					//Console.WriteLine ("Deserialized Type: {0}", JsonDe.GetType ());

					JsonSerializer serializer = new JsonSerializer();
					serializer.CheckAdditionalContent= false;

					JsonTextReader reader = new JsonTextReader(new StringReader(jsonRequest));
					reader.SupportMultipleContent = true;
					reader.CloseInput = false;
					reader.Read ();
					//serializer.Deserialize (
					RpcRequest rpcRequest = serializer.Deserialize<RpcRequest> (reader);
					
					HandleMessage (rpcRequest);
				}
        }
        
        void SendResponse (RpcResponse rpcResponse) {        	
        	string jsonResponse = JsonConvert.SerializeObject(rpcResponse);
            Console.WriteLine("Server Sending Response : {0}", jsonResponse);
			server.SendFrame(jsonResponse);
        }
        
        
        public void HandleMessage(RpcRequest msg)
        {
            if (msg.UtcExpiryTime != null && msg.UtcExpiryTime < DateTime.UtcNow)
            {
              //  Global.DefaultWatcher.WarnFormat("Msg {0}.{1} from {2} has been expired", msg.DeclaringType, msg.MethodName, msg.ResponseAddress);
                return;
            }

            var response = new RpcResponse
            {
                RequestId = msg.Id,
            };
            try
            {
                var methodInfo = InternalDependencies.MethodMatcher.Match<T>(msg);
                if (methodInfo == null)
                {
                    throw new Exception(string.Format("Could not find a match member of type {0} for method {1} of {2}", msg.MemberType.ToString(), msg.MethodName, msg.DeclaringType));
                }
                
                var parameters = methodInfo.GetParameters();
                
                //NOTE: Fix param type due to int32/int64 serialization problem
                foreach (var param in parameters)
                {
                    if (param.ParameterType.IsPrimitive)
                    {
                        msg.Params[param.Name] = msg.Params[param.Name].ConvertToCorrectTypeValue(param.ParameterType);
                    }
                }

                object[] parameterValues = msg.Params.Values.ToArray();
                response.ReturnValue = methodInfo.Invoke(_realInstance, parameterValues);
                var keys = msg.Params.Keys.ToArray();

                for (int i = 0; i < msg.Params.Count; i++)
                {
                    msg.Params[keys[i]] = parameterValues[i];
                }
                response.ChangedParams = msg.Params;

            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

          	SendResponse (response);
        }
    }
}