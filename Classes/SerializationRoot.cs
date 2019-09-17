////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov
////////////////////////////////////////////////
using System;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization;

namespace LocalBitcoinsAPI.Classes.lb_Serialize
{
    [DataContract]
    public class SerializationRoot
    {
        /// <summary>
        /// Сериализует объект в JSON строку
        /// </summary>
        /// <returns>строка в формате JSON</returns>
        public string GetAsString()
        {
            using (MemoryStream m_stream = new MemoryStream())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(this.GetType());
                ser.WriteObject(m_stream, this);
                m_stream.Position = 0;
                StreamReader sr = new StreamReader(m_stream);
                string s = sr.ReadToEnd();
                return s;
            }
        }

        /// <summary>
        /// Прочитать JSON строку в Объект
        /// </summary>
        /// <param name="t">Тип, в котороый требуется преобразовать JON строку</param>
        /// <param name="json">Строка JSON</param>
        /// <returns></returns>
        public static object ReadObject(Type t, string json)
        {
            using (MemoryStream my_stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(my_stream))
                {
                    writer.Write(json);
                    writer.Flush();
                    my_stream.Position = 0;
                    try
                    {
                        return new DataContractJsonSerializer(t).ReadObject(my_stream);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }
    }
}
