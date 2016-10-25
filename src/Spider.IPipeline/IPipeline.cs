using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider {
    public interface IPipeline {
        void Extract(string page);
    }
}
