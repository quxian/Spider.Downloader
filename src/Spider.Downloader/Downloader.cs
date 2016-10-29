using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Spider {
    public class Downloader : IDownloader<HttpResponseMessage> {
        private readonly HttpClient http = new HttpClient();
        private ConcurrentQueue<string> DawnloadPageUrlQueue = new ConcurrentQueue<string>();
        private event Action<HttpResponseMessage> DownloadPageEvent;

        private int _threadCount;
        public Downloader(int threadCount = 1) {
            _threadCount = threadCount;
        }
        private async void DownloadPage() {
            while (true) {
                if (DawnloadPageUrlQueue.IsEmpty)
                    continue;
                var url = string.Empty;
                DawnloadPageUrlQueue.TryDequeue(out url);
                if (string.Empty.Equals(url))
                    continue;
                var httpResponseMessage = await http.GetAsync(url);
                if (!httpResponseMessage.IsSuccessStatusCode)
                    return;
                DownloadPageEvent(httpResponseMessage);
            }
        }

        public void Run() {
            for (int i = 0; i < _threadCount; i++) {
                new Thread(DownloadPage).Start();
            }
        }

        public void AddUrl(string url) {
            DawnloadPageUrlQueue.Enqueue(url);
        }

        public void AddDownloadPageEventListens(Action<HttpResponseMessage> action) {
            DownloadPageEvent += action;
        }
    }
}