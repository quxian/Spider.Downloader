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
        private readonly HttpClient http = new HttpClient();
        private ConcurrentQueue<string> DawnloadPageUrlQueue = new ConcurrentQueue<string>();
        private event Action<string> DownloadPageEvent;

        private int _threadCount;
        public Downloader(int threadCount = 1) {
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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
                try {
                    var page = await http.GetStringAsync(url);
                    DownloadPageEvent(page);
                } catch (Exception e) {
                    Console.WriteLine(new { url = url, Exception = e });
                }

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

        public void AddDownloadPageEventListens(Action<string> action) {
            DownloadPageEvent += action;
        }
    }
}