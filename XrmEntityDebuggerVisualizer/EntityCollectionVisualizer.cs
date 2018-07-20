using Microsoft.VisualStudio.DebuggerVisualizers;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmEntityDebuggerVisualizer;

[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(EntityCollectionVisualizer),
    typeof(EntityCollectionVisualizerObjectSource),
    Target = typeof(EntityCollection),
    Description = "Entity Collection Visualizer")
]
namespace XrmEntityDebuggerVisualizer
{
    public static class Serializer
    {
        public static string Serialize<T>(T obj)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(memStream, obj);

                string jsonString = Encoding.UTF8.GetString(memStream.ToArray());

                return jsonString;
            }
        }

        public static T Deserialize<T>(string json)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                StreamWriter writer = new StreamWriter(memStream);
                writer.Write(json);
                writer.Flush();

                memStream.Position = 0;

                T deserializedObj = (T)serializer.ReadObject(memStream);

                return deserializedObj;
            }
        }
    }

    public class EntityCollectionVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            EntityCollection entityCollection = (EntityCollection)target;

            string serialized = Serializer.Serialize(entityCollection);

            var writer = new BinaryWriter(outgoingData);
            writer.Write(serialized);
        }
    }

    public class EntityCollectionVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {           
            string serialized = "";
            using (StreamReader reader = new StreamReader(objectProvider.GetData(), Encoding.UTF8))
            {
                serialized = reader.ReadToEnd();
            }

            serialized = serialized.Substring(2);

            EntityCollection ecoll = Serializer.Deserialize<EntityCollection>(serialized);

            EntityCollectionViewerForm form = new EntityCollectionViewerForm(ecoll);
            form.ShowDialog();
        }
       
        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(EntityCollectionVisualizer), typeof(EntityCollectionVisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
