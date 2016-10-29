using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extend {
    public static class ExtendHtmlFindAllUrls {
        public static List<string> FindAllUrs(this string html) {
            return new List<string> {
                "https://www.microsoft.com/en-us"
            };
        }
    }
}
