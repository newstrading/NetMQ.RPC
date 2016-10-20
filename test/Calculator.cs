
using System;

namespace test
{

	public class Calculator : ICalculator
	{
		double ICalculator.Add (double a, double b) {
			return a+b;
		}
	}
}
