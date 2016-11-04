using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Spider {
    public class Scheduler : IScheduler {
        private ConcurrentQueue<string> UrlQueue = new ConcurrentQueue<string>();
        private ConcurrentDictionary<string, bool> UrlSet = new ConcurrentDictionary<string, bool>();
        private event Action<string> UrlDequeueEvent;

        private int _threadCount;
        private List<Thread> _threads = new List<Thread>();
        private bool _threadIsStop = false;

        public Scheduler(int threadCount = 1) {
            _threadCount = threadCount;
        }

        public void AddUrlDequeueEventListens(Action<string> action) {
            UrlDequeueEvent += action;
        }

        public void AddUrls(List<string> urls) {
            urls.ForEach(url => UrlQueue.Enqueue(url));
        }

        public void Run() {
            for (int i = 0; i < 1; i++) {
                _threads.Add(new Thread(UrlDequeue));
            }
            _threads.ForEach(thread => thread.Start());
        }

        private void UrlDequeue() {
            while (!_threadIsStop) {
                if (UrlQueue.IsEmpty)
                    continue;
                var url = string.Empty;
                UrlQueue.TryDequeue(out url);
                if (string.Empty.Equals(url) || null == url)
                    continue;
                if (UrlSet.ContainsKey(url))
                    continue;
                UrlSet.TryAdd(url, true);

                UrlDequeueEvent?.Invoke(url);
            }
        }

        public void Dispose() {
            _threadIsStop = true;
            _threads.ForEach(thread => thread.Join());
        }
    }
}
