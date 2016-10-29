using Spider;
using Spider.Pipeline;
using Spider.ProcessDOMPipeline;
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
                .AddUrls("https://github.com/quxian/Spider.Downloader/blob/master/src/Spider.Scheduler/Scheduler.cs")
                .AddUrls("http://stackoverflow.com/questions/2248411/get-all-links-on-html-page")
                .AddPipeline(new ProcessDOMPipeline().NextPipeline(new FindAllUrlPipeline()))
                .Run();
        }
    }
}
