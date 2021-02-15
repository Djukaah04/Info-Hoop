using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Bson;
using MongoDB.Driver;

namespace Projekat3
{
    public class Trener
    {
        public ObjectId Id { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public Int32 godisnjaplata { get; set; }
        public List<string> stiligre { get; set; }
        public MongoDBRef Klub { get; set; }

    }
}
