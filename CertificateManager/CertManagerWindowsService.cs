using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Web;

namespace CertificateManager
{
    public class CertManagerWindowsService : ServiceBase
    {
        public ServiceHost serviceHost = null;

        public CertManagerWindowsService()
        {
            // Name the Windows Service
            ServiceName = "CertManagerWindowsService";
        }

        public static void Main()
        {
            ServiceBase.Run(new CertManagerWindowsService());
        }

        // Start the Windows service.
        protected override void OnStart(string[] args)
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }

            // Create a ServiceHost for the CalculatorService type and 
            // provide the base address.
            serviceHost = new ServiceHost(typeof (CertManagerWindowsService));

            // Open the ServiceHostBase to create listeners and start 
            // listening for messages.
            serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }

    }
}