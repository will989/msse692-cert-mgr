using System.Configuration;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace CertificateManager.Data
{
    using Entities;
    using MongoDB.Driver;
    

    public class MongoConnectionHandler<T> where T : IMongoEntity
    {
        
        public MongoCollection<T> MongoCollection { get; private set; }

        private readonly string _connectionString = System.Environment.GetEnvironmentVariable("CERTMGR_MONGO_URI");
        
        public MongoConnectionHandler()
             {

            //was trying to use app.config or web.config but commented out for now, using an EnvironmentVariable instead
            //string connection = ConfigurationManager.AppSettings.Get("MongoConnectionString");
            //string database = ConfigurationManager.AppSettings.Get("databaseName");

            //if (connection != null)
            //{
                //string connectionString = connection;
                //string databaseName = database;

            try
            {
                //// Get a thread-safe client object by using a connection string
                var mongoClient = new MongoClient(_connectionString);

                //// Get a reference to a server object from the Mongo client object
                var mongoServer = mongoClient.GetServer();

                //// Get a reference to the "certificateManager" database object 
                //// from the Mongo server object
                const string databaseName = "certificateManager";
                var db = mongoServer.GetDatabase(databaseName);

                //// Get a reference to the collection object from the Mongo database object
                //// The collection name is the type converted to lowercase + "s"
                MongoCollection = db.GetCollection<T>(typeof (T).Name.ToLower() + "s");
            }
            catch (Exception ex)
            {
                log.Error("Caught exception in MongoConnectionHandler: {0}", ex);
                throw;
            }
            // }
        }

        static MongoDatabase retreive_mongohq_db()
        {

            return MongoServer.Create(

              ConfigurationManager.ConnectionStrings["MongoHQ"].ConnectionString)

              .GetDatabase("t2");

        }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}
