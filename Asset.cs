using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PurityEngine {
    public class Serializer {
        public static string Serialize(dynamic obj) {
            //Get object type
            Type type = obj.GetType();
            try {
                return "PurityAssetJSON" + (char)type.Name.Length + type.Name + Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            } catch {
                MemoryStream ms = new MemoryStream();


                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                x.Serialize(ms,obj);

                string encodedObject = Convert.ToBase64String(ms.ToArray());

                ms.Close();
                return "PurityAssetXML" + encodedObject;
            }
        }

        public static dynamic Deserialize(string str) {
            if (str.StartsWith("PurityAssetXML")) {
                int typeLength = str[0];
                string typeName = str.Substring(1, typeLength);
                string data = str.Substring(typeLength + 1);
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Type.GetType(typeName));
                byte[] bytes = Convert.FromBase64String(str);
                MemoryStream stream = new MemoryStream(bytes);
                return x.Deserialize(stream);
            }
            if (!str.StartsWith("PurityAssetJSON")) {
                throw new PuritySerializationException("Error: Invalid asset");
            }
            return DeserializeInternal(str.Substring("PurityAssetJSON".Length));
        }

        public static dynamic DeserializeInternal(string json) {
            //Get object type
            int typeLength = json[0];
            string typeName = json.Substring(1, typeLength);
            //Get object data
            string data = json.Substring(typeLength + 1);
            //Deserialize object
            return Newtonsoft.Json.JsonConvert.DeserializeObject(data, Type.GetType(typeName));
        }
    }
    [Serializable]
    public class Asset {
        public virtual string Serialize() {
            return Serializer.Serialize(this);
        }
    }
    [Serializable]
    public class PuritySerializationException : Exception
    {
        public PuritySerializationException() { }
        public PuritySerializationException(string message) : base(message) { }
        public PuritySerializationException(string message, System.Exception inner) : base(message, inner) { }
        protected PuritySerializationException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}