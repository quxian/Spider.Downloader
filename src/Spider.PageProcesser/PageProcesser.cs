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
        private List<Thread> _threads = new List<Thread>();
        private bool _threadIsStop = false;

        public PageProcesser(int threadCount = 1) {
            _threadCount = threadCount;
        }

        public void DequeuePage() {
            while (!_threadIsStop) {
                if (PageQueue.IsEmpty)
                    continue;

                var page = string.Empty;
                PageQueue.TryDequeue(out page);
                if (string.Empty.Equals(page))
                    continue;
                ThreadPool.QueueUserWorkItem(new WaitCallback(x => {
                    var allUrls = page.FindAllUrls();
                    if (allUrls?.Count > 0) {
                        FindAllUrlsEvent?.Invoke(allUrls);
                        PipelineEvent?.Invoke(page);
                    }
                }));
            }
        }

        public void Run() {
            for (int i = 0; i < 1; i++) {
                _threads.Add(new Thread(DequeuePage));
            }
            _threads.ForEach(thread => thread.Start());
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

        public void Dispose() {
            _threadIsStop = true;
            _threads.ForEach(thread => thread.Join());
        }
    }
}
