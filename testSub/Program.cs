using System;
using System.Threading;

using zmqRPC.PubSub;

using test;

namespace testSub
{
	class Program
	{
		public static void Main(string[] args)
		{
			string cs = ">tcp://127.0.0.1:19888";
			var subscriber = new Subscriber(cs);
			Console.WriteLine("Subscribing at: " + cs);
			
			subscriber.Subscribe ("a", (Bunny b) => {
			                      	Console.WriteLine ("Bunny Received: {0}", b.Age);
			                      });
			
			Console.Write("Press any key to stop subscribing . . . ");
			Console.ReadKey(true);
			subscriber.Stop();
		}
	}
}