using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core
{
    public class ConstUrl
    {
        public const string RulesUrl = "/dieu-khoan";
        public const string PrivacyUrl = "/chinh-sach-rieng-tu";
        public const string TermAndCorditionUrl = "/qui-che-hoat-dong";
        public const string AboutUrl = "/gioi-thieu";
        public const string ContactUrl = "/lien-he";
        public const string SitemapUrl = "/sitemap";
        public const string FaqUrl = "/cau-hoi-thuong-gap";
        public const string ThanksRegister = "/dang-ky-nhan-email";

        public const string AdvicesUrl = "/tu-van";
        public const string AdvicesPagingUrl = "/tu-van/p{0}";
        public const string AdvicesCateUrl = "/tu-van/{0}-c{1}";
        public const string AdvicesCatePagingUrl = "/tu-van/{0}-c{1}/p{2}";
        public const string AdvicesDetailUrl = "/{0}/{1}-ad{2}";
        public const string AdvicesDetailUrlV1 = "/{0}/{1}-id{2}";
        public const string AdvicesDetailAMPUrl = "/{0}/{1}-ad{2}/amp";

        public const string AssessmentUrl = "/danh-gia-xe";
        public const string AssessmentPagingUrl = "/danh-gia-xe/p{0}";
        public const string AssessmentCateUrl = "/{0}-c{1}";
        public const string AssessmentCatePagingUrl = "/{0}-c{1}/p{2}";
        public const string AssessmentDetailUrl = "/{0}";

        public const string AssessmentDetailAMPUrl = "/danh-gia-xe/{0}-id{1}/amp";

        public const string AssessmentSegmentDefaultUrl = "/danh-gia-xe/phan-khuc/";
        public const string AssessmentBySegmentUrl = "/danh-gia-xe/phan-khuc/{0}-id{1}";
        public const string AssessmentBySegmentPagingUrl = "/danh-gia-xe/phan-khuc/{0}-id{1}/p{2}";

        //public const string AssessmentSearchUrl = "/danh-gia-xe-{0}";
        //public const string AssessmentSearchPagingUrl = "/danh-gia-xe-{0}/p{1}";
        public const string AssessmentSearchUrl = "/bai-viet-danh-gia-xe-{0}";
        public const string AssessmentSearchPagingUrl = "/bai-viet-danh-gia-xe-{0}/p{1}";

        public const string PriceUrl = "/gia-xe";
        public const string PriceCateUrl = "/gia-xe/{0}-c{1}";
        public const string PriceCatePagingUrl = "/gia-xe/{0}-c{1}/p{2}";
        public const string PriceCateSpecUrl = "/gia-xe-{0}";
        public const string PriceCateModelUrl = "/gia-xe-{0}-{1}";
        public const string PriceDetailUrl = "/gia-xe/{0}-id{1}";
        public const string PriceDetailSpecUrl = "/gia-xe-{0}/{1}";
        public const string PriceDetailAMPUrl = "/gia-xe/{0}-id{1}/amp";
        public const string PriceDetailSpecAMPUrl = "/gia-xe-{0}/amp";


        public const string BikeUrl = "/xe-may";

        public const string BikePriceUrl = "/gia-xe-may";
        public const string BikePriceSpecUrl = "/gia-xe-may-{0}";
        public const string BikePriceBrandModelUrl = "/gia-xe-may-{0}-{1}";
        public const string BikePriceCateUrl = "/gia-xe-may/{0}-c{1}";
        public const string BikePriceCatePagingUrl = "/gia-xe-may/{0}-c{1}/p{2}";
        public const string BikePriceDetailUrl = "/gia-xe-may/{0}-id{1}";
        public const string BikePriceDetailSpecUrl = "/gia-xe-may-{0}/{1}";
        public const string BikePriceDetailSpecAMPUrl = "/gia-xe-may-{0}/amp";
        //public const string BikePriceDetailAMPUrl = "/gia-xe-may/{0}-id{1}/amp";
        //public const string BikePriceDetailSpecAMPUrl = "/gia-xe-may-{0}/{1}/amp";

        #region Tìm kiếm giá xe

        public const string SearchCarPriceUrl = "/tim-kiem-gia-xe-{0}";
        public const string SearchCarPricePagingUrl = "/tim-kiem-gia-xe-{0}/p{1}";
        public const string SearchCarPriceAllUrl = "/tim-kiem-gia-xe-tat-ca-cac-hang";
        public const string SearchCarPriceAllPagingUrl = "/tim-kiem-gia-xe-tat-ca-cac-hang/p{0}";

        public const string SearchBikePriceUrl = "/tim-kiem-gia-xe-may-{0}";
        public const string SearchBikePricePagingUrl = "/tim-kiem-gia-xe-may-{0}/p{1}";
        public const string SearchBikePriceAllUrl = "/tim-kiem-gia-xe-may-tat-ca-cac-hang";
        public const string SearchBikePriceAllPagingUrl = "/tim-kiem-gia-xe-may-tat-ca-cac-hang/p{0}";

        #endregion

        public const string GalleryUrl = "/bo-suu-tap";
        public const string GalleryPagingUrl = "/bo-suu-tap/p{0}";
        public const string GalleryCateUrl = "/bo-suu-tap/{0}-c{1}";
        public const string GalleryCatePagingUrl = "/bo-suu-tap/{0}-c{1}/p{2}";
        public const string GalleryDetailUrl = "/{0}/{1}-glr{2}";
        public const string GalleryDetailUrlV1 = "/bo-suu-tap/{0}-id{1}";

        public const string GalleryVideoXe = "/video-xe";
        public const string GalleryVideoXePage = "/video-xe/p{0}";
        public const string GalleryVideoHaiHuoc = "/video-hai-huoc";
        public const string GalleryVideoHaiHuocPage = "/video-hai-huoc/p{0}";
        public const string GalleryImageXe = "/anh-xe";
        public const string GalleryImageXePage = "/anh-xe/p{0}";
        public const string GalleryImageGiaiTri = "/anh-giai-tri";
        public const string GalleryImageGiaiTriPage = "/anh-giai-tri/p{0}";

        public const string CarInfoUrl = "/thong-tin-xe";
        public const string CarInfoPagingUrl = "/thong-tin-xe/p{0}";
        public const string CarInfoCateUrl = "/thong-tin-xe/{0}-c{1}";
        public const string CarInfoCatePagingUrl = "/thong-tin-xe/{0}-c{1}/p{2}";
        public const string CarInfoDetailUrl = "/thong-tin-xe/{0}-id{1}";
        public const string CarInfoDetailAMPUrl = "/thong-tin-xe/{0}-id{1}/amp";

        public const string CarInfoSearchAllUrl = "/tat-ca-xe";
        public const string CarInfoSearchAll1Url = "/tat-ca-xe/p{0}";
        public const string CarInfoSearchUrl = "/xe-{0}";
        public const string CarInfoSearch2Url = "/xe-{0}/p{1}";
        public const string CarInfoSearch3Url = "/xe-{0}/f{1}";
        public const string CarInfoSearch4Url = "/xe-{0}/f{1}/p{2}";
        public const string CarInfoSearch5Url = "/xe-{0}/m{1}";
        public const string CarInfoSearch6Url = "/xe-{0}/m{1}/p{2}";
        public const string CarInfoSearch7Url = "/xe-{0}/m{1}/f{2}";
        public const string CarInfoSearch8Url = "/xe-{0}/m{1}/f{2}/p{3}";

        public const string NewsUrl = "/tin-tuc";
        public const string NewsPagingUrl = "/tin-tuc/p{0}";
        //public const string NewsCateUrl = "/tin-tuc/{0}-c{1}";
        //public const string NewsCatePagingUrl = "/tin-tuc/{0}-c{1}/p{2}";
        public const string NewsCateUrl = "/{0}";
        public const string NewsCatePagingUrl = "/{0}/p{1}";
        public const string NewsDetailUrl = "/{0}/{1}-id{2}";
        public const string NewsDetailAMPUrl = "/{0}/{1}-id{2}/amp";

        public const string OldSearchNewsUrl = "/tim-kiem/q-{0}";
        public const string OldSearchNewsPagingUrl = "/tim-kiem/q-{0}/p{1}";
        public const string OldSearchNewsByTypeUrl = "/tim-kiem/{0}/q-{1}";
        public const string OldSearchNewsByTypePagingUrl = "/tim-kiem/{0}/q-{1}/p{2}";

        public const string SearchNewsUrl = "/tim-kiem/{0}";
        public const string SearchNewsPagingUrl = "/tim-kiem/{0}/p{1}";
        public const string SearchNewsByTypeUrl = "/tim-kiem-{0}/{1}";
        public const string SearchNewsByTypePagingUrl = "/tim-kiem-{0}/{1}/p{2}";

        public const string SearchNewsAjaxUrl = "/api/search/news";

        public const string NewsDetailForShareSocialUrl = "/npi-{0}";

        //public const string TopicNewsUrl = "/chuyen-de-xe/{0}-topic{1}";
        //public const string TopicNewsPagingUrl = "/chuyen-de-xe/{0}-topic{1}/p{2}";

        public const string TopicDetailUrl = "/{0}-cd{1}";
        public const string TopicDetailPagingUrl = "/{0}-cd{1}/p{2}";
        public const string TopicDetailV1Url = "/{0}-topic{1}";
        public const string TopicDetailV1PagingUrl = "/{0}-topic{1}/p{2}";
        public const string TopicDetailV10Url = "/chuyen-de-xe/{0}-topic{1}";
        public const string TopicDetailV10PagingUrl = "/chuyen-de-xe/{0}-topic{1}/p{2}";

        public const string AMPUrl = "{0}/amp";

        public const string TagNewsUrl = "/{0}-{1}";
        public const string TagNewsPagingUrl = "/{0}-{1}/p{2}";
        
    }
}
