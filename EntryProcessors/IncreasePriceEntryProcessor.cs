using Hazelcast.DistributedObjects;
using Hazelcast.Serialization;
using HazelcastCaching.Models;

namespace HazelcastCaching.EntryProcessors
{
    public class IncreasePriceEntryProcessor : IEntryProcessor<Product>, IIdentifiedDataSerializable
    {
        public int FactoryId => 5;

        public int ClassId => 1;

        private float _value;

        public IncreasePriceEntryProcessor(float value)
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
