namespace EasyHttp
{
    public class HttpBaseData
    {
        public HttpBaseData()
        {
            Header = new Header();
            Body = new Body();
        }

        public Body Body { get; set; }
        public Header Header { get; set; }
    }
}