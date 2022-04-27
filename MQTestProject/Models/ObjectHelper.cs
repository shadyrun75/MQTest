using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MQTestProject.Models
{
    public class ObjectHelper
    {
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static byte[] ObjectToJsonToByteArray(Object obj)
        {
            var str = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(str);
        }

        public static string ObjectToJson(Object obj) => JsonConvert.SerializeObject(obj);

        public static T ByteArrayToObject<T>(byte[] arrBytes)
            where T : class
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return (T)obj;
            }
        }
    }
}
