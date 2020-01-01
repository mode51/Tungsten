namespace W.Data.Sql.Attributes
{
    public class Attributes
    {
        public NamedColumnAttribute NamedColumn { get; internal set; }
        public bool IsPrimaryKey { get; internal set; }
        public bool IsKeyColumn { get; internal set; }
        public bool IsNullable { get; internal set; }
    }
}
