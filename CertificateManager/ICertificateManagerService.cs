﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CertificateManager
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ICertificateManagerService
    {

        [OperationContract]
        List<X509Certificate2> ListCertificatesInStore(string storeName, StoreLocation storeLocation);

        [OperationContract]
        List<X509Certificate2> ListCertificatesInRemoteStore(string storeName, StoreLocation storeLocation,
            string serverName);

        [OperationContract]
        List<X509Certificate2> ListExpiringCertificatesInStore(string storeName, StoreLocation storeLocation, int days);

        [OperationContract]
        void PrintCertificateInfo(X509Certificate2 certificate);

        [OperationContract]
        void EnumCertificatesByStoreName(StoreName name, StoreLocation location);

        [OperationContract]
        void EnumCertificates(string name, StoreLocation location);

        [OperationContract]
        bool InstallCertificateLocal(X509Store store, X509Certificate2 certificate);

        [OperationContract]
        bool DeleteCertificate(string certificateName, string storeName, StoreLocation location);

        [OperationContract]
        bool DeleteCertificateByThumbprint(string certificateName, string thumbprint, string storeName,
            StoreLocation location);

        [OperationContract]
        bool RemoveCertificateLocal(X509Store store, X509Certificate2 certificate);

        [OperationContract]
        List<X509Certificate2> CompareCertificatesInStore(string storeName, StoreLocation storeLocation,
            string serverA, string serverB);

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
