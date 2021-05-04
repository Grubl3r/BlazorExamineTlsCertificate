using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Betc.Client.Data
{
    public class CertificateCatcher
    {
        private object sync = new object();
        private Dictionary<string, CertMetadata> certCatches = new Dictionary<string, CertMetadata>();

        public IReadOnlyList<CertMetadata> Certificates => certCatches.Values.ToList();

        /// <summary>
        /// Event handler for HttpClientHandler.ServerCertificateCustomValidationCallback
        /// </summary>
        public bool Validate(HttpRequestMessage request, X509Certificate2 cert, X509Chain certChain, SslPolicyErrors policyErrors)
        {
            var certThumbprint = cert.Thumbprint.Trim().ToLowerInvariant();

            lock (sync)
            {
                if (!certCatches.ContainsKey(certThumbprint))
                {
                    certCatches[certThumbprint] = new CertMetadata
                    {
                        Subject = cert.Subject,
                        Thumbprint = certThumbprint,
                        ExpiryDate = new DateTimeOffset(cert.NotAfter)
                    };
                }
            }
            return cert.Verify(); // is there a better way?
        }

        public override string ToString()
        {
            lock (sync)
            {
                return string.Join("\n----\n", certCatches.Values.Select(c => c.ToString())) + "\n";
            }
        }
    }
}
