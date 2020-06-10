using Newtonsoft.Json;
using SW.Pmm.Core;
using SW.PrimitiveTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SW.Pmm.UnitTests
{
    static class TestHelper
    {

        public static TReq GetTestObjectJson<TReq>(string filename)
        {
            return JsonConvert.DeserializeObject<TReq>(File.ReadAllText($@"TestData/{filename}.json"));

        }

        public static TReq GetTestObjectXml<TReq>(string body)
        {
            if (string.IsNullOrEmpty(body)) return default(TReq);

            using (TextReader sr = new StringReader(body))
            {
                var serializer = new XmlSerializer(typeof(TReq));
                TReq response = (TReq)serializer.Deserialize(sr);
                      
                return response;
            }

          
        
           
          
        }
    }
}
