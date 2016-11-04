using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spider {
    public class Downloader : IDownloader<string> {
        private static readonly HttpClient http = new HttpClient();
        private ConcurrentQueue<string> DawnloadPageUrlQueue = new ConcurrentQueue<string>();
        private event Action<string> DownloadPageEvent;
        private List<Task> tasks = new List<Task>();
        private SemaphoreSlim throttler;

        private int _threadCount;
        private List<Thread> _threads = new List<Thread>();
        private bool _threadIsStop = false;
        public Downloader(int threadCount = 1) {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _threadCount = threadCount;
        }
        private void DownloadPage() {
            while (!_threadIsStop) {
                if (DawnloadPageUrlQueue.IsEmpty)
                    continue;
                var url = string.Empty;
                DawnloadPageUrlQueue.TryDequeue(out url);
                if (string.Empty.Equals(url))
                    continue;

                ThreadPool.QueueUserWorkItem(new WaitCallback(async x => {
                    try {
                        var page = await http.GetStringAsync(url);
                        DownloadPageEvent?.Invoke(page);
                    } catch (Exception e) {
                        Console.WriteLine(new { url = url, Exception = e });
                    }
                }));

            }
        }

        public void Run() {
            for (int i = 0; i < 1; i++) {
                _threads.Add(new Thread(DownloadPage));
            }
            _threads.ForEach(thread => thread.Start());
        }

        public void AddUrl(string url) {
            DawnloadPageUrlQueue.Enqueue(url);
        }

        public void AddDownloadPageEventListens(Action<string> action) {
            DownloadPageEvent += action;
        }

        public void Dispose() {
            _threadIsStop = true;
            _threads.ForEach(thread => thread.Join());
            
            http.Dispose();
        }
    }
}