using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;
using SmartSchoolLifeAPI.Models.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace SmartSchoolLifeAPI.Models.Extensions
{
    /// <summary>
    /// Class for Extensions methods that help to map the objects when using ADO.NET
    /// instead of using Manual mapping
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// Returns a mapped list of objects.
        /// </summary>
        /// <param name="reader">The SqlDataReader that reads values from the database.</param>
        /// <param name="customDate">The DateFormat that you need.</param>
        /// <returns>A list containing objects with formatted DateTime and trimmed strings.</returns>
        public static List<dynamic> MapAll(this SqlDataReader reader, string customDate = "dd-MM-yyyy hh:mm tt")
        {
            List<dynamic> resultList = new List<dynamic>();

            while (reader.Read())
            {
                dynamic grade = new System.Dynamic.ExpandoObject();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    object columnValue = reader.GetValue(i);

                    if (columnValue != DBNull.Value)
                    {
                        ((IDictionary<string, object>)grade)[columnName] =
                            (columnValue is DateTime)
                                ? ((DateTime)columnValue).ToString(customDate)
                                : (columnValue is string)
                                    ? ((string)columnValue).Trim()
                                    : columnValue;
                    }
                    else
                    {
                        ((IDictionary<string, object>)grade)[columnName] = null;
                    }
                }
                resultList.Add(grade);
            }
            return resultList;
        }

        /// <summary>
        /// Returns a mapped object.
        /// </summary>
        /// <param name="reader">The SqlDataReader that reads values from the database.</param>
        /// <param name="customDate">The DateFormat that you need.</param>
        /// <returns>An object with formatted DateTime and trimmed strings, or null if the reader doesn't read.</returns>
        public static dynamic MapSingle(this SqlDataReader reader, string customDate = "dd-MM-yyyy hh:mm tt")
        {
            if (reader.Read())
            {
                dynamic result = new System.Dynamic.ExpandoObject();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    object columnValue = reader.GetValue(i);

                    if (columnValue != DBNull.Value)
                    {
                        ((IDictionary<string, object>)result)[columnName] =
                            (columnValue is DateTime)
                                ? ((DateTime)columnValue).ToString(customDate)
                                : (columnValue is string)
                                    ? ((string)columnValue).Trim()
                                    : columnValue;
                    }
                    else
                    {
                        ((IDictionary<string, object>)result)[columnName] = null;
                    }
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// Convert object from type to other type.
        /// </summary>
        /// <param name="TTarget">The Type that you need to convert your object to.</param>
        /// <param name="source">The object that you need to convert</param>
        /// <returns>A new object </returns>
        public static TTarget MapObjectTo<TTarget>(this object source) where TTarget : new()
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            TTarget result = new TTarget();

            var sourceDict = (IDictionary<string, object>)source;

            foreach (var kvp in sourceDict)
            {
                var destinationProperty = typeof(TTarget).GetProperty(kvp.Key);

                if (destinationProperty != null)
                {
                    object value = kvp.Value;
                    destinationProperty.SetValue(result, Convert.ChangeType(value, destinationProperty.PropertyType));
                }
            }

            return result;
        }

        public static List<TTarget> MapListTo<TTarget>(this IEnumerable<dynamic> sourceList) where TTarget : new()
        {
            if (sourceList == null)
                throw new ArgumentNullException(nameof(sourceList));

            List<TTarget> resultList = new List<TTarget>();

            foreach (var source in sourceList)
            {
                TTarget resultItem = new TTarget();

                var sourceDict = (IDictionary<string, object>)source;

                foreach (var kvp in sourceDict)
                {
                    var destinationProperty = typeof(TTarget).GetProperty(kvp.Key);

                    if (destinationProperty != null)
                    {
                        object value = kvp.Value;
                        destinationProperty.SetValue(resultItem, Convert.ChangeType(value, destinationProperty.PropertyType));
                    }
                }

                resultList.Add(resultItem);
            }

            return resultList;
        }

    }
}
