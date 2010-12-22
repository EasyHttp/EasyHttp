using System;
using System.Collections.Generic;
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
            string type = DataWriterProvider.ParseMediaType(contentTypeHeader);

            IDataReader reader;
            if (_readersByMime.TryGetValue(type, out reader))
            {
                return reader;
            }

            return null;
        }
    }
}