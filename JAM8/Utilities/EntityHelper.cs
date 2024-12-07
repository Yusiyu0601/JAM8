using System.Data;
using System.Reflection;

namespace JAM8.Utilities
{
    /// <summary>
    /// 实体类与dataTable转换类(Native AOT存在Bug)
    /// </summary>
    public class EntityHelper
    {
        /// <summary>
        /// 将泛型列表对象转换成DataTable,如果列表为空将返回空的DataTable结构
        /// </summary>
        public static DataTable entities_to_dataTable<T>(IList<T> entities)
        {
            DataTable dt = new();

            //取类型T所有Propertie
            Type entityType = typeof(T);
            PropertyInfo[] entityProperties = entityType.GetProperties();
            Type colType = null;
            foreach (PropertyInfo propInfo in entityProperties)
            {
                if (propInfo.PropertyType.IsGenericType)
                {
                    colType = Nullable.GetUnderlyingType(propInfo.PropertyType);
                }
                else
                {
                    colType = propInfo.PropertyType;
                }

                if (colType.FullName.StartsWith("System"))
                {
                    dt.Columns.Add(propInfo.Name, colType);
                }
            }

            if (entities != null && entities.Count > 0)
            {
                foreach (T entity in entities)
                {
                    DataRow newRow = dt.NewRow();
                    foreach (PropertyInfo propInfo in entityProperties)
                    {
                        if (dt.Columns.Contains(propInfo.Name))
                        {
                            object objValue = propInfo.GetValue(entity, null);
                            newRow[propInfo.Name] = objValue == null ? DBNull.Value : objValue;
                        }
                    }
                    dt.Rows.Add(newRow);
                }
            }

            return dt;
        }

        /// <summary>
        /// 将DataTable转换成泛型列表对象
        /// </summary>
        public static List<T> dataTable_to_entities<T>(DataTable dt)
        {
            List<T> entiyList = new();

            Type entityType = typeof(T);
            PropertyInfo[] entityProperties = entityType.GetProperties();

            foreach (DataRow row in dt.Rows)
            {
                T entity = Activator.CreateInstance<T>();
                foreach (PropertyInfo propInfo in entityProperties)
                {
                    if (dt.Columns.Contains(propInfo.Name))
                    {
                        if (!row.IsNull(propInfo.Name))
                        {
                            propInfo.SetValue(entity, row[propInfo.Name], null);
                        }
                    }
                }
                entiyList.Add(entity);
            }

            return entiyList;
        }

        public static List<T> DataTableToEntities<T>(DataTable dt) where T : new()
        {
            var entityList = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T entity = new T();
                foreach (DataColumn column in dt.Columns)
                {
                    var propInfo = typeof(T).GetProperty(column.ColumnName);
                    if (propInfo != null && propInfo.CanWrite && !row.IsNull(column.ColumnName))
                    {
                        propInfo.SetValue(entity, Convert.ChangeType(row[column.ColumnName], propInfo.PropertyType));
                    }
                }
                entityList.Add(entity);
            }
            return entityList;
        }
    }
}
