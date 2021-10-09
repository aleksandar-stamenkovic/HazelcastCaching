using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HazelcastCaching
{
    internal static class ServiceSettings
    {
        #region Mongo Settings
        internal static string MongoConnectionString => "mongodb://localhost:27017";
        internal static string MongoDatabaseName => "my_products_db";
        internal static string MongoCollectionName => "products";
        #endregion

        #region Hazelcast Settings
        internal static string HazelcastClusterName => "dev";
        internal static string HazelcastClientName => "dotnet";
        /*internal static IEnumerable<string> HazelcastNetworkingAddresses => new List<string>
        {
            "127.0.0.1:5701",
        };*/
        internal static string HazelcastCacheName => "products123";
        #endregion
    }
}
