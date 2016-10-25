using Extend;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Spider {
    public class PageProcesser : IPageProcesser {
        private event Action<List<string>> FindAllUrlsEvent;
        private event Action<string> PipelineEvent;
        private ConcurrentQueue<string> PageQueue = new ConcurrentQueue<string>();

        public void DequeuePage() {
            while (true) {
                if (PageQueue.IsEmpty)
                    continue;

                var page = string.Empty;
                PageQueue.TryDequeue(out page);
                if (string.Empty.Equals(page))
                    continue;

                var allUrls = page.FindAllUrs();
                FindAllUrlsEvent(allUrls);
                PipelineEvent(page);
            }
        }

        public void Run() {
            new Thread(DequeuePage).Start();
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
