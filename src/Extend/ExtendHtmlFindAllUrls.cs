using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Extend {
    public static class ExtendHtmlFindAllUrls {
        public static List<string> FindAllUrls(this string page) {
            var doc = new HtmlDocument();
            doc.LoadHtml(page);

            return _FindAllUrls(doc);
        }

        public static List<string> FindAllUrls(this HtmlDocument doc) {
            return _FindAllUrls(doc);
        }

        private static List<string> _FindAllUrls(HtmlDocument doc) {
            var urls = new List<string>();
            var links = doc.DocumentNode.SelectNodes("//a[@href]");
            if (null == links)
                return null;
            foreach (var link in links) {
                var hrefValue = link.Attributes["href"].Value;
                if (hrefValue.Contains("http"))
                    urls.Add(hrefValue);
            }

            return urls;
        }
    }
}
