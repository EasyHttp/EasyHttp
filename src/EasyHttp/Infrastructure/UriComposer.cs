namespace EasyHttp.Infrastructure
{
    public class UriComposer
    {
        public string Compose(params string[] sections)
        {
            var uri = string.Empty;
            foreach (var section in sections)
            {

                if (section.StartsWith("/"))
                {
                    uri = uri + section.Remove(0, 1);
                } else
                {
                    uri = uri + section;
                }

                if (!section.EndsWith("/"))
                {
                    uri = uri + "/";
                }
            }
            
            return uri.Remove(uri.Length -1, 1);
        }
    }
}