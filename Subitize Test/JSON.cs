using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;

namespace Subitize_Test
{
    public static class JSON<T> where T : class
    {
        public static T Parse(string json)
        {
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return serializer.ReadObject(stream) as T;
            }
        }
        public static string Parse(T instance)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, instance);
                return Encoding.Default.GetString(stream.ToArray());
            }
        }
    }
}