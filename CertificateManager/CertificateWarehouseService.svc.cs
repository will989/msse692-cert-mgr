﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;
using CertificateManager.Data;
using CertificateManager.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
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
            var certificate = new Certificate();

            try
            {
                //get connection to database
                var mongoConnectionHandler = new MongoConnectionHandler<Certificate>();

                //next two lines from docs.mongodb.org http://docs.mongodb.org/ecosystem/tutorial/getting-started-with-csharp-driver/
                //query = Query<Certificate>.EQ(c => c.Id, certificateId);
                //var query = Query<Certificate>.EQ<Certificate>(ObjectId.Parse(certificateId));
                
                //other possibilities:
                //certificate = mongoConnectionHandler.MongoCollection.FindOneById(ObjectId.Parse(certificateId));
               // certificate = mongoConnectionHandler.MongoCollection.FindOneById(certificateId);

                //but this one works -- remember to convert the string "certificateId" passed in, into a Mongodb Id..
                certificate = mongoConnectionHandler.MongoCollection.FindOneByIdAs<Certificate>(certificateObjectId);


                System.Diagnostics.Debug.WriteLine("Certificate info....");

                System.Diagnostics.Debug.WriteLine("Certificate thumbprint: {0}", certificate.Thumbprint);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception caught: {0}", ex);
            }

            return certificate;
        }

        [WebMethod]
        public Certificate GetCertificateByName(string name)
        {

            //get connection to database
            var mongoConnectionHandler = new MongoConnectionHandler<Certificate>();

            var certificate = new Certificate();

            //get the collection
            var collection = mongoConnectionHandler.MongoCollection;

            //for debug mostly, how many certificates are in the collection?
            var result =
    (from c in collection.AsQueryable<Certificate>()
     select c)
    .Count();

            System.Diagnostics.Debug.WriteLine("Result = {0}", result);

            //this is our query -- is there a cert that matches the thumbprint passed in?
            var query = from c in collection.AsQueryable()
                        where c.Name == name
                        select c;

            //should only be one match, but this loop works either way
            foreach (var testcert in query)
            {
                if (testcert.Name.Equals(name))
                {
                    certificate = testcert;
                    break;
                }

            }
            

            System.Diagnostics.Debug.WriteLine("Certificate info....");
            System.Diagnostics.Debug.WriteLine("Certificate thumbprint: {0}", certificate.Thumbprint);

            return certificate;
        }


        [WebMethod]
        public Certificate GetCertificateByThumbprint(string thumbprint)
        {

            //get connection to database
            var mongoConnectionHandler = new MongoConnectionHandler<Certificate>();

            var certificate = new Certificate();

            //get the collection
            var collection = mongoConnectionHandler.MongoCollection;

            //for debug mostly, how many certificates are in the collection?
            var result =
    (from c in collection.AsQueryable<Certificate>()
     select c)
    .Count();

            System.Diagnostics.Debug.WriteLine("Result = {0}", result);
            
            //this is our query -- is there a cert that matches the thumbprint passed in?
            var query = from c in collection.AsQueryable()
                where c.Thumbprint == thumbprint
                select c;

            //should only be one match, but this loop works either way
            foreach (var testcert in query)
            {
                if (testcert.Thumbprint.Equals(thumbprint))
                {
                    certificate = testcert;
                    break;
                }

            }
            

            System.Diagnostics.Debug.WriteLine("Certificate info....");
            System.Diagnostics.Debug.WriteLine("Certificate thumbprint: {0}", certificate.Thumbprint);

            return certificate;
        }


        [WebMethod]
        public IEnumerable<Certificate> GetCertificatesDetails(int limit, int skip)
        {
            //get connection to database
            var mongoConnectionHandler = new MongoConnectionHandler<Certificate>();
            var certificatesCursor = mongoConnectionHandler.MongoCollection.FindAllAs<Certificate>()
                .SetSortOrder(SortBy<Certificate>.Descending(c => c.Name))
                .SetLimit(limit)
                .SetSkip(skip)
                .SetFields(Fields<Certificate>.Include(c => c.Id, c => c.Name, c => c.ExpirationDate, c => c.Thumbprint));
            return certificatesCursor;
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
