using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Spider {
    public class Downloader : IDownloader {
        private readonly HttpClient http = new HttpClient();
        private ConcurrentQueue<string> DawnloadPageUrlQueue = new ConcurrentQueue<string>();
        private event Action<string> DownloadPageEvent;
     
        private async void DownloadPage() {
            while (true) {
                if (DawnloadPageUrlQueue.IsEmpty)
                    continue;
                var url = string.Empty;
                DawnloadPageUrlQueue.TryDequeue(out url);
                if (string.Empty.Equals(url))
                    continue;
                var result = await http.GetStringAsync(url);
                DownloadPageEvent(result);
            }
        }

        public void Run() {
            new Thread(DownloadPage).Start();
        }

        public void AddUrl(string url) {
            DawnloadPageUrlQueue.Enqueue(url);
        }

        public void AddDownloadPageEventListens(Action<string> action) {
            DownloadPageEvent += action;
        }
    }
}