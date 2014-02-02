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

            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;

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
