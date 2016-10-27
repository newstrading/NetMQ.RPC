/*
 * Created by SharpDevelop.
 * User: florian
 * Date: 10/27/2016
 * Time: 10:46 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace zmqRPC.Serialization
{
	/// <summary>
    /// Implement this interface and use it with <see cref="ITunnel"/> to serialize messages before publishing it to rabbitmq server
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serialize a message to byte[]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        byte[] Serialize<T>(T message);
        /// <summary>
        /// Deserialize byte[] back to a strongly type object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        T Deserialize<T>(byte[] bytes);
    }
}
