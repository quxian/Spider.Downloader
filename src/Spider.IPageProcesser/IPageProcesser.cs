using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider {
    public interface IPageProcesser {
        void Run();
        void AddPage(string page);
        void AddFindAllUrlsEventListens(Action<List<string>> action);
        void AddPipelineEventListens(Action<string> action);
    }
}
