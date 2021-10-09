using Hazelcast;
using Hazelcast.Serialization;
using System.Threading.Tasks;

namespace Service1.Caching
{
    public class HazelcastCaching<KeyType, ValueType> : IHazelcastCaching<KeyType, ValueType> where ValueType : IIdentifiedDataSerializable
    {
        private readonly IHazelcastClient client;

        public HazelcastCaching(IHazelcastClient client)
        {
            this.client = client;
        }

        public async Task<ValueType> ReadFromCacheAsync(string cacheName, KeyType key)
        {
            var map = await client.GetMapAsync<KeyType, ValueType>(cacheName);
            var value = await map.GetAsync(key);

            return value;
        }

        public async Task WriteToCacheAsync(string cacheName, KeyType key, ValueType value)
        {
            var map = await client.GetMapAsync<KeyType, ValueType>(cacheName);
            await map.PutAsync(key, value);
        }
    }
}
