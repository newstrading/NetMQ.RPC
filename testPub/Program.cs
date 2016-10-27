using System;
using System.Threading;

using zmqRPC.PubSub;
 	
using test;
 	
namespace testPub
{
	class Program
	{
		public static void Main(string[] args)
		{			
			// TODO: Implement Functionality Here
			
			 /*Global.DefaultSerializer = new JsonSerializer();
            Global.DefaultWatcher.InfoFormat("This demo will show you how Burrow.NET publish messages from RabbitMQ.\nPress anykey to continue!!!");
            Console.ReadKey();
            Console.Clear();
      
			RabbitSetupTest.CreateExchangesAndQueues();
            Global.DefaultWatcher.InfoFormat("Press anykey to publish messages ..."); 
            Console.ReadKey(false);
            PublishingTest.Publish(2, 5000);
            Global.DefaultWatcher.InfoFormat("If you check queue Burrow.Queue.BurrowTestApp.Bunny, you should see 10K msgs there");
   			*/
			
   			Bunny b = new Bunny ();
   			b.Age = 99;
   			b.Color = "Green";
   			b.Name ="Bugs";
   			
   			var publisher = new Publisher ("@tcp://127.0.0.1:19888");
   			
   			
   			Console.Title = "zmq Publisher";
			Console.Write("Press any key to start to generate data . . . ");
			Console.ReadKey(true);
			Console.Clear();
			
			for (uint i=0; i<100; i++) {
				b.Age = 10 + i;
				publisher.Publish<Bunny> ("test", b);
				Thread.Sleep (1000);
			}
			
			Console.Write("Finished Generating Data. . . ");
			Console.ReadKey(true);
		}
	}
}