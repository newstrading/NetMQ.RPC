using System;

using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;

namespace zmqRPC.PubSub
{

	public class Publisher
	{
		PublisherSocket publisherSocket;
		
		public Publisher(string connectionStringPublisher)
		{
			publisherSocket = new PublisherSocket (connectionStringPublisher);
			
			 // fix timeout behind NAT
			publisherSocket.Options.TcpKeepalive = true;
			publisherSocket.Options.TcpKeepaliveInterval = new TimeSpan(0, 0, 55);
			publisherSocket.Options.TcpKeepaliveIdle = new TimeSpan(0, 0, 25);
		}
		
		public void Stop () {
			publisherSocket.Dispose();
		}
		
		public void Publish<T>(string subscriptionName, T data )
        {
            try
            {
            	string json = JsonConvert.SerializeObject(data);
            	string datatype = typeof(T).ToString();
            	//Console.WriteLine ("Publishing {0}:{1}", subscriptionName, datatype);
            	publisherSocket
					.SendMoreFrame(subscriptionName)
					.SendMoreFrame(datatype)
					.SendFrame(json);
            	
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Publish failed: '{0}'", ex.Message), ex);
            }
        }
		
		
		  
		  
		
		  
		  
		  
		
	}
}
