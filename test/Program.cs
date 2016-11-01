using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Burrow.RPC;


namespace test
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			// Serialize Test
			string json = JsonConvert.SerializeObject(User.Super());
			object o  =  JsonConvert.DeserializeObject(json,typeof(List<User>) );
			Console.WriteLine ("Deserialized type: {0}", o.GetType() );
			
			bool logMessages = true;
			var server = RpcFactory.CreateServer<ICalculator>(new Calculator (), "tcp://127.0.0.1:13777", logMessages);
			server.Start();
			

			var calculator = RpcFactory.CreateClient<ICalculator>(">tcp://127.0.0.1:13777");			
			var result = calculator.Add (1,2);
			Console.WriteLine ("Result is: {0}", result); 
			
			
			var programmers = calculator.Programmers ();
			Console.WriteLine ("Programmer Count: {0}", programmers.Count); 
			
			calculator.SetProgrammers (programmers);
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
			server.Stop();
			Console.WriteLine ("Shutting Down..");
			
		}
	}
}