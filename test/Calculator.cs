using System;
using System.Collections.Generic;
using System.Linq;

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
		
		List<User> ICalculator.GoodProgrammers (List<User> all) {
			return all.Where(p => p.Good).ToList();
		}
	}
}
