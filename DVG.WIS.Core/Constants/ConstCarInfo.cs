using System;
using System.Collections.Generic;
using System.Linq;

namespace DVG.WIS.Core.Constants
{
    public static class FilterValue
    {
        [Flags]
        public enum States : long
        {
            //hopso
            KhongChon = (long)1 << 0,   //1
            FuelTypeXang = (long)1 << 1,   //2
            FuelTypeDiesel = (long)1 << 2, //4
            FuelTypeHybrid = (long)1 << 3, //8
            FuelTypeDien = (long)1 << 4,   //16
            FuelTypeKhac = (long)1 << 5,   //32
            NumOfSeat2 = (long)1 << 6,      //64
            NumOfSeat4 = (long)1 << 7,      //128
            NumOfSeat5 = (long)1 << 8,   //256
            NumOfSeat7 = (long)1 << 9,    //512
            NumOfSeat9 = (long)1 << 10,
            NumOfSeat12 = (long)1 << 11,
            NumOfSeat16 = (long)1 << 12,
            NumOfSeatKhac = (long)1 << 13,
            ClassificationSedan = (long)1 << 14,
            ClassificationSuv = (long)1 << 15,
            ClassificationCoupe = (long)1 << 16,
            ClassificationCrossover = (long)1 << 17,
            ClassificationHatchback = (long)1 << 18,
            ClassificationConvertible = (long)1 << 19,
            ClassificationTruck = (long)1 << 20,
            ClassificationVan = (long)1 << 21,
            ClassificationWagon = (long)1 << 22,
            SecondHandCu = (long)1 << 23,
            SecondHandMoi = (long)1 << 24,
            TransmissionSoTay = (long)1 << 25,
            TransmissionSoTuDong = (long)1 << 26,
            TransmissionHonHop = (long)1 << 27,
            MadeInTrongNuoc = (long)1 << 28,
            MadeInNhapKhau = (long)1 << 29,
            ClassificationMpv = (long)1 << 30,
            ClassificationPickUp = (long)1 << 31,
            ClassificationSportCar = (long)1 << 32,
            ClassificationCityCar = (long)1 << 33,
            NumOfDoor2 = (long)1 << 34,
            NumOfDoor3 = (long)1 << 35,
            NumOfDoor4 = (long)1 << 36,
            NumOfDoor5 = (long)1 << 37,
        };
        public static long TurnOn(long s, States b) { return s | (long)b; }
        public static long TurnOff(long s, States b) { return s & ~(long)b; }

        public static long TurnOn(long s, long b) { return s | b; }
        public static long TurnOff(long s, long b) { return s & ~b; }

        public static bool HasState(long s, States b)
        {
            return (s & (long)b) == (long)b;
        }
        public static bool HasState(long s, long b)
        {
            return (s & b) == b;
        }
        public static long Keep(long s, params States[] b) { return s & (long)b.Cast<long>().Sum(); }
        public static bool SingleState(long s)
        {
            for (int i = 1; i <= 37; i++)
            {
                if ((1 << i) == s)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class ConstCarInfo
    {
        #region AutoChange
        // Enum biểu diễn sự thay đổi của Autos
        public static class AutoChange
        {
            [Flags]
            public enum States : long
            {
                Title = 1 << 0,   //1
                MakeID = 1 << 1,   //2
                ModelID = 1 << 2,   //4
                ModelDetail = 1 << 3,   //8
                Year = 1 << 4,   //16
                MadeIn = 1 << 5,   //32
                SecondHand = 1 << 6,   //64
                NumOfKm = 1 << 7,   //128
                ClassificationID = 1 << 8,   //256
                Price = 1 << 9,   //512
                ExteriorColor = 1 << 10,   //1024
                InteriorColor = 1 << 11,   //2048
                NumOfDoor = 1 << 12,   //4096
                NumOfSeat = 1 << 13,   //8192
                Transmission = 1 << 14,
                Drivetrain = 1 << 15,
                FuelType = 1 << 16,
                FuelInjection = 1 << 17,
                FuelEconomy = 1 << 18,
                Description = 1 << 19,
                LinkAddress = 1 << 20,
                CityID = 1 << 21,
                Size = 1 << 22,
                BasicLength = 1 << 23,
                BasicWidth = 1 << 24,
                Weight = 1 << 25,
                FuelCapacity = 1 << 26,
                Engine = 1 << 27,
                EngineType = 1 << 28,
                CylinderCapacity = 1 << 29,
                Brake = 1 << 30,
                Absorber = 2147483648,
                Tyre = 4294967296,
                Rim = 8589934592,
                AnotherTech = 17179869184,
                SeatCozy = 34359738368,
                MediaCozy = 68719476736,
                AnotherCozy = 137438953472,
                Image = 274877906944,
                FullName = 549755813888,
                Mobile = 1099511627776,
                Phone = 2199023255552,
                Address = 4398046511104, // Thêm mới
                Email = 8796093022208 // Thêm mới
            }
            public static long TurnOn(long s, States b) { return s | (long)(ulong)b; }
            public static long TurnOff(long s, States b) { return s & ~(long)b; }

            public static long TurnOn(long s, long b) { return s | b; }
            public static long TurnOff(long s, long b) { return s & ~b; }

            public static bool HasState(long s, States b)
            {
                return (s & (long)b) == (long)b;
            }
            public static bool HasState(long s, long b)
            {
                return (s & b) == b;
            }
            public static long Keep(long s, params States[] b) { return s & (long)b.Cast<long>().Sum(); }
        }

        #endregion

        public static Dictionary<string, PropertyObject> MadeIn = new Dictionary<string, PropertyObject>()
        {
            {"Trong nước", new PropertyObject(1, (long)FilterValue.States.MadeInTrongNuoc)},
            {"Nhập khẩu", new PropertyObject(2, (long)FilterValue.States.MadeInNhapKhau)}
        };

        public static Dictionary<string, PropertyObject> SecondHand = new Dictionary<string, PropertyObject>()
        {
            {"Đã qua sử dụng", new PropertyObject(1, (long)FilterValue.States.SecondHandCu)},
            {"Mới", new PropertyObject(2, (long)FilterValue.States.SecondHandMoi)}
        };

        public static Dictionary<string, PropertyObject> Color = new Dictionary<string, PropertyObject>()
        {
            {"Đen", new PropertyObject(12, 1 << 12, "den")},
            {"Trắng", new PropertyObject(4, 1 << 4, "trang")},
            {"Bạc", new PropertyObject(1, 1 << 1, "bac")},
            {"Đỏ", new PropertyObject(9, 1 << 9, "do")},
            {"Vàng", new PropertyObject(6, 1 << 6, "vang")},
            {"Xanh lam", new PropertyObject(8, 1 << 8, "lam")},
            {"Xanh lục", new PropertyObject(10, 1 << 10, "luc")},
            {"Kem (Be)", new PropertyObject(5, 1 << 5, "kem")},
            {"Xám (ghi)", new PropertyObject(11, 1 << 11, "xam")},
            {"Tím", new PropertyObject(2, 1 << 2, "tim")},
            {"Hồng", new PropertyObject(3, 1 << 3, "hong")},
            {"Nâu", new PropertyObject(7, 1 << 7, "nau")},
            {"Hai màu", new PropertyObject(13, 1 << 13, "2mau")},
            {"Màu khác", new PropertyObject(14, 1 << 14, "khac")}
        };

        public static Dictionary<string, PropertyObject> Transmission = new Dictionary<string, PropertyObject>()
        {
            {"Số tay", new PropertyObject(1, (long)FilterValue.States.TransmissionSoTay)},
            {"Số tự động", new PropertyObject(2, (long)FilterValue.States.TransmissionSoTuDong)},
            {"Số hỗn hợp", new PropertyObject(3, (long)FilterValue.States.TransmissionHonHop)},
            {"Số sàn", new PropertyObject(4, (long)FilterValue.States.TransmissionHonHop)},
            {"Tự động 4 cấp", new PropertyObject(5, (long)FilterValue.States.TransmissionSoTuDong)},
            {"Tự động 8 cấp", new PropertyObject(6, (long)FilterValue.States.TransmissionSoTuDong)},
            {"Vô cấp", new PropertyObject(7, (long)FilterValue.States.TransmissionHonHop)},
            {"Số sàn 5 cấp", new PropertyObject(8, (long)FilterValue.States.TransmissionSoTuDong)},
        };
        /*
         Số sàn
            Số sàn 5 cấp
            Tự động
            Tự động 4 cấp
            Tự động 8 cấp
            Vô cấp
             */
        public static Dictionary<string, byte> Drivetrain = new Dictionary<string, byte>()
        {
            {"FWD - Dẫn động cầu trước", 1},
            {"RFD - Dẫn động cầu sau", 2},
            {"4WD - Dẫn động 4 bánh", 3},
            {"AWD - 4 bánh toàn thời gian", 4},
            {"4WD hoặc AWD", 5},
            {"Khác", 6},
        };
        public static Dictionary<string, byte> DrivetrainExtend = new Dictionary<string, byte>()
        {
            {"Cầu trước",1},
            {"Cầu trước 2WD",1},
            {"Cầu sau", 2},
            {"Dẫn động 4 bánh bán thời gian",3},
            {"Dẫn động 4 bánh",3},
            {"Dẫn động 4 bánh gài cầu điện tử",3},
            {"Dẫn động 4 bánh 4WD",3},
            {"Dẫn động 4 bánh Dẫn động 4 bánh",3},
            {"Dẫn động 4 bánh Dẫn động bốn bánh",3},
            {"Dẫn động 4 bánh Easy Select 4WD",3},
            {"Dẫn động 4 bánh 4MATIC",4},
            {"Dẫn động 4 bánh 4MOTION",4},
            {"Dẫn động 4 bánh AWD",4},
            {"Dẫn động 4 bánh Bốn bánh toàn thời gian 4-Motion",4},
            {"Dẫn động 4 bánh chủ động",4},
            {"Dẫn động 4 bánh đối xứng AWD",4},
            {"Dẫn động 4 bánh đối xứng Symmertrical AWD",4},
            {"Dẫn động 4 bánh đối xứng Symmetrical AWD",4},
            {"Dẫn động 4 bánh Symmetrical AWD",4},
            {"Dẫn động 4 bánh toàn phần",4},
            {"Dẫn động 4 bánh toàn phần chủ động",4},
            {"Dẫn động 4 bánh toàn thời gian",4},
            {"Dẫn động 4 bánh toàn thời gian 4Matic",4},
            {"Dẫn động 4 bánh toàn thời gian đối xứng Symmetrical AWD",4},
            {"Dẫn động 4 bánh toàn thời gian Quattro",4},
            {"Dẫn động 4 bánh toàn thời gian xDrive",4},
            {"Dẫn động 4 bánh xDrive",4},
        };
        public static Dictionary<string, PropertyObject> FuelType = new Dictionary<string, PropertyObject>()
        {
            {"Xăng", new PropertyObject(1, (long)FilterValue.States.FuelTypeXang)},
            {"Diesel", new PropertyObject(2, (long)FilterValue.States.FuelTypeDiesel)},
            {"Hybrid", new PropertyObject(3, (long)FilterValue.States.FuelTypeHybrid)},
            {"Điện", new PropertyObject(4, (long)FilterValue.States.FuelTypeDien)},
            {"Loại khác", new PropertyObject(99, (long)FilterValue.States.FuelTypeKhac)}
        };
        public static Dictionary<string, PropertyObject> NumOfSeat = new Dictionary<string, PropertyObject>()
        {
            {"2 chỗ", new PropertyObject(2, (long)FilterValue.States.NumOfSeat2)},
            {"4 chỗ", new PropertyObject(4, (long)FilterValue.States.NumOfSeat4)},
            {"5 chỗ", new PropertyObject(5, (long)FilterValue.States.NumOfSeat5)},
            {"7 chỗ", new PropertyObject(7, (long)FilterValue.States.NumOfSeat7)},
            {"9 chỗ", new PropertyObject(9, (long)FilterValue.States.NumOfSeat9)},
            {"16 chỗ", new PropertyObject(16, (long)FilterValue.States.NumOfSeat16)},
            {"> 16 chỗ", new PropertyObject(99, (long)FilterValue.States.NumOfSeatKhac)}
        };
        public static Dictionary<string, PropertyObject> NumOfDoor = new Dictionary<string, PropertyObject>()
        {
            {"2 cửa", new PropertyObject(2, (long)FilterValue.States.NumOfDoor2)},
            {"3 cửa", new PropertyObject(3, (long)FilterValue.States.NumOfDoor3)},
            {"4 cửa", new PropertyObject(4, (long)FilterValue.States.NumOfDoor4)},
            {"5 cửa", new PropertyObject(5, (long)FilterValue.States.NumOfDoor5)}
        };
        public static Dictionary<string, PropertyObject> Classifications = new Dictionary<string, PropertyObject>()
        {
            {"Sedan", new PropertyObject(1, (long)FilterValue.States.ClassificationSedan)},
            {"SUV", new PropertyObject(2, (long)FilterValue.States.ClassificationSuv)},
            {"Coupe", new PropertyObject(3, (long)FilterValue.States.ClassificationCoupe)},
            {"CUV", new PropertyObject(4, (long)FilterValue.States.ClassificationCrossover)},
            {"Hatchback", new PropertyObject(5, (long)FilterValue.States.ClassificationHatchback)},
            {"Convertible", new PropertyObject(6, (long)FilterValue.States.ClassificationConvertible)},//Cabriolet
            {"Truck", new PropertyObject(7, (long)FilterValue.States.ClassificationTruck)},
            {"Van/Minivan", new PropertyObject(8, (long)FilterValue.States.ClassificationVan)},
            {"Wagon", new PropertyObject(9, (long)FilterValue.States.ClassificationWagon)},
            {"MPV", new PropertyObject(12, (long)FilterValue.States.ClassificationMpv)},
            {"Pick-up Truck", new PropertyObject(14, (long)FilterValue.States.ClassificationPickUp)},
            {"Sport Car", new PropertyObject(15, (long)FilterValue.States.ClassificationSportCar)},
            {"City Car", new PropertyObject(16, (long)FilterValue.States.ClassificationCityCar)}
        };
        public static Dictionary<string, PropertyObject> ClassificationShortName = new Dictionary<string, PropertyObject>()
        {
            {"sedan", new PropertyObject(1, (long)FilterValue.States.ClassificationSedan)},
            {"suv", new PropertyObject(2, (long)FilterValue.States.ClassificationSuv)},
            {"coupe", new PropertyObject(3, (long)FilterValue.States.ClassificationCoupe)},
            {"cuv", new PropertyObject(4, (long)FilterValue.States.ClassificationCrossover)},
            {"hatchback", new PropertyObject(5, (long)FilterValue.States.ClassificationHatchback)},
            {"convertible", new PropertyObject(6, (long)FilterValue.States.ClassificationConvertible)},//Cabriolet
            {"truck", new PropertyObject(7, (long)FilterValue.States.ClassificationTruck)},
            {"van-minivan", new PropertyObject(8, (long)FilterValue.States.ClassificationVan)},
            {"wagon", new PropertyObject(9, (long)FilterValue.States.ClassificationWagon)},
            {"mpv", new PropertyObject(12, (long)FilterValue.States.ClassificationMpv)},
            {"pick-up-truck", new PropertyObject(14, (long)FilterValue.States.ClassificationPickUp)},
            {"sport-car", new PropertyObject(15, (long)FilterValue.States.ClassificationSportCar)},
            {"city-car", new PropertyObject(16, (long)FilterValue.States.ClassificationCityCar)}
        };
        public static Dictionary<string, byte> FuelInjection = new Dictionary<string, byte>()
        {
            {"Phun xăng điện tử", 1},
            {"Chế hòa khí", 2}
        };
        public static Dictionary<string, byte> PriceType = new Dictionary<string, byte>()
        {
            {"VND", 1},
            {"USD", 2}
        };

        public static Dictionary<string, byte> VipType = new Dictionary<string, byte>()
        {
            {"Vip", 1},
            {"Thường", 0}
        };

        public static Dictionary<string, string> Menh = new Dictionary<string, string>()
        {
            {"Kim", "kim"},
            {"Mộc", "moc"},
            {"Thổ", "tho"},
            {"Thủy", "thuy"},
            {"Hỏa", "hoa"}
        };

        public static List<ServiceType> ListServiceType = new List<ServiceType>()
        {
            new ServiceType(1, "Gara chính hãng", ""),
            new ServiceType(2,  "Gara thường", "/images/garathuong.png"),
            new ServiceType(3,  "Phụ tùng xe", "/images/phutung.png"),
            new ServiceType(4,  "Nội ngoại thất - Đồ chơi", "/images/dochoi.png"),
            new ServiceType(5,  "Bãi đỗ xe", "/images/baidoxe.png"),
            new ServiceType(6,  "Bãi rửa xe", "/images/ruaxe.png"),
            new ServiceType(7,  "Cho thuê xe", "/images/chothue.png"),
            new ServiceType(8,  "Dạy lái xe", "/images/daylai.png"),
            new ServiceType(9,  "Trạm xăng", "/images/tramxang.png")
        };

        public static Dictionary<string, byte> VideoCategory = new Dictionary<string, byte>()
        {
            {"Đánh giá xe", 1},
            {"Giao thông", 2},
            {"Độc và lạ", 3}
        };

        public static Dictionary<string, PropertyObject> Banks = new Dictionary<string, PropertyObject>()
        {
            {"Carcheck", new PropertyObject(1, 1 << 0)},
            {"Viettin bank", new PropertyObject(2,1 << 1)},
            {"Pvcom bank", new PropertyObject(3,1 << 2 )}
        };

        public static Dictionary<string, byte> EmailType = new Dictionary<string, byte>()
        {
            {"Ngay khi có tin", 1},
            {"Hàng ngày", 2},
            {"2 lần trên tuần", 3},
            {"Hàng tuần", 4}
        };
        public enum TypeKeywordCommon : byte
        {
            SecondHand = 1,
            Classification = 2
        }
        public enum StaticContentType : int
        {
            About = -1,
            Sapo = -2
        }
        public enum SubscribeEmailStatus : byte
        {
            Delete = 0,
            New = 1,
            Lock = 2
        }

        public enum SubscribeEmailSendingType : byte
        {
            Now = 1,
            EveryDay = 2,
            TwoOfWeek = 3,
            EveryWeek = 4
        }

        public enum RegisterMarketingEmailStatus : byte
        {
            New = 1,
            UnSubscribe = 2
        }

        /// <summary>
        /// Định nghĩa kiểu nhận khuyến mại tiền vào tài khoản hay vòng quay may mắn.
        /// </summary>
        public enum SurveyDecember2016Type : byte
        {
            AddMoney = 1,
            LuckyWheel = 2
        }
        public enum ReportOption
        {
            Parameters = 1 << 0,
            Price = 1 << 1,
            Image = 1 << 2,
            Coincide = 1 << 3,
            Bogus = 1 << 4,
            NotCommunicate = 1 << 5,
            Sold = 1 << 6,
            ReportContent = 1 << 7,
            ReportFunction = 1 << 8,
            ReportError = 1 << 9
        }
        public enum VideosStatus
        {
            Delete = 1 << 0,          //1
            DisAprrove = 1 << 1,          //2 
            Draff = 1 << 2,          //4
            Aprroved = 1 << 3,          //8
            MostRead = 1 << 4,          //16
            HotHome = 1 << 5,          //32 
            HotCate = 1 << 6,          //64 
            //ContainVideo = 1 << 7       //128
        }
    }

    public class ServiceType
    {
        public ServiceType(int id, string serviceName, string icon)
        {
            this.Id = id;
            this.ServiceName = serviceName;
            this.Icon = icon;
        }
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Icon { get; set; }
    }

    public class PropertyObject
    {
        public PropertyObject(byte iD, long bitValue)
        {
            ID = iD;
            BitValue = bitValue;
        }
        public PropertyObject(byte iD, long bitValue, string extValue)
        {
            ID = iD;
            BitValue = bitValue;
            ExtValue = extValue;
        }
        public byte ID { get; set; }
        public long BitValue { get; set; }
        public string ExtValue { get; set; }
    }
}
