using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider {
    public class FindAllUrlPipeline : IPipeline<HtmlDocument, List<string>> {

        public event Action<List<string>> Next;

        public void Extract(HtmlDocument page) {
            var urls = new List<string>();
            var links = page.DocumentNode.SelectNodes("//a[@href]");
            if (null == links)
                return;
            foreach (var link in links) {
                var hrefValue = link.Attributes["href"].Value;
                urls.Add(hrefValue);
                Console.WriteLine(hrefValue);
            }

            if (null == Next)
                return;
            Next(urls);
        }

        public IPipeline<HtmlDocument, List<string>> NextPipeline<V>(IPipeline<List<string>, V> nextPipeline) {
            Next += nextPipeline.Extract;

            return this;
        }
    }
}
