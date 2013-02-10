using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Lib.Web.Mvc.DynamicColumns;
using oikonomos.data;
using oikonomos.common;

namespace oikonomos.web.Controllers
{
    public class SysAdminController : Controller
    {
        public JsonResult RunSql(string queryString)
        {
            queryString = queryString.Replace("\\n", "\r\n").Replace("\"", string.Empty);
            var response = new DynamicColumnResponse();
            var currentPerson = (Person)Session[SessionVariable.LoggedOnPerson];
            if (currentPerson.HasPermission(Permissions.SystemAdministrator))
            {
                try
                {
                    using (var con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
                    {
                        con.Open();

                        using (var cmd = new SqlCommand(queryString, con))
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            var results = new DataTable();
                            response.ColumnModel = new List<ColumnModel>();
                            da.Fill(results);
                            foreach (DataColumn column in results.Columns)
                            {
                                response.ColumnModel.Add(new ColumnModel { index = column.ColumnName, label = column.ColumnName, name = column.ColumnName, width = 100 });
                            }

                            response.RowValues = new List<Dictionary<string, object>>();

                            if (response.ColumnModel.Count == 0)
                            {
                                response.ColumnModel.Add(new ColumnModel
                                                             {
                                                                 index = "0",
                                                                 label = "Response",
                                                                 name = "Response",
                                                                 width = 100
                                                             });
                                response.RowValues.Add(new Dictionary<string, object> { { "Response", "No results" } });
                            }
                            else
                            {
                                foreach (DataRow row in results.Rows)
                                {
                                    var rowValues = new Dictionary<string, object>();
                                    foreach (DataColumn column in results.Columns)
                                    {
                                        if (row[column] is DateTime)
                                        {
                                            rowValues.Add(column.ColumnName, ((DateTime)row[column]).ToString("yyyy/MM/dd HH:mm"));
                                            continue;
                                        }
                                        rowValues.Add(column.ColumnName, row[column]);
                                    }
                                    response.RowValues.Add(rowValues);
                                }
                            }
                        }
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    response.Message = string.Format("There was a problem running the query. {0}", ex.Message);
                }
            }
            else
            {
                response.Message = ExceptionMessage.InvalidCredentials;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}
