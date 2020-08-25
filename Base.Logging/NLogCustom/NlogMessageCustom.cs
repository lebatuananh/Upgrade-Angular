using Newtonsoft.Json;

namespace Base.Logging.NLogCustom
{
    public class NlogMessageCustom
    {
        public string Messages { get; set; }
        public object[] Params { get; set; }

        public static string ObjectSerialize(string mesage, params object[] paramsObjects)
        {
            return JsonConvert.SerializeObject(new
            { Messages = mesage, Params = paramsObjects });
        }
    }
}