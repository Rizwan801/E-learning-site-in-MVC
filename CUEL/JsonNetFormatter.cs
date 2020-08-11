using System;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace CUEL
{
    internal class JsonNetFormatter : MediaTypeFormatter
    {
        private JsonSerializerSettings jsonSerializerSettings;

        public JsonNetFormatter(JsonSerializerSettings jsonSerializerSettings)
        {
            this.jsonSerializerSettings = jsonSerializerSettings;
        }

        public override bool CanReadType(Type type)
        {
            throw new NotImplementedException();
        }

        public override bool CanWriteType(Type type)
        {
            throw new NotImplementedException();
        }
    }
}