using RabbitMQ.Client;
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace RAbbitMQ.Publisher
{
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info = 4
    }

  


    class Program
    {
        static void Main(string[] args)
        {
          
            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://vxdvxesj:VT6vpW9ieHhgHroI29tDgiM2yqHaXezh@toad.rmq.cloudamqp.com/vxdvxesj");


         
            using (var connnection = factory.CreateConnection())
            {
               

                var channel = connnection.CreateModel();

                //Bir önceki örnekte queue oluşturmuştuk. Bu sefer sadece Exchange oluşturuyorum.
                channel.ExchangeDeclare("logs-direct",durable:true,type:ExchangeType.Direct);


                Enum.GetNames(typeof(LogNames)).ToList().ForEach (x => 
                {
                    var routeKey = $"route-{x}"; // Her bir log um ilgili routuna gitmeli
                    var queueName = $"direct-queue-{x}"; // LogName içindeki Logismini al
                    channel.QueueDeclare(queueName, true, false, false);// Bu log ismi ile queue oluştur.
                    channel.QueueBind(queueName, "logs-direct",routeKey, null);   // Oluşturduğun her bir kuyruğa-  Exchange ilgili route üzerinden mesaj gönderimi yapabilecek bind ederek        
                
                }) ;
             


               


              
                foreach (var item in Enumerable.Range(1,125))
                {
                    LogNames log = (LogNames)new Random().Next(1, 5);// Foreach her dönüşünde 1-4 arasındaki değerlerde bir logName alıcak
                       
                    string message = $"log-type:{log}"; // Aldığı LogName i message değişkenine atıca.

                    var messageBody = Encoding.UTF8.GetBytes(message);// Değişken ile gelen LogName i Kuyruğa yönlendirmek üzere Byte[] alıcam

                    var routeKey = $"route-{log}"; // Her bir log um ilgili routuna gidecek


                   
                    channel.BasicPublish("logs-direct",routeKey, null, messageBody);

                    Console.WriteLine($"Log gönderilmiştir : {message}");

                }

                Console.ReadKey();
            }
        }
    }
}
