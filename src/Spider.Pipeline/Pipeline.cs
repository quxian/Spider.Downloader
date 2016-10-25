using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Spider.Pipeline {
    public class Pipeline : IPipeline {
        private event Action<string> Next;
        public void Extract(string page) {
            File.WriteAllText($@"C:\spider\{Guid.NewGuid().ToString("N")}", page);
            if (null == Next)
                return;
            Next(page);
        }

        public IPipeline NextPipeline(IPipeline nextPipeline) {
            Next += nextPipeline.Extract;

            return this;
        }
    }
}
