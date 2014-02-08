using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
            //X509Store store = new X509Store("CA", StoreLocation.LocalMachine);
            System.Diagnostics.Debug.WriteLine("Running TestListCertificates");
            //instantiate web service
            CertificateManagerService.CertificateManagerServiceClient
                wsref = new CertificateManagerService.CertificateManagerServiceClient();

            StoreName storeName = StoreName.CertificateAuthority;

            string strStoreName = storeName.ToString();

            if (strStoreName.Equals("CertificateAuthority"))
            {
                strStoreName = "CA";
            }
            System.Diagnostics.Debug.WriteLine("strStoreName = {0}", strStoreName);

            List<X509Certificate2> certList = new List<X509Certificate2>(wsref.ListCertificatesInStore(strStoreName, StoreLocation.LocalMachine));

            int size = certList.Count;
            System.Diagnostics.Debug.WriteLine("certList count = {0}", size);


            Assert.IsTrue(size > 0);

        }

        [TestMethod]
        public void TestEnumCertificates()
        {
            //X509Store store = new X509Store("CA", StoreLocation.LocalMachine);
            System.Diagnostics.Debug.WriteLine("Running TestEnumCertificates");
            //instantiate web service
            CertificateManagerService.CertificateManagerServiceClient
                wsref = new CertificateManagerService.CertificateManagerServiceClient();

            StoreName storeName = StoreName.CertificateAuthority;

            string strStoreName = storeName.ToString();

            if (strStoreName.Equals("CertificateAuthority"))
            {
                strStoreName = "CA";
            }
            System.Diagnostics.Debug.WriteLine("strStoreName = {0}", strStoreName);

            wsref.EnumCertificates(strStoreName, StoreLocation.LocalMachine);

        }

        [TestMethod]
        public void TestChangeStoreLocations()
        {

            //instantiate web service
            CertificateManagerService.CertificateManagerServiceClient
                wsref = new CertificateManagerService.CertificateManagerServiceClient();


            foreach (StoreLocation storeLocation in (StoreLocation[])
                Enum.GetValues(typeof (StoreLocation)))
            {
                foreach (StoreName storeName in (StoreName[])
                    Enum.GetValues(typeof (StoreName)))
                {
                    //not using store as of 2/1 8:17pm - maybe put back.  Seemed to be throwing error because
                    //the store wasn't open
                   // X509Store store = new X509Store(storeName, storeLocation);

                   //for now we'll pass the store name as a string, and the store location
                   string strStoreName = storeName.ToString();

                    if (strStoreName.Equals("CertificateAuthority"))
                    {
                        strStoreName = "CA";
                    }

                    System.Diagnostics.Debug.WriteLine("stringStoreName = {0}, storeLocation = {1}", strStoreName, storeLocation.ToString());
                    try
                    {
                        List<X509Certificate2> certList = new List<X509Certificate2>(wsref.ListCertificatesInStore(strStoreName, storeLocation));

                        System.Diagnostics.Debug.WriteLine("Yes    {0,4}  {1}, {2}",
                          certList.Count, strStoreName, storeLocation.ToString());

                        
                        if (certList.Count >= 0)
                        {
                            foreach (X509Certificate2 certificate2 in certList)
                            {
                                byte[] rawdata = certificate2.RawData;
                                System.Diagnostics.Debug.WriteLine("Properties of certificate");
                                System.Diagnostics.Debug.WriteLine("Certificate Simple Name: {0}",
                                    certificate2.GetNameInfo(X509NameType.SimpleName, true));
                                System.Diagnostics.Debug.WriteLine("Certificate start date (NotBefore): {0}",
                                    certificate2.NotBefore);
                                System.Diagnostics.Debug.WriteLine("Certificate expiration date (NotAfter): {0}",
                                    certificate2.NotAfter);
                                System.Diagnostics.Debug.WriteLine("Certificate thumbprint: {0}",
                                    certificate2.Thumbprint);
                                System.Diagnostics.Debug.WriteLine("Certificate SerialNumber {0}",
                                    certificate2.SerialNumber);
                                System.Diagnostics.Debug.WriteLine(
                                    "********************************************************");

                            }//end foreach
                        }//end if
                            

                        // store.Open(OpenFlags.OpenExistingOnly);

                        //System.Diagnostics.Debug.WriteLine("Yes    {0,4}  {1}, {2}",
                          //  store.Certificates.Count, store.Name, store.Location);
                    }
                    catch (CryptographicException)
                    {
                        System.Diagnostics.Debug.WriteLine("No           {0}, {1}",
                            strStoreName, storeLocation);
                    }
                }
                System.Diagnostics.Debug.WriteLine("");
            }
        }

        [TestMethod]
        public void TestInstallCertificateLocal()
        {
            
            System.Diagnostics.Debug.WriteLine("Running TestInstallCertificateLocal");

            //instantiate web service
            CertificateManagerService.CertificateManagerServiceClient
                wsref = new CertificateManagerService.CertificateManagerServiceClient();

            //create a unique subjectName
            string subjectName = string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);

            X509Store store = new X509Store("CA", StoreLocation.LocalMachine);
            X509Certificate2 certificate = new X509Certificate2(CreateCertificate.CreateSelfSignedCertificate(subjectName));

            int count = store.Certificates.Count;

            bool added = wsref.InstallCertificateLocal(store, certificate);

           Assert.IsTrue(added);

        }

        [TestMethod]
        public void TestRemoveCertificateLocal()
        {
            bool removed = false;
            System.Diagnostics.Debug.WriteLine("Running TestRemoveCertificateLocal");

            //instantiate web service
            CertificateManagerService.CertificateManagerServiceClient
                wsref = new CertificateManagerService.CertificateManagerServiceClient();

            X509Store store = new X509Store("CA", StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadWrite);
            System.Diagnostics.Debug.WriteLine("Store info:");
            System.Diagnostics.Debug.WriteLine("Store Name: {0}", store.Name);
            System.Diagnostics.Debug.WriteLine("Store Location: {0}", store.Location);

            string certificateName = "2014";

            string thumbprint = "‎fd0fc22532bb346e2239810ce2dbfd8ce2dbc911".ToUpper();

            System.Diagnostics.Debug.WriteLine("Cert Thumbprint: {0}", thumbprint);

            /*  DEBUG STUFF...****************************************************************************************************
            //X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, true);
            X509Certificate2Collection certificates2 = store.Certificates.Find(X509FindType.FindBySubjectName, certificateName, true);
            foreach (X509Certificate2 certificate3 in certificates2)
            {
                byte[] rawdata = certificate3.RawData;
                System.Diagnostics.Debug.WriteLine("Properties of certificate");
                System.Diagnostics.Debug.WriteLine("Certificate Simple Name: {0}",
                    certificate3.GetNameInfo(X509NameType.SimpleName, true));
                System.Diagnostics.Debug.WriteLine("Certificate start date (NotBefore): {0}",
                    certificate3.NotBefore);
                System.Diagnostics.Debug.WriteLine("Certificate expiration date (NotAfter): {0}",
                    certificate3.NotAfter);
                System.Diagnostics.Debug.WriteLine("Certificate thumbprint: {0}",
                    certificate3.Thumbprint);
                System.Diagnostics.Debug.WriteLine("Certificate SerialNumber {0}",
                    certificate3.SerialNumber);
                System.Diagnostics.Debug.WriteLine(
                    "********************************************************");
            }
            */

            X509Certificate2Collection certificates = store.Certificates;

            foreach (X509Certificate2 certificate2 in certificates)
            {
                byte[] rawdata = certificate2.RawData;
                System.Diagnostics.Debug.WriteLine("Properties of certificate");
                System.Diagnostics.Debug.WriteLine("Certificate Simple Name: {0}",
                    certificate2.GetNameInfo(X509NameType.SimpleName, true));
                System.Diagnostics.Debug.WriteLine("Certificate start date (NotBefore): {0}",
                    certificate2.NotBefore);
                System.Diagnostics.Debug.WriteLine("Certificate expiration date (NotAfter): {0}",
                    certificate2.NotAfter);
                System.Diagnostics.Debug.WriteLine("Certificate thumbprint: {0}",
                    certificate2.Thumbprint);
                System.Diagnostics.Debug.WriteLine("Certificate SerialNumber {0}",
                    certificate2.SerialNumber);
                System.Diagnostics.Debug.WriteLine(
                    "********************************************************");
            }

            int count = certificates.Count;
            System.Diagnostics.Debug.WriteLine("Certificates found count = {0}", count);


            if (certificates.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine("in certificates.Count loop");
                foreach (X509Certificate2 certificate2 in certificates)
                {
                    System.Diagnostics.Debug.WriteLine("certificate2.SimpleName = {0}", certificate2.GetNameInfo(X509NameType.SimpleName, false));
                    //if (certificate2.Thumbprint != null && certificate2.Thumbprint.Equals(thumbprint))
                    var nameInfo = certificate2.GetNameInfo(X509NameType.SimpleName, false);
                    if (nameInfo != null && nameInfo.ToString().Contains(certificateName))
                    {
                        System.Diagnostics.Debug.WriteLine("Found a match!");
                        removed = wsref.RemoveCertificateLocal(store, certificate2);
                        
                        //2/2/14:
                        //This works when uncommented, but needs to be in the web service instead
                        //store.Open(OpenFlags.ReadWrite);
                        //store.Remove(certificate);
                        //removed = true;
                    }
                }
            }
            
            Assert.IsTrue(removed);


        }

        [TestMethod]
        public void TestDeleteCertificateByThumbprintLocal()
        {
            bool removed = false;
            System.Diagnostics.Debug.WriteLine("Running TestRemoveCertificateByThumbprintLocal");

            //instantiate web service
            CertificateManagerService.CertificateManagerServiceClient
                wsref = new CertificateManagerService.CertificateManagerServiceClient();

            //assign variables for service call
            StoreLocation storeLocation = new StoreLocation();
            storeLocation = StoreLocation.LocalMachine;

            string certificateName = "CN=2014-02-02_11-18-34-AM";
            //this string didn't work
            //string thumbprint = "‎fd0fc22532bb346e2239810ce2dbfd8ce2dbc911".ToUpper();

            //copied same thumbprint from cert mgr and removed spaces, but still doesn't match:
            //string thumbprint = "‎fd0fc22532bb346e2239810ce2dbfd8ce2dbc911".ToUpper();
            
            //this one did delete the certificate
            //string thumbprint = "5edb577eb2b62efd3196bfad81cba7acc89aa14c".ToUpper();

            //third try, this worked, too
            //string thumbprint = "78e0797ae57a345e9656e7f8de90c586170a85d8".ToUpper();

            //fourth try, didn't work until I added the "String.Compare..." stuff to the method:
            string thumbprint = "‎d692f85f29d09208640b96f88591858e775b499e".ToUpper();



            string storeName = "CA";

            System.Diagnostics.Debug.WriteLine("Cert Thumbprint: {0}", thumbprint);

                        //System.Diagnostics.Debug.WriteLine("Found a match!");
                        removed = wsref.DeleteCertificateByThumbprint(certificateName, thumbprint, storeName ,storeLocation);
                        
                        //2/2/14:
                        //This works when uncommented, but needs to be in the web service instead
                        //store.Open(OpenFlags.ReadWrite);
                        //store.Remove(certificate);
                        //removed = true;
            Assert.IsTrue(removed);
                    }
            
            


        }

    }

