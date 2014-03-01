using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Services;

namespace CertificateManager
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class CertificateManagerService : ICertificateManagerService
    {
        //Return a list of certificates
        [WebMethod]
        public List<X509Certificate2> ListCertificatesInStore(string storeName, StoreLocation storeLocation)
        {
            X509Store store = new X509Store(storeName, storeLocation);
            int certCount = 0;
            List<X509Certificate2> certList = new List<X509Certificate2>();

            //OpenExistingOnly so no exception is thrown for missing AddressBook for example
            store.Open(OpenFlags.OpenExistingOnly);

            X509Certificate2Collection collection = (X509Certificate2Collection) store.Certificates;

            System.Diagnostics.Debug.WriteLine("The collection's size is {0}", collection.Count);

            foreach (X509Certificate2 x509 in collection)
            {
                try
                {
                    certList.Add(x509);
                }
                catch (CryptographicException)
                {
                    Console.WriteLine("CryptographicException caught.");
                }
                finally
                {
                    certCount = certList.Count;
                    System.Diagnostics.Debug.WriteLine("The list's size is {0}", certList.Count);
                    if (certCount != collection.Count)
                    {
                        System.Diagnostics.Debug.WriteLine("THE COUNTS WERE DIFFERENT {0}, {1}", certCount,
                            collection.Count);
                    }
                }
            }
            store.Close();
            return certList;
        }

        //Return a list of certificates from remote machine
        [WebMethod]
        public List<X509Certificate2> ListCertificatesInRemoteStore(string storeName, StoreLocation storeLocation,
            string serverName)
        {
            string newStoreName = null;
            //make sure we aren't connecting to localhost, and got a good servername
            if (!serverName.ToUpper().Equals("LOCALHOST") && serverName.Length > 3)
            {
                // trying to concatenate the server name and store name for remote connection:
                newStoreName = string.Format(@"\\{0}\{1}", serverName, storeName);
            }
            else
            {
                //we didn't get a good server name - use local host for now
                newStoreName = string.Format("{0}", storeName);
            }
            var store = new X509Store(newStoreName, storeLocation);


            int certCount = 0;

            var certList = new List<X509Certificate2>();
            try
            {
                //OpenExistingOnly so no exception is thrown for missing AddressBook for example
                store.Open(OpenFlags.OpenExistingOnly);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Cannot connect to remote machine: {0}", serverName));
            }

            var collection = (X509Certificate2Collection) store.Certificates;

            System.Diagnostics.Debug.WriteLine("The collection's size is {0}", collection.Count);

            foreach (X509Certificate2 x509 in collection)
            {
                try
                {
                    certList.Add(x509);
                }
                catch (CryptographicException)
                {
                    Console.WriteLine("CryptographicException caught.");
                }
                finally
                {
                    certCount = certList.Count;
                    System.Diagnostics.Debug.WriteLine("The list's size is {0}", certList.Count);
                    if (certCount != collection.Count)
                    {
                        System.Diagnostics.Debug.WriteLine("THE COUNTS WERE DIFFERENT {0}, {1}", certCount,
                            collection.Count);
                    }
                }
            }
            store.Close();
            return certList;
        }


        [WebMethod]
        public List<X509Certificate2> ListExpiringCertificatesInStore(string storeName, StoreLocation storeLocation,
            int days)
        {
            //X509Store store = new X509Store(storeName, storeLocation);
            int certCount = 0;
            List<X509Certificate2> expiringCertList = new List<X509Certificate2>();
            var today = DateTime.Now;

            //OpenExistingOnly so no exception is thrown for missing AddressBook for example
            // store.Open(OpenFlags.OpenExistingOnly);

            List<X509Certificate2> testStoreList = ListCertificatesInStore(storeName, storeLocation);
            //X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            //System.Diagnostics.Debug.WriteLine("The collection's size is {0}", collection.Count);

            foreach (X509Certificate2 x509 in testStoreList)
            {
                //we want to add the number of days from today so we know whether the certificate will still be good then
                if (x509.NotAfter <= today.AddDays(days))
                {
                    try
                    {
                        expiringCertList.Add(x509);
                    }
                    catch (CryptographicException)
                    {
                        Console.WriteLine("CryptographicException caught.");
                    }
                    finally
                    {
                        certCount = expiringCertList.Count;
                        System.Diagnostics.Debug.WriteLine("The list's size is {0}", expiringCertList.Count);
                    }
                }
            }
            //store.Close();
            return expiringCertList;
        }

        [WebMethod]
        public List<X509Certificate2> ListExpiringCertificatesInRemoteStore(string storeName, StoreLocation storeLocation,
            int days, string serverName)
        {
            //X509Store store = new X509Store(storeName, storeLocation);
            int certCount = 0;
            List<X509Certificate2> expiringCertList = new List<X509Certificate2>();
            var today = DateTime.Now;

            string newStoreName = null;
            //make sure we aren't connecting to localhost, and got a good servername
            if (!serverName.ToUpper().Equals("LOCALHOST") && serverName.Length > 3)
            {
                // trying to concatenate the server name and store name for remote connection:
                newStoreName = string.Format(@"\\{0}\{1}", serverName, storeName);
            }
            else
            {
                //we didn't get a good server name - use local host for now
                newStoreName = string.Format("{0}", storeName);
            }
            var store = new X509Store(newStoreName, storeLocation);

            List<X509Certificate2> testStoreList = ListCertificatesInStore(storeName, storeLocation);
            
            foreach (X509Certificate2 x509 in testStoreList)
            {
                //we want to add the number of days from today so we know whether the certificate will still be good then
                if (x509.NotAfter <= today.AddDays(days))
                {
                    try
                    {
                        expiringCertList.Add(x509);
                    }
                    catch (CryptographicException)
                    {
                        Console.WriteLine("CryptographicException caught.");
                    }
                    finally
                    {
                        certCount = expiringCertList.Count;
                        System.Diagnostics.Debug.WriteLine("The list's size is {0}", expiringCertList.Count);
                    }
                }
            }
            //store.Close();
            return expiringCertList;
        }

        [WebMethod]
        public void PrintCertificateInfo(X509Certificate2 certificate)
        {
            System.Diagnostics.Debug.WriteLine("Name: {0}", certificate.FriendlyName);
            System.Diagnostics.Debug.WriteLine("Issuer: {0}", certificate.IssuerName.Name);
            System.Diagnostics.Debug.WriteLine("Subject: {0}", certificate.SubjectName.Name);
            System.Diagnostics.Debug.WriteLine("Version: {0}", certificate.Version);
            System.Diagnostics.Debug.WriteLine("Valid from: {0}", certificate.NotBefore);
            System.Diagnostics.Debug.WriteLine("Valid until: {0}", certificate.NotAfter);
            System.Diagnostics.Debug.WriteLine("Serial number: {0}", certificate.SerialNumber);
            System.Diagnostics.Debug.WriteLine("Signature Algorithm: {0}", certificate.SignatureAlgorithm.FriendlyName);
            System.Diagnostics.Debug.WriteLine("Thumbprint: {0}", certificate.Thumbprint);
            System.Diagnostics.Debug.WriteLine("");
        }

        [WebMethod]
        public void EnumCertificatesByStoreName(StoreName name, StoreLocation location)
        {
            X509Store store = new X509Store(name, location);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                foreach (X509Certificate2 certificate in store.Certificates)
                {
                    PrintCertificateInfo(certificate);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                store.Close();
            }
        }

        [WebMethod]
        public void EnumCertificates(string name, StoreLocation location)
        {
            X509Store store = new X509Store(name, location);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                foreach (X509Certificate2 certificate in store.Certificates)
                {
                    System.Diagnostics.Debug.WriteLine("Name: {0}", certificate.FriendlyName);
                    System.Diagnostics.Debug.WriteLine("Issuer: {0}", certificate.IssuerName.Name);
                    System.Diagnostics.Debug.WriteLine("Subject: {0}", certificate.SubjectName.Name);
                    System.Diagnostics.Debug.WriteLine("Version: {0}", certificate.Version);
                    System.Diagnostics.Debug.WriteLine("Valid from: {0}", certificate.NotBefore);
                    System.Diagnostics.Debug.WriteLine("Valid until: {0}", certificate.NotAfter);
                    System.Diagnostics.Debug.WriteLine("Serial number: {0}", certificate.SerialNumber);
                    System.Diagnostics.Debug.WriteLine("Signature Algorithm: {0}",
                        certificate.SignatureAlgorithm.FriendlyName);
                    System.Diagnostics.Debug.WriteLine("Thumbprint: {0}", certificate.Thumbprint);
                    System.Diagnostics.Debug.WriteLine("");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                store.Close();
            }
        }

        [WebMethod]
        public bool InstallCertificateLocal(X509Store store, X509Certificate2 certificate)
        {
            //X509Store store = new X509Store(StoreName.TrustedPublisher, StoreLocation.LocalMachine);

            bool added = false;
            try
            {
                store.Open(OpenFlags.ReadWrite);
                store.Add(certificate);
                added = true;
            }
            catch (Exception ex)
            {
                added = false;
                System.Diagnostics.Debug.WriteLine(ex);
            }

            finally
            {
                store.Close();
            }
            return added;
        }

        [WebMethod]
        public bool InstallCertificateRemote(X509Store store, X509Certificate2 certificate, string serverName)
        {

            //in this case we get a store passed in, so look inside and get the StoreLocation and Name values:
            StoreLocation location = store.Location;
            string newStoreName = store.Name.ToString();
            string storeName = null;
            System.Diagnostics.Debug.WriteLine("newStoreName = {0}", newStoreName);

            //make sure we aren't connecting to localhost, and got a good servername
            if (!serverName.ToUpper().Equals("LOCALHOST") && serverName.Length > 3)
            {
                // trying to concatenate the server name and store name for remote connection:
                storeName = string.Format(@"\\{0}\{1}", serverName, newStoreName);
            }
            else
            {
                //we didn't get a good server name - use local host for now by omitting the \\serverName
                storeName = string.Format("{0}", newStoreName);
            }

            X509Store newStore = new X509Store(storeName, location);

            bool added = false;
            try
            {
                newStore.Open(OpenFlags.ReadWrite);
                newStore.Add(certificate);
                added = true;
            }
            catch (Exception ex)
            {
                added = false;
                System.Diagnostics.Debug.WriteLine(ex);
            }

            finally
            {
                newStore.Close();
            }
            return added;
        }


        [WebMethod]
        public bool DeleteCertificate(string certificateName, string storeName, StoreLocation location)
        {
            bool success = false;

            X509Store store = new X509Store(storeName, location);
            try
            {
                store.Open(OpenFlags.ReadWrite);

                X509Certificate2Collection certificates =
                    store.Certificates.Find(X509FindType.FindBySubjectName, certificateName, true);

                if (certificates != null && certificates.Count > 0)
                {
                    store.RemoveRange(certificates);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                store.Close();
            }

            return success;
        }

        [WebMethod]
        public bool DeleteCertificateRemote(string certificateName, string storeName, StoreLocation location, string serverName)
        {
            bool success = false;

            string newStoreName = null;
            //make sure we aren't connecting to localhost, and got a good servername
            if (!serverName.ToUpper().Equals("LOCALHOST") && serverName.Length > 3)
            {
                // trying to concatenate the server name and store name for remote connection:
                newStoreName = string.Format(@"\\{0}\{1}", serverName, storeName);
            }
            else
            {
                //we didn't get a good server name - use local host for now
                newStoreName = string.Format("{0}", storeName);
            }

            X509Store store = new X509Store(newStoreName, location);
            try
            {
                store.Open(OpenFlags.ReadWrite);

                X509Certificate2Collection certificates =
                    store.Certificates.Find(X509FindType.FindBySubjectName, certificateName, true);

                if (certificates != null && certificates.Count > 0)
                {
                    store.RemoveRange(certificates);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                store.Close();
            }

            return success;
        }

        [WebMethod]
        public bool DeleteCertificateByThumbprint(string certificateName, string thumbprint, string storeName,
            StoreLocation location)
        {
            bool success = false;

            X509Store store = new X509Store(storeName, location);

            try
            {
                store.Open(OpenFlags.ReadWrite);

                System.Diagnostics.Debug.WriteLine("Calling EnumCertificates...", thumbprint);
                //EnumCertificates(storeName, location);

                X509Certificate2Collection certificates =
                    store.Certificates;

                //store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, true);

                //if (certificates != null && certificates.Count > 0)
                {
                    foreach (X509Certificate2 certificate in certificates)

                    {
                        int result = String.Compare(certificate.Thumbprint, thumbprint, new CultureInfo("en-US"),
                            CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreWidth | CompareOptions.IgnoreCase);

                        System.Diagnostics.Debug.WriteLine("result = {0}", result);

                        if (result != 0)
                        {
                            {
                                System.Diagnostics.Debug.WriteLine("certificate.Thumbprint: {0}", certificate.Thumbprint);
                                System.Diagnostics.Debug.WriteLine("does not equal");
                                System.Diagnostics.Debug.WriteLine("This cert thumbprint..: {0}", thumbprint);
                            }
                        }
                        else
                        {
                            store.Remove(certificate);
                            success = true;
                            break;
                        }
                        // else
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                store.Close();
            }

            return success;
        }


        [WebMethod]
        public bool RemoveCertificateLocal(X509Store store, X509Certificate2 certificate)
        {
            bool removed = false;
            try
            {
                store.Open(OpenFlags.ReadWrite);
                store.Remove(certificate);
                removed = true;
            }
            catch (Exception ex)
            {
                removed = false;
                System.Diagnostics.Debug.WriteLine(ex);
            }

            finally
            {
                store.Close();
            }

            return removed;
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }


        [WebMethod]
        public List<X509Certificate2> CompareCertificatesInStore(string storeName, StoreLocation storeLocation,
            string serverA, string serverB)
        {
            int certCount = 0;

            //get certificates in remote stores for both servers
            var storeListA = ListCertificatesInRemoteStore(storeName, storeLocation, serverA);
            var storeListB = ListCertificatesInRemoteStore(storeName, storeLocation, serverB);

            //compare the contents of serverA and serverB, storing the differences
            List<X509Certificate2> noMatchingCertListA = storeListA.Except(storeListB).ToList();
            List<X509Certificate2> noMatchingCertListB = storeListB.Except(storeListA).ToList();

            //this should be the list of certificates that are on one server but not the other
            var differencesList = noMatchingCertListA.Union(noMatchingCertListB).ToList();

            return differencesList;
        }
    }
}
