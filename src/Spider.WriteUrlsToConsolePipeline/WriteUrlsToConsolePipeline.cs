using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider {
    public class WriteUrlsToConsolePipeline : IPipeline<List<string>, List<string>> {
        public event Action<List<string>> Next;
        private long urlsCount = 0;
        private event Action onDispose;
        public void Extract(List<string> urls) {
            urls.ForEach(url => { Console.WriteLine($"{++urlsCount}:{url}"); });

            Next?.Invoke(urls);
        }

        public IPipeline<List<string>, List<string>> NextPipeline<V>(IPipeline<List<string>, V> nextPipeline) {
            Next += nextPipeline.Extract;
            onDispose += nextPipeline.Dispose;

            return this;
        }

        public void Dispose() {
            onDispose?.Invoke();
        }
    }
}
