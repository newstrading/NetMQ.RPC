using System;
using System.Collections.Generic;

namespace test
{
	public interface ICalculator
	{
		double Add (double a, double b);
		
		List<User> Programmers ();
	}
	
	
	public class User {
		public string Name;
		public bool Authorized;
		
		public static List<User> Super () {
			var list = new List<User> ();
			list.Add( new User () { Name = "Walter", Authorized = true} );
			list.Add( new User () { Name = "Chris", Authorized = false} );
			return list;
		}
	}
}
