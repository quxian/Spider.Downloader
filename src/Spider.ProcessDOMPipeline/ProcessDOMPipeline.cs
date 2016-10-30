using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Spider.ProcessDOMPipeline {
    public class ProcessDOMPipeline : IPipeline<string, HtmlDocument> {
        public event Action<HtmlDocument> Next;

        public void Extract(string page) {
            var htmlDOM = new HtmlDocument();
            htmlDOM.LoadHtml(page);
            if (null == Next)
                return;
            Next(htmlDOM);
        }

        public IPipeline<string, HtmlDocument> NextPipeline<V>(IPipeline<HtmlDocument, V> nextPipeline) {
            Next += nextPipeline.Extract;

            return this;
        }
    }
}
