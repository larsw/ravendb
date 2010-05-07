using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Raven.Database.Json
{
	public static class JsonExtensions
	{
		public static T Deserialize<T>(this JObject self)
		{
			var jsonSerializer = new JsonSerializer();
			jsonSerializer.Converters.Add(new JsonEnumConverter());
			return (T) jsonSerializer.Deserialize(new JsonTokenReader(self), typeof (T));
		}

		public static object Deserialize(this JObject self, Type type)
		{
			var jsonSerializer = new JsonSerializer();
			jsonSerializer.Converters.Add(new JsonEnumConverter());
			return jsonSerializer.Deserialize(new JsonTokenReader(self), type);
		}

		public static JObject ToJObject(this byte [] self)
		{
			return JObject.Load(new JsonTextReader(new StreamReader(new MemoryStream(self), Encoding.UTF8)));
		}

		public static T JsonDeserialization<T>(this byte [] self)
		{
			return (T) new JsonSerializer().Deserialize(new JsonTextReader(new StreamReader(new MemoryStream(self))), typeof (T));
		}

		public static T JsonDeserialization<T>(this JObject self)
		{
			return (T)new JsonSerializer().Deserialize(new JsonTokenReader(self), typeof(T));
		}
	}
}