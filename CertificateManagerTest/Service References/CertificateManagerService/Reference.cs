﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CertificateManagerTest.CertificateManagerService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CompositeType", Namespace="http://schemas.datacontract.org/2004/07/CertificateManager")]
    [System.SerializableAttribute()]
    public partial class CompositeType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool BoolValueField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StringValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool BoolValue {
            get {
                return this.BoolValueField;
            }
            set {
                if ((this.BoolValueField.Equals(value) != true)) {
                    this.BoolValueField = value;
                    this.RaisePropertyChanged("BoolValue");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StringValue {
            get {
                return this.StringValueField;
            }
            set {
                if ((object.ReferenceEquals(this.StringValueField, value) != true)) {
                    this.StringValueField = value;
                    this.RaisePropertyChanged("StringValue");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CertificateManagerService.ICertificateManagerService")]
    public interface ICertificateManagerService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICertificateManagerService/ListCertificatesInStore", ReplyAction="http://tempuri.org/ICertificateManagerService/ListCertificatesInStoreResponse")]
        System.Security.Cryptography.X509Certificates.X509Certificate2[] ListCertificatesInStore(string storeName, System.Security.Cryptography.X509Certificates.StoreLocation storeLocation);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICertificateManagerService/InstallCertificateLocal", ReplyAction="http://tempuri.org/ICertificateManagerService/InstallCertificateLocalResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Security.Cryptography.X509Certificates.StoreLocation))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Security.Cryptography.X509Certificates.X509Certificate2[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Security.Cryptography.X509Certificates.X509Certificate2))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(System.Security.Cryptography.X509Certificates.X509Certificate))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(CertificateManagerTest.CertificateManagerService.CompositeType))]
        bool InstallCertificateLocal(System.Security.Cryptography.X509Certificates.X509Store store, System.Security.Cryptography.X509Certificates.X509Certificate2 certificate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICertificateManagerService/GetData", ReplyAction="http://tempuri.org/ICertificateManagerService/GetDataResponse")]
        string GetData(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICertificateManagerService/GetDataUsingDataContract", ReplyAction="http://tempuri.org/ICertificateManagerService/GetDataUsingDataContractResponse")]
        CertificateManagerTest.CertificateManagerService.CompositeType GetDataUsingDataContract(CertificateManagerTest.CertificateManagerService.CompositeType composite);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICertificateManagerServiceChannel : CertificateManagerTest.CertificateManagerService.ICertificateManagerService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CertificateManagerServiceClient : System.ServiceModel.ClientBase<CertificateManagerTest.CertificateManagerService.ICertificateManagerService>, CertificateManagerTest.CertificateManagerService.ICertificateManagerService {
        
        public CertificateManagerServiceClient() {
        }
        
        public CertificateManagerServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CertificateManagerServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CertificateManagerServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CertificateManagerServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Security.Cryptography.X509Certificates.X509Certificate2[] ListCertificatesInStore(string storeName, System.Security.Cryptography.X509Certificates.StoreLocation storeLocation) {
            return base.Channel.ListCertificatesInStore(storeName, storeLocation);
        }
        
        public bool InstallCertificateLocal(System.Security.Cryptography.X509Certificates.X509Store store, System.Security.Cryptography.X509Certificates.X509Certificate2 certificate) {
            return base.Channel.InstallCertificateLocal(store, certificate);
        }
        
        public string GetData(int value) {
            return base.Channel.GetData(value);
        }
        
        public CertificateManagerTest.CertificateManagerService.CompositeType GetDataUsingDataContract(CertificateManagerTest.CertificateManagerService.CompositeType composite) {
            return base.Channel.GetDataUsingDataContract(composite);
        }
    }
}
