using Extend;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using Spider.Model;

namespace Spider {
    public class PageProcesser : IPageProcesser<DownloadResult> {
        private event Action<List<string>> FindAllUrlsEvent;
        public event Action<DownloadResult> PipelineEvent;
        private ConcurrentQueue<DownloadResult> PageQueue = new ConcurrentQueue<DownloadResult>();

        private int _threadCount;
        private List<Thread> _threads = new List<Thread>();
        private bool _threadIsStop = false;

        private SpinLock _spinLcok = new SpinLock(false);

        public PageProcesser(int threadCount = 1) {
            _threadCount = threadCount;
        }

        public void DequeuePage() {
            while (!_threadIsStop) {
                if (PageQueue.IsEmpty)
                    continue;
                var lockTake = false;

                ThreadPool.QueueUserWorkItem(new WaitCallback(x => {
                    try {
                        _spinLcok.TryEnter(ref lockTake);
                        DownloadResult page = null;
                        PageQueue.TryDequeue(out page);
                        if (null == page)
                            return;
                        var allUrls = page.Page.FindAllUrls();
                        if (allUrls?.Count > 0) {
                            FindAllUrlsEvent?.Invoke(allUrls);
                            PipelineEvent?.Invoke(page);
                        }
                    } finally {
                        if (lockTake)
                            _spinLcok.Exit(false);
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

        public void AddPage(DownloadResult page) {
            PageQueue.Enqueue(page);
        }

        public void AddFindAllUrlsEventListens(Action<List<string>> action) {
            FindAllUrlsEvent += action;
        }

        public void AddPipelineEventListens(Action<DownloadResult> action) {
            PipelineEvent += action;
        }

        public void Dispose() {
            _threadIsStop = true;
            _threads.ForEach(thread => thread.Join());
        }
    }
}
