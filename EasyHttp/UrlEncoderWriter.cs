using System.Collections.Generic;
using JsonFx.Common;
using JsonFx.Serialization;

namespace EasyHttp
{
    public class UrlEncoderWriter:CommonWriter 
    {
        DataWriterSettings writerSettings;

        public UrlEncoderWriter(DataWriterSettings writerSettings)
        {
            this.writerSettings = writerSettings;
        }

        protected override ITextFormatter<CommonTokenType> GetFormatter()
        {
            return new UrlEncoderTextFormatter();
        }

        public override IEnumerable<string> ContentType
        {
            get { return new List<string>() {"application/x-www-form-urlencoded"}; }
        }

        public override IEnumerable<string> FileExtension
        {
            get { return new List<string>();  }
        }
    }
}