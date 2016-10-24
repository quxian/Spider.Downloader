using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Spider.Scheduler {
    public class Scheduler {
        public ConcurrentQueue<string> UrlQueue = new ConcurrentQueue<string>();
        private HashSet<string> UrlSet = new HashSet<string>();

        public event Action<string> OnUrlDequeue;

        public void Start() {
            UrlDequeue();
        }

        private void UrlDequeue() {
            new Thread(() => {
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
                    OnUrlDequeue(url);
                }
            }).Start();
        }
    }
}
