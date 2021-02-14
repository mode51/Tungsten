using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace W
{
    public class Disposable : IDisposable
    {
        private W.Threading.Lockers.Disposer _disposer = new Threading.Lockers.Disposer();
        public bool IsDisposing => _disposer?.IsDisposing ?? true;
        public bool IsDisposed => _disposer?.IsDisposed ?? true;
        protected virtual void OnDispose()
        {

        }
        public void Dispose()
        {
            _disposer.Dispose(() =>
            {
                OnDispose();
            });
        }
    }
}
