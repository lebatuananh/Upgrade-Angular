using System;
using System.ComponentModel;

namespace Base.Logging.NLogCustom
{
    public enum LogSourceTypeEnums
    {
        [Description("[TinXe] CMS")] CMS = 1,

        [Description("[TinXe] FE")] FE = 2,

        [Description("[TinXe] Win Service")] WS = 3,

        [Description("[TinXe] Private API")] PrivateAPI = 4,

        [Description("[APDashboard] Site")] APDSite = 5,

        [Description("[FE] AutoMexico")] MexicoFe = 50,
        [Description("[Worker] APDashboard")]
        APDWorker = 6,
        [Description("[CMS] IndianAuto")]
        IndianAutoCMS = 7,
        [Description("[CMS] Chobrod")]
        ChobrodCMS = 8,
        [Description("[CMS] Philkotse")]
        PhilkotseCMS = 9,
        [Description("[CMS] NaijAuto")]
        NaijAutoCMS = 10,
        [Description("[CMS] AutoMexico")]
        AutoMexicoCMS = 11,
        [Description("[CMS] Cintamobil")]
        CintamobilCMS = 12,
        [Description("[FE] Otomurah")]
        OtomurahFE = 13,
        [Description("[FE] APLogging Monitor")]
        APLoggingMonitor = 14,
        [Description("[Worker] AutoMexico")]
        AutoMexicoWorker = 15,
        [Description("[Worker] Chobrod")]
        ChobrodWorker = 16,
        [Description("[Worker] IndianAuto")]
        IndianAutoWorker = 17,
        [Description("[Worker] Philkotse")]
        PhilkotseWorker = 18,
        [Description("[Worker] Cintamobil")]
        CintamobilWorker = 19,
        [Description("[Worker] NaijAuto")]
        NaijAutoWorker = 20,
        [Description("[Worker] Internal Tools Export Product")]
        InternalToolsExportProductWorker = 21,
        [Description("[CMS] Chobrod WVT")]
        ChobrodCMSWVT = 22,
        [Description("[FE] IndianAuto")]
        IndianAutoFE = 23,
        [Description("[FE] Autodelujomx")]
        AutodelujomxFE = 24,
        [Description("[API] AutoMexico Satellite FE")]
        MexicoSatelliteAPIFE = 25,
        [Description("[API] Cintamobil CRM")]
        CintamobilApiCrm = 26,
        [Description("[API] Philkotse CRM")]
        PhilkotseApiCrm = 27,
        [Description("[API] NaijAuto CRM")]
        NaijAutoApiCrm = 28,
        [Description("[API] AutoMexico CRM")]
        AutoMexicoApiCrm = 29,
        [Description("[FE] Cintamobil FE")]
        CintamobilNewCore = 30,
        [Description("[FE] Khaorot FE")]
        KhaorotNewCore = 31,
        [Description("[API] Cintamobil APP")]
        CintamobilApiApp = 32,
        [Description("[FE] AutoMexico")]
        AutoMexicoFE = 50,
        [Description("[API] IndianAuto APP")]
        IndianAutoApiApp = 60,
        [Description("[FE] IndianAuto Leadplatform")]
        IndianAutoLeadplatform = 61,
        [Description("[CMS] IndianAuto WVT")]
        IndianAutoCMSWVT = 62,
        [Description("[API] IndianAuto CRM")]
        IndianAutoApiCrm = 63,
        [Description("[Worker] IndianAuto FE Main")]
        IndianAutoFEMainWorker = 64,
        [Description("[Worker] IndianAuto FE WVT")]
        IndianAutoFEWVTWorker = 65,
        [Description("[Worker] IndianAuto CMS PushProduct")]
        IndianAutoCMSPushProductWorker = 66,
        [Description("[Worker] IndianAuto CMS SyncInfo")]
        IndianAutoCMSSyncInfoWorker = 67,
        [Description("[Worker] IndianAuto CMS LeadsCollection")]
        IndianAutoCMSLeadsCollectionWorker = 68,
        [Description("[CRM] IndianAuto")]
        IndianAutoCrm = 69,
        [Description("[API] IndianAuto Leadplatform CRM")]
        IndianAutoLeadplatformApiCrm = 70,
        [Description("[API] Chobrod CRM")]
        ChobrodCrm = 71,

        [Description("[Worker] Cloud Kitchen")]
        CloudKitchenWorker = 72,
        [Description("[WEB] Cloud Kitchen")]
        CloudKitchenWebClient = 73,
        [Description("[API] Cloud Kitchen")]
        CloudKitchenWebApi = 74,
        [Description("[API] OMS Cloud Kitchen")]
        CloudKitchenOMSApi = 75,

    }

    public static class EnumConvert
    {
        public static string GetEnumDescription(Enum value)
        {
            try
            {
                var fi = value.GetType().GetField(value.ToString());

                var attributes =
                    (DescriptionAttribute[])fi.GetCustomAttributes(
                        typeof(DescriptionAttribute),
                        false);
                return attributes?.Length > 0 ? attributes[0].Description : value.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
