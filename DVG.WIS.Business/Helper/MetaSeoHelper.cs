using DVG.WIS.Caching;
using DVG.WIS.Entities;
using DVG.WIS.Utilities;
using DVG.WIS.Utilities.Serialization;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace DVG.WIS.Business.Helper
{
    public class MetaSeoHelper
    {
        private static readonly Lazy<MetaSeoHelper> _lazy = new Lazy<MetaSeoHelper>(() => new MetaSeoHelper());
        public static readonly MetaSeoHelper Instance = _lazy.Value;

        private readonly IISCached _cache;

        private const string fileName = "meta.json";
        private readonly string filePath;
        private const string cacheKey = "cache_file_meta";

        public MetaSeoHelper()
        {
            _cache = new IISCached();
            filePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Configs")).GetFileInfo(fileName).PhysicalPath;
        }

        public MetaConfig GetAll()
        {
            var rule = _cache.Get<MetaConfig>(cacheKey);
            if(rule == null)
            {
                var json = FileConfigHelper.GetFileContent(filePath);
                if (json != "")
                {
                    rule = NewtonJson.Deserialize<MetaConfig>(json);

                    var dependency = new FileCacheDependency(fileName);
                    _cache.Set(cacheKey, rule, dependency);
                }
            }
            return rule;
        }
    }
}
