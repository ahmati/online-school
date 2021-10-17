using System;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace ItalWebConsulting.Infrastructure.DataAccess.AdoNet
{
    public static class IDataReaderExtension
    {
        public static string ToCSV(this IDataReader dataReader, bool includeHeaderAsFirstRow = true, string separator = ";")
        {

            StringBuilder csvRows = new StringBuilder();
            StringBuilder tmpRow = new StringBuilder();
            int columns;
            using (var dataTable = new DataTable())
            {
                dataTable.Load(dataReader);
                columns = dataTable.Columns.Count;
                //Create Header
                if (includeHeaderAsFirstRow)
                {
                    for (int index = 0; index < columns; index++)
                    {
                        tmpRow.Append((dataTable.Columns[index]));
                        if (index < columns - 1)
                            tmpRow.Append(separator);
                    }
                    tmpRow.Append(Environment.NewLine);
                }
                csvRows.Append(tmpRow);

                //Create Rows
                for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                {
                    tmpRow.Clear();
                    //Row
                    for (int index = 0; index < columns; index++)
                    {
                        var value = dataTable.Rows[rowIndex][index].ToString();

                        //If type of field is string
                        if (dataTable.Rows[rowIndex][index] is string)
                        {
                            //If double quotes are used in value, ensure each are replaced by double quotes.
                            if (value.IndexOf("\"") >= 0)
                                value = value.Replace("\"", "\"\"");

                            //If separtor are is in value, ensure it is put in double quotes.
                            if (value.IndexOf(separator) >= 0)
                                value = "\"" + value + "\"";

                            //If string contain new line character
                            while (value.Contains("\r"))
                                value = value.Replace("\r", "");

                            while (value.Contains("\n"))
                                value = value.Replace("\n", "");

                        }
                        tmpRow.Append(value);
                        if (index < columns - 1)
                            tmpRow.Append(separator);
                    }
                    dataTable.Rows[rowIndex][columns - 1].ToString().ToString().Replace(separator, " ");
                    tmpRow.Append(Environment.NewLine);
                    csvRows.Append(tmpRow);
                }
            }

            return csvRows.ToString();
        }
        public static char GetChar(this IDataReader reader, string colName)
        {
            try
            {
                var i = reader.GetOrdinal(colName);

                return reader.GetString(i).ToCharArray()[0];
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }

        }

        public static char? GetNullableChar(this IDataReader reader, string colName)
        {
            try
            {
                var i = reader.GetOrdinal(colName);
                if (reader.IsDBNull(i)) return new char?();

                return GetChar(reader, colName);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static string GetString(this IDataReader dr, string colName)
        {
            try
            {
                if (dr.IsDBNull(dr.GetOrdinal(colName)))
                {
                    return string.Empty;
                }
                return dr.GetString(dr.GetOrdinal(colName));
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static string GetNullableString(this IDataReader reader, string colName)
        {
            try
            {
                var i = reader.GetOrdinal(colName);

                return GetNullableString(reader, i);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static string GetNullableString(this IDataReader reader, Expression<Func<object>> expression)
        {
            return GetNullableString(reader, expression.Name);
        }

        public static string GetNullableString(this IDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;

            return reader.GetString(index).Trim();
        }

        public static Guid GetGuid(this IDataReader dr, string colName)
        {
            try
            {
                return dr.GetGuid(dr.GetOrdinal(colName));

            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static Guid? GetNullableGuid(this IDataReader reader, string colName)
        {
            try
            {
                var i = reader.GetOrdinal(colName);

                return GetNullableGuid(reader, i);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static Guid? GetNullableGuid(this IDataReader reader, Expression<Func<object>> expression)
        {
            return GetNullableGuid(reader, expression.Name);
        }

        public static Guid? GetNullableGuid(this IDataReader reader, int index)
        {
            return reader.IsDBNull(index) ? default(Guid?) : reader.GetGuid(index);
        }

        public static double GetDouble(this IDataReader dr, string colName)
        {
            try
            {
                return dr.GetDouble(dr.GetOrdinal(colName));
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static double? GetNullableDouble(this IDataReader reader, string colName)
        {
            try
            {
                var i = reader.GetOrdinal(colName);

                return GetNullableDouble(reader, i);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static double? GetNullableDouble(this IDataReader reader, Expression<Func<object>> expression)
        {
            return GetNullableDouble(reader, expression.Name);
        }

        public static double? GetNullableDouble(this IDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return new double?();

            return reader.GetDouble(index);
        }

        public static DateTime GetDateTime(this IDataReader dr, string colName)
        {
            try
            {
                return dr.GetDateTime(dr.GetOrdinal(colName));
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static DateTime? GetNullableDateTime(this IDataReader reader, string colName)
        {
            try
            {
                return GetNullableDateTime(reader, reader.GetOrdinal(colName));
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static DateTime? GetNullableDateTime(this IDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return new DateTime?();

            return reader.GetDateTime(index);
        }

        public static int GetInt32(this IDataReader dr, string colName)
        {
            try
            {
                return dr.GetInt32(dr.GetOrdinal(colName));

            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static int? GetNullableInt32(this IDataReader dr, string colName)
        {
            try
            {
                return dr.GetNullableInt32(dr.GetOrdinal(colName));
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static int? GetNullableInt32(this IDataReader dr, int index)
        {
            if (dr.IsDBNull(index))
            {
                return null;
            }
            return dr.GetInt32(index);
        }

        public static short GetInt16(this IDataReader dr, string colName)
        {
            try
            {
                return dr.GetInt16(dr.GetOrdinal(colName));
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static short? GetNullableInt16(this IDataReader dr, string colName)
        {
            try
            {
                return dr.GetNullableInt16(dr.GetOrdinal(colName));

            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static short? GetNullableInt16(this IDataReader dr, int index)
        {
            if (dr.IsDBNull(index))
            {
                return null;
            }
            return dr.GetInt16(index);
        }
        public static ulong GetUlong(this IDataReader dr, string colName)
        {
            try
            {
                return Convert.ToUInt64(dr.GetInt64(dr.GetOrdinal(colName)));

            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static ulong? GetNullableUlong(this IDataReader dr, string colName)
        {
            try
            {
                return dr.GetNullableUlong(dr.GetOrdinal(colName));
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static ulong? GetNullableUlong(this IDataReader dr, int index)
        {
            if (dr.IsDBNull(index))
            {
                return null;
            }
            return Convert.ToUInt64(dr[index]);
        }

        public static byte GetByte(this IDataReader dr, string colName)
        {
            try
            {
                return dr.GetByte(dr.GetOrdinal(colName));

            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static byte? GetNullableByte(this IDataReader dr, string colName)
        {
            try
            {
                return dr.GetNullableByte(dr.GetOrdinal(colName));

            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static byte? GetNullableByte(this IDataReader dr, int index)
        {
            if (dr.IsDBNull(index))
            {
                return null;
            }
            return dr.GetByte(index);
        }

        public static object GetValue(this IDataReader dr, string colName)
        {
            try
            {
                return dr.GetValue(dr.GetOrdinal(colName));

            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static bool IsDBNull(this IDataReader dr, string colName)
        {
            try
            {
                return Convert.IsDBNull(dr.GetValue(dr.GetOrdinal(colName)));

            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static bool GetBoolean(this IDataReader dr, string colName)
        {
            try
            {
                return dr.GetBoolean(dr.GetOrdinal(colName));
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static bool? GetNullableBoolean(this System.Data.IDataReader dr, int index)
        {
            if (dr.IsDBNull(index))
            {
                return null;
            }
            return dr.GetBoolean(index);
        }

        public static bool? GetNullableBoolean(this IDataReader dr, string colName)
        {
            try
            {
                return dr.GetNullableBoolean(dr.GetOrdinal(colName));

            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static short? GetNullableShort(this IDataReader reader, string colName)
        {
            try
            {
                var i = reader.GetOrdinal(colName);

                return GetNullableShort(reader, i);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(System.Reflection.MethodBase.GetCurrentMethod().Name + ": " + colName, ex);
            }
        }

        public static short? GetNullableShort(this IDataReader reader, Expression<Func<object>> expression)
        {
            return GetNullableShort(reader, expression.Name);
        }

        public static short? GetNullableShort(this IDataReader reader, int index)
        {
            if (reader.IsDBNull(index)) return null;

            return reader.GetInt16(index);
        }
    }
}
