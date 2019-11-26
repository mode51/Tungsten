using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Tungsten.SQL.Attributes
{
    public class Attributes
    {
        public NamedColumnAttribute NamedColumn { get; internal set; }
        public bool IsPrimaryKey { get; internal set; }
        public bool IsKeyColumn { get; internal set; }
        public bool IsNullable { get; internal set; }
    }
}
