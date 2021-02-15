using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Bson;
using MongoDB.Driver;

namespace Projekat3
{
    public class Kosarkas
    {
        public ObjectId Id { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public string pozicija { get; set; }
        public int brojnadresu { get; set; }
        public Int32 visina { get; set; }
        public Int32 tezina { get; set; }
        public string koledz { get; set; }
        public MongoDBRef Klub { get; set; }
//        public string slika { get; set; }
        public byte [] slika { get; set; }
    }
}
