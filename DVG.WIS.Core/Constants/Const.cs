using DVG.WIS.Utilities;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Security.Policy;

namespace DVG.WIS.Core
{
    public class Const
    {
        public const int FixReviewBikeCateId = 22; // danh-gia-xe-may

        public const string HnAndHcmCityId = "HnAndHcmCityId";
        public const string TechcomAndVpBankId = "TechcomAndVpBankId";
        public const string SavingTechcomAndVpBankId = "SavingTechcomAndVpBankId";
        
        public const string KeyWeekCacheTime = "WeekCacheTime";
        public const string KeyLongCacheTime = "LongCacheTime";
        public const string KeyMediumCacheTime = "MediumCacheTime";
        public const string KeyShortCacheTime = "ShortCacheTime";

        public const string UrlPageNotFound = "NotfoundPage";
        public const string UrlNotfoundContent = "NotfoundContent";
        public const string LinkAppBXH = "LinkAppBXH";

        public const string DateTimeFormatStringClient = "yyyy-MM-dd";
        public const string CustomeDateFormat = "dd/MM/yyyy HH:mm";
        public const string NormalDateFormat = "dd/MM/yyyy";
        public const string LongDateFormat = "dd/MM/yyyy HH:mm:ss";

        public const string Million = "triệu";
        public const string Billion = "tỷ";

        public const long MinValuePrice = 0;
        public const long MaxValuePrice = 99000000000;
        public const int MinValueYear = 0;
        public const int MaxValueYear = 2017;
        public const long StepCovertedPrice = 10000000;

        public const int MaxWordOnTitleForFirstHighLightRevert = 24;
        public const int MaxWordOnSapoForFirstHighLightRevert = 25;

        public const int MaxWordOnTitleForFirstHighLightRevertMobile = 13;
        public const int MaxWordOnSapoForFirstHighLightRevertMobile = 25;

        public const int MaxWordOnTitleForFirstHighLight = 20;
        public const int MaxWordOnSapoForFirstHighLight = 50;
        public const int MaxWordOnTitleForOtherHighLight = 10;
        public const int MaxWordOnTitleForOtherHighLight1 = 13;
        public const int MaxWordOnSapoForOtherHighLight = 15;

        public const int MaxWordTitleForNewsList = 20;
        public const int MaxWordSapoForNewsList = 35;

        public const int MaxWordTitleForNewsSmallList = 5;
        public const int MaxWordSapoForNewsSmallList = 10;

        public const int MaxWordTitleForAssessmentTitleUppercase = 14;

        #region Worker service

        public const string DurationFAQProcess = "DurationFAQProcess";
        public const string DurationOpportunityProcess = "DurationOpportunityProcess";

        #endregion

        /// <summary>
        /// Số bản ghi trang đầu tiên - Trang chủ
        /// </summary>
        public const int TimeLineFirstPageSize = 21;        
        public const int TimeLineBlock1 = 5;
        public const int TimeLineBlock2 = 5;
        /// <summary>
        /// số bản ghi khi load xem thêm
        /// </summary>
        public const int TimeLinePageSize = 12;


        public const int PAGESIZE = 10;
        public const int PageSizeCarInfo = 20;
        public const int PageSizeCarAssessment = 10;
        public const int PortalTopContract = 2;

        public const string TargetExternal = "TargetExternal";
        public const string TargetOutside = "TargetOutside";
        public const string DebugMode = "DebugMode";
        public const string CrawlerDetected = "CrawlerDetected";
        public const string LocalMode = "LocalMode";
        public const string KeyAccountId = "KeyAccountId";
        public const string SupperAdmin = "SupperAdmin";
        public const string KeyNameSystem = "KeyNameSystem";
        public const string KeyNameSystemDefault = "KeyNameSystemDefault";
        public const string ValueSystem = "ValueSystem";
        public const string ValueSystemDefault = "ValueSystemDefault";
        public const string RequireLoginToShowLink = "RequireLoginToShowLink";
        public const string EnabledEncryptUrl = "EnabledEncryptUrl";
        public const string KeyDecodeUrl = "dvg.seolink";
        public const string TopBoxHomeHighlight = "TopBoxHomeHighlight";
        public const string TopBoxMostView = "TopBoxMostView";
        public const string TopBoxAssessmentHighlight = "TopBoxAssessmentHighlight";
        public const string TopBoxMostSearch = "TopBoxMostSearch";
        public const string TopBoxNewsFocus = "TopBoxNewsFocus";

        public const string LoginUrl = "LoginUrl";
        public const string DoLogin = "DoUrl";
        public const string RegisterUrl = "RegisterUrl";
        public const string LogoutUrl = "LogoutUrl";
        public const string MainTitle = "MainTitle";
        public const string MainDescription = "MainDescription";
        public const string MainKeyword = "MainKeyword";
        public const string FacebookAppId = "FacebookAppId";
        public const string FacebookAppSecret = "FacebookAppSecret";
        public const string GoogleClientId = "GoogleClientId";
        public const string GoogleClientSecret = "GoogleClientSecret";

        public const string EmailImgLogo = "EmailImgLogo";
        public const string EmailImgTop = "EmailImgTop";
        public const string EmailImgBottom = "EmailImgBottom";

        public const string ListSearchCrop = "ListSearchAvatar";

        public const string SessionAuthUserPermission = "AuthUserPermission";
        public const string SessionLoginFailName = "LoginFail";
        public const string SessionCaptcharName = "CaptchaForLogin";
        public const string SessionLoginFaceBook = "SessionLoginFaceBook";
        public const string TabIdCalculation = "TabIdCalculation";

        public const string SessionKeyDelayRegisterCard = "SessionKeyDelayRegisterCard";
        public const string SessionKeyDelayPostComment = "SessionKeyDelayPostComment";
        public const string SessionKeyDelayThanks = "SessionKeyDelayThanks";
        public const string SessionKeyDelayReport = "SessionKeyDelayReport";
        public const string SessionKeyDelayVote = "SessionKeyDelayVote";
        public const string SessionCurrentUrl = "SessionCurrentUrlName";

        public const string TotalLoanItem = "TotalLoanItem";

        public const string FrontEndUrl = "FrontEndUrl";
        public const string MobileParam = "isMobile";
        public const int ExpiredCookie = 3600;

        public const string ApiEncryptData = "ApiEncryptData";
        public const string MailTemplateHandler = "MailTemplateHandler";
        public const string QueryStringMailTemaplate = "?userType={0}&mailType={1}&amount={2}";

        public const string DateTimeFormatAdmin = "dd/MM/yyyy HH:mm";
        public const string DateTimeFormatFull = "dd/MM/yyyy HH:mm:ss";
        public const string DateTimeShortFormatAdmin = "dd/MM/yyyy";
        public const string DateTimeMonthYearFormatAdmin = "MM/yyyy";

        #region

        public const string BasicInterestRate = "LAI_XUAT_CO_BAN";
        public const string BasicInterestRateColumn = "BasicInterestRate";
        public const string Amplitude = "BIEN_DO";
        public const string AmplitudeColumn = "Amplitude";
        #endregion


        #region Crop Image
        public static string FacebookAvatar = AppSettings.Instance.GetString("FacebookAvatar", "/crop/620x324");
        public static string StandardAvatar = AppSettings.Instance.GetString("StandardAvatar");
        public static string HighLightAvatar = AppSettings.Instance.GetString("HighLightAvatar");
        public static string ListNewsAvatar = AppSettings.Instance.GetString("ListNewsAvatar");
        public static string HighLightCategoryAvatar = AppSettings.Instance.GetString("HighLightCategoryAvatar");
        public static string HighLightCategoryAvatarSmall = AppSettings.Instance.GetString("HighLightCategoryAvatarSmall");
        public static string UserAvatar = AppSettings.Instance.GetString("UserAvatar");
        public static string InterestAvatar = AppSettings.Instance.GetString("InterestAvatar");
        public static string TopicAvatar = AppSettings.Instance.GetString("TopicAvatar");
        public static string MostViewAvartar = AppSettings.Instance.GetString("MostViewAvatar");
        public static string CompareCarAvatar = AppSettings.Instance.GetString("CompareCarAvatar");
        public static string AssementHomeAvatar = AppSettings.Instance.GetString("AssementHomeAvatar");
        public static string GalleryAvatar = AppSettings.Instance.GetString("GalleryAvatar");
        public static string CropSize230 = "CropSize230";
        public static string CropSizeCMS = "CropSizeCMS";

        public static string SlideNewsImage = AppSettings.Instance.GetString("SlideNewsImage");
        public static string SlideNewsSubImage = AppSettings.Instance.GetString("SlideNewsSubImage");

        public static string NewsRelation = AppSettings.Instance.GetString("NewsRelation");
        public static string VideoViewlarge = AppSettings.Instance.GetString("VideoViewlarge");

        #endregion

        #region News

        //public const int TopHighLightOnCate = 3;
        //public const int PageSize = 8;
        //public const int PageSizeVideo = 4;
        //public const int LoanPageSize = 5;
        //public const int TopVideo = 4;
        public const int TopNewsLatest = 4;
        #endregion

        //Loanpackage
        //public const string BanXeHoi = "bxh";
        //public const string BatDongSan = "bds";

        //Bank product
        //public const int TopBankProduct = 2;

        //Captcha
        public const string CaptchaForRegistry = "CaptchaForRegistry";
        public const string CaptchaForLogin = "CaptchaForLogin";
        public const string CaptchaForApps = "CaptchaForApps";

        //LostPassword
        public const string LostPassSession = "LostPassSession";
        public const int TimeOutLostPass = 30;

        //member
        //public const int MemberUserProfile = 1;
        //public const int MemberDealHistory = 2;
        //public const int MemberChangeUserProfile = 3;
        //public const int MemberChangeUserFinance = 4;
        //public const int MemberChangePassword = 5;

        //#region Session key

        //public const string SearchDebitCard = "SearchDebitCard";
        //public const string SubmitQuestion = "SubmitQuestion";
        //public const string SubmitAnswer = "SubmitAnswer";

        //#endregion

        //Boxnews
        public const int TopBoxNews = 4;
        public const int TakeTopBoxNews = 20;

        public const string TopBoxNewsEmbed = "TopBoxNewsEmbed";
        //BoxOther
        public const int TopBoxOther = 4;

        #region Link detail

        public const string TopAutoCompleteLinkDetail = "TopAutoCompleteLinkDetail";

        #endregion



        #region SSO

        public const string SSOLogin = "SSOLogin";
        public const string SSOLogout = "SSOLogout";
        public const string SSOLogoutCallback = "SSOLogoutCallback";
        public const string SSOToken = "SSOToken";
        public const string SSOAdminAccount = "SSOAdminAccount";
        public const string UsingSSO = "UsingSSO";
        public const string GodAdminAccount = "GodAdminAccount";

        #endregion

        #region Activity

        public const string ActivityLogin = "Đăng nhập";
        public const string ActivityLogOut = "Đăng xuất";
        public const string ActivityChangePass = "Đổi mật khẩu";
        public const string ActivityAccessPermission = "Phân quyền";
        public const string ActivityBank = "Ngân hàng";
        public const string ActivityLoan = "Gói vay";
        public const string ActivitySaving = "Gói tiết kiệm";
        public const string ActivityAtmCard = "Thẻ ATM & Debit";
        public const string ActivityMaster = "Thẻ tín dụng";
        public const string ActivityNews = "Tin tức";

        #endregion

        public const string FaqUsingRedis = "FaqUsingRedis";
        public const string FaqRedisIP = "FaqRedisIP";
        public const string FaqRedisPort = "FaqRedisPort";
        public const string FaqRedisDB = "FaqRedisDB";

        public const string PortalUsingRedis = "PortalUsingRedis";
        public const string PortalRedisIP = "PortalRedisIP";
        public const string PortalRedisPort = "PortalRedisPort";
        public const string PortalRedisDB = "PortalRedisDB";

        public const string Male = "Nam";
        public const string Female = "Nữ";
        public const string UnknowPrice = "Chưa công bố";
        //public const string FieldValue = "Trạng thái";
        //public const string UerStatusRegisterClass = "register-status";
        //public const string UerStatusActivedClass = "actived-status";
        //public const string UerStatusDeletedClass = "delete-status";

        #region constant for contract
        public const string CustomerInfo = "Họ Tên: {0} \r\n Địa chỉ: {1} \r\n Email: {2} \r\n Sđt: {3}";
        public const string ProductInfo = "{0} \r\n Thời hạn: {1} tháng \r\n Lãi cố định: {2} tháng";
        public const string AverageIncomeInfo = "{0} {1} {2}";
        public const string SummaryContractFormat = "/{0}/{1}?productType={2}&bankId={3}&provinceId={4}&districtId={5}&branchId={6}&fromDate={7}&todate={8}";

        //public const string ExpiredForContractChangeBranch = "ExpiredForContractChangeBranch";
        //public const string ExpiredForContractConsider = "ExpiredForContractConsider";
        //public const string ExpiredForContractCustomerYetNeed = "ExpiredForContractCustomerYetNeed";
        //public const string ExpiredForCustomerAppointment = "ExpiredForCustomerAppointment";
        //public const string ExpiredForNewContract = "ExpiredForNewContract";
        //public const string SendingMailSchedule = "SendingMailSchedule";
        public const string ListMailAdmin = "ListMailAdmin";


        //public const int ExpiredForContractChangeBranchDefaultValue = 1; // days
        //public const int ExpiredForContractConsiderDefaultValue = 10;// days
        //public const int ExpiredForContractCustomerYetNeedDefaultValue = 30;// days
        //public const int ExpiredForCustomerAppointmentDefaultValue = 3;// days
        //public const int ExpiredForNewContractDefaultValue = 1;// days
        public const string SendingMailScheduleDefaultValue = "17:00";
        public const string ListMailAdminDefaultValue = "dungqd86@gmail.com";//Add a acc then delimiter by the comma

        public const string TemplateAdminFolder = "TemplateEmails\\Admin\\";
        public const string TemplateBankFolder = "TemplateEmails\\Bank\\";
        public const string TemplateBankBranchFolder = "TemplateEmails\\BankBranch\\";


        public const string BranchEmpty = "Hồ sơ chưa phân bổ";

        #endregion

        public const int PageItemNews = 5;
        public const int PageItemNewsOfTag = 10;
        public const int PageItemNewsOfTopic = 12;
        public const int PageItemCards = 6;
        public const int PageItemInterestNews = 8;
        public const int SubLengthUrlNews = 30;
        public const int MaxLengMetaDescription = 75;

        public const string NewsTitle = "NewsTitle";
        public const string NewsDescription = "NewsDescription";
        public const string NewsMetaKeyword = "MetaKeywordNews";

        public const string GalleryTitle = "GalleryTitle";
        public const string GalleryDescription = "GalleryDescription";
        public const string GalleryMetaKeyword = "GalleryMetaKeyword";

        public const string CarInfoTitle = "CarInfoTitle";
        public const string CarInfoDescription = "CarInfoDescription";
        public const string CarInfoMetaKeyword = "CarInfoMetaKeyword";

        public const string AssessmentTitle = "AssessmentTitle";
        public const string AssessmentDescription = "AssessmentDescription";
        public const string AssessmentMetaKeyword = "AssessmentMetaKeyword";

        public const string AdvicesTitle = "AdvicesTitle";
        public const string AdvicesDescription = "AdvicesDescription";
        public const string AdvicesMetaKeyword = "AdvicesMetaKeyword";

        public const string MainFunctionConfig = "MainFunctionConfig";
        public const string ProvinceUnsignNameConfig = "ProvinceUnsignNameConfig";
        public const string TotalCardCompare = "TotalCardCompare";
        public const string MonthlyIncomeConfig = "MonthlyIncomeConfig";

        public const string AboutTitle = "AboutTitle";
        public const string AboutDescription = "AboutDescription";
        public const string FaqTitle = "FaqTitle";
        public const string FaqDescription = "FaqDescription";

        public const string TopHighLightCard = "TopHighLightCard";

        public const string ProductionDomain = "Domain";
        public const string ClientDomain = "ClientDomain";
        public const string PublishDomain = "PublishDomain";
        public const string SpellCheckerApiDomain = "SpellCheckerApiDomain";
        public const string LogoAddImage = "LogoAddImage";
        public const string NoImage = "NoImage";
        public const string SubjectMailContact = "Yêu cầu hỗ trợ khách hàng";
        public const string GGQR = "GGQR";

        #region SiteMap

        public const string SiteMapPath = "SiteMapPath";

        #endregion


        #region CKEditor
        public const string CKEditorVersion = "CKEditorVersion";
        public const string UploadProject = "Upload-Project";
        public const string VideoAESKey = "Video-AES-Key";
        public const string VideoAESIV = "Video-AES-IV";
        public const string VideoViewDomain = "Video-View-Domain";
        public const string VideoUploadDomain = "Video-Upload-Domain";
        public const string VideoUploadDomainClient = "Video-Upload-Domain-Client";
        public const string VideoUploadHandler = "Video-Upload-Handler";
        public const string UploadHandler = "Upload-Handler";
        public const string VideoThumbDomain = "Video-Thumb-Domain";
        #endregion

        #region NewsRelation
        public const int MaxNewsRelation = 10;
        public const int MaxCarInfoRelation = 5;
        public const int MaxAssessmentRelation = 2;
        public const int MaxGalleryRelation = 6;
        public const int MaxAdvicesRelation = 6;
        public const int MaxPricingRelation = 6;


        public const string MaxNewsLandingRelation = "MaxNewsLandingRelation";
        #endregion

        #region NewsRecommend 
        public const int MaxNewsRecommend = 3;
        #endregion

        #region CarInfoSimilar
        public const int MaxCarInfoSimilar = 6;
        public const int TopCarInfoSameStyle = 3;
        public const int TopCarInfoSamePrice = 3;
        #endregion

        #region Tin Xe
        public const int NewsNumberGalleryOnList = 6;
        public const int NewsNumberGalleryHighLight = 4;
        public const int NewsNumberVideoMostView = 5;
        public const int TopCarModelHot = 20;
        public const int PageSizeGallery = 20;
        public const int NewsNumberGalleryRelation = 8;
        public const int TopVideoRelationWap = 8;
        public const int TopNewsRelationCarInfo = 8;
        public const int TopVideoRelationWithGallery = 10;
        public const int TopNewsRelationWithGallery = 10;
        public const int TopVideoLatest = 8;
        public const int TopPhotoLatest = 4;
        public const int TopVideoHomeLatest = 4;
        public const int TopNewsCarMostSale = 10;
        public const int TopNewsCarHighlight = 5;
        public const int TopNewsCarSimilar = 6;

        public const int NewsPageSize = 20;

        public const string TopAssessmentLastest = "TopAssessmentLastest";
        public const int TopNewsOfCarinfoWithLatestTag = 3;
        public const int TopNewsOfCarinfo = 5;
        public const int TopNewsPrice = 4;
        public const int TopNewsPriceSameBrand = 6;
        public const string TopTagCloud = "TopTagCloud";

        public static int TopMenuPrice = AppSettings.Instance.GetInt32("TopMenuPrice", 0);

        public const string VideoRelation = "Video liên quan";
        public const string ImageRelation = "Ảnh liên quan";

        public const string PriceCar = "Giá";
        public const string ProductionYearCar = "Đời xe";
        public const string Made = "Xuất xứ";
        public const string StyleNameCar = "Dáng xe";
        public const string AvgRatingCar = "Điểm trung bình";
        public const string SizeCar = "Kích thước tổng thể";
        public const string BasicLengthCar = "Chiều dài cơ sở";
        public const string BasicWidthCar = "Chiều rộng cơ sở";
        public const string GroundClearance = "Khoảng sáng gầm xe";
        public const string MinimumRotationRadius = "Bán kính quay vòng tối thiểu";
        public const string Wheel = "Vành xe";
        public const string TyreCar = "Thông số lốp";
        public const string FuelCapacityCar = "Dung tích bình nhiên liệu";
        public const string WeightCar = "Trọng lượng";
        public const string EngineCar = "Động cơ";
        public const string EngineTypeCar = "Kiểu động cơ";
        public const string CylinderCapacityCar = "Dung tích xi lanh";
        public const string BrakeCar = "Bộ hấp thụ";
        public const string Drivetrain = "Kiểu dẫn động";
        public const string BrakeSystemsAgo = "Hệ thống phanh trước";
        public const string MaxPowerCar = "Công suất cực đại";
        public const string MaxTorqueMomentCar = "Mô men xoắn cực đại";
        public const string GearCar = "Hộp số";
        public const string AbsorberCar = "Hệ thống phanh sau";
        public const string FrontSuspension = "Hệ thống treo trước";
        public const string RearSuspension = "Hệ thống treo sau";
        public const string FuelConsumptionCar = "Mức tiêu hao nhiên liệu trung bình";
        public const string MaxSpeed = "Vận tốc tối đa";
        public const string AccelerationTime = "Thời gian tăng tốc 0-100km/h";
        public const string NumOfSeatCar = "Số ghế ngồi";
        public const string RimCar = "Đèn ban ngày";
        public const string Headlight = "Đèn chiếu sáng";
        public const string Foglight = "Đèn sương mù";
        public const string RearviewMirror = "Gương chiếu hậu";
        public const string Grille = "Lưới tản nhiệt";
        public const string SteeringWheel = "Vô lăng";
        public const string GearStick = "Cần số";
        public const string Seat = "Ghế ngồi";
        public const string AirConditioning = "Điều hòa không khí";
        public const string GlassDoorElectricControl = "Kính cửa điều khiển điện";
        public const string SunScreen = "Màn chắn nắng";
        public const string InformationMonitor = "Màn hình hiển thị đa thông tin";
        public const string SoundSystem = "Hệ thống âm thanh";
        public const string HandBackseat = "Tựa tay hàng ghế sau";
        public const string CupHolder = "Giá để ly";
        public const string Sunroof = "Cửa sổ trời";
        public const string SensorWipers = "Gạt mưa có cảm biến";
        public const string ABS = "Chống bó cứng phanh";
        public const string Airbag = "Túi khí";
        public const string AutoDoorLock = "Cửa tự động khóa";
        public const string TractionControl = "Kiểm soát lực kéo";
        public const string ClimbingControl = "Hỗ trợ khởi hành ngang dốc";
        public const string EBD = "Phân phối lực phanh điện tử";
        public const string ESP = "Ổn định thân xe điện tử";
        public const string FourWD = "Hệ thống dẫn động";
        public const string AntiRollSystem = "Hệ thống chống lật xe";
        public const string AntiSlipSystem = "Hệ thống chống trượt";
        public const string DownhillSupport = "Hỗ trợ đổ đèo";
        public const string AntiTheftSystem = "Hệ thống chống trộm";
        public const string BackwardSensor = "Cảm biến lùi";
        public const string BackCamera = "Camera lùi";


        public const string ModelVersionCar = "- Lựa chọn phiên bản -";
        public const string FormatDate2 = "HH:ss - dd/MM/yyyy";

        #endregion

        #region API Call
        public const string DefaultPassword = "DefaultPassword";
        #endregion

        #region Gallery 

        public const int TopNewsByCateOnGalleryPage = 9;


        #endregion

        public const int CateMotorbike = 18;

        #region AutoSave
        public const string NewsAutoSave = "NewsAutoSave";
        public const string NewsAutoSaveInterval = "NewsAutoSaveInterval";
        #endregion

        #region Language
        public static string DefaultLanguage = AppSettings.Instance.GetString("DefaultLanguage");
        #endregion

        #region Store get data chart Content page
        public static string StoreChartListingOrganicCrawler = AppSettings.Instance.GetString("StoreChartListingOrganicCrawler");
        public static string StoreChartListingCrawler = AppSettings.Instance.GetString("StoreChartListingCrawler");
        public static string StoreChartListingReviewedNonReviewed = AppSettings.Instance.GetString("StoreChartListingReviewedNonReviewed");
        public static string StoreChartPublishedContent = AppSettings.Instance.GetString("StoreChartPublishedContent");
        public static string StoreChartUsers = AppSettings.Instance.GetString("StoreChartUsers");
        #endregion

        #region Google service

        public static int RowLimitWebmaster = AppSettings.Instance.GetInt32("WebmasterRowLimit", 1000);

        #endregion

        #region JWT
        public const string JWTSecretKey = "JWTSecretKey";
        public const string JWTIssuer = "JWTIssuer";
        public const string JWTAudience = "JWTAudience";
        public const string JWTTimeout = "JWTTimeout";
        #endregion
    }
}
