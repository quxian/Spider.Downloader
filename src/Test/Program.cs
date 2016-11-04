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
            var engine = new Engine(
                 new Downloader(),
                 new PageProcesser(),
                 new Scheduler())
                 .AddUrls("https://www.baidu.com/")
                 //.AddUrls("http://www.sina.com.cn/")
                 .AddUrls("http://www.163.com/")
                 .AddUrls("http://www.ifeng.com/")
                 .AddUrls("http://mai.sogou.com/")
                 .AddPipeline(
                     new ProcessDOMPipeline()
                     .NextPipeline(
                         new FindAllUrlPipeline()
                         .NextPipeline(new WriteUrlsToFilePileline("urls.txt"))
                         .NextPipeline(new WriteUrlsToConsolePipeline())
                     )
                 )
                 .Run();

            while ('y' != Console.ReadKey().KeyChar) ;

            engine.Dispose();

            Console.WriteLine("end!");
            Console.ReadLine();
        }
    }
}
