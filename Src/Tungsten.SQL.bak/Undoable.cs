using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tungsten.SQL
{
    [Serializable()]
    public class Undoable<T> : IDisposable
    {
        private readonly List<T> _history = null;
        private int _index = -1; //adding the initial item will set it to 0
        public bool _isDisposed { get; set; } = false;
        public object _disposeLock { get; private set; } = new object();

        public Undoable() : this(10)
        {
        }
        public Undoable(int capacity)
        {
            _history = new List<T>(capacity);
        }
        ~Undoable()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (_isDisposed) return;
            lock (_disposeLock)
            {
                // dispose each item, if it implements IDisposable
                foreach (var disposableItem in _history.Select(item => item as IDisposable))
                {
                    disposableItem?.Dispose();
                }
                _history.Clear();
                _isDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        public void Push(T item)
        {
            //increment the indexer
            _index++;
            //clear out all Redo's
            while (_history.Count > _index)
                _history.RemoveAt(_index);
            //now push the clone
            _history.Add(Tungsten.Reflection.BinaryClone.Clone(item));
        }
        public T Undo()
        {
            if (_index > 0)
                _index--;
            return _history[_index];
        }
        public T Redo()
        {
            if (_index < _history.Count)
                _index++;
            return _history[_index];
        }
        public T Current => _history[_index];
        public T this[int index] => _history[index];
        public int Count => _history.Count;
    }
}
