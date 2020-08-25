using DVG.WIS.Caching.Interfaces;
using DVG.WIS.Utilities;
using DVG.WIS.Utilities.Serialization;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DVG.WIS.Caching.Test
{
    public class CachingTest
    {
        ICached cacheClient;
        [SetUp]
        public void Setup()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", true)
              .Build();

            AppSettings.Instance.SetConfiguration(configuration);

            string json = NewtonJson.Serialize(configuration);

            cacheClient = new RedisCached(new Configs.RedisConfiguration()
            {
                Server = "172.16.0.102",
                Port = 6392,
                Database = 6,
                Name = "TEST",
                Timeout = 3600
            });
            // cacheClient = new RedisCached();
        }

        [Test]
        public void SetStringToRedis()
        {
            bool result = cacheClient.Set<string>("kitchen:test", "1234567", 10);
            Assert.IsTrue(result);
        }

        [Test]
        public void GetStringFromRedis()
        {
            string result = cacheClient.Get("kitchen:test");
            Assert.IsTrue(!string.IsNullOrEmpty(result));
            Console.WriteLine(result);
        }

        [Test]
        public void SetToRedis()
        {
            TestModel model = new TestModel()
            {
                Id = 1,
                Name = "Narnia"
            };

            bool result = cacheClient.Set<TestModel>("kitchen:test:model", model, 10);
            Assert.IsTrue(result);
        }

        [Test]
        public void GetFromRedis()
        {
            TestModel result = cacheClient.Get<TestModel>("kitchen:test:model");
            Assert.IsTrue(result != null);
            Console.WriteLine(NewtonJson.Serialize(result));
        }

        [Test]
        public void SorteSetSet()
        {
            var key = "test_sortedSet";
            cacheClient.SortedSetAddAsync<string>(key, "Mess 1", 1).Wait();
            cacheClient.SortedSetAddAsync<string>(key, "Mess 2", 2).Wait();
            var result = cacheClient.SortedSetRangeByRankAsync<string>(key).Result;
        }
    }

    [Serializable]
    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}