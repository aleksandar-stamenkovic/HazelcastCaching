﻿using Hazelcast.Serialization;
using System.Threading.Tasks;

namespace Service1.Caching
{
    public interface IHazelcastCaching<KeyType, ValueType> where ValueType : IIdentifiedDataSerializable
    {
        Task WriteToCacheAsync(string cacheName, KeyType key, ValueType value);
        Task<ValueType> ReadFromCacheAsync(string cacheName, KeyType key);
    }
}