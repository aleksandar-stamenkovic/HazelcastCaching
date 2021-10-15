using Hazelcast.Serialization;

namespace HazelcastCaching.Models
{
    public class ProductSerializableFactory : IDataSerializableFactory
    {
        public const int FactoryId = 1000;

        public IIdentifiedDataSerializable Create(int typeId)
        {
            if (typeId == Product.ClassId) return new Product();
            return null;
        }
    }
}
