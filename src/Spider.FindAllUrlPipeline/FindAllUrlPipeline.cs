using Extend;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider {
    public class FindAllUrlPipeline : IPipeline<HtmlDocument, List<string>> {

        public event Action<List<string>> Next;

        public void Extract(HtmlDocument page) {
            var urls = page.FindAllUrls();
            if (null == Next || null == urls || urls.Count == 0)
                return;
            Next(urls);
        }

        public IPipeline<HtmlDocument, List<string>> NextPipeline<V>(IPipeline<List<string>, V> nextPipeline) {
            Next += nextPipeline.Extract;

            return this;
        }
    }
}
