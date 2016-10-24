using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Spider.Downloader {
    public class Downloader {
        private readonly HttpClient http;
        public ConcurrentQueue<string> DawnloadPageUrlQueue = new ConcurrentQueue<string>();
        public event Action<string> OnDownloadPage;
        public Downloader() {
            http = new HttpClient();
            
        }
        public void Start() {
            DownloadPage();
        }
        private async void DownloadPage() {
            while (true) {
                if (!DawnloadPageUrlQueue.IsEmpty) {
                    var url = string.Empty;
                    DawnloadPageUrlQueue.TryDequeue(out url);
                    if (string.Empty.Equals(url))
                        continue;
                    var result = await http.GetStringAsync(url);

                    OnDownloadPage(result);
                }
            }

            
        }
    }
}