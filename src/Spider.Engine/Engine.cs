using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Spider {
    public class Engine {
        private readonly IDownloader<string> downloader;
        private readonly IPageProcesser<string> pageProcesser;
        private readonly IScheduler scheduler;

        public Engine(
            IDownloader<string> downloader,
            IPageProcesser<string> pageProcesser,
            IScheduler scheduler
            ) {
            this.downloader = downloader;
            this.pageProcesser = pageProcesser;
            this.scheduler = scheduler;
        }

        public void Run() {
            new Thread(EngineCore).Start();
        }

        public Engine AddUrls(string bootstrapUrl) {
            downloader.AddUrl(bootstrapUrl);
            return this;
        }

        public Engine AddPipeline<T>(IPipeline<string, T> pipeline) {
            pageProcesser.AddPipelineEventListens(pipeline.Extract);
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
    }
}
