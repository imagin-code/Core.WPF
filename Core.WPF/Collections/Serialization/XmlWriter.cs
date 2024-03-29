﻿using Imagin.Core.Analytics;
using Imagin.Core.Linq;
using Imagin.Core.Serialization;
using Imagin.Core.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Imagin.Core.Collections.Serialization
{
    public class XmlWriter<T> : Writer<T>
    {
        readonly string Root;

        ///

        XmlCallbackSerializer serializer = null;
        XmlCallbackSerializer Serializer => serializer ?? (serializer = new XmlCallbackSerializer(typeof(List<T>), new XmlAttributeOverrides(), AllTypes, new XmlRootAttribute(Root), string.Empty));

        Type[] allTypes = null;
        Type[] AllTypes => allTypes ?? (allTypes = GetTypes().Distinct().ToArray());

        readonly Type[] Types;

        ///

        public Encoding Encoding { get => this.Get(Encoding.ASCII); set => this.Set(value); }

        ///

        public XmlWriter(string root, string folderPath, string fileName, string fileExtension, string singleFileExtension, Limit limit = default, params Type[] types) : base(folderPath, fileName, fileExtension, singleFileExtension, limit)
        {
            Root = root;
            Types = types;
        }

        ///

        IEnumerable<Type> GetTypes()
        {
            foreach (var i in Types)
                yield return i;
        }

        ///

        public override Result Deserialize(string filePath, out object result)
        {
            StringReader reader = null;
            try
            {
                //1. Get an XML string from the file
                string text = Storage.File.Long.ReadAllText(filePath, Encoding.Convert());
                reader = new StringReader(text);

                //2b. Deserialize the XML string
                result = Serializer.Deserialize(reader);
                reader.Close();
                return new Success();
            }
            catch (Exception e)
            {
                result = Enumerable.Empty<T>();
                reader?.Close();

                var error = new Error(e);
                Log.Write<XmlWriter<T>>(error);
                return error;
            }
        }

        public override Result Serialize(string filePath, object input)
        {
            Result result = null;

            var data = new List<T>(input as IEnumerable<T>);
            StringWriter writer = new();

            try
            {
                Storage.Folder.Long.Create(Path.GetDirectoryName(filePath));

                //1. Serialize to an XML string
                Serializer.Serialize(writer, data);

                string text = writer.ToString();

                //2b. Write the XML string to the file
                Storage.File.Long.WriteAllText(filePath, text, Encoding.Convert());
                result = new Success();
            }
            catch (Exception e)
            {
                result = new Error(e);
                Log.Write<XmlWriter<T>>(result);
            }
            finally { writer.Close(); }
            return result;
        }
    }
}