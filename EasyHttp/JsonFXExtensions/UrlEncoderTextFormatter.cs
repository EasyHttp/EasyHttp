using System;
using System.Collections.Generic;
using System.IO;
using JsonFx.Common;
using JsonFx.Serialization;

namespace EasyHttp.JsonFXExtensions
{
    public class UrlEncoderTextFormatter : ITextFormatter<CommonTokenType>
    {
        public void Format(IEnumerable<Token<CommonTokenType>> tokens, TextWriter writer)
        {
            var firstProperty = true;
            
            foreach(var token in tokens)
            {
                switch (token.TokenType)
                {
                    case CommonTokenType.None:
                        break;
                    case CommonTokenType.ObjectBegin:
                        break;
                    case CommonTokenType.ObjectEnd:
                        break;
                    case CommonTokenType.ArrayBegin:
                        break;
                    case CommonTokenType.ArrayEnd:
                        break;
                    case CommonTokenType.Property:
                        if (!firstProperty)
                        {
                            writer.Write("&");
                        }
                        firstProperty = false;
                        writer.Write(token.Name);
                        continue;
                    case CommonTokenType.Primitive:
                        writer.Write(String.Format("={0}", token.Value));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public string Format(IEnumerable<Token<CommonTokenType>> tokens)
        {
            using (var writer = new StringWriter())
            {
                Format(tokens, writer);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}