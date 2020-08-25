using DVG.WIS.Utilities;
using DVG.WIS.Utilities.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core
{
    public class DVGException : Exception
    {
        public DVGException() : base()
        {
        }

        public DVGException(string message) : base(message)
        {
        }

        public DVGException(string message, DVGException innerException) : base(message, innerException)
        {
        }

        public override string Message { get; }

        public override string Source { get; set; }

        public object[] InputParams { get; set; }

        public override string ToString()
        {
            DVGException self = this;

            if (self == null) return string.Empty;

            string jsonString = NewtonJson.Serialize(self);

            return jsonString;
        }
    }
}
