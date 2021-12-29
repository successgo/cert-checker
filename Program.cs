using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CertChecker
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Wait a moment...");
            
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

            var tasks = new List<Task>();
            foreach (var url in args)
            {
                var task = Task.Run(() => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url)));
                tasks.Add(task);
            }

            // Wait all async tasks
            foreach (var task in tasks)
            {
                await task;
            }

            // Distinct
            var certs2 = certs.Distinct().ToList();

            // Sort by NotAfter
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
            certs2.Sort(comparison);

            foreach (var cert in certs2)
            {
                Console.WriteLine("{0:MMM dd yyyy} {1}", cert.NotAfter, cert.Subject);
            }
            
            Console.WriteLine("Done!");
        }
    }
}
