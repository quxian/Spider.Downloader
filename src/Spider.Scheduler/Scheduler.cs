using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Spider {
    public class Scheduler : IScheduler {
        private ConcurrentQueue<string> UrlQueue = new ConcurrentQueue<string>();
        private HashSet<string> UrlSet = new HashSet<string>();

        private event Action<string> UrlDequeueEvent;

        public void AddUrlDequeueEventListens(Action<string> action) {
            UrlDequeueEvent += action;
        }

        public void AddUrls(List<string> urls) {
            urls.ForEach(url => UrlQueue.Enqueue(url));
        }

        public void Run() {
            new Thread(UrlDequeue).Start();
        }

        private void UrlDequeue() {
            while (true) {
                if (UrlQueue.IsEmpty)
                    continue;
                var url = string.Empty;
                UrlQueue.TryDequeue(out url);
                if (string.Empty.Equals(url))
                    continue;
                if (UrlSet.Contains(url))
                    continue;
                UrlSet.Add(url);
                UrlDequeueEvent(url);
            }
        }
    }
}
