using Spider.Downloader;
using Spider.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestSpiderDownloader {
    public class Program {
        public static void Main(string[] args) {
            TestDownloaderModule();
            TestSchedulerModule();

        }

        private static void TestSchedulerModule() {
            var scheduler = new Scheduler();
            scheduler.OnUrlDequeue += x => {
                Console.WriteLine(x);
            };

            for (int i = 0; i < 10; i++) {
                scheduler.UrlQueue.Enqueue($"url: {i}");
                scheduler.UrlQueue.Enqueue($"url: {i}");

            }
            Console.WriteLine(scheduler.UrlQueue.Count);

            scheduler.Start();

            Console.ReadLine();
        }

        private static void TestDownloaderModule() {
            var spiderDownloader = new Downloader();
            spiderDownloader.OnDownloadPage += x => {
                Console.WriteLine(x);
                Console.WriteLine("-----------------------------------------------------------");

            };

            spiderDownloader.DawnloadPageUrlQueue.Enqueue("https://www.baidu.com/");

            spiderDownloader.Start();

            spiderDownloader.DawnloadPageUrlQueue.Enqueue("http://www.cnblogs.com/quark/archive/2012/03/19/2406024.html");

            Console.ReadLine();
            Console.ReadLine();
        }
    }
}
