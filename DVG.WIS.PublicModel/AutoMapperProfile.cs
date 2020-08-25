using AutoMapper;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using DVG.WIS.Utilities;

namespace DVG.WIS.PublicModel
{
    public class AutoMapperProfile
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {

                //cfg.CreateMap<Entities.Article, ArticleOnListModel>()
                //.ForMember(i => i.Url, opt => opt.MapFrom(i => BuildUrlByType(i.Title, i.Url, "article")))
                //.ForMember(i => i.Avatar, opt => opt.MapFrom(i => CoreUtils.BuildAvatar(i.Avatar, StaticVariable.NoImage)))
                //;

                //cfg.CreateMap<Entities.Article, ArticleDetailModel>()
                //.ForMember(i => i.Url, opt => opt.MapFrom(i => BuildUrlByType(i.Title, i.Url, "article")))
                //.ForMember(i => i.Avatar, opt => opt.MapFrom(i => CoreUtils.BuildAvatar(i.Avatar, StaticVariable.NoImage)))
                ;
            });
        }

        private static string BuildUrlByType(string name, string url, string type)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                return "/" + url.TrimStart('/');
            }
            //else
            //{
            //    return type == "article"
            //        ? $"/{Utils.UnicodeToUnsignAndDash(name)}.html"
            //        : "/" + Utils.UnicodeToUnsignAndDash(name);
            //}
            return "/";
        }
    }
}