using DVG.WIS.Utilities;
using DVG.WIS.Utilities.Configs;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Core.Constants
{
    public class StaticVariable
    {
        public static string DomainImage = AppSettings.Instance.GetString("DomainImage");

        public static string Domain = AppSettings.Instance.GetString("Domain");
        public static string DomainMobile = AppSettings.Instance.GetString("DomainMobile");

        public static string SiteName = AppSettings.Instance.GetString("SiteName");

        public static string BaseUrl = AppSettings.Instance.GetString("BaseUrl");
        public static string BaseUrlNoSlash = BaseUrl.TrimEnd('/');
        public static string BaseMobileUrl = AppSettings.Instance.GetString("BaseMobileUrl");
        public static string BaseMobileUrlNoSlash = BaseMobileUrl.TrimEnd('/');

        public static string NoImage = AppSettings.Instance.GetString("NoImage");
        public static string NoImageSquare = AppSettings.Instance.GetString("NoImageSquare");
        public static string NoImage43 = AppSettings.Instance.GetString("NoImage43");
        public static string NoImage169 = AppSettings.Instance.GetString("NoImage169");

        public static string ResizeSizeContentMobile = AppSettings.Instance.GetString("ResizeSizeContentMobile");

        public static int WeekCacheTime = AppSettings.Instance.GetInt32(Const.KeyWeekCacheTime);
        public static int LongCacheTime = AppSettings.Instance.GetInt32(Const.KeyLongCacheTime);
        public static int MediumCacheTime = AppSettings.Instance.GetInt32(Const.KeyMediumCacheTime);
        public static int ShortCacheTime = AppSettings.Instance.GetInt32(Const.KeyShortCacheTime);

        // API
        public static string AhaMoveAPI = AppSettings.Instance.GetString("AhaMoveAPI");
        public static string AhaMoveKey = AppSettings.Instance.GetString("AhaMoveKey");
        public static string AhaMoveServiceId = AppSettings.Instance.GetString("AhaMoveServiceId");
        public static string Kitchen1Address = AppSettings.Instance.GetString("Kitchen1Address");
        public static string Kitchen1Phone = AppSettings.Instance.GetString("Kitchen1Phone");

        public static string PrintKitchenApi = AppSettings.Instance.GetString("PrintKitchenApi");
        public static string PrintCashierApi = AppSettings.Instance.GetString("PrintCashierApi");
        public static string APISuggestAddress = AppSettings.Instance.GetString("API_SuggestAddress");
        public static bool EnablePrint = AppSettings.Instance.GetBool("EnablePrint");

        #region Crop Image
        public static string FacebookAvatar = AvatarConfigs.Value("FacebookAvatar", "/crop/620x324");
        public static string StandardAvatar = AvatarConfigs.Value("StandardAvatar");

        public static string HomTimelineBig = AvatarConfigs.Value("HomTimelineBig");
        public static string HomeHighlightMediumAvatar = AvatarConfigs.Value("HomeHighlightMediumAvatar");
        public static string HomeHighlightSmallAvatar = AvatarConfigs.Value("HomeHighlightSmallAvatar");
        public static string HomeMostViewAvatar = AvatarConfigs.Value("HomeMostViewAvatar");
        public static string HomeGalleryAvatar = AvatarConfigs.Value("HomeGalleryAvatar");
        public static string HomReviewAvatar = AvatarConfigs.Value("HomReviewAvatar");
        public static string HomVideoBigAvatar = AvatarConfigs.Value("HomVideoBigAvatar");

        public static string NewsHighlight = AvatarConfigs.Value("NewsHighlight");
        public static string ListNewsAvatar = AvatarConfigs.Value("ListNewsAvatar");
        public static string HighLightCategoryAvatar = AvatarConfigs.Value("HighLightCategoryAvatar");
        public static string DetailRelationAvatar = AvatarConfigs.Value("DetailRelationAvatar");

        public static string GalleryAvatar = AvatarConfigs.Value("GalleryAvatar");
        public static string GallerySmallAvatar = AvatarConfigs.Value("GallerySmallAvatar");
        public static string SlideBig = AvatarConfigs.Value("SlideBig");
        public static string SlideBigResize = AvatarConfigs.Value("SlideBigResize");
        public static string SlideAvatarRelation = AvatarConfigs.Value("SlideAvatarRelation");

        public static string CropSizeCMS = AvatarConfigs.Value("CropSizeCMS");
        public static string SlideNewsSubImage = AvatarConfigs.Value("SlideNewsSubImage");

        public static string CarInfoAvatar = AvatarConfigs.Value("CarInfoAvatar");
        public static string CarInfoDetailAvatar = AvatarConfigs.Value("CarInfoDetailAvatar");

        public static string MobileHighlightAvatar = AvatarConfigs.Value("MobileHighlightAvatar");
        public static string MobileBigAvatar = AvatarConfigs.Value("MobileBigAvatar");
        public static string MobileMediumAvatar = AvatarConfigs.Value("MobileMediumAvatar");
        public static string MobileSmallAvatar = AvatarConfigs.Value("MobileSmallAvatar");
        public static string MobileSmallNewsAvatar = AvatarConfigs.Value("MobileSmallNewsAvatar");
        public static string MobileResizeSlideBig = AvatarConfigs.Value("MobileResizeSlideBig");
        public static string MobileListAvatar = AvatarConfigs.Value("MobileListAvatar");
        public static string MobileOneCateAvatar = AvatarConfigs.Value("MobileOneCateAvatar");
        public static string MobilePlayListAvatar = AvatarConfigs.Value("MobilePlayListAvatar");

        public static string SizeImage480x480 = AppSettings.Instance.GetString("SizeImage480x480");
        public static string SizeImage480x270 = AppSettings.Instance.GetString("SizeImage480x270");
        public static string SizeImage262x262 = AppSettings.Instance.GetString("SizeImage262x262");
        public static string SizeImage120x120 = AppSettings.Instance.GetString("SizeImage120x120");
        #endregion

        #region Meta tag

        public static string PrefixTitle = "";
        public static string KeyNewList = MetaConfigs.Value("MetaKeywordNews");

        public static string DomainTitle = MetaConfigs.Value("DomainTitle");

        public static string MetaMainTitle = MetaConfigs.Value("MainTitle");
        public static string MetaMainDescription = MetaConfigs.Value("MainDescription");
        public static string MetaMainKeyword = MetaConfigs.Value("MainKeyword");

        public static string NewsTitle = MetaConfigs.Value("NewsTitle");
        public static string NewsDescription = MetaConfigs.Value("NewsDescription");
        public static string NewsMetaKeyword = MetaConfigs.Value("NewsMetaKeyword");

        public static string TagsNewsTitle = MetaConfigs.Value("TagsNewsTitle");

        public static string GalleryTitle = MetaConfigs.Value("GalleryTitle");
        public static string GalleryDescription = MetaConfigs.Value("GalleryDescription");
        public static string GalleryMetaKeyword = MetaConfigs.Value("GalleryMetaKeyword");

        public static string CarInfoTitle = MetaConfigs.Value("CarInfoTitle");
        public static string CarInfoDescription = MetaConfigs.Value("CarInfoDescription");
        public static string CarInfoMetaKeyword = MetaConfigs.Value("CarInfoMetaKeyword");

        public static string AssessmentTitle = MetaConfigs.Value("AssessmentTitle");
        public static string AssessmentDescription = MetaConfigs.Value("AssessmentDescription");
        public static string AssessmentMetaKeyword = MetaConfigs.Value("AssessmentMetaKeyword");

        public static string AdvicesTitle = MetaConfigs.Value("AdvicesTitle");
        public static string AdvicesDescription = MetaConfigs.Value("AdvicesDescription");
        public static string AdvicesMetaKeyword = MetaConfigs.Value("AdvicesMetaKeyword");

        public static string CarPriceTitle = MetaConfigs.Value("CarPriceTitle");
        public static string CarPriceDescription = MetaConfigs.Value("CarPriceDescription");
        public static string CarPriceMetaKeyword = MetaConfigs.Value("CarPriceMetaKeyword");

        public static string BikePriceTitle = MetaConfigs.Value("BikePriceTitle");
        public static string BikePriceDescription = MetaConfigs.Value("BikePriceDescription");
        public static string BikePriceMetaKeyword = MetaConfigs.Value("BikePriceMetaKeyword");

        public static string AboutTitle = MetaConfigs.Value("AboutTitle");
        public static string AboutDescription = MetaConfigs.Value("AboutDescription");

        #endregion

        #region Email

        public static string EmailSupport = AppSettings.Instance.GetString("EmailSupport");
        public static string EmailMaster = AppSettings.Instance.GetString("EmailMaster", "noreply.topbank.vn@gmail.com");
        public static string EmailNoReply = AppSettings.Instance.GetString("EmailNoReply", "noreply.topbank.vn@gmail.com");
        public static string EmailContact = AppSettings.Instance.GetString("Mail-Contact");
        public static string PassEmailNoReply = AppSettings.Instance.GetString("PassEmailNoReply", "fintech2016");

        #endregion

        public static string PrefixCarInfoEmpty = AppSettings.Instance.GetString("PrefixCarInfoSpecEmpty", "x");

        public static int TopProductFocus = 8;
        public static int TopArticleInProductCategory = 8;
        public static int TopArticleFocus = 8;
        public static int TopArticleList = 10;
    }
}