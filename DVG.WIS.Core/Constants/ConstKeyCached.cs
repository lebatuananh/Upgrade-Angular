using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core
{
    public class ConstKeyCached
    {
        public const string GetNewsLandingDetail = "GetNewsLandingDetail";
        public const string GetNewsLandingTop = "GetNewsLandingTop";
        public const string GetListNewsRelationByNewsId = "GetListNewsRelationByNewsId";

        public const string GetNewsDetail = "GetNewsDetail";
        public const string GetNewsById = "GetNewsById";
        public const string GetCarInfoDetailsByNewsId = "GetCarInfoDetailsByNewsId";
        public const string GetNewsDetailPrice = "GetNewsDetailPrice";
        public const string GetPriceByUrlMd5 = "GetPriceByUrlMd5";
        public const string GetNewsPriceSameBrand = "GetNewsPriceSameBrand";
        public const string GetNewsPriceByBrandModel = "GetNewsPriceByBrandModel";
        public const string GetNewsPriceByBrandModelTotalRows = "GetNewsPriceByBrandModelTotalRows";
        public const string GetTopNewsByPosition = "GetTopNewsByPosition";
        public const string GetTopLatestByDisplayStyle = "GetTopLatestByDisplayStyle";
        public const string GetAllByType = "GetAllByType";
        public const string GetAllByCateId = "GetAllByCateId"; 
        public const string GetTopMostView = "GetTopMostView"; 
        public const string GetNewsAssessmentDetail = "GetNewsAssessmentDetail";
        public const string GetNewsByReferId = "GetNewsByReferId";
        public const string GetNewsByCatUrl = "GetNewsByCatUrl";
        public const string GetHighLightNews = "GetHighLightNews";
        public const string GetAllBank = "GetAllBank";
        public const string BankGetById = "BankGetById";
        public const string GetPartnerBank = "GetPartnerBank";
        public const string CreditCardGetTop = "CreditCardGetTop";
        public const string GetByListId = "GetByListId";
        public const string CardGetById = "CardGetById";
        public const string CardGetList = "CardGetList";
        public const string NewsGetLatest = "NewsGetLatest";
        public const string NewsGetTopLatest = "NewsGetTopLatest";
        public const string NewsGetTopLatestV2 = "NewsGetTopLatestV2";
        public const string NewsGetTopByTopic = "NewsGetTopByTopic";
        public const string GetTopHighLightNews = "GetTopHighLightNews";
        public const string GetListByCategoryPaging = "GetListByCategoryPaging";
        public const string GetListNewsCarPaging = "GetListNewsCarPaging";
        public const string GetListNewsCateIdPaging = "GetListNewsCateIdPaging";
        public const string GetListByTypeAndStatus = "GetListByTypeAndStatus";
        public const string GetListHighLightCategory = "GetListHighLightCategory";
        public const string GetListByTagsName = "GetListByTagsName";
        public const string GetListByTagId = "GetListByTagId";
        public const string NewsGetNewsOther = "NewsGetNewsOther";
        public const string NewsGetRelateNewsByTags = "NewsGetRelateNewsByTags";
        public const string NewsViewCount = "NewsViewCount";
        public const string NewssLetterKey = "NewssLetterKey";
        public const string GetTopNewsRelationByNewsOrTags = "GetTopNewsRelationByNewsOrTags";
        public const string GetTopRecommendByNewsId = "GetTopRecommendByNewsId";
        public const string GetAssessmentByCateId = "GetAssessmentByCateId";
        public const string GetAssessmentByType = "GetAssessmentByType"; 
        public const string GetTopAssessmentByPosition = "GetTopAssessmentByPosition";
        public const string GetByOriginalUrl = "GetByOriginalUrl"; 
        public const string GetCarInfoByType = "GetCarInfoByType";
        public const string GetListCarInfoPaging = "GetListCarInfoPaging";
        public const string GetListAssessmentPaging = "GetListAssessmentPaging";
        public const string GetMenuHeadingByNewsId = "GetMenuHeadingByNewsId";
        public const string NewsCarRelationByNewsId = "NewsCarRelationByNewsId";
        public const string TopNews = "TopNews";
        public const string GetNewsSameTopic = "GetNewsSameTopic"; 
        public const string GetListImageByNewsId = "GetListImageByNewsId";
        public const string NewsSearch = "NewsSearch";
        public const string NewsSearchCount = "NewsSearchCount";
        public const string NewsSearchCountCarInfo = "NewsSearchCountCarInfo";
        public const string GetTopNewsByPositionAndType = "GetTopNewsByPositionAndType";
        public const string GetTopNewsByCateId = "GetTopNewsByCateId";
        public const string GetTopNewsByNewsTypeId = "GetTopNewsByNewsTypeId";
        public const string GetCategoryAll = "GetAllOfCategory";
        public const string GetTopicByCateId = "GetTopicByCateId";
        public const string GetTopicByListTags = "GetTopicByListTags";
        public const string GetTopicByTopicId = "GetTopicByTopicId";
        public const string GetTopNewsByTopicId = "GetTopNewsByTopicId";
        public const string GetTopNewsMostView = "GetTopNewsMostView";
        public const string GetTopNewsByDisplayPosition = "GetTopNewsByDisplayPosition";
        public const string GetTopSearch = "GetTopSearch";
        public const string GetTopVideoViewMost = "GetTopVideoViewMost";
        public const string GetTopVideoRelation = "GetTopVideoRelation";
        public const string GetTopCarModelHot = "GetTopCarModelHot";
        public const string GetCateById = "GetCateById";
        public const string GetListGalleryRelation = "GetListGalleryRelation";
        public const string GetTopCarSegment = "GetTopCarSegment";
        public const string GetdetailCarSegment = "GetdetailCarSegment";
        public const string GetTopAssessment = "GetTopAssessment";
        public const string NewsImageGetListByNewsId = "NewsImageGetListByNewsId";
        public const string CarStyleAll = "CarStyleAll";
        public const string GetAssessmentPaging = "GetAssessmentPaging";
        public const string GetNewsExtendByNewsId = "GetNewsExtendByNewsId";
        public const string GetTopNewsCarMostSale = "GetTopNewsCarMostSale";
        public const string GetAssessmentSameTopic = "GetAssessmentSameTopic";
        public const string AuthUserPermission = "AuthUserPermission";
        public const string RightNowBoGetByRangeDataDateLimit = "RightNowBoGetByRangeDataDateLimit";
        public const string GoogleSearchAnalyticsBoGetByRangeDataDateLimit = "GoogleSearchAnalyticsBoGetByRangeDataDateLimit";
        public const string NewsAutoSaveByUserId = "NewsAutoSaveByUserId";

        public const string CarInforGetByNewsId = "CarInforGetByNewsId";

        public const string GetCarInfoAnalysTitle = "GetCarInfoAnalysTitle";
        public const string GetListSameVersion = "GetListSameVersion";
        public const string GetCarInfoByStatus = "GetCarInfoByStatus";
        public const string GetListCarinfoPaging = "GetListCarinfoPaging";
        public const string GetTopByStyleId = "GetTopByStyleId";
        public const string GetListCarPrice = "GetListCarPrice";
        public const string GetTopCarinfoSamePrice = "GetTopCarinfoSamePrice";
        public const string GetListPricingHighlight = "GetListPricingHighlight";
        public const string GetTopNewsRelationDetail = "GetTopNewsRelationDetail";
        public const string AnalyticsTitleCarPrice = "AnalyticsTitleCarPrice";
        public const string AnalyticsTitleBikePrice = "AnalyticsTitleBikePrice";

        #region BoxNewsEmbed

        public const string BoxNewsEmbedGetList = "BoxNewsEmbedGetList";

        #endregion

        #region NewsPosition
        public const string GetTopByPosition = "GetTopByPosition";
        #endregion

        #region Bank
        public const string BankGetByCode = "BankGetByCode";

        #endregion

        #region province

        public const string ProvinceGetList = "ProvinceGetList";
        public const string ProvinceGetById = "ProvinceGetById";
        public const string ProvinceGetByUnsignName = "ProvinceGetByUnsignName";
        public const string ProvinceGetLocationInfo = "ProvinceGetLocationInfo";

        #endregion

        #region Tags

        public const string TagsGetById = "TagsGetById";
        public const string TagsGetByName = "TagsGetByName";
        public const string TagsGetByUnsignName = "TagsGetByUnsignName";
        public const string TagsGetList = "TagsGetList";
        public const string TagsGetListByName = "TagsGetListByName";
        public const string TagsGetListByNewsId = "TagsGetListByNewsId";
        #endregion

        #region CarInfo
        public const string CarInforDetailById = "CarInforDetailById";
        public const string CarInforDetailByNewsId = "CarInforDetailByNewsId";
        public const string CarInforListSameVersion = "CarInforListSameVersion";
        public const string CarInforAnalysTitle = "CarInforAnalysTitle";
        public const string CarInforGetByVersion = "CarInforGetByVersion";
        public const string CarInforGetListGetSimilar = "CarInforGetListGetSimilar";
        public const string CarInfoGetByBrandAndModel = "CarInfoGetByBrandAndModel";

        public const string GetListYearFromCarinfo = "GetListYearFromCarinfo";

        #endregion

        #region CarModels

        public const string CarModelsGetAll = "CarModelsGetAll";
        public const string CarModelsGetByBrandId = "CarModelsGetByBrandId";

        #endregion

        #region CarBrand

        public const string CarBrandGetAll = "CarBrandGetAll";

        #endregion

        #region CarModelDetails

        public const string CarModelDetailGetAll = "CarModelDetailGetAll";
        public const string CarModelDetailGetByBrandAndModel = "CarModelDetailGetByBrandAndModel";
        public const string CarModelDetailGetByBrandAndModelV2 = "CarModelDetailGetByBrandAndModelV2";
        public const string CarModelDetailGetByModelId = "CarModelDetailGetByModelId";

        #endregion

        #region CarInfoGroup

        public const string CarInfoGroupGetAll = "CarInfoGroupGetAll";

        #endregion

        #region CarInfoDetail

        public const string CarInfoDetailGetByGroupId = "CarInfoDetailGetByGroupId";

        #endregion

        #region BoxLinkSEO
        public const string BoxLinkSEOGetAll = "BoxLinkSEOGetAll";
        public const string BoxLinkSEOGetAllDetail = "BoxLinkSEOGetAllDetail";
        #endregion

        #region Banner
        public const string BannerListAll = "BannerListAll";
        #endregion

        #region BikeBrand

        public const string BikeBrandGetAll = "BikeBrandGetAll";
        public const string BikeModelsGetList = "BikeModelsGetAll";
        public const string GetBikePrice = "GetBikePrice";
        //public const string GetListBrandModelAll = "GetListBrandModelAll";

        #endregion

        #region OMS
        public const string CashierConstraintKey = "CashierConstraintKey";

        public const string JWTToken = "JWTToken";
        public const string ObsoleteJWTToken = "ObsoleteJWTToken";

        public const string NotifyAccount = "NotifyAccount";
        #endregion
    }
}
