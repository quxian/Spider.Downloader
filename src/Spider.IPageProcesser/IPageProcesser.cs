﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider {
    public interface IPageProcesser<T> : IDisposable {
        event Action<T> PipelineEvent;
        void Run();
        void AddPage(T page);
        void AddFindAllUrlsEventListens(Action<List<string>> action);
        void AddPipelineEventListens(Action<T> action);
    }
}
