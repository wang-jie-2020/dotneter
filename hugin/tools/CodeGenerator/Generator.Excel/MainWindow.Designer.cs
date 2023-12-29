namespace ExcelGenerator
{
    partial class MainWindow
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.textExcel = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkAutoIntToEnum = new System.Windows.Forms.CheckBox();
            this.chkAutoIntToBool = new System.Windows.Forms.CheckBox();
            this.chkRemarkAsAnnotation = new System.Windows.Forms.CheckBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textExcel
            // 
            this.textExcel.Location = new System.Drawing.Point(3, 47);
            this.textExcel.Multiline = true;
            this.textExcel.Name = "textExcel";
            this.textExcel.Size = new System.Drawing.Size(1000, 518);
            this.textExcel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkAutoIntToEnum);
            this.panel1.Controls.Add(this.chkAutoIntToBool);
            this.panel1.Controls.Add(this.chkRemarkAsAnnotation);
            this.panel1.Controls.Add(this.btnCreate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 568);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1007, 92);
            this.panel1.TabIndex = 2;
            // 
            // chkAutoIntToEnum
            // 
            this.chkAutoIntToEnum.AutoSize = true;
            this.chkAutoIntToEnum.Checked = true;
            this.chkAutoIntToEnum.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoIntToEnum.Location = new System.Drawing.Point(16, 52);
            this.chkAutoIntToEnum.Margin = new System.Windows.Forms.Padding(2);
            this.chkAutoIntToEnum.Name = "chkAutoIntToEnum";
            this.chkAutoIntToEnum.Size = new System.Drawing.Size(102, 16);
            this.chkAutoIntToEnum.TabIndex = 5;
            this.chkAutoIntToEnum.Text = "尝试int转Enum";
            this.chkAutoIntToEnum.UseVisualStyleBackColor = true;
            // 
            // chkAutoIntToBool
            // 
            this.chkAutoIntToBool.AutoSize = true;
            this.chkAutoIntToBool.Checked = true;
            this.chkAutoIntToBool.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoIntToBool.Location = new System.Drawing.Point(16, 18);
            this.chkAutoIntToBool.Margin = new System.Windows.Forms.Padding(2);
            this.chkAutoIntToBool.Name = "chkAutoIntToBool";
            this.chkAutoIntToBool.Size = new System.Drawing.Size(102, 16);
            this.chkAutoIntToBool.TabIndex = 5;
            this.chkAutoIntToBool.Text = "尝试int转bool";
            this.chkAutoIntToBool.UseVisualStyleBackColor = true;
            // 
            // chkRemarkAsAnnotation
            // 
            this.chkRemarkAsAnnotation.AutoSize = true;
            this.chkRemarkAsAnnotation.Location = new System.Drawing.Point(162, 18);
            this.chkRemarkAsAnnotation.Margin = new System.Windows.Forms.Padding(2);
            this.chkRemarkAsAnnotation.Name = "chkRemarkAsAnnotation";
            this.chkRemarkAsAnnotation.Size = new System.Drawing.Size(120, 16);
            this.chkRemarkAsAnnotation.TabIndex = 4;
            this.chkRemarkAsAnnotation.Text = "属性注释包含说明";
            this.chkRemarkAsAnnotation.UseVisualStyleBackColor = true;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(861, 18);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(134, 62);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.Text = "生成文件";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.textExcel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1007, 568);
            this.panel2.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(12, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 21);
            this.label5.TabIndex = 2;
            this.label5.Text = "Excel";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 660);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1027, 710);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(855, 460);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "代码生成器";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textExcel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkRemarkAsAnnotation;
        private System.Windows.Forms.CheckBox chkAutoIntToEnum;
        private System.Windows.Forms.CheckBox chkAutoIntToBool;
    }
}

