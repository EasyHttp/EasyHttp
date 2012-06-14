﻿#region License
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

using System.Collections.Generic;
using EasyHttp.Codecs;
using EasyHttp.Codecs.JsonFXExtensions;
using JsonFx.Json;
using JsonFx.Json.Resolvers;
using JsonFx.Model.Filters;
using JsonFx.Serialization;
using JsonFx.Serialization.Resolvers;
using JsonFx.Xml;
using JsonFx.Xml.Resolvers;

namespace EasyHttp.Configuration
{
    public class DefaultEncoderDecoderConfiguration : IEncoderDecoderConfiguration
    {
        public IEncoder GetEncoder()
        {
            var jsonWriter = new JsonWriter(new DataWriterSettings(CombinedResolverStrategy()),
                                            new[] {"application/.*json", "text/.*json"});
            var xmlWriter = new XmlWriter(new DataWriterSettings(CombinedResolverStrategy()),
                                          new[] {"application/xml", "text/.*xhtml", "text/xml", "text/html"});

            var urlEncoderWriter = new UrlEncoderWriter(new DataWriterSettings(CombinedResolverStrategy()),
                                                        new[] {"application/x-www-form-urlencoded"});

            var writers = new List<IDataWriter> { jsonWriter, xmlWriter, urlEncoderWriter };

            var dataWriterProvider = new RegExBasedDataWriterProvider(writers);

            return new DefaultEncoder(dataWriterProvider);
        }

        public IDecoder GetDecoder()
        {
            var jsonReader = new JsonReader(new DataReaderSettings(CombinedResolverStrategy(), new Iso8601DateFilter()), new[] { "application/.*json", "text/.*json" });
            var xmlReader = new XmlReader(new DataReaderSettings(CombinedResolverStrategy(), new Iso8601DateFilter()),
                                          new[] {"application/.*xml", "text/.*xhtml", "text/xml", "text/html"});

            var readers = new List<IDataReader> {jsonReader, xmlReader};

            var dataReaderProvider = new RegExBasedDataReaderProvider(readers);
            
            return new DefaultDecoder(dataReaderProvider);
        }

        public static CombinedResolverStrategy CombinedResolverStrategy()
        {
            return new CombinedResolverStrategy(
                new JsonResolverStrategy(),
                new DataContractResolverStrategy(),                                     
                new XmlResolverStrategy(),                                                              
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.PascalCase),       
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.CamelCase),    
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.NoChange),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.NoChange, "_"),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.NoChange, "-"),
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Lowercase, "-"),   
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Uppercase, "_"));
        }
    }
}