﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;

using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;


namespace zmqRPC.PubSub
{
	
	public class Subscriber
	{
		SubscriberSocket sub;
		bool  running = false;
		Thread subThread;
		
		public Subscriber(string connectionStringPublisher)
		{
			sub = new SubscriberSocket ( connectionStringPublisher);
			sub.SubscribeToAnyTopic();
		
			
			subThread = new Thread (SubWorker);
			subThread.Start();
		}
		
		
		void SubWorker () {
			running = true;		
			while (running) {

				List<string> data = sub.ReceiveMultipartStrings ();
				if (data.Count==3) {
					
					string subscriptionName = data[0];
					string sType = data[1];
					string json = data[2];				
					Console.WriteLine ( "{0} : {1}", subscriptionName, sType);
					
					typehelper th;
					if (dictTypes.TryGetValue (sType, out th)) {
					    	var o = JsonConvert.DeserializeObject(json,th.t);
					    	th.methodInfo.Invoke (null, new object[]  {o} );
					}
				}
			}
		}
			
		public void Stop () {
			running = false;
			if (sub != null) { 
				subThread.Abort();
				sub.Dispose();
			}
		}
		
		class typehelper {
			public Type t;
			public MethodInfo methodInfo;
		}
		private Dictionary<string,typehelper> dictTypes = new Dictionary<string,typehelper> ();
		
		public void Subscribe<T>(string subscriptionName, Action<T> onReceiveMessage)
        {
			sub.Subscribe (subscriptionName);
			
			
			
			string sType = typeof(T).ToString();
			if (!dictTypes.ContainsKey (sType)) {
				typehelper th = new typehelper ();
				th.t = typeof(T);
				th.methodInfo = onReceiveMessage.Method ;
				dictTypes.Add (sType,th );
			}
		}
		
		

				
	}
}