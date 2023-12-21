using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GoShared.Helpers
{
    public static class XmlHelper
    {
        public static (IList<T>? data, string error) Reads<T>(string path) {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                List<T> dezerializedList;
                using (FileStream stream = File.OpenRead(path))
                {
                    dezerializedList = (List<T>?)serializer.Deserialize(stream);
                }

                return (dezerializedList, "");
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        public static string WriteXml(string content, string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.WriteAllText(path, String.Empty);
                    FileStream stream = File.OpenWrite(path);
                    XmlSerializer serializer = new XmlSerializer(typeof(string));
                    serializer.Serialize(stream, content);
                    stream.Close();
                    stream.Dispose();
                }
                else
                {
                    //File.Create(Path + fileName);
                    CreateXml(path);
                    File.WriteAllText(path, String.Empty);
                    FileStream stream = File.OpenWrite(path);
                    XmlSerializer serializer = new XmlSerializer(typeof(string));
                    serializer.Serialize(stream, content);
                    stream.Close();
                    stream.Dispose();
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public static string WriteXml<T>(IList<T> list,string path) where T : new()
        {
            try
            {
                if (File.Exists(path))
                {
                    File.WriteAllText(path, String.Empty);
                    FileStream stream = File.OpenWrite(path);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                    serializer.Serialize(stream, list);
                    stream.Close();
                    stream.Dispose();
                }
                else
                {
                    File.Create(path);
                    File.WriteAllText(path, String.Empty);
                    FileStream stream = File.OpenWrite(path);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                    serializer.Serialize(stream, list);
                    stream.Close();
                    stream.Dispose();
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static string CreateXml(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("<?xml version='1.0'?> \n");
                        sw.WriteLine("<ArrayOfTableInfo xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'> \n");
                        sw.WriteLine("</ArrayOfTableInfo>");
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
