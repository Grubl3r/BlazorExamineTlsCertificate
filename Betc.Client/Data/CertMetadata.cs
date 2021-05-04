using System;

namespace Betc.Client.Data
{
    public class CertMetadata
    {
        public string Subject { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
        public string Thumbprint { get; set; }

        public override string ToString()
        {
            return $"Subject: '{Subject}'\nExpires: {ExpiryDate.ToString("s")}\nThumb.: {Thumbprint}";
        }
    }
}
