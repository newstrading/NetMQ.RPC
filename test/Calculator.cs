using System;
using System.Collections.Generic;

namespace test
{

	public class Calculator : ICalculator
	{
		double ICalculator.Add (double a, double b) {
			return a+b;
		}
		
		List<User> ICalculator.Programmers () {
			return User.Super();
		}
	}
}
