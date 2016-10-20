using System;
using System.Collections.Generic;

namespace test
{

	public class Calculator : ICalculator
	{
		double ICalculator.Add (double a, double b) {
			return a+b;
		}
		
		// Generic Lists are normally not handled well by the NewtonSoft.JSON serializer
		
		List<User> ICalculator.Programmers () {
			return User.Super();
		}
		
		void ICalculator.SetProgrammers (List<User> programmers) {
			return; 
		}
	}
}
