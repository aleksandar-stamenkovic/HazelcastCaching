using Hazelcast.DistributedObjects;
using Hazelcast.Serialization;
using HazelcastCaching.Models;

namespace HazelcastCaching.EntryProcessors
{
    public class DecreasePriceEntryProcessor : IEntryProcessor<Product>, IIdentifiedDataSerializable
    {
        public int FactoryId => 6;

        public int ClassId => 2;

        private float _value;

        public DecreasePriceEntryProcessor(float value)
        {
            _value = value;
        }

        public void ReadData(IObjectDataInput input)
        {
            _value = input.ReadFloat();
        }

        public void WriteData(IObjectDataOutput output)
        {
            output.WriteFloat(_value);
        }
    }
}
