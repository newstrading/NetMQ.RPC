using System;
using System.Collections.Generic;

namespace test
{
	public interface ICalculator
	{
		double Add (double a, double b);
		
		List<User> Programmers ();
		
		void SetProgrammers (List<User> programmers);
		
		List<User> GoodProgrammers(List<User> allProgrammers);
	}
	
	
	public class User {
		public string Name;
		public bool Authorized;
		public bool Good;
		
		public static List<User> Super () {
			var list = new List<User> ();
			list.Add( new User () { Name = "Walter", Authorized = true, Good = true} );
			list.Add( new User () { Name = "Chris", Authorized = false, Good= false} );
			return list;
		}
	}
}
