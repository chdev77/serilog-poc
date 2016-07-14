using MongoDB.Bson;
using MongoDB.Driver;
using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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
            //    .WriteTo.LiterateConsole()
            //    .WriteTo.RollingFile("\\Logs\\Activity-{Date}.txt")
            //    .CreateLogger();

            //log.Information("Test");
            //Console.ReadKey();

            var cert = new X509Certificate2("cert\\client-x509.pfx", "8164053997");

            var settings = new MongoClientSettings
            {
                SslSettings = new SslSettings
                {
                    ClientCertificates = new[] { cert },
                },
                UseSsl = true,
                Server = new MongoServerAddress("mongodb.chdev77.com", 449)
            };

            var client = new MongoClient(settings);
            var db = client.GetDatabase("LibraryDb");
            var collection = db.GetCollection<BookStore>("BookStore");

            BookStore bookStore = new BookStore
            {
                BookTitle = "MongoDB Basics",
                ISBN = "8767687689898yu",
                Auther = "Tanya",
                Category = "NoSQL DBMS"
            };

            collection.InsertOne(bookStore);

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
