using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CertificateManager.Data.Entities;

namespace CertificateManager
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICertificateWarehouse" in both code and config file together.
    [ServiceContract]
    public interface ICertificateWarehouseService
    {
        [OperationContract]
        bool AddCertificateToOrganization(string organizationId, OrganizationCertificate organizationCertificate);

        [OperationContract]
        bool AddOrganizationToDatabase(Organization organization);

        [OperationContract]
        bool AddCertificateToDatabase(Certificate certificate);

        [OperationContract]
        bool RemoveCertificateFromDatabase(String thumbprint);

        [OperationContract]
        Certificate GetCertificateById(string certificateId);

        [OperationContract]
        Certificate GetCertificateByName(string name);

        [OperationContract]
        Certificate GetCertificateByThumbprint(string thumbprint);

        [OperationContract]
        IEnumerable<Certificate> GetCertificatesDetails(int limit, int skip);

        [OperationContract]
        IEnumerable<Certificate> GetCertificatesByExpirationDate(int futureDays);

        [OperationContract]
        void DoWork();
    }
}
