using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Projekat3
{
    public class Klub
    {
        public ObjectId Id { get; set; }
        public string naziv { get; set; }
        public string konferencija { get; set; }
        public string divizija { get; set; }
        public string vlasnik { get; set; }
        public string dvorana { get; set; }
        public List<string> titule { get; set; }
        public List<MongoDBRef> Kosarkasi { get; set; }

        public Klub()
        {
            Kosarkasi = new List<MongoDBRef>();
        }
    }
}
