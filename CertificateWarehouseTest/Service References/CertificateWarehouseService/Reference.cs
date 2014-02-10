﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CertificateWarehouseTest.CertificateWarehouseService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CertificateWarehouseService.ICertificateWarehouseService")]
    public interface ICertificateWarehouseService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICertificateWarehouseService/AddCertificateToOrganization", ReplyAction="http://tempuri.org/ICertificateWarehouseService/AddCertificateToOrganizationRespo" +
            "nse")]
        bool AddCertificateToOrganization(string organizationId, CertificateManager.Data.Entities.OrganizationCertificate organizationCertificate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICertificateWarehouseService/AddOrganizationToDatabase", ReplyAction="http://tempuri.org/ICertificateWarehouseService/AddOrganizationToDatabaseResponse" +
            "")]
        bool AddOrganizationToDatabase(CertificateManager.Data.Entities.Organization organization);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICertificateWarehouseService/AddCertificateToDatabase", ReplyAction="http://tempuri.org/ICertificateWarehouseService/AddCertificateToDatabaseResponse")]
        bool AddCertificateToDatabase(CertificateManager.Data.Entities.Certificate certificate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICertificateWarehouseService/DoWork", ReplyAction="http://tempuri.org/ICertificateWarehouseService/DoWorkResponse")]
        void DoWork();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICertificateWarehouseServiceChannel : CertificateWarehouseTest.CertificateWarehouseService.ICertificateWarehouseService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CertificateWarehouseServiceClient : System.ServiceModel.ClientBase<CertificateWarehouseTest.CertificateWarehouseService.ICertificateWarehouseService>, CertificateWarehouseTest.CertificateWarehouseService.ICertificateWarehouseService {
        
        public CertificateWarehouseServiceClient() {
        }
        
        public CertificateWarehouseServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CertificateWarehouseServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CertificateWarehouseServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CertificateWarehouseServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool AddCertificateToOrganization(string organizationId, CertificateManager.Data.Entities.OrganizationCertificate organizationCertificate) {
            return base.Channel.AddCertificateToOrganization(organizationId, organizationCertificate);
        }
        
        public bool AddOrganizationToDatabase(CertificateManager.Data.Entities.Organization organization) {
            return base.Channel.AddOrganizationToDatabase(organization);
        }
        
        public bool AddCertificateToDatabase(CertificateManager.Data.Entities.Certificate certificate) {
            return base.Channel.AddCertificateToDatabase(certificate);
        }
        
        public void DoWork() {
            base.Channel.DoWork();
        }
    }
}
