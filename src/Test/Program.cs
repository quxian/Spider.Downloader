using Spider;
using Spider.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test {
    public class Program {
        public static void Main(string[] args) {
            new Engine(
                new Downloader(),
                new PageProcesser(),
                new Scheduler())
                .AddUrls("http://webmagic.io/docs/zh/posts/ch1-overview/architecture.html")
                .AddPipelineEventListens(new Pipeline())
                .Run();
        }
    }
}
