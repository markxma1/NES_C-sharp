namespace NES
{
    public interface Address
    {
        byte Value { get; set; }
        bool isNew();
        string ToString();
    }
}