namespace EasyHttp.Codecs
{
    public interface IMemberBehavior
    {
        void SetDefaultValue(string memberName, object value);
    }
}