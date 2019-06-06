using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;  

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
                var timesheetDate = new DateTime(2019, 05, 30);

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

            for (int i = 0; i < dt.Rows.Count ; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                }
            }
            workbook.SaveAs("D:\\ClickTimeReport.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }
    }
}
