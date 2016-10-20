using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Burrow.RPC
{

	public static class JSON
	{
		public static RpcResponse DeserializeResponse ( RpcRequest request, string json ) {
			RpcResponse response =  JsonConvert.DeserializeObject<RpcResponse>(json);
			
			if (response.ReturnValue.GetType() == typeof (Newtonsoft.Json.Linq.JArray)) {
	
				string typename = request.DeclaringType +"," + request.DeclaringAssembly ; // = "test.ICalculator,test";
				Type dataType = Type.GetType (typename); // request.DeclaringType);
				MethodInfo mi = dataType.GetMethod (request.MethodName);
				
				Type returnType = mi.ReturnType;
				
				Newtonsoft.Json.Linq.JArray jarray = (Newtonsoft.Json.Linq.JArray) response.ReturnValue;
				string dataJson = jarray.ToString ();
				
				var data =   JsonConvert.DeserializeObject (dataJson, returnType);
				response.ReturnValue = data;
				int j = 34;
				//MethodInfo mi = typeof(JSON).GetMethods ();
			    //response.ReturnValue = response.ReturnValue.ToObject<List<SelectableEnumItem>>()
			   }
			
			return response;
		}
			
		
	}
}
