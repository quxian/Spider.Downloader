using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider {
    public class WriteUrlsToConsolePipeline : IPipeline<List<string>, List<string>> {
        public event Action<List<string>> Next;

        public void Extract(List<string> page) {
            page.ForEach(url => { Console.WriteLine(url); });
            if (null == Next)
                return;
            Next(page);
        }

        public IPipeline<List<string>, List<string>> NextPipeline<V>(IPipeline<List<string>, V> nextPipeline) {
            Next += nextPipeline.Extract;

            return this;
        }
    }
}
