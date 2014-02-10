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


            //set organizationId
            string organizationId = "52f7ed2f5b66e8377430d324";

            organizationCertificate.Purpose = 1;
            organizationCertificate.Active = true;

            
            //add certificate to test organization
            bool added = wsref.AddCertificateToOrganization(organizationId, organizationCertificate);

            System.Diagnostics.Debug.WriteLine("added = {0}", added);

            Assert.IsTrue(added);

        }
        
    }
}
