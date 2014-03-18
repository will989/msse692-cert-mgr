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

        //these should work but I don't want to mess with it
        //private readonly string _connectionString = System.Environment.GetEnvironmentVariable("CERTMGR_MONGO_URI");
        //private readonly string _database = System.Environment.GetEnvironmentVariable("CERTMGR_MONGO_DB");

        private readonly string _connectionString = "mongodb://CertMgrMongo:Xdv_WjVaSofIga167ec4EoU36VLUu53L1m9iXaiwpQg-@ds030827.mongolab.com:30827/";
        private readonly string _database = "CertMgrMongo";


        public MongoConnectionHandler()
         {
   

            try
            {
                //// Get a thread-safe client object by using a connection string
                var mongoClient = new MongoClient(_connectionString);
                log.Debug("After mongoClient");

                //// Get a reference to a server object from the Mongo client object
                var mongoServer = mongoClient.GetServer();

                //// Get a reference to the "certificateManager" database object 
                //// from the Mongo server object
                 //const string databaseName = "certificateManager";
                //var db = mongoServer.GetDatabase(databaseName);
                
                //get database from constant above
                var db = mongoServer.GetDatabase(_database);

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
