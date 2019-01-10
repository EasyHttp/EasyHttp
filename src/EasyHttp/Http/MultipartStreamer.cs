﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EasyHttp.Infrastructure;

namespace EasyHttp.Http
{
    public class MultiPartStreamer
    {
        readonly String _boundary;
        readonly String _boundaryCode;
        readonly IList<MultiPartFileDataAbstraction> _multipartFileData;
        readonly IDictionary<string, object> _multipartFormData;

        public MultiPartStreamer(IDictionary<string, object> multipartFormData, IList<MultiPartFileDataAbstraction> multipartFileData)
        {
            _boundaryCode = DateTime.Now.Ticks.GetHashCode() + "548130";
            _boundary = string.Format("\r\n----------------{0}", _boundaryCode);

            _multipartFormData = multipartFormData;
            _multipartFileData = multipartFileData;
        }

        public void StreamMultiPart(Stream stream)
        {
            stream.WriteString(_boundary);
			 
            if (_multipartFormData != null)
            {
                foreach (var entry in _multipartFormData)
                {
                    stream.WriteString(CreateFormBoundaryHeader(entry.Key, entry.Value));
                    stream.WriteString(_boundary);
                }
            }
			 
            if (_multipartFileData != null)
            {
                foreach (var fileData in _multipartFileData)
                {
                    using (var file = fileData.GetStream())
                    {
                        stream.WriteString(CreateFileBoundaryHeader(fileData));

                        StreamFileContents(file, fileData, stream);

                        stream.WriteString(_boundary);
                    }
                }
            }
            stream.WriteString("--");
        }

        static void StreamFileContents(Stream file, MultiPartFileDataAbstraction fileData, Stream requestStream)
        {
            var buffer = new byte[8192];

            int count;

            while ((count = file.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (fileData.ContentTransferEncoding == HttpContentTransferEncoding.Base64)
                {
                    string str = Convert.ToBase64String(buffer, 0, count);

                    requestStream.WriteString(str);
                }
                else if (fileData.ContentTransferEncoding == HttpContentTransferEncoding.Binary)
                {
                    requestStream.Write(buffer, 0, count);
                }
            }
        }

        public string GetContentType()
        {
            return string.Format("multipart/form-data; boundary=--------------{0}", _boundaryCode);

        }

        public long GetContentLength()
        {
            var ascii = new ASCIIEncoding();
            long contentLength = ascii.GetBytes(_boundary).Length;

            // Multipart Form
            if (_multipartFormData != null)
            {
                foreach (var entry in _multipartFormData)
                {
                    contentLength += ascii.GetBytes(CreateFormBoundaryHeader(entry.Key, entry.Value)).Length; // header
                    contentLength += ascii.GetBytes(_boundary).Length;
                }
            }


            if (_multipartFileData != null)
            {
                foreach (var fileData in _multipartFileData)
                {
                    contentLength += ascii.GetBytes(CreateFileBoundaryHeader(fileData)).Length;
                    contentLength += fileData.GetLength();
                    contentLength += ascii.GetBytes(_boundary).Length;
                }
            }

            contentLength += ascii.GetBytes("--").Length; // ending -- to the boundary

            return contentLength;
        }

        static string CreateFileBoundaryHeader(MultiPartFileDataAbstraction fileData)
        {
            return string.Format(
                "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                "Content-Type: {2}\r\n" +
                "Content-Transfer-Encoding: {3}\r\n\r\n"
                , fileData.FieldName, fileData.GetFilenameForDisposition(), fileData.ContentType,
                fileData.ContentTransferEncoding);
        }

        static string CreateFormBoundaryHeader(string name, object value)
        {
            return string.Format("\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", name, value);
        }
    }
}