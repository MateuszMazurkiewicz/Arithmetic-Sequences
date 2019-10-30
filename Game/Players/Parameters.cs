using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Game.Players
{
    [Serializable()]
    public class Parameters
    {
        [XmlElement("Lower")]
        public int Lower { get; set; }

        [XmlElement("Upper")]
        public int Upper { get; set; }

        [XmlElement("SetCount")]
        public int SetCount { get; set; }

        [XmlElement("SequenceLength")]
        public int SequenceLength { get; set; }

        [XmlElement("FirstPlayerType")]
        public string FirstPlayerType { get; set; }

        [XmlElement("SecondPlayerType")]
        public string SecondPlayerType { get; set; }

        [XmlElement("GamesCount")]
        public int GamesCount { get; set; }
        
        public static Parameters Deserialize(string path)
        {  
            XmlSerializer serializer = new XmlSerializer(typeof(Parameters));

            StreamReader reader = new StreamReader(path);
            var parameters = (Parameters)serializer.Deserialize(reader);
            reader.Close();
            return parameters;
        }
    }
}
