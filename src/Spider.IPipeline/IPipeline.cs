using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider {
    public interface IPipeline<T,U> {
        event Action<U> Next;
        void Extract(T page);
        IPipeline<T,U> NextPipeline<V>(IPipeline<U,V> nextPipeline);
    }
}
