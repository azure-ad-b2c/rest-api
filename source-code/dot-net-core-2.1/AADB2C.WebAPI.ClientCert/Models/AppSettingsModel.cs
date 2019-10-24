using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AADB2C.WebAPI.ClientCert.Models
{
    public class AppSettingsModel
    {
        public string ClientCertificateSubject { get; set; }
        public string ClientCertificateIssuer { get; set; }
        public string ClientCertificateThumbprint { get; set; }
    }
}
