using System;
using System.Collections.Generic;

using HtmlAgilityPack;

namespace Detektor.Core
{
    public interface IDetektor
    {
        string GetTitle();
    }

    public class Detektor : IDetektor
    {
        private readonly IWebClientWrapper _webClient;

        private readonly string _address;

        private readonly Dictionary<string, string> _optionalHeaders;
        
        public Detektor(IWebClientWrapper webClient, string address)
        {
            _webClient = webClient;
            _address = address;
            _optionalHeaders = new Dictionary<string, string> { { "Content-Type", "application/xml" } };
        }

        public string GetTitle()
        {
            try
            {
                var content = _webClient.DownloadString(_address, _optionalHeaders);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(content);
                var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='nowplaying nowplaying-wortstream white']");
                var artist = node.SelectSingleNode("//strong[@class='streaming-artist']");
                var wordmarquee = node.SelectSingleNode("//span[@id='wordmarquee']");
                return $"{artist.InnerText}\n{wordmarquee.InnerText}";
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }
    }
}
