using System.Collections.Generic;
using System.ServiceModel;
using CertificateManager.Data.Entities;

namespace CertificateManager.Data.Services
{

    [ServiceContract]
    public interface IEntityService<T> where T: IMongoEntity
    {
        [OperationContract]
        void Create(T entity);

        [OperationContract]
        void Delete(string id);

        [OperationContract]
        T GetByName(string name);

        [OperationContract]
        IEnumerable<Certificate> GetCertificateDetails();

        [OperationContract]
        T GetById(string id);
        
        [OperationContract]
        void Update(T entity);

        [OperationContract]
        void DoWork();
    }
}
   

