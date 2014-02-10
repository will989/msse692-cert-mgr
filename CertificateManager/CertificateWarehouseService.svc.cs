using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Services;
using CertificateManager.Data;
using CertificateManager.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Internal;

namespace CertificateManager
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CertificateWarehouse" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CertificateWarehouse.svc or CertificateWarehouse.svc.cs at the Solution Explorer and start debugging.
    public class CertificateWarehouse : ICertificateWarehouseService
    {

        [WebMethod]
        public bool AddCertificateToOrganization(string organizationId, OrganizationCertificate organizationCertificate)
        {
            bool added = false;

            //create ObjectId from organizationId passed in
            var organizationObjectId = new ObjectId(organizationId);
            
            //don't think I'll use this - probably just be part of the orgCert
            //var certificateObjectId = new ObjectId(certificateId);

            //get connection to database
            var mongoConnectionHandler = new MongoConnectionHandler<Organization>();


            if (WriteConcern.Acknowledged != null)
            {
                var updateResult =
                    mongoConnectionHandler.MongoCollection.Update(Query<Organization>.EQ(o => o.Id, organizationObjectId),
                        Update<Organization>.Push(o => o.OrganizationCertificates, organizationCertificate),
                        new MongoUpdateOptions
                        {
                            WriteConcern = WriteConcern.Acknowledged
                        });
                System.Diagnostics.Debug.WriteLine("DocumentsAffected == {0}", updateResult.DocumentsAffected);


                if (updateResult.DocumentsAffected == 0)
                {
                    //// Something went wrong
                    added = false;

                }
                else
                {
                    added = true;
                }
            }

            return added;
        }

        [WebMethod]
        public Certificate GetCertificateById(string certificateId)
        {

            //create ObjectId from certificateId passed in
            var certificateObjectId = new ObjectId(certificateId);

            //get connection to database
            var mongoConnectionHandler = new MongoConnectionHandler<Certificate>();


            Certificate certificate = mongoConnectionHandler.MongoCollection.FindOneById(certificateId);

            System.Diagnostics.Debug.WriteLine("Certificate info....");
            System.Diagnostics.Debug.WriteLine("Certificate thumbprint: {0}", certificate.Thumbprint);

            return certificate;
        }

        [WebMethod]
        public bool AddCertificateToDatabase(Certificate certificate)
        {
            bool added;

            var mongoConnectionHandler = new MongoConnectionHandler<Certificate>();

            WriteConcern writeConcern = new WriteConcern();
            writeConcern = WriteConcern.Acknowledged;

            var createResult = mongoConnectionHandler.MongoCollection.Save(certificate, writeConcern);

            bool error = createResult.HasLastErrorMessage;

            if (error == true)
                added = false;
            else
            {
                added = true;
            }
            return added;
        }


        [WebMethod]
        public bool AddOrganizationToDatabase(Organization organization)
        {
            bool added;
            
            var mongoConnectionHandler = new MongoConnectionHandler<Organization>();

            WriteConcern writeConcern = new WriteConcern();
            writeConcern = WriteConcern.Acknowledged;
            
            var createResult = mongoConnectionHandler.MongoCollection.Save(organization, writeConcern);

            bool error = createResult.HasLastErrorMessage;

            if (error == true)
                added = false;
            else
            {
                added = true;
            }
            return added;
        }

        public void DoWork()
        {
        }
    }
}
