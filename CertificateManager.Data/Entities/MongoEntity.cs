using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CertificateManager.Data.Entities
{

    //The MongoEntity class uses the MongoDB.Bson.Serialization.Attributes.BsonIdAttribute (BsonId) 
    //attribute to specify that the Id field must be mapped to the _id field for each document
    //during serialization and deserialization
    public class MongoEntity : IMongoEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
