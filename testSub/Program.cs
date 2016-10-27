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
			
			X x = new X ();
			subscriber.Subscribe<Bunny> ("a", x.print );
			subscriber.Subscribe<Bunny> ("b", GlobalPrint );
			
			Console.Write("Press any key to stop subscribing . . . ");
			Console.ReadKey(true);
			subscriber.Stop();
		}
		
		static void GlobalPrint (Bunny b) {
			Console.WriteLine ("Global Printer: Bunny Received: {0}", b.Age);
		}
		
		class X {
			public void print (Bunny b) {
				Console.WriteLine ("Bunny Received: {0}", b.Age);
			}
		}
	}
}