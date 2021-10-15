using Hazelcast;
using Hazelcast.Serialization;
using HazelcastCaching.EntryProcessors;
using HazelcastCaching.Models;
using System;
using System.Threading.Tasks;

namespace HazelcastCaching.Caching
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

        public async Task DeleteFromCacheAsync(string cacheName, KeyType key)
        {
            var map = await client.GetMapAsync<KeyType, ValueType>(cacheName);
            await map.DeleteAsync(key);
        }

        public async Task<Product> ExecuteAsync(string cacheName, ChangePriceType changePriceType, KeyType key, float percent)
        {
            var map = await client.GetMapAsync<KeyType, ValueType>(cacheName);

            return changePriceType switch
            {
                ChangePriceType.Increase => await map.ExecuteAsync(new IncreasePriceEntryProcessor(percent), key),
                ChangePriceType.Decrease => await map.ExecuteAsync(new DecreasePriceEntryProcessor(percent), key),
                _ => throw new Exception("The specified ChangePriceType not provided."),
            };
        }
    }
}
