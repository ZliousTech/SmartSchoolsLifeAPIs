using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;

namespace SmartSchoolLifeAPI.Models.Shared
{
    public static class MapperExtensions
    {
        public static List<dynamic> MapAll(this SqlDataReader reader)
        {
            List<dynamic> resultList = new List<dynamic>();

            while (reader.Read())
            {
                dynamic grade = new System.Dynamic.ExpandoObject();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    object columnValue = reader.GetValue(i);

                    if (columnValue is DateTime)
                        ((IDictionary<string, object>)grade)[columnName] = ((DateTime)columnValue).ToString("dd-MM-yyyy hh:mm tt");
                    else if (columnValue is string)
                        ((IDictionary<string, object>)grade)[columnName] = ((string)columnValue).Trim();
                    else
                        ((IDictionary<string, object>)grade)[columnName] = columnValue;
                }

                resultList.Add(grade);
            }

            return resultList;
        }

        public static dynamic MapSingle(this SqlDataReader reader)
        {
            if (reader.Read())
            {
                dynamic result = new System.Dynamic.ExpandoObject();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    object columnValue = reader.GetValue(i);

                    if (columnValue is DateTime)
                        ((IDictionary<string, object>)result)[columnName] = ((DateTime)columnValue).ToString("dd-MM-yyyy hh:mm:ss tt");
                    else if (columnValue is string)
                        ((IDictionary<string, object>)result)[columnName] = ((string)columnValue).Trim();
                    else
                        ((IDictionary<string, object>)result)[columnName] = columnValue;
                }

                return result;
            }

            return null;
        }
    }
}
