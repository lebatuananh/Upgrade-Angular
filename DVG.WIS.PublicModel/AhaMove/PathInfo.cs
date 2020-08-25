using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.PublicModel.AhaMove
{
    public class PathInfo
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }

        /// <summary>
        /// Cache on delivery
        /// </summary>
        public int COD { get; set; }
    }
}
