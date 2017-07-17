using System;

namespace  Burrow.RPC
{

	public enum Direction {
		Sent,
		Received
	}
	
	public delegate void logDelegate(Direction direction, string json);
	
	/*public class Logging
	{
		public Logging()
		{
		}
	}*/
}
