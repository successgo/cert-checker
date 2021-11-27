using System;
using System.Net.Http;

namespace CertChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "https://baidu.com";
            var httpClientHandler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (_, cert, _, _) =>
                {
                    Console.WriteLine(cert.Subject);
                    Console.WriteLine(cert.NotAfter);
                    return true;
                }
            };

            var httpClient = new HttpClient(httpClientHandler);
            httpClient.Send(new HttpRequestMessage(HttpMethod.Head, url));
        }
    }
}