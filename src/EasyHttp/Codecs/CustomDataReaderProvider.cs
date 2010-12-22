using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;

namespace EasyHttp.Codecs
{
    public class CustomDataReaderProvider: IDataReaderProvider
    {
        readonly IDictionary<string, IDataReader> _readersByMime = new Dictionary<string, IDataReader>(StringComparer.OrdinalIgnoreCase);

        public CustomDataReaderProvider(IEnumerable<IDataReader> dataReaders)
        {
            if (dataReaders != null)
            {
                foreach (IDataReader reader in dataReaders)
                {
                    foreach (string contentType in reader.ContentType)
                    {
                        if (String.IsNullOrEmpty(contentType) ||
                            _readersByMime.ContainsKey(contentType))
                        {
                            continue;
                        }

                        _readersByMime[contentType] = reader;
                    }
                }
            }

        }

        public IDataReader Find(string contentTypeHeader)
        {
            var type = DataWriterProvider.ParseMediaType(contentTypeHeader);

            var readers = from reader in _readersByMime
                                where Regex.Match(type, reader.Key, RegexOptions.Singleline).Success
                                select reader;

            return readers.First().Value;
        }
    }
}