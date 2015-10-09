namespace NES
{
    public interface Address
    {
        byte Value { get; set; }
        bool isNew();
        void setAsOld();
        string ToString();
    }
}