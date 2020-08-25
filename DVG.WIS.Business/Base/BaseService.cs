using DVG.WIS.Caching;
using DVG.WIS.Caching.Configs;
using DVG.WIS.Caching.Interfaces;
using DVG.WIS.Core.Constants;
using DVG.WIS.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DVG.WIS.Business.Base
{
    public abstract class BaseService : IBaseService
    {
        protected readonly ICached cacheClient;
        protected int shortCacheDuration = StaticVariable.ShortCacheTime;
        protected int mediumCacheDuration = StaticVariable.MediumCacheTime;
        protected int longCacheDuration = StaticVariable.LongCacheTime;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public static bool AllowCached = AppSettings.Instance.GetInt32("CacheType", (int)CachedType.NoCache) > (int)CachedType.NoCache ? true : false;
        private int cacheType = AppSettings.Instance.GetInt32("CacheType", (int)CachedType.Redis);

        #region constructors 
        public BaseService(ICached cacheClient)
        {
            //switch (cacheType)
            //{
            //    case (int)CachedType.Redis:
            //        cacheClient = new RedisCached();
            //        AllowCached = true;
            //        break;
            //    case (int)CachedType.NoCache:
            //    default:
            //        AllowCached = false;
            //        break;
            //}
            this.cacheClient = cacheClient;

            _httpContextAccessor = new HttpContextAccessor();
        }

        public BaseService(ICached cached, IHttpContextAccessor httpContextAccessor)
        {
            this.cacheClient = cached;
            this._httpContextAccessor = httpContextAccessor;
        }

        #endregion

        protected T Execute<T>(Func<T> func, string funcName, int cachedInMinutes, bool isSerialize = false, params object[] args)
        {
            if (!AllowCached)
            {
                return func.Invoke();
            }

            string cacheName = isSerialize ? KeyCacheHelper.GenCacheKeySerialize(funcName, args) : KeyCacheHelper.GenCacheKey(funcName, args);

            T obj = cacheClient.Get<T>(cacheName, _httpContextAccessor.HttpContext);

            if (obj == null || EqualityComparer<T>.Default.Equals(obj, default(T)))
            {
                obj = func.Invoke();
                if (obj != null && !EqualityComparer<T>.Default.Equals(obj, default(T)))
                {
                    cachedInMinutes = cachedInMinutes > 0 ? cachedInMinutes : shortCacheDuration;
                    cacheClient.Set<T>(cacheName, obj, cachedInMinutes);
                }
            }

            return obj;
        }

        protected T Execute<T>(Func<T> func, string funcName, int cachedInMinutes, params object[] args)
        {
            if (!AllowCached)
            {
                return func.Invoke();
            }

            string cacheName = KeyCacheHelper.GenCacheKeySerialize(funcName, args);

            T obj = cacheClient.Get<T>(cacheName, _httpContextAccessor.HttpContext);

            if (obj == null || EqualityComparer<T>.Default.Equals(obj, default(T)))
            {
                obj = func.Invoke();
                if (obj != null && !EqualityComparer<T>.Default.Equals(obj, default(T)))
                {
                    cachedInMinutes = cachedInMinutes > 0 ? cachedInMinutes : shortCacheDuration;
                    cacheClient.Set<T>(cacheName, obj, cachedInMinutes);
                }
            }

            return obj;
        }

        protected async Task<T> ExecuteAsync<T>(Func<Task<T>> func, int cachedInMinutes, params object[] args)
        {
            if (!AllowCached)
            {
                return await func.Invoke();
            }

            string cacheKey = CreateCacheKey(func, args);
            T obj = await cacheClient.GetAsync<T>(cacheKey, _httpContextAccessor.HttpContext);

            if (obj == null || EqualityComparer<T>.Default.Equals(obj, default(T)))
            {
                obj = await func.Invoke();
                if (obj != null && !EqualityComparer<T>.Default.Equals(obj, default(T)))
                {
                    cachedInMinutes = cachedInMinutes > 0 ? cachedInMinutes : shortCacheDuration;
                    await cacheClient.SetAsync(cacheKey, obj, cachedInMinutes);
                }
            }

            return obj;
        }

        protected T Execute<T>(Func<T> func, int cachedInMinutes, params object[] args)
        {
            if (!AllowCached)
            {
                return func.Invoke();
            }

            string cacheKey = CreateCacheKey(func, args);
            T obj = cacheClient.Get<T>(cacheKey, _httpContextAccessor.HttpContext);

            if (obj == null || EqualityComparer<T>.Default.Equals(obj, default(T)))
            {
                obj = func.Invoke();
                if (obj != null && !EqualityComparer<T>.Default.Equals(obj, default(T)))
                {
                    cachedInMinutes = cachedInMinutes > 0 ? cachedInMinutes : shortCacheDuration;
                    cacheClient.Set<T>(cacheKey, obj, cachedInMinutes);
                }
            }

            return obj;
        }

        protected T Execute<T>(Func<T> func, int cachedInMinutes, Func<string> funcGenarateKeyCached)
        {
            if (!AllowCached)
            {
                return func.Invoke();
            }

            string cacheName = funcGenarateKeyCached.Invoke();

            T obj = cacheClient.Get<T>(cacheName, _httpContextAccessor.HttpContext);

            if (obj == null || EqualityComparer<T>.Default.Equals(obj, default(T)))
            {
                obj = func.Invoke();
                if (obj != null && !EqualityComparer<T>.Default.Equals(obj, default(T)))
                {
                    cachedInMinutes = cachedInMinutes > 0 ? cachedInMinutes : shortCacheDuration;
                    cacheClient.Set<T>(cacheName, obj, cachedInMinutes);
                }
            }

            return obj;
        }

        protected T Execute<T>(Func<T> func, int cachedInMinutes, string cacheName)
        {
            if (!AllowCached)
            {
                return func.Invoke();
            }

            T obj = cacheClient.Get<T>(cacheName, _httpContextAccessor.HttpContext);

            if (obj == null || EqualityComparer<T>.Default.Equals(obj, default(T)))
            {
                obj = func.Invoke();
                if (obj != null && !EqualityComparer<T>.Default.Equals(obj, default(T)))
                {
                    cachedInMinutes = cachedInMinutes > 0 ? cachedInMinutes : shortCacheDuration;
                    cacheClient.Set<T>(cacheName, obj, cachedInMinutes);
                }
            }

            return obj;
        }

        protected void RemoveCache<T>(string funcName, params object[] args)
        {
            var cacheName = "<" + funcName + ">" + ":" + GetFriendlyTypeName(typeof(T));
            string cacheKey = KeyCacheHelper.GenCacheKeySerialize(cacheName, args);
            cacheClient.Remove(cacheKey);
        }

        private string CreateCacheKey<T>(Func<T> func, object[] args)
        {
            var cacheName = func.Method.Name.Replace("b__0", string.Empty) + ":" + GetFriendlyTypeName(typeof(T));
            return KeyCacheHelper.GenCacheKeySerialize(cacheName, args);
        }

        /// <summary>
        /// Toan add for get ObjectName from generictype
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetFriendlyTypeName(Type type)
        {
            string friendlyName = type.Name;
            if (type.IsGenericType)
            {
                int iBacktick = friendlyName.IndexOf('`');
                if (iBacktick > 0)
                {
                    friendlyName = friendlyName.Remove(iBacktick);
                }
                friendlyName += "<";
                Type[] typeParameters = type.GetGenericArguments();
                string typeParamName = "";
                for (int i = 0; i < typeParameters.Length; ++i)
                {
                    typeParamName = GetFriendlyTypeName(typeParameters[i]);
                    friendlyName += (i == 0 ? typeParamName : "," + typeParamName);
                }
                friendlyName += ">";

            }

            return friendlyName.Replace('+', '.');
        }
    }
}