using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BluePrint.Dictionary.Providers.Entities
{
    public class NameEntity:IDataNode 
    {
        public ObjectId id;
        public NameEntity()
        {
            preWord = new List<string>();
            postWord = new List<string>();
        }
        [BsonElement("name")]
        public string Word { get; set; }
        [BsonElement("freq")]
        public double Frequency { get; set; }
        public int length;
        public List<string> preWord;
        public List<string> postWord;

        [BsonIgnore]
        public SegmentFramework.POSType POS
        {
            get
            {
                return SegmentFramework.POSType.A_NR;
            }
            set
            {
               
            }
        }

        public override string ToString()
        {
            return Word;
        }
    }
}
