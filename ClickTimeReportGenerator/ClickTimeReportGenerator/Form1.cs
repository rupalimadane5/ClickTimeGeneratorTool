using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClickTimeReportGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var timesheetDate = new DateTime(2020, 04, 29);

                CTReportGenerator generator = new CTReportGenerator(timesheetDate);

                Task<DataTable> tasks = Task.Run(() => generator.GetReport());
                tasks.Wait();

                ExportToExcel(tasks.Result);

                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error : {ex.InnerException.Message}");
            }
        }


        private void ExportToExcel(DataTable dt)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            worksheet.Name = "ClickTime data";


            for (int i = 1; i < dt.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dt.Columns[i - 1].Caption;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();

                    if (dt.Columns[j].ColumnName.Equals("Status"))
                    {
                        Microsoft.Office.Interop.Excel.Range range = worksheet.get_Range($"G{i + 2}");
                        if (Convert.ToString(range.Value) == "Open")
                        {
                            range.Interior.Color = Color.Red;
                        }
                    }

                    //if (dt.Columns[j].ColumnName.Equals("TotalTimesheetHours"))
                    //{
                    //    Microsoft.Office.Interop.Excel.Range range = worksheet.get_Range($"J{i + 2}");
                    //    if (Convert.ToDouble(range.Value) < 8.5)
                    //    {
                    //        range.Interior.Color = Color.Red;
                    //    }
                    //}
                }
            }

            Microsoft.Office.Interop.Excel.Range rowRange = worksheet.get_Range("A1:K1", Missing.Value);
            rowRange.Interior.Color = Color.Yellow;

            if (File.Exists("D:\\ClickTimeReport.xls"))
            {
                File.Delete("D:\\ClickTimeReport.xls");
            }

            workbook.SaveAs("D:\\ClickTimeReport.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }
    }
}
