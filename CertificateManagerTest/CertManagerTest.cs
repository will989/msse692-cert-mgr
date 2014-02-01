using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using CertificateManagerTest.CertificateManagerService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CertificateManagerTest
{
    [TestClass]
    public class CertManagerTest
    {
        [TestMethod]
        public void TestListCertificates()
        {
            X509Store store = new X509Store("CA", StoreLocation.LocalMachine);
            System.Diagnostics.Debug.WriteLine("Running TestListCertificates");
            //instantiate web service
            CertificateManagerService.CertificateManagerServiceClient
                wsref = new CertificateManagerService.CertificateManagerServiceClient();

            List<X509Certificate2> certList = new List<X509Certificate2>(wsref.ListCertificatesInStore(store));

            int size = certList.Count;
            System.Diagnostics.Debug.WriteLine("certList count = {0}:", size);


            Assert.IsTrue(size > 0);
          

            //List<X509Certificate2> certList = ListCertificate
            //if (certList == null) throw new ArgumentNullException("certList");
        }
    }
}
