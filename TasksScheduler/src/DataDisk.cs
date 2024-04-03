using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.Formats.Asn1;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using static System.Windows.Forms.LinkLabel;

namespace TasksScheduler.src
{
    public class DataDisk
    {
        public string dataPath = "Data.xml";
        public string dataPathJson = "DataJ.json";
        public string docPath = "";
        public static string SoundFilenamesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sounds");

        public DataDisk() { }

        public void SerializeObjectJson<T>(T toSerialize, string filename)
        {
            if (toSerialize == null) { return; }

            try
            {
                string json = JsonConvert.SerializeObject(toSerialize,
                                                          new JsonSerializerSettings()
                                                          {
                                                              TypeNameHandling = TypeNameHandling.Auto
                                                          });

                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, filename)))
                {
                    outputFile.Write(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public T? DeSerializeObjectJson<T>(string filename)
        {
            string json;
            T? returningObject = default;

            try
            {
                using (StreamReader outputFile = new StreamReader(Path.Combine(docPath, filename)))
                {
                    json = outputFile.ReadToEnd();
                }
                returningObject = JsonConvert.DeserializeObject<T>(json,
                                                                    new JsonSerializerSettings()
                                                                    {
                                                                        TypeNameHandling = TypeNameHandling.Auto
                                                                    });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return returningObject;
        }

        public void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default; }

            T objectOut = default;

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return objectOut;
        }
    }
}
