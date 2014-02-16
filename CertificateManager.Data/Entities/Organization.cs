using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CertificateManager.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class Organization : MongoEntity
    {
        public Organization()
        {
            OrganizationCertificates = new List<OrganizationCertificate>();
        }

    
        public string Name { get; set; }

        public List<OrganizationCertificate> OrganizationCertificates { get; set; }
    }
}