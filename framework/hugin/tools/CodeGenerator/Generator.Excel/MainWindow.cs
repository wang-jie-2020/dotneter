using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Generator;

namespace ExcelGenerator
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Global.IsRemarkAsAnnotation = chkRemarkAsAnnotation.Checked;
            Global.IsAutoIntToBool = chkAutoIntToBool.Checked;
            Global.IsAutoIntToEnum = chkAutoIntToEnum.Checked;

            var dataInput = textExcel.Text;
            if (string.IsNullOrWhiteSpace(dataInput))
            {
                return;
            }

            var _table = new TempTable();

            var lines = dataInput.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var match = Regex.Match(lines[0], "[a-zA-Z]");
            var index = match.Success ? match.Index : -1;
            if (index == -1)
            {
                throw new Exception("第一行要包含数据表描述");
            }

            _table.Name = lines[0].Substring(index).Trim();
            _table.DisplayName = lines[0].Substring(0, index - 1).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];

            for (index = 2; index < lines.Length; index++)
            {
                var data = lines[index].Split('\t');
                if (data.Length != 6)
                {
                    throw new Exception($"第{index + 1}行Tab数量不正确");
                }

                _table.Columns.Add(new TempColumn()
                {
                    Name = data[2],
                    DisplayName = data[1],
                    Type = data[3],
                    AllowNull = data[4].Equals("Y", StringComparison.OrdinalIgnoreCase),
                    Remark = data[5]
                });
            }

            CodeGenerator.Instance.Start(_table);
            Close();
        }
    }
}
