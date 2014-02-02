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
    }
}
