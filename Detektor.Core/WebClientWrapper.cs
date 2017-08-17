using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Detektor.Core
{
    public interface IWebClientWrapper
    {
        string DownloadString(string address, IDictionary<string, string> optionalHeaders);
    }

    public class WebClientWrapper : IWebClientWrapper
    {
        public string DownloadString(string address, IDictionary<string, string> optionalHeaders)
        {
            using (var webClient = new WebClient())
            {
                foreach (var header in optionalHeaders)
                    webClient.Headers.Set(header.Key, header.Value);

                var data = webClient.DownloadData(address);
                return Encoding.UTF8.GetString(data);
            }
        }
    }
}
