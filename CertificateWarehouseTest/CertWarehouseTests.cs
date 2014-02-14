using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using CertificateManager;
using CertificateManager.Data.Entities;
using CertificateManagerTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestUtilities;

namespace CertificateWarehouseTest
{
    [TestClass]
    public class CertWarehouseTests
    {

        [TestMethod]
        public void FindCertificateByIdTest()
        {
            //instantiate web service
            CertificateWarehouseService.CertificateWarehouseServiceClient
                wsref = new CertificateWarehouseService.CertificateWarehouseServiceClient();

            //get certificate by id  52f81ef35b66e82f50d02ade
            Certificate certificate = wsref.GetCertificateById("52f81ef35b66e82f50d02ade");

            bool certFound;

            //if we're able to retrieve a property then the certificate was returned
            if (certificate.IsDeleted == true || certificate.IsDeleted == false)
                certFound = true;
            else
            {
                certFound = false;
            }


            System.Diagnostics.Debug.WriteLine("certFound = {0}", certFound);
            System.Diagnostics.Debug.WriteLine("Certificate name = {0}", certificate.Name);
            System.Diagnostics.Debug.WriteLine("Thumbprint = {0}", certificate.Thumbprint);
            System.Diagnostics.Debug.WriteLine("Expiration date = {0}", certificate.ExpirationDate);

            Assert.IsTrue(certFound);

        }

        [TestMethod]
        public void FindCertificateByNameTest()
        {
            //instantiate web service
            CertificateWarehouseService.CertificateWarehouseServiceClient
                wsref = new CertificateWarehouseService.CertificateWarehouseServiceClient();

            const string name = "CN=2014-02-11_06-53-35-PM";

            //get certificate by name
            Certificate certificate = wsref.GetCertificateByName(name);

            bool certFound;

            //if we're able to retrieve a property then the certificate was returned
            if (certificate.Name.Equals(name))
                certFound = true;
            else
            {
                certFound = false;
            }


            System.Diagnostics.Debug.WriteLine("certFound = {0}", certFound);
            System.Diagnostics.Debug.WriteLine("Certificate name = {0}", certificate.Name);
            System.Diagnostics.Debug.WriteLine("Thumbprint = {0}", certificate.Thumbprint);
            System.Diagnostics.Debug.WriteLine("Expiration date = {0}", certificate.ExpirationDate);

            Assert.IsTrue(certFound);

        }

        [TestMethod]
        public void FindCertificateByThumbprintTest()
        {
            const string thumbprint = "4CFF175B12392946F2995CFA9F4BA7F642BE4DB6";
            //instantiate web service
            CertificateWarehouseService.CertificateWarehouseServiceClient
                wsref = new CertificateWarehouseService.CertificateWarehouseServiceClient();

            //get certificate by thumbprint - this one should fail:
            //Certificate certificate = wsref.GetCertificateByThumbprint("4CFF175B12392946F2995CFA9F4BA7F642BE4DB7");

            //this one has the actual thumbprint (ends in B6)
            Certificate certificate = wsref.GetCertificateByThumbprint(thumbprint);

            bool certFound;

            //if we're able to retrieve a property then the certificate was returned
            if (certificate.Thumbprint.Equals(thumbprint))
                certFound = true;
            else
            {
                certFound = false;
            }


            System.Diagnostics.Debug.WriteLine("certFound = {0}", certFound);
            System.Diagnostics.Debug.WriteLine("Certificate name = {0}", certificate.Name);
            System.Diagnostics.Debug.WriteLine("Expiration date = {0}", certificate.ExpirationDate);

            Assert.IsTrue(certFound);

        }


        [TestMethod]
        public void AddOrganizationTest()
        {
            //instantiate web service
            CertificateWarehouseService.CertificateWarehouseServiceClient
                wsref = new CertificateWarehouseService.CertificateWarehouseServiceClient();

            Organization organization = new Organization();
            
            //create a unique organizationName
            string organizationName = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
            organization.Name = organizationName;
            organization.OrganizationCertificates = new List<OrganizationCertificate>();

            bool added = wsref.AddOrganizationToDatabase(organization);

            System.Diagnostics.Debug.WriteLine("added = {0}", added);

            Assert.IsTrue(added);

        }

        [TestMethod]
        public void AddCertificateToDatabaseTest()
        {
            //instantiate web service
            CertificateWarehouseService.CertificateWarehouseServiceClient
                wsref = new CertificateWarehouseService.CertificateWarehouseServiceClient();

            Certificate certificate = new Certificate();

            //create a unique subjectName
            string subjectName = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);

            //create a new certificate for testing
            X509Certificate2 testCertificate2 =
                new X509Certificate2(CreateCertificate.CreateSelfSignedCertificate(subjectName));

            certificate.Name = testCertificate2.SubjectName.Name;
            certificate.StartDate = testCertificate2.NotBefore;
            certificate.EndDate = testCertificate2.NotAfter;
            certificate.IsDeleted = false;
            certificate.ExpirationDate = testCertificate2.NotAfter;
            certificate.Thumbprint = testCertificate2.Thumbprint;
            certificate.Content = testCertificate2.RawData;

            
            //add certificate to database
            bool added = wsref.AddCertificateToDatabase(certificate);

            System.Diagnostics.Debug.WriteLine("added = {0}", added);

            Assert.IsTrue(added);

        }

        [TestMethod]
        public void AddCertificateToOrganizationTest()
        {
            //instantiate web service
            CertificateWarehouseService.CertificateWarehouseServiceClient
                wsref = new CertificateWarehouseService.CertificateWarehouseServiceClient();

            OrganizationCertificate organizationCertificate = new OrganizationCertificate();

            
            //get certificate by id  52f81ef35b66e82f50d02ade
            Certificate certificate = wsref.GetCertificateById("52f81ef35b66e82f50d02ade");


            //set organizationId
            string organizationId = "52f7ed2f5b66e8377430d324";

            organizationCertificate.Purpose = 1;
            organizationCertificate.Active = true;
            organizationCertificate.CertificateId = certificate.Id;

            
            //add certificate to test organization
            bool added = wsref.AddCertificateToOrganization(organizationId, organizationCertificate);

            System.Diagnostics.Debug.WriteLine("added = {0}", added);

            Assert.IsTrue(added);

           }

        [TestMethod]
        public void GetCertificateDetailsTest()
        {
            //instantiate web service
            CertificateWarehouseService.CertificateWarehouseServiceClient
                wsref = new CertificateWarehouseService.CertificateWarehouseServiceClient();

            int length = 0;
            
            //get certificate by id  52f81ef35b66e82f50d02ade
            var certificatesCursor = wsref.GetCertificatesDetails(100, 0);

            if (certificatesCursor != null)
            {
                 length = certificatesCursor.Length;
            }

            foreach (var certificate in certificatesCursor)
            {
                System.Diagnostics.Debug.WriteLine("Certificate Name = {0}", certificate.Name);
                System.Diagnostics.Debug.WriteLine("Certificate Thumbprint = {0}", certificate.Thumbprint);
                System.Diagnostics.Debug.WriteLine("Certificate Expiration Dt = {0}", certificate.ExpirationDate);
            }
            
            //make sure that the certificatesCursor 
            Assert.IsTrue(length > 0);
        }

    }
}
