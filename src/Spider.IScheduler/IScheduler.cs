using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider {
    public interface IScheduler {
        void Run();
        void AddUrls(List<string> urls);
        void AddUrlDequeueEventListens(Action<string> action);
    }
}
