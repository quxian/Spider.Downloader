using HtmlAgilityPack;
using Spider.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Spider.ProcessDOMPipeline {
    public class ProcessDOMPipeline : IPipeline<DownloadResult, HtmlDocument> {
        public event Action<HtmlDocument> Next;
        private event Action onDispose;

        public void Dispose() {

            onDispose?.Invoke();
        }

        public void Extract(DownloadResult page) {
            var htmlDOM = new HtmlDocument();
            htmlDOM.LoadHtml(page.Page);
            if (null == Next)
                return;
            Next(htmlDOM);
        }

        public IPipeline<DownloadResult, HtmlDocument> NextPipeline<V>(IPipeline<HtmlDocument, V> nextPipeline) {
            Next += nextPipeline.Extract;
            onDispose += nextPipeline.Dispose;
            return this;
        }
    }
}
