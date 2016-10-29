using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Spider.ProcessDOMPipeline {
    public class ProcessDOMPipeline : IPipeline<HttpResponseMessage, HtmlDocument> {
        public event Action<HtmlDocument> Next;

        public void Extract(HttpResponseMessage page) {
            var htmlDOM = new HtmlDocument();
            htmlDOM.LoadHtml(page.Content.ReadAsStringAsync().Result);
            if (null == Next)
                return;
            Next(htmlDOM);
        }

        public IPipeline<HttpResponseMessage, HtmlDocument> NextPipeline<V>(IPipeline<HtmlDocument, V> nextPipeline) {
            Next += nextPipeline.Extract;

            return this;
        }
    }
}
