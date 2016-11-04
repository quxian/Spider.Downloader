using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spider {
    public class WriteUrlsToFilePileline : IPipeline<List<string>, List<string>> {
        private readonly string _filePath;
        private readonly ConcurrentQueue<string> _urlsQueue = new ConcurrentQueue<string>();
        private long _urlsCount = 0;
        private object _lock = new object();
        private StreamWriter streamWriter;
        private StringBuilder sb = new StringBuilder(1024 * 1024);
        private event Action onDispose;

        public event Action<List<string>> Next;

        public WriteUrlsToFilePileline(string filePath) {
            _filePath = filePath;
            streamWriter = new StreamWriter(new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.Write));
        }


        public void Extract(List<string> urls) {
            urls.ForEach(url => _urlsQueue.Enqueue(url));
            if (_urlsQueue.Count > 10000) {
                ThreadPool.QueueUserWorkItem(new WaitCallback(WriteUrlsToFile));
            }

            Next?.Invoke(urls);
        }

        public IPipeline<List<string>, List<string>> NextPipeline<V>(IPipeline<List<string>, V> nextPipeline) {
            Next += nextPipeline.Extract;
            onDispose += nextPipeline.Dispose;

            return this;
        }

        private void WriteUrlsToFile(object obj) {
            lock (_lock) {
                while (true) {
                    if (_urlsQueue.IsEmpty)
                        break;
                    var url = string.Empty;
                    _urlsQueue.TryDequeue(out url);
                    if (string.Empty.Equals(url) || null == url)
                        continue;
                    sb.AppendLine($"{++_urlsCount}:{url}");
                }
                streamWriter.Write(sb.ToString());
                streamWriter.Flush();

                sb.Clear();
            }
        }

        public void Dispose() {

            WriteUrlsToFile(null);

            streamWriter.Dispose();

            onDispose?.Invoke();
        }
    }
}
