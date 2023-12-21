using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoPOS.Service.Common
{
    public class NumberHandler<T> : SqlMapper.TypeHandler<T>
    {
        public override T Parse(object value)
        {
            if (value == null || value is DBNull) return default(T);

            if (value is long longValue)
            {
                if (typeof(T) == typeof(decimal))
                {
                    return (T)(object)decimal.Parse(longValue.ToString());
                }
                if (typeof(T) == typeof(double))
                {
                    return (T)(object)double.Parse(longValue.ToString());
                }
            }
            if (value is short shortValue)
            {
                if (typeof(T) == typeof(decimal))
                {
                    return (T)(object)(decimal)shortValue;
                }
                if (typeof(T) == typeof(double))
                {
                    return (T)(object)(double)shortValue;
                }
            }

            return (T)value;
        }

        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = value;
        }
    }
}
