using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

    
namespace CertificateManager.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class OrganizationCertificate
    {
        public ObjectId CertificateId { get; set; }
        public int Purpose { get; set; }
        public bool Active { get; set; }
    }
}
