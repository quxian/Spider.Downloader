using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Spider.Pipeline {
    public class Pipeline : IPipeline {
        public void Extract(string page) {
            File.WriteAllText($@"C:\spider\{Guid.NewGuid().ToString("N")}", page);
        }
    }
}
