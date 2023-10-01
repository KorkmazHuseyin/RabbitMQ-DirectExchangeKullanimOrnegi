using Microsoft.AspNet.SignalR.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;

namespace RAbbitMQ.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {

          
            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://vxdvxesj:VT6vpW9ieHhgHroI29tDgiM2yqHaXezh@toad.rmq.cloudamqp.com/vxdvxesj");


          
            using (var connnection = factory.CreateConnection())
            {              

                var channel = connnection.CreateModel();
               
               


                channel.BasicQos(0,1,false);

                var consumer = new EventingBasicConsumer(channel);


                var queueName = "direct-queue-Critical";
                channel.BasicConsume(queueName, false,consumer);
                Console.WriteLine("Log alınıyor........");

                consumer.Received += (object sender, BasicDeliverEventArgs e)=> 
                {
                    var message = Encoding.UTF8.GetString(e.Body.ToArray());
                    Console.WriteLine("Gelen Mesaj =======>" + message);
                    File.AppendAllText("log-critical.txt", message+ "\n");
                    

                    channel.BasicAck(e.DeliveryTag, false);
                };

                Console.ReadKey();

            }
            
        }

       
    }
}

