/*
 * Created by SharpDevelop.
 * User: florian
 * Date: 10/27/2016
 * Time: 10:49 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

 	
namespace testPub
{
	/// <summary>
	/// Description of Publisher.
	/// </summary>
	public class Publisherxxxx
	{
		public static void Publish(int totalThread, int numberOfRabbitToCreatePerThread)
        {
           /* var tunnel = RabbitTunnel.Factory.Create();

            tunnel.SetSerializer(new JsonSerializer());

            var tasks = new List<Task>();
            var sw = new Stopwatch();
            sw.Start();
            tunnel.DedicatedPublishingChannel.ConfirmSelect();
            for (var i = 0; i < totalThread; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    uint index;
                    for (index = 0; index < numberOfRabbitToCreatePerThread; index++)
                    {
                        try
                        {
                            tunnel.Publish(new Bunny
                            {
                                Age = index,
                                Color = "White",
                                Name = "The Energizer Bunny"
                            });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            break;
                        }
                    }
                }));
            }
            tunnel.DedicatedPublishingChannel.WaitForConfirmsOrDie();

            Task.WaitAll(tasks.ToArray());
            sw.Stop();
            Global.DefaultWatcher.InfoFormat("Published {0} \"rabbits\" in {1}.", numberOfRabbitToCreatePerThread * totalThread, sw.Elapsed);
            
            */
        }
	}
}
