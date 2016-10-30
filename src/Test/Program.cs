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
                new Downloader(3),
                new PageProcesser(),
                new Scheduler(3))
                .AddUrls("https://www.baidu.com/")
                .AddUrls("http://www.sina.com.cn/")
                .AddUrls("http://www.163.com/")
                .AddUrls("http://www.ifeng.com/")
                .AddUrls("http://mai.sogou.com/")
                .AddPipeline(
                    new ProcessDOMPipeline()
                    .NextPipeline(
                        new FindAllUrlPipeline()
                        .NextPipeline(new WriteUrlsToConsolePipeline())
                        .NextPipeline(new WriteUrlsToFilePileline("urls.txt"))
                    )
                )
                .Run();
        }
    }
}
