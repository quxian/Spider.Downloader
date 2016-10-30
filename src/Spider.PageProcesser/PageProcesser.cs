using Extend;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;

namespace Spider {
    public class PageProcesser : IPageProcesser<string> {
        private event Action<List<string>> FindAllUrlsEvent;
        private event Action<string> PipelineEvent;
        private ConcurrentQueue<string> PageQueue = new ConcurrentQueue<string>();

        private int _threadCount;

        public PageProcesser(int threadCount = 1) {
            _threadCount = threadCount;
        }

        public void DequeuePage() {
            while (true) {
                if (PageQueue.IsEmpty)
                    continue;

                var page = string.Empty;
                PageQueue.TryDequeue(out page);
                if (string.Empty.Equals(page))
                    continue;

                var allUrls = page.FindAllUrls();
                if (allUrls?.Count > 0)
                    FindAllUrlsEvent(allUrls);
                PipelineEvent(page);
            }
        }

        public void Run() {
            for (int i = 0; i < _threadCount; i++) {
                new Thread(DequeuePage).Start();
            }
        }

        public void AddPage(string page) {
            PageQueue.Enqueue(page);
        }

        public void AddFindAllUrlsEventListens(Action<List<string>> action) {
            FindAllUrlsEvent += action;
        }

        public void AddPipelineEventListens(Action<string> action) {
            PipelineEvent += action;
        }
    }
}
