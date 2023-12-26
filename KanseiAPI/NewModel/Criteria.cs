﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace KanseiAPI.NewModel
{
    public class Criteria
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        private string _id;

        [BsonElement("name")]
        private string _name;

        public Criteria(string id, string name)
        {
            _id = id;
            _name = name;
        }
        public Criteria() { }

        public string Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
    }
}
