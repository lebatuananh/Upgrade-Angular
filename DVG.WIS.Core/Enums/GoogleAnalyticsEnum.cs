using System.ComponentModel;

namespace DVG.WIS.Core.Enums
{
    public class GoogleAnalyticsEnum
    {
        public enum Dimension
        {
            exitPagePath,
            landingPagePath,
            nextPagePath,
            pagePath,
            [Description("rt:pagePath")]
            RTpagePath,
            pageTitle,
            previousPagePath,
            secondPagePath,
            browser,
            browserVersion,
            city,
            connectionSpeed,
            country,
            [Description("ga:date")]
            Date,
            daysSinceLastVisit,
            day,
            flashVersion,
            hostname,
            isMobile,
            [Description("ga:hour")]
            Hour,
            javaEnabled,
            language,
            latitude,
            longitude,
            month,
            networkDomain,
            networkLocation,
            operatingSystem,
            operatingSystemVersion,
            pageDepth,
            region,
            screenColors,
            screenResolution,
            subContinent,
            userDefinedValue,
            visitCount,
            visitLength,
            visitorType,
            week,
            year,
            [Description("ga:channelGrouping")]
            ChannelGrouping,
            [Description("ga:socialNetwork")]
            SocialNetwork,
            [Description("ga:segment")]
            Segment,
            [Description("ga:medium")]
            Medium,
            [Description("rt:source")]
            RTsource,
            [Description("rt:minutesAgo")]
            RTminutesAgo
        }

        public enum Metric
        {
            [Description("ga:pageviews")]
            Pageviews,
            [Description("ga:organicSearches")]
            OrganicSearches,
            [Description("ga:sessions")]
            Sessions,
            [Description("ga:exits")]
            Exits,
            [Description("ga:users")]
            Users,
            [Description("ga:pageviewsPerSession")]
            PageviewsPerSession,
            [Description("ga:avgSessionDuration")]
            AvgSessionDuration,
            [Description("rt:activeUsers")]
            RTactiveUsers,
            [Description("rt:pageviews")]
            RTpageviews
        }

        public enum Segment
        {
            [Description("gaid::-1")]
            AllUsers,
            [Description("gaid::-7")]
            DirectTraffic,
            [Description("gaid::-5")]
            OrganicTraffic,
            [Description("gaid::-8")]
            ReferralTraffic,
            [Description("gaid::-18")]
            OtherTraffic
        }

        public enum WebmastersDimension
        {
            [Description("query")]
            Query,
            [Description("date")]
            Date,
            [Description("page")]
            Page
        }

        public enum AuthenticateType
        {
            GoogleAnalytics,
            Webmasters
        }

        public enum DateRanges
        {
            [Description("Ngày hôm qua")]
            LastDay = -1,
            [Description("7 Ngày vừa qua")]
            Last7Days = -7,
            [Description("30 Ngày vừa qua")]
            Last30Days = -30,
            [Description("90 Ngày vừa qua")]
            Last90Days = -90
        }

        public enum TimeRanges
        {
            [Description("Hour")]
            Hour = 1,
            [Description("Day")]
            Day = 2,
            [Description("Week")]
            Week = 3,
            [Description("Month")]
            Month = 4
        }

        public enum MetricsSource
        {
            [Description("Sessions")]
            Sessions = 1,
            [Description("Users")]
            Users = 2,
            [Description("Pages / Session")]
            PagesPerSession = 3,
            [Description("Pageviews")]
            Pageviews = 4,
            [Description("Avg. Session Duration")]
            AvgSessionDuration = 5
        }

        public enum GASource
        {
            [Description("All")]
            All = 1,
            [Description("Direct")]
            Direct = 2,
            [Description("Organic")]
            Organic = 3,
            [Description("Referral")]
            Referral = 4,
            [Description("Social")]
            Social = 5,
            [Description("Orther")]
            Orther = 6
        }
    }
}