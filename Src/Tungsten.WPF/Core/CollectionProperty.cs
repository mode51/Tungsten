using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W.WPF.Core
{
    public class CollectionProperty<TValue> : Property<ObservableCollection<TValue>>
    {
        public CollectionProperty() : base(new ObservableCollection<TValue>())
        {
        }
        public CollectionProperty(params TValue[] values) : this(null, values)
        {
        }
        public CollectionProperty(System.Collections.Specialized.NotifyCollectionChangedEventHandler onCollectionChanged, params TValue[] values) : base(new ObservableCollection<TValue>())
        {
            if (onCollectionChanged != null)
                Value.CollectionChanged += onCollectionChanged;
            foreach (var value in values)
                Value.Add(value);
        }
    }

    public class CollectionProperty<TOwner, TValue> : Property<TOwner, ObservableCollection<TValue>> where TOwner: class
    {
        public CollectionProperty()
        {
            Value = new ObservableCollection<TValue>();
        }
        public CollectionProperty(params TValue[] values) : this(null, null, values)
        {
        }
        public CollectionProperty(TOwner owner, params TValue[] values) : this(owner, null, values)
        {
        }
        public CollectionProperty(System.Collections.Specialized.NotifyCollectionChangedEventHandler onCollectionChanged, params TValue[] values) : this(null, onCollectionChanged, values)
        {
        }
        public CollectionProperty(TOwner owner, System.Collections.Specialized.NotifyCollectionChangedEventHandler onCollectionChanged, params TValue[] values) : base(owner, new ObservableCollection<TValue>())
        {
            if (onCollectionChanged != null)
                Value.CollectionChanged += onCollectionChanged;
            foreach (var value in values)
                Value.Add(value);
        }
    }
}
