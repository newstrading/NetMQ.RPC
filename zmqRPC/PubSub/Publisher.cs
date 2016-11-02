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
