using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Accounting.Models;

namespace Accounting
{
    internal static class DataTableConfig
    {
        internal static void ConfigDataTable(this DataTable table, Type type)
        {
            IEnumerable<PropertyInfo> properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var item in properties)
            {
                table.Columns.Add(item.Name.Replace("_"," "));
            }
        }

        internal static void FillData<T>(this DataTable table, List<T> Models) where T : IAccountingModel
        {
            foreach (var Model in Models)
            {
                DataRow row = table.NewRow();
                PropertyInfo[] properties = Model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                int i = 0;
                foreach (var column in table.Columns)
                {
                    var Field = typeof(T).GetProperty(properties[i].Name);
                    row[column.ToString()] = Field.GetValue(Model) is null ? "null" : Field.GetValue(Model).ToString();
                    i++;
                }
                table.Rows.Add(row);
            }
        }

        internal static async Task FillDataAsync<T>(this DataTable table, List<T> Models) where T : IAccountingModel
        {
            await Task.Run(() => FillData(table, Models));
        }
         
        internal static void FillDataByFilter<T>(this DataTable table, List<T> Models, string FilterText) where T : IAccountingModel
        {
            IEnumerable<T> Data = null;
            switch (typeof(T).Name)
            {
                case nameof(AccountingForOriginals_Model):
                    Data = (IEnumerable<T>)(from range in Models as List<AccountingForOriginals_Model>
                                where range.Обозначение_документа.ToLower().Contains(FilterText.ToLower())
                                || range.Наименование_документа.ToLower().Contains(FilterText.ToLower())
                                || range.Инвентаризационный_номер_документа.ToString().ToLower().Contains(FilterText.ToLower())
                                select range);
                    break;
                case nameof(AccountingForNotifications_Model):
                    Data = (IEnumerable<T>)(from range in Models as List<AccountingForNotifications_Model>
                                where range.Индекс_изделия.Contains(FilterText.ToLower())
                                || range.Обозначение_Извещения.ToLower().Contains(FilterText.ToLower())
                                || range.Обозначение_Изменяемого_документа.ToLower().Contains(FilterText.ToLower())
                                select range);
                    break;
                default:
                    Data = null;
                    break;
            }
            table.ConfigDataTable(typeof(T));
            table.FillData(Data.ToList());
        }
    }
}
