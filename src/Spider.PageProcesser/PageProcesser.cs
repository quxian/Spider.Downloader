using Extend;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;

namespace Spider {
    public class PageProcesser : IPageProcesser<HttpResponseMessage> {
        private event Action<List<string>> FindAllUrlsEvent;
        private event Action<HttpResponseMessage> PipelineEvent;
        private ConcurrentQueue<HttpResponseMessage> PageQueue = new ConcurrentQueue<HttpResponseMessage>();

        private int _threadCount;

        public PageProcesser(int threadCount = 1) {
            _threadCount = threadCount;
        }

        public void DequeuePage() {
            while (true) {
                if (PageQueue.IsEmpty)
                    continue;

                HttpResponseMessage page = null;
                PageQueue.TryDequeue(out page);
                if (null == page)
                    continue;

                var allUrls = page.Content.ReadAsStringAsync().Result.FindAllUrs();
                FindAllUrlsEvent(allUrls);
                PipelineEvent(page);
            }
        }

        public void Run() {
            for (int i = 0; i < _threadCount; i++) {
                new Thread(DequeuePage).Start();
            }
        }

        public void AddPage(HttpResponseMessage page) {
            PageQueue.Enqueue(page);
        }

        public void AddFindAllUrlsEventListens(Action<List<string>> action) {
            FindAllUrlsEvent += action;
        }

        public void AddPipelineEventListens(Action<HttpResponseMessage> action) {
            PipelineEvent += action;
        }
    }
}
