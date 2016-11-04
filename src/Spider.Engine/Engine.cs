using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Spider {
    public class Engine : IDisposable {
        private readonly IDownloader<string> downloader;
        private readonly IPageProcesser<string> pageProcesser;
        private readonly IScheduler scheduler;
        private event Action onDispose;
        public Engine(
            IDownloader<string> downloader,
            IPageProcesser<string> pageProcesser,
            IScheduler scheduler
            ) {
            this.downloader = downloader;
            this.pageProcesser = pageProcesser;
            this.scheduler = scheduler;
        }

        public Engine Run() {
            new Thread(EngineCore).Start();
            return this;
        }

        public Engine AddUrls(string bootstrapUrl) {
            downloader.AddUrl(bootstrapUrl);
            return this;
        }

        public Engine AddPipeline<T>(IPipeline<string, T> pipeline) {
            pageProcesser.AddPipelineEventListens(pipeline.Extract);
            onDispose += pipeline.Dispose;
            return this;
        }

        private void EngineCore() {
            downloader.AddDownloadPageEventListens(page => {
                pageProcesser.AddPage(page);
            });

            pageProcesser.AddFindAllUrlsEventListens(urls => {
                scheduler.AddUrls(urls);
            });

            scheduler.AddUrlDequeueEventListens(url => {
                downloader.AddUrl(url);
            });

            downloader.Run();
            pageProcesser.Run();
            scheduler.Run();
        }

        public void Dispose() {
            downloader.Dispose();
            pageProcesser.Dispose();
            scheduler.Dispose();

            onDispose?.Invoke();
        }
    }
}
