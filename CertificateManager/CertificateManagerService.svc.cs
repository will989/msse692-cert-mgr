using System;
using System.Collections.Generic;
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
                    System.Diagnostics.Debug.WriteLine("Signature Algorithm: {0}", certificate.SignatureAlgorithm.FriendlyName);
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
        public bool DeleteCertificateByThumbprint(string certificateName, string thumbprint, string storeName,
            StoreLocation location)
        {
            bool success = false;

            X509Store store = new X509Store(storeName, location);

            try
            {
                store.Open(OpenFlags.ReadWrite);

                System.Diagnostics.Debug.WriteLine("Calling EnumCertificates...",thumbprint);
                //EnumCertificates(storeName, location);

                X509Certificate2Collection certificates =
                    store.Certificates;

                //store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, true);

                //if (certificates != null && certificates.Count > 0)
                {
                    foreach (X509Certificate2 certificate in certificates)

                        {
                            if (certificate.Thumbprint.Equals(thumbprint))
                            {
                                store.Remove(certificate);
                                success = true;
                                break;
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("certificate.Thumbprint: {0}", certificate.Thumbprint);
                                System.Diagnostics.Debug.WriteLine("does not equal");
                                System.Diagnostics.Debug.WriteLine("This cert thumbprint..: {0}", thumbprint);

                            }
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
    }
}
