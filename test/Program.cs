
using System;

using Burrow.RPC;


namespace test
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			var server = RpcFactory.CreateServer<ICalculator>(new Calculator (), "tcp://127.0.0.1:13777");
			server.Start();
			

			var calculator = RpcFactory.CreateClient<ICalculator>(">tcp://127.0.0.1:13777");			
			var result = calculator.Add (1,2);
			Console.WriteLine ("Result is: {0}", result); 
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
			server.Stop();
			Console.WriteLine ("Shutting Down..");
			
		}
	}
}