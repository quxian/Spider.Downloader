using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extend
{
    public static class ExtendHtmlFindAllUrls
    {
        public static List<string> FindAllUrs(this string html) {
            return new List<string> {
                "http://webmagic.io/docs/zh/posts/ch1-overview/architecture.html",
                "https://www.baidu.com"
            };
        }
    }
}
