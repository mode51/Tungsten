namespace W
{
    public interface IProperty<TValue> : IProperty
    {
        TValue Value { get; set; }
    }

    public interface IProperty
    {
        bool IsDirty { get; set; }
    }
}