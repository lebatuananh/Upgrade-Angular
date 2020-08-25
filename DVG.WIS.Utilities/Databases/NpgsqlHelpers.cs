using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVG.WIS.Utilities.Databases
{
    public class NpgsqlHelpers
    {
        protected NpgsqlParameter NpgsqlParameter<T>(string paramName, T obj)
        {
            return obj != null ? new NpgsqlParameter(paramName.ToLower(), obj)
            : new NpgsqlParameter(paramName.ToLower(), DBNull.Value);
        }

        /// <summary>
        /// The NPGSQL parameter
        /// Author: ThanhDT
        /// Created date: 8/8/2020 4:41 PM
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="obj">The object.</param>
        /// <param name="npgsqlDbType">Type of the NPGSQL database.</param>
        /// <returns></returns>
        protected NpgsqlParameter NpgsqlParameter<T>(string paramName, T obj, NpgsqlDbType npgsqlDbType)
        {
            var param = new NpgsqlParameter(paramName.ToLower(), npgsqlDbType);
            if (obj != null)
            {
                param.Value = obj;
            }
            else
            {
                param.Value = DBNull.Value;
            }
            return param;
        }
    }
}
