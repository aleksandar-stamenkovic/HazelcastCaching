﻿using Hazelcast.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HazelcastCaching.Models
{
    public class Product : IIdentifiedDataSerializable
    {
        public const int ClassId = 100;

        [BsonId]
        public ObjectId Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }

        public void ReadData(IObjectDataInput input)
        {
            Name = input.ReadString();
            Price = input.ReadFloat();
            Description = input.ReadString();
        }

        public void WriteData(IObjectDataOutput output)
        {
            output.WriteString(Name);
            output.WriteFloat(Price);
            output.WriteString(Description);
        }

        public int FactoryId => SampleDataSerializableFactory.FactoryId;
        int IIdentifiedDataSerializable.ClassId => 100;
    }

    // [TODO]: Move this class to a new file.
    public class SampleDataSerializableFactory : IDataSerializableFactory
    {
        public const int FactoryId = 1000;

        public IIdentifiedDataSerializable Create(int typeId)
        {
            if (typeId == Product.ClassId) return new Product();
            return null;
        }
    }
}
