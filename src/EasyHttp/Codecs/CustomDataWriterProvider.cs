#region License
// Distributed under the BSD License
// =================================
// 
// Copyright (c) 2010, Hadi Hariri
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Hadi Hariri nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// =============================================================
// 
// 
// Parts of this Software use JsonFX Serialization Library which is distributed under the MIT License:
// 
// Distributed under the terms of an MIT-style license:
// 
// The MIT License
// 
// Copyright (c) 2006-2009 Stephen M. McKamey
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using JsonFx.Serialization;
using JsonFx.Serialization.Providers;

namespace EasyHttp.Codecs
{
    // TODO: This is a copy of the DataWriterProvider in JsonFX. Need to clean it up and move things elsewhere
    public class CustomDataWriterProvider: IDataWriterProvider
    {
    	readonly IDataWriter _defaultWriter;
		readonly IDictionary<string, IDataWriter> _writersByExt = new Dictionary<string, IDataWriter>(StringComparer.OrdinalIgnoreCase);
		readonly IDictionary<string, IDataWriter> _writersByMime = new Dictionary<string, IDataWriter>(StringComparer.OrdinalIgnoreCase);

		
		public CustomDataWriterProvider(IEnumerable<IDataWriter> writers)
		{
			if (writers != null)
			{
				foreach (IDataWriter writer in writers)
				{
					if (_defaultWriter == null)
					{
						// TODO: decide less arbitrary way to choose default
						// without hardcoding value into IDataWriter.
						// Currently first DataWriter wins default.
						_defaultWriter = writer;
					}

					foreach (string contentType in writer.ContentType)
					{
						if (String.IsNullOrEmpty(contentType) ||
							_writersByMime.ContainsKey(contentType))
						{
							continue;
						}

						_writersByMime[contentType] = writer;
					}

					foreach (string fileExt in writer.FileExtension)
					{
						if (String.IsNullOrEmpty(fileExt) ||
							_writersByExt.ContainsKey(fileExt))
						{
							continue;
						}

						string ext = NormalizeExtension(fileExt);
						_writersByExt[ext] = writer;
					}
				}
			}
		}


		public IDataWriter DefaultDataWriter
		{
			get { return _defaultWriter; }
		}


		public IDataWriter Find(string extension)
		{
			extension = NormalizeExtension(extension);

			IDataWriter writer;
			if (_writersByExt.TryGetValue(extension, out writer))
			{
				return writer;
			}

			return null;
		}

		public IDataWriter Find(string acceptHeader, string contentTypeHeader)
		{
		    foreach (string type in ParseHeaders(acceptHeader, contentTypeHeader))
			{
			    IDataWriter writer;

                if (_writersByMime.TryGetValue(type, out writer))
				{
					return writer;
				}
			}

		    return null;
		}


		public static IEnumerable<string> ParseHeaders(string accept, string contentType)
		{
			string mime;

			// check for a matching accept type
			foreach (string type in SplitTrim(accept, ','))
			{
				mime = DataWriterProvider.ParseMediaType(type);
				if (!String.IsNullOrEmpty(mime))
				{
					yield return mime;
				}
			}

			// fallback on content-type
			mime = DataWriterProvider.ParseMediaType(contentType);
			if (!String.IsNullOrEmpty(mime))
			{
				yield return mime;
			}
		}

		public static string ParseMediaType(string type)
		{
			foreach (string mime in SplitTrim(type, ';'))
			{
				// only return first part
				return mime;
			}

			// if no parts then was empty
			return String.Empty;
		}

		private static IEnumerable<string> SplitTrim(string source, char ch)
		{
			if (String.IsNullOrEmpty(source))
			{
				yield break;
			}

			int length = source.Length;
			for (int prev=0, next=0; prev<length && next>=0; prev=next+1)
			{
				next = source.IndexOf(ch, prev);
				if (next < 0)
				{
					next = length;
				}

				string part = source.Substring(prev, next-prev).Trim();
				if (part.Length > 0)
				{
					yield return part;
				}
			}
		}

		private static string NormalizeExtension(string extension)
		{
			if (String.IsNullOrEmpty(extension))
			{
				return String.Empty;
			}

			// ensure is only extension with leading dot
			return Path.GetExtension(extension);
		}


    }
}