using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CertificateManager.Data.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CertificateManager.Data.Entities
{
    [BsonIgnoreExtraElements]
 public class Certificate : MongoEntity
    {
        public string Name { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime StartDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime EndDate { get; set; }

        public bool IsDeleted { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ExpirationDate { get; set; }

        public string Thumbprint { get; set; }

        public byte[] Content { get; set; }
        
    }
}
