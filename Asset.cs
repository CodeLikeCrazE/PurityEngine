using System;

namespace PurityEngine {
    public class Serializer {
        public static string Serialize(dynamic obj) {
            //Get object type
            Type type = obj.GetType();
            return (char)type.Name.Length + type.Name + Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static dynamic Deserialize(string json) {
            //Get object type
            int typeLength = json[0];
            string typeName = json.Substring(1, typeLength);
            //Get object data
            string data = json.Substring(typeLength + 1);
            //Deserialize object
            return Newtonsoft.Json.JsonConvert.DeserializeObject(data, Type.GetType(typeName));
        }
    }
    public class Asset {
        public virtual string Serialize() {
            return Serializer.Serialize(this);
        }
    }
}