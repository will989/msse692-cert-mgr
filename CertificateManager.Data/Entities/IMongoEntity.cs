using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CertificateManager.Data.Entities
{
    using MongoDB.Bson;

    //The IMongoEntity interface defines an Id field of the MongoDB.Bson.ObjectId type that represents a BSON ObjectId
    public interface IMongoEntity
    {
        ObjectId Id { get; set; }
    }
}