using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace tSQLt.Client.Net
{
    public class Test
    {
        [XmlAttribute("classname")]    
        public string ClassName;

        [XmlAttribute("name")]
        public string Name;
        
        public bool Failed;
        
        [XmlElement("failure")]
       public Failure Failure;
        
    }
}
