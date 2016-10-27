using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Burrow.RPC
{

	public static class JSON
	{
		
		public static RpcRequest DeSerializeRequest( string json) {
			RpcRequest request =  JsonConvert.DeserializeObject<RpcRequest>(json);
			string typename = request.DeclaringType +"," + request.DeclaringAssembly ; 
			Type dataType = Type.GetType (typename);
			MethodInfo mi = dataType.GetMethod (request.MethodName);
			
			ParameterInfo[] prms = mi.GetParameters();
			foreach (var p in prms) {
				if (request.Params.ContainsKey (p.Name)) {
					Type paramType = p.ParameterType;
					  request.Params[p.Name] = Sanitize (paramType,  request.Params[p.Name]);
				}
			}
			return request;
		}
		
		
		public static RpcResponse DeserializeResponse ( RpcRequest request, string json ) {
			RpcResponse response =  JsonConvert.DeserializeObject<RpcResponse>(json);
			
			string typename = request.DeclaringType +"," + request.DeclaringAssembly ; // = "test.ICalculator,test";
			Type dataType = Type.GetType (typename); // request.DeclaringType);
			MethodInfo mi = dataType.GetMethod (request.MethodName);
			Type returnType = mi.ReturnType;
			
			response.ReturnValue = Sanitize (returnType, response.ReturnValue);
			
			/*if (response.ReturnValue.GetType() == typeof (Newtonsoft.Json.Linq.JArray)) {
				Newtonsoft.Json.Linq.JArray jarray = (Newtonsoft.Json.Linq.JArray) response.ReturnValue;
				string dataJson = jarray.ToString ();
				
				var data =   JsonConvert.DeserializeObject (dataJson, returnType);
				response.ReturnValue = data;
				int j = 34;
		   }*/
			
			
			
			return response;
		}
		
		static object Sanitize (Type type, object o) {
			if (o==null) return null;
			string json = JsonConvert.SerializeObject (o);
			object data =   JsonConvert.DeserializeObject (json, type);
			int j = 55;
			return data;
		}
			
		
	}
}
