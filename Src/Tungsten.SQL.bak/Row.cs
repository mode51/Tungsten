using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using Tungsten.SQL.Attributes;
using Tungsten.Reflection;

namespace Tungsten.SQL
{
    [Serializable()]
    public class Row
    {
        protected bool _isMarkedForDeletion = false;
        protected bool _isNew = false;

        //TODO:  TheGeneral.Common.Data.Row:  Provide a binary Clone method
        public Row()
        {
        }
        public void MarkForDeletion()
        {
            _isMarkedForDeletion = true;
        }
        public bool IsMarkedForDeletion
        {
            get
            {
                return _isMarkedForDeletion;
            }
        }
        public bool IsNew
        {
            get
            {
                return _isNew;
            }
        }

        internal T LoadFromAnother<T>(T clone) where T : Row
        {
            T newItem = this as T;
            Type type = typeof(T);

            //shallow copy fields
            foreach (FieldInfo fi in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                fi.SetValue(newItem, fi.GetValue(clone));
            }

            //shallow copy properties
            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                pi.SetValue(newItem, pi.GetValue(clone, null), null);
            }
            return newItem;
        }
        internal T LoadFromReader<T>(IDataReader reader) where T : Row
        {
            var newItem = this as T;
            for (int t = 0; t < reader.FieldCount; t++)
            {
                //have to try both fields and properties (so just make it a practice to use either, but not both (or at least both with discretion))
                ByFields.SetFieldByColumnName<T>(newItem, reader.GetName(t), reader[t]);
                ByProperties.SetPropertyByColumnName<T>(newItem, reader.GetName(t), reader[t]);
            }
            return newItem;
        }
    }

    [Serializable()]
    public class Row<T> : Row where T : Row, new()
    {
        private readonly Undoable<T> _history = new Undoable<T>(10);

        public Row()
        {
            _isNew = true;
            _history.Push(this as T);//new ControlType());
        }
        public Row(T original)
        {
            _history.Push(LoadFromAnother(original));
        }
        public Row(IDataReader reader)
        {
            _history.Push(LoadFromReader<T>(reader));
        }

        public void Push()
        {
            T newItem = _history.Current.CloneByProperties<T>();
            _history.Push(newItem);
        }
        public void Undo()
        {
            _history.Undo();
        }
        public void Redo()
        {
            _history.Redo();
        }

        public T Original => _history[0];
        public bool IsModified => (_history.Count > 1);
    }
}
