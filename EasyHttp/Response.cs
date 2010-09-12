namespace EasyHttp
{
    public class Response
    {
        public Body Body { get; set; }
        public Header Header { get; set; }
        
        public Response()
        {
            Header = new Header();
            Body = new Body();
        }

    }
}