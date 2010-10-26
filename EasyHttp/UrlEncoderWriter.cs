using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JsonFx.Common;
using JsonFx.Markup;
using JsonFx.Serialization;

namespace EasyHttp
{
    public class UrlEncoderWriter : CommonWriter
    {
        public UrlEncoderWriter(DataWriterSettings writerSettings): base(writerSettings)
        {
        }

        protected override ITextFormatter<CommonTokenType> GetFormatter()
        {
            //return new TransformFormatter<CommonTokenType, MarkupTokenType>(new XmlFormatter(this.Settings), new XmlOutTransformer(this.Settings))
            return new TransformFormatter<CommonTokenType, MarkupTokenType>(new UrlEncoderFormatter(this.Settings),
                                                                            new UrlEncoderOutTransformer(this.Settings));
        }

        public override IEnumerable<string> ContentType
        {
            get { return new List<string>() {"x-www-form-urlencoded"}; }
        }

        public override IEnumerable<string> FileExtension
        {
            get { return null; }
        }
    }

    public class UrlEncoderOutTransformer : IDataTransformer<CommonTokenType, MarkupTokenType>
    {
        public UrlEncoderOutTransformer(DataWriterSettings settings)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Token<MarkupTokenType>> Transform(IEnumerable<Token<CommonTokenType>> input)
        {
            throw new NotImplementedException();
        }
    }

    public class UrlEncoderFormatter : ITextFormatter<MarkupTokenType>
    {
        public UrlEncoderFormatter(DataWriterSettings settings)
        {
            throw new NotImplementedException();
        }

        public void Format(IEnumerable<Token<MarkupTokenType>> tokens, TextWriter writer)
        {
            throw new NotImplementedException();
        }

        public string Format(IEnumerable<Token<MarkupTokenType>> tokens)
        {
            throw new NotImplementedException();
        }
    }
}