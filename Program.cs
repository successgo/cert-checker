using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace CertChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            var certs = new List<X509Certificate2>();

            var httpClientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (_, cert, _, _) =>
                {
                    certs.Add(cert);
                    return true;
                }
            };

            var httpClient = new HttpClient(httpClientHandler);

            foreach (var s in args)
            {
                string url = s;
                httpClient.Send(new HttpRequestMessage(HttpMethod.Head, url));
            }

            var comparison = new Comparison<X509Certificate2>((a, b) =>
            {
                if (a.NotAfter > b.NotAfter)
                {
                    return 1;
                }

                if (a.NotAfter < b.NotAfter)
                {
                    return -1;
                }

                return 0;
            });
            certs.Sort(comparison);

            foreach (var cert in certs)
            {
                Console.Write(cert.NotAfter);
                Console.Write(", ");
                Console.WriteLine(cert.Subject);
            }
        }
    }
}
