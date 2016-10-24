using Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider.PageProcesser {
    public class PageProcesser {
        public event Action<List<string>> OnFindAllUrls;
        public event Action<string> OnPipeline;
        public PageProcesser() {
        }

        public void OnDownloadPage(string page) {
            var allUrls = page.FindAllUrs();
            OnFindAllUrls(allUrls);
            OnPipeline(page);
        }
    }
}
