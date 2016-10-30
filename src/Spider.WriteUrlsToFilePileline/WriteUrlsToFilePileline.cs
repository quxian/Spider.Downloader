using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Spider {
    public class WriteUrlsToFilePileline : IPipeline<List<string>, List<string>> {
        private readonly string _filePath;
        public WriteUrlsToFilePileline(string filePath) {
            _filePath = filePath;
            if (!File.Exists(_filePath)) {
                File.Create(_filePath);
            }
        }

        public event Action<List<string>> Next;

        public void Extract(List<string> page) {
            var btyes = page.SelectMany(url => System.Text.Encoding.UTF8.GetBytes(url+"\r\n")).ToArray();
            using (var fs = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {
                fs.Position = fs.Length;
                fs.WriteAsync(btyes, 0, btyes.Count());                
            }

            if (null == Next)
                return;

            Next(page);
        }

        public IPipeline<List<string>, List<string>> NextPipeline<V>(IPipeline<List<string>, V> nextPipeline) {
            Next += nextPipeline.Extract;

            return this;
        }
    }
}
