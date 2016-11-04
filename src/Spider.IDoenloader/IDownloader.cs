using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider {
    public interface IDownloader<T> : IDisposable {
        void Run();
        void AddUrl(string url);
        void AddDownloadPageEventListens(Action<T> action);
    }
}
