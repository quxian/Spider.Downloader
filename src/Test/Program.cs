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
                .AddUrls("https://www.microsoft.com/en-us/")
                .AddUrls("https://github.com/quxian/Spider.Downloader/blob/master/src/Spider.Scheduler/Scheduler.cs")
                .AddPipeline(new Pipeline().NextPipeline(new Pipeline()))
                .AddPipeline(new Pipeline())
                .Run();
        }
    }
}
