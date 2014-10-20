namespace 班級考試成績單
{
    partial class frmHome
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
            this.lstSubject = new SmartSchool.Common.ListViewEX();
            this.btnTemplate = new DevComponents.DotNetBar.ButtonX();
            this.cmbSemester = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSchoolYear = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbExamList = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbPrefixList = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.btnPrint = new DevComponents.DotNetBar.ButtonX();
            this.groupPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupPanel2
            // 
            this.groupPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPanel2.AutoScroll = true;
            this.groupPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupPanel2.BackColor = System.Drawing.Color.Transparent;
            this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
            this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.groupPanel2.Controls.Add(this.lstSubject);
            this.groupPanel2.DrawTitleBox = false;
            this.groupPanel2.Location = new System.Drawing.Point(4, 63);
            this.groupPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.groupPanel2.Name = "groupPanel2";
            this.groupPanel2.Size = new System.Drawing.Size(283, 243);
            // 
            // 
            // 
            this.groupPanel2.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.groupPanel2.Style.BackColorGradientAngle = 90;
            this.groupPanel2.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.groupPanel2.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderBottomWidth = 1;
            this.groupPanel2.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.groupPanel2.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderLeftWidth = 1;
            this.groupPanel2.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderRightWidth = 1;
            this.groupPanel2.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.groupPanel2.Style.BorderTopWidth = 1;
            this.groupPanel2.Style.Class = "";
            this.groupPanel2.Style.CornerDiameter = 4;
            this.groupPanel2.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.groupPanel2.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
            this.groupPanel2.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.groupPanel2.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseDown.Class = "";
            this.groupPanel2.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.groupPanel2.StyleMouseOver.Class = "";
            this.groupPanel2.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.groupPanel2.TabIndex = 10;
            this.groupPanel2.Text = "科目";
            // 
            // lstSubject
            // 
            this.lstSubject.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            // 
            // 
            // 
            this.lstSubject.Border.Class = "ListViewBorder";
            this.lstSubject.Border.CornerDiameter = 0;
            this.lstSubject.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lstSubject.CheckBoxes = true;
            this.lstSubject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lstSubject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSubject.HideSelection = false;
            this.lstSubject.Location = new System.Drawing.Point(0, 0);
            this.lstSubject.Margin = new System.Windows.Forms.Padding(0);
            this.lstSubject.Name = "lstSubject";
            this.lstSubject.Size = new System.Drawing.Size(277, 216);
            this.lstSubject.TabIndex = 1;
            this.lstSubject.UseCompatibleStateImageBehavior = false;
            this.lstSubject.View = System.Windows.Forms.View.List;
            // 
            // btnTemplate
            // 
            this.btnTemplate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTemplate.BackColor = System.Drawing.Color.Transparent;
            this.btnTemplate.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnTemplate.Location = new System.Drawing.Point(131, 344);
            this.btnTemplate.Name = "btnTemplate";
            this.btnTemplate.Size = new System.Drawing.Size(75, 23);
            this.btnTemplate.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnTemplate.TabIndex = 0;
            this.btnTemplate.Text = "列印設定";
            this.btnTemplate.Click += new System.EventHandler(this.btnTemplate_Click);
            // 
            // cmbSemester
            // 
            this.cmbSemester.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSemester.DisplayMember = "Text";
            this.cmbSemester.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSemester.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSemester.FormattingEnabled = true;
            this.cmbSemester.ItemHeight = 19;
            this.cmbSemester.Location = new System.Drawing.Point(195, 7);
            this.cmbSemester.Name = "cmbSemester";
            this.cmbSemester.Size = new System.Drawing.Size(90, 25);
            this.cmbSemester.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(155, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 17);
            this.label2.TabIndex = 15;
            this.label2.Text = "學期";
            // 
            // cmbSchoolYear
            // 
            this.cmbSchoolYear.DisplayMember = "Text";
            this.cmbSchoolYear.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSchoolYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSchoolYear.FormattingEnabled = true;
            this.cmbSchoolYear.ItemHeight = 19;
            this.cmbSchoolYear.Location = new System.Drawing.Point(58, 7);
            this.cmbSchoolYear.Name = "cmbSchoolYear";
            this.cmbSchoolYear.Size = new System.Drawing.Size(85, 25);
            this.cmbSchoolYear.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 13;
            this.label1.Text = "學年度";
            // 
            // cmbExamList
            // 
            this.cmbExamList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbExamList.DisplayMember = "Text";
            this.cmbExamList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbExamList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExamList.FormattingEnabled = true;
            this.cmbExamList.ItemHeight = 19;
            this.cmbExamList.Location = new System.Drawing.Point(58, 38);
            this.cmbExamList.Name = "cmbExamList";
            this.cmbExamList.Size = new System.Drawing.Size(227, 25);
            this.cmbExamList.TabIndex = 17;
            this.cmbExamList.SelectedIndexChanged += new System.EventHandler(this.cmbExamList_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(18, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 17);
            this.label3.TabIndex = 18;
            this.label3.Text = "試別";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(11, 318);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 17);
            this.label4.TabIndex = 19;
            this.label4.Text = "類組排名群組";
            // 
            // cmbPrefixList
            // 
            this.cmbPrefixList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPrefixList.DisplayMember = "Text";
            this.cmbPrefixList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPrefixList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrefixList.FormattingEnabled = true;
            this.cmbPrefixList.ItemHeight = 19;
            this.cmbPrefixList.Location = new System.Drawing.Point(103, 314);
            this.cmbPrefixList.Name = "cmbPrefixList";
            this.cmbPrefixList.Size = new System.Drawing.Size(182, 25);
            this.cmbPrefixList.TabIndex = 20;
            // 
            // btnPrint
            // 
            this.btnPrint.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnPrint.Location = new System.Drawing.Point(212, 344);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnPrint.TabIndex = 21;
            this.btnPrint.Text = "列印";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // frmHome
            // 
            this.ClientSize = new System.Drawing.Size(291, 368);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.cmbPrefixList);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbExamList);
            this.Controls.Add(this.cmbSemester);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbSchoolYear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTemplate);
            this.Controls.Add(this.groupPanel2);
            this.Name = "frmHome";
            this.Text = "";
            this.TitleText = "個人考試成績單";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.ButtonX btnTemplate;
        private SmartSchool.Common.ListViewEX lstSubject;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSemester;
        private System.Windows.Forms.Label label2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSchoolYear;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbExamList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbPrefixList;
        private DevComponents.DotNetBar.ButtonX btnPrint;
    }
}
