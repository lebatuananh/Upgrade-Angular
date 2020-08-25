using DVG.WIS.Caching.Configs;
using DVG.WIS.Caching.Interfaces;
using DVG.WIS.Utilities.Logs;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DVG.WIS.Caching
{
    public class RedisCached : RedisClientBase, ICached
    {
        public RedisCached(): base()
        {

        }

        public RedisCached(RedisConfiguration configuration): base(configuration)
        {

        }

        #region sync 
        public bool Set<T>(string key, T item, int expireInMinute = 0)
        {
            var result = false;

            try
            {
                var currentTime = DateTime.Now;
                var expired = currentTime.AddMinutes(expireInMinute) - currentTime;

                var client = CreateInstanceForWrite();

                if (typeof(T) == typeof(string))
                {
                    if (expireInMinute > 0)
                        result = client.StringSet(key, CacheHelpers.Serialize(item), expired);
                    else
                        result = client.StringSet(key, CacheHelpers.Serialize(item));
                }
                else
                {
                    var bytes = ZipToBytes(item, key);

                    if (expireInMinute > 0)
                        result = client.StringSet(key, bytes, expired);
                    else
                        result = client.StringSet(key, bytes);
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);
            }

            return result;
        }

        public bool Remove(string key)
        {
            bool result = false;

            try
            {
                var client = CreateInstanceForWrite();

                result = client.KeyDelete(key);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return result;
        }

        public T Get<T>(string key, HttpContext context = null)
        {
            var result = default(T);
            try
            {
                var client = CreateInstanceRead();

                if (CacheHelpers.IsRequestClearCache(context)) client.KeyDelete(key);

                if (typeof(T) == typeof(string))
                {
                    RedisValue redisValue = client.StringGet(key);
                    result = (T)(object)redisValue.ToString();
                }
                else
                {
                    byte[] redisValue = client.StringGet(key);

                    result = UnZipFromBytes<T>(redisValue);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return result;
        }

        public string Get(string key, HttpContext context = null)
        {
            return Get<string>(key, context);
        }

        #endregion sync

        #region async

        public async Task<bool> SetAsync<T>(string key, T item, int expireInMinute = 0)
        {
            var result = false;

            try
            {
                RedisKey redisKey = key;
                RedisValue redisValue = ZipToBytes(item, key);

                var currentTime = DateTime.Now;
                var expired = currentTime.AddMinutes(expireInMinute) - currentTime;

                var client = await CreateInstanceAsyncForWrite();

                if (expireInMinute > 0)
                    result = await client.StringSetAsync(redisKey, redisValue, expired);
                else
                    result = await client.StringSetAsync(redisKey, redisValue);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return result;
        }

        public async Task<T> GetAsync<T>(string key, HttpContext context = null)
        {
            var result = default(T);
            try
            {
                var client = await CreateInstanceAsyncRead();

                if (CacheHelpers.IsRequestClearCache(context)) client.KeyDelete(key);

                byte[] bytes = await client.StringGetAsync(key);

                result = UnZipFromBytes<T>(bytes);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return result;
        }

        public async Task<long> EnqueueAsync<T>(string key, T item)
        {
            long result = 0;

            try
            {
                RedisKey redisKey = key;
                RedisValue redisValue = ZipToBytes(item, key);
                var client = await CreateInstanceAsyncForWrite();
                result = await client.ListLeftPushAsync(redisKey, redisValue);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return result;
        }

        public async Task<T> DequeueAsync<T>(string key)
        {
            var result = default(T);
            try
            {
                var client = await CreateInstanceAsyncRead();
                byte[] bytes = await client.ListRightPopAsync(key);
                result = UnZipFromBytes<T>(bytes);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return result;
        }

        public async Task<bool> SortedSetAddAsync<T>(string key, T item, double score)
        {
            var result = false;

            try
            {
                RedisKey redisKey = key;
                RedisValue redisValue = ZipToBytes(item, key);
                var client = await CreateInstanceAsyncForWrite();
                result = await client.SortedSetAddAsync(key, redisValue, score);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return result;
        }

        public async Task<List<T>> SortedSetRangeByRankAsync<T>(string key)
        {
            var result = new List<T>();
            try
            {
                var client = await CreateInstanceAsyncRead();

                var lstBytes = await client.SortedSetRangeByRankAsync(key);

                if(lstBytes.Any())
                {
                    foreach (var bytes in lstBytes)
                    {
                        var val = UnZipFromBytes<T>(bytes);
                        result.Add(val);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return result;
        }

        #endregion async

        public bool ContainsKey(string key)
        {
            bool result = false;

            try
            {
                var client = CreateInstanceRead();

                result = client.KeyExists(key);
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex);

            }

            return result;
        }
    }
}
