using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Accounting.Models;
using Accounting.Utils;

namespace Accounting
{
    internal static class DataTableConfig
    {
        internal static void ConfigDataTable(this DataTable table, Type type)
        {
            IEnumerable<PropertyInfo> properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var item in properties)
            {
                switch (type.Name)
                {
                    case "AccountingForNotifications_Model":
                        table.Columns.Add(LangHelper.AccountingForNotification_ModelDictionary[item.Name]);
                        break;
                    case "AccountingForOriginals_Model":
                        table.Columns.Add(LangHelper.AccountingForOriginals_ModelDictionary[item.Name]);
                        break;
                    default:
                        break;
                }
                
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
                                where range.Document_Name.ToLower().Contains(FilterText.ToLower())
                                || range.Title_of_the_document.ToLower().Contains(FilterText.ToLower())
                                || range.Inventory_Document_Number.ToString().ToLower().Contains(FilterText.ToLower())
                                select range);
                    break;
                case nameof(AccountingForNotifications_Model):
                    Data = (IEnumerable<T>)(from range in Models as List<AccountingForNotifications_Model>
                                where range.Product_index.Contains(FilterText.ToLower())
                                || range.Designation_Notice.ToLower().Contains(FilterText.ToLower())
                                || range.Document_Change_Designation.ToLower().Contains(FilterText.ToLower())
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
