using MongoDB.Bson;
using MongoDB.Driver;
using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.POC.ConsoleLogGen
{
    class Program
    {
        static void Main(string[] args)
        {
            //var template = "[{Timestamp:HH:mm:ss} {Level} {Message}{NewLine}{Exception}]";
            ////var snk = LoggerSinkConfiguration  //.LiterateConsole(Events.LogEventLevel.Verbose, template, null);
            //var log = new LoggerConfiguration()
            //    .WriteTo.MongoDB()
            //    .CreateLogger();

            //log.Information("Test");
            //Console.ReadKey();

            var cert = new X509Certificate2("cert\\certificate.pfx", "8164053997");

            var settings = new MongoClientSettings
            {
                //SslSettings = new SslSettings
                //{
                //    ClientCertificates = new[] { cert },
                //},
                SslSettings = new SslSettings
                {
                    ClientCertificates = new[] { cert },
                    ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    {
                        foreach (var item in chain.ChainElements)
                        {
                            foreach (var elemStatus in item.ChainElementStatus)
                            {
                                Console.WriteLine(item.Certificate.Subject + "->" + elemStatus.StatusInformation);
                            }
                        }

                        return true; //NOT FOR PRODUCTION: this line will bypass certificate errors.
                    }
                },
                UseSsl = true,
                Server = new MongoServerAddress("mongodb.chdev77.com", 449)
            };

            var client = new MongoClient(settings);
            var db = client.GetDatabase("LibraryDb2");
            var collection = db.GetCollection<BookStore>("BookStore");

            //10,000,000 inserts
            Parallel.For(0, 10, i =>
            {
                for (int j = 0; j < 10000; j++)
                {
                    for (int k = 0; k < 100; k++)
                    {
                        BookStore bookStore = new BookStore
                        {
                            BookTitle = $"MongoDB Basics_{i}_{j}_{k}",
                            ISBN = $"8767687689898yu2{i}_{j}_{k}",
                            Auther = $"Tanya {i}_{j}_{k}",
                            Category = $"{i}_{j}_{k}"
                        };

                        collection.InsertOne(bookStore);
                    }

                }
            }); // Parallel.For


            var tmp = 1;

        }


    }

    public class BookStore
    {
        public ObjectId Id { get; set; }
        public string BookTitle { get; set; }
        public string Auther { get; set; }
        public string Category { get; set; }
        public string ISBN { get; set; }
    }
}
