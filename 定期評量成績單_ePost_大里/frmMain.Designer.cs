namespace 班級考試成績單
{
    partial class frmMain
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
			this.lstSubject = new SmartSchool.Common.ListViewEX();
			this.cmbExamList = new DevComponents.DotNetBar.Controls.ComboBoxEx();
			this.btnPrint = new DevComponents.DotNetBar.ButtonX();
			this.groupPanel1 = new DevComponents.DotNetBar.Controls.GroupPanel();
			this.lstStudentCategory = new SmartSchool.Common.ListViewEX();
			this.groupPanel2 = new DevComponents.DotNetBar.Controls.GroupPanel();
			this.groupPanel3 = new DevComponents.DotNetBar.Controls.GroupPanel();
			this.groupPanel4 = new DevComponents.DotNetBar.Controls.GroupPanel();
			this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
			this.checkBox6 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.groupPanel5 = new DevComponents.DotNetBar.Controls.GroupPanel();
			this.cmbPrefixList = new DevComponents.DotNetBar.Controls.ComboBoxEx();
			this.groupPanel1.SuspendLayout();
			this.groupPanel2.SuspendLayout();
			this.groupPanel3.SuspendLayout();
			this.groupPanel4.SuspendLayout();
			this.panelEx1.SuspendLayout();
			this.groupPanel5.SuspendLayout();
			this.SuspendLayout();
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
			this.lstSubject.Size = new System.Drawing.Size(482, 179);
			this.lstSubject.TabIndex = 0;
			this.lstSubject.UseCompatibleStateImageBehavior = false;
			this.lstSubject.View = System.Windows.Forms.View.List;
			// 
			// cmbExamList
			// 
			this.cmbExamList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbExamList.DisplayMember = "Text";
			this.cmbExamList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cmbExamList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbExamList.FormattingEnabled = true;
			this.cmbExamList.ItemHeight = 19;
			this.cmbExamList.Location = new System.Drawing.Point(138, 7);
			this.cmbExamList.Name = "cmbExamList";
			this.cmbExamList.Size = new System.Drawing.Size(167, 25);
			this.cmbExamList.TabIndex = 4;
			this.cmbExamList.SelectedIndexChanged += new System.EventHandler(this.cmbExamList_SelectedIndexChanged);
			// 
			// btnPrint
			// 
			this.btnPrint.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPrint.BackColor = System.Drawing.Color.Transparent;
			this.btnPrint.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnPrint.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
			this.btnPrint.Location = new System.Drawing.Point(381, 619);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(109, 23);
			this.btnPrint.TabIndex = 6;
			this.btnPrint.Text = "列印";
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// groupPanel1
			// 
			this.groupPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupPanel1.BackColor = System.Drawing.Color.Transparent;
			this.groupPanel1.CanvasColor = System.Drawing.SystemColors.Control;
			this.groupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
			this.groupPanel1.Controls.Add(this.lstStudentCategory);
			this.groupPanel1.Location = new System.Drawing.Point(2, 389);
			this.groupPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.groupPanel1.Name = "groupPanel1";
			this.groupPanel1.Size = new System.Drawing.Size(488, 149);
			// 
			// 
			// 
			this.groupPanel1.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.groupPanel1.Style.BackColorGradientAngle = 90;
			this.groupPanel1.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.groupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel1.Style.BorderBottomWidth = 1;
			this.groupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.groupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel1.Style.BorderLeftWidth = 1;
			this.groupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel1.Style.BorderRightWidth = 1;
			this.groupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel1.Style.BorderTopWidth = 1;
			this.groupPanel1.Style.Class = "";
			this.groupPanel1.Style.CornerDiameter = 4;
			this.groupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
			this.groupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
			this.groupPanel1.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.groupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
			// 
			// 
			// 
			this.groupPanel1.StyleMouseDown.Class = "";
			this.groupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// 
			// 
			this.groupPanel1.StyleMouseOver.Class = "";
			this.groupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.groupPanel1.TabIndex = 9;
			this.groupPanel1.Text = "步驟四：選取不列入各科平均計算之類別";
			// 
			// lstStudentCategory
			// 
			this.lstStudentCategory.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
			// 
			// 
			// 
			this.lstStudentCategory.Border.Class = "ListViewBorder";
			this.lstStudentCategory.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.lstStudentCategory.CheckBoxes = true;
			this.lstStudentCategory.Cursor = System.Windows.Forms.Cursors.Hand;
			this.lstStudentCategory.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstStudentCategory.HideSelection = false;
			this.lstStudentCategory.Location = new System.Drawing.Point(0, 0);
			this.lstStudentCategory.Name = "lstStudentCategory";
			this.lstStudentCategory.Size = new System.Drawing.Size(482, 122);
			this.lstStudentCategory.TabIndex = 9;
			this.lstStudentCategory.UseCompatibleStateImageBehavior = false;
			this.lstStudentCategory.View = System.Windows.Forms.View.List;
			// 
			// groupPanel2
			// 
			this.groupPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupPanel2.AutoScroll = true;
			this.groupPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupPanel2.BackColor = System.Drawing.Color.Transparent;
			this.groupPanel2.CanvasColor = System.Drawing.SystemColors.Control;
			this.groupPanel2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
			this.groupPanel2.Controls.Add(this.lstSubject);
			this.groupPanel2.Location = new System.Drawing.Point(2, 68);
			this.groupPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.groupPanel2.Name = "groupPanel2";
			this.groupPanel2.Size = new System.Drawing.Size(488, 206);
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
			this.groupPanel2.Text = "步驟二：選取考試科目";
			// 
			// groupPanel3
			// 
			this.groupPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupPanel3.BackColor = System.Drawing.Color.Transparent;
			this.groupPanel3.CanvasColor = System.Drawing.SystemColors.Control;
			this.groupPanel3.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
			this.groupPanel3.Controls.Add(this.cmbExamList);
			this.groupPanel3.Location = new System.Drawing.Point(0, 0);
			this.groupPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.groupPanel3.Name = "groupPanel3";
			this.groupPanel3.Size = new System.Drawing.Size(491, 62);
			// 
			// 
			// 
			this.groupPanel3.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.groupPanel3.Style.BackColorGradientAngle = 90;
			this.groupPanel3.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.groupPanel3.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel3.Style.BorderBottomWidth = 1;
			this.groupPanel3.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.groupPanel3.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel3.Style.BorderLeftWidth = 1;
			this.groupPanel3.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel3.Style.BorderRightWidth = 1;
			this.groupPanel3.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel3.Style.BorderTopWidth = 1;
			this.groupPanel3.Style.Class = "";
			this.groupPanel3.Style.CornerDiameter = 4;
			this.groupPanel3.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
			this.groupPanel3.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
			this.groupPanel3.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.groupPanel3.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
			// 
			// 
			// 
			this.groupPanel3.StyleMouseDown.Class = "";
			this.groupPanel3.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// 
			// 
			this.groupPanel3.StyleMouseOver.Class = "";
			this.groupPanel3.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.groupPanel3.TabIndex = 11;
			this.groupPanel3.Text = "步驟一：選取試別";
			// 
			// groupPanel4
			// 
			this.groupPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupPanel4.BackColor = System.Drawing.Color.Transparent;
			this.groupPanel4.CanvasColor = System.Drawing.SystemColors.Control;
			this.groupPanel4.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
			this.groupPanel4.Controls.Add(this.panelEx1);
			this.groupPanel4.Location = new System.Drawing.Point(6, 282);
			this.groupPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.groupPanel4.Name = "groupPanel4";
			this.groupPanel4.Size = new System.Drawing.Size(484, 98);
			// 
			// 
			// 
			this.groupPanel4.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.groupPanel4.Style.BackColorGradientAngle = 90;
			this.groupPanel4.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.groupPanel4.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel4.Style.BorderBottomWidth = 1;
			this.groupPanel4.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.groupPanel4.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel4.Style.BorderLeftWidth = 1;
			this.groupPanel4.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel4.Style.BorderRightWidth = 1;
			this.groupPanel4.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel4.Style.BorderTopWidth = 1;
			this.groupPanel4.Style.Class = "";
			this.groupPanel4.Style.CornerDiameter = 4;
			this.groupPanel4.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
			this.groupPanel4.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
			this.groupPanel4.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.groupPanel4.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
			// 
			// 
			// 
			this.groupPanel4.StyleMouseDown.Class = "";
			this.groupPanel4.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// 
			// 
			this.groupPanel4.StyleMouseOver.Class = "";
			this.groupPanel4.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.groupPanel4.TabIndex = 10;
			this.groupPanel4.Text = "步驟三：選取統計欄位";
			// 
			// panelEx1
			// 
			this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
			this.panelEx1.Controls.Add(this.checkBox6);
			this.panelEx1.Controls.Add(this.checkBox2);
			this.panelEx1.Controls.Add(this.checkBox1);
			this.panelEx1.Controls.Add(this.checkBox5);
			this.panelEx1.Controls.Add(this.checkBox3);
			this.panelEx1.Controls.Add(this.checkBox4);
			this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelEx1.Location = new System.Drawing.Point(0, 0);
			this.panelEx1.Name = "panelEx1";
			this.panelEx1.Size = new System.Drawing.Size(478, 71);
			this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
			this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
			this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.panelEx1.Style.GradientAngle = 90;
			this.panelEx1.TabIndex = 9;
			// 
			// checkBox6
			// 
			this.checkBox6.AutoSize = true;
			this.checkBox6.Location = new System.Drawing.Point(196, 40);
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.Size = new System.Drawing.Size(79, 21);
			this.checkBox6.TabIndex = 6;
			this.checkBox6.Text = "類組排名";
			this.checkBox6.UseVisualStyleBackColor = true;
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new System.Drawing.Point(7, 39);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(79, 21);
			this.checkBox2.TabIndex = 4;
			this.checkBox2.Text = "總分排名";
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(7, 13);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(53, 21);
			this.checkBox1.TabIndex = 5;
			this.checkBox1.Text = "總分";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// checkBox5
			// 
			this.checkBox5.AutoSize = true;
			this.checkBox5.Checked = true;
			this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox5.Location = new System.Drawing.Point(89, 40);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(105, 21);
			this.checkBox5.TabIndex = 3;
			this.checkBox5.Text = "加權平均排名";
			this.checkBox5.UseVisualStyleBackColor = true;
			// 
			// checkBox3
			// 
			this.checkBox3.AutoSize = true;
			this.checkBox3.Checked = true;
			this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox3.Location = new System.Drawing.Point(196, 13);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(79, 21);
			this.checkBox3.TabIndex = 1;
			this.checkBox3.Text = "加權總分";
			this.checkBox3.UseVisualStyleBackColor = true;
			// 
			// checkBox4
			// 
			this.checkBox4.AutoSize = true;
			this.checkBox4.Checked = true;
			this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox4.Location = new System.Drawing.Point(89, 13);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(79, 21);
			this.checkBox4.TabIndex = 2;
			this.checkBox4.Text = "加權平均";
			this.checkBox4.UseVisualStyleBackColor = true;
			// 
			// groupPanel5
			// 
			this.groupPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupPanel5.BackColor = System.Drawing.Color.Transparent;
			this.groupPanel5.CanvasColor = System.Drawing.SystemColors.Control;
			this.groupPanel5.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
			this.groupPanel5.Controls.Add(this.cmbPrefixList);
			this.groupPanel5.Location = new System.Drawing.Point(2, 546);
			this.groupPanel5.Margin = new System.Windows.Forms.Padding(0);
			this.groupPanel5.Name = "groupPanel5";
			this.groupPanel5.Size = new System.Drawing.Size(488, 66);
			// 
			// 
			// 
			this.groupPanel5.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
			this.groupPanel5.Style.BackColorGradientAngle = 90;
			this.groupPanel5.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
			this.groupPanel5.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel5.Style.BorderBottomWidth = 1;
			this.groupPanel5.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
			this.groupPanel5.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel5.Style.BorderLeftWidth = 1;
			this.groupPanel5.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel5.Style.BorderRightWidth = 1;
			this.groupPanel5.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
			this.groupPanel5.Style.BorderTopWidth = 1;
			this.groupPanel5.Style.Class = "";
			this.groupPanel5.Style.CornerDiameter = 4;
			this.groupPanel5.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
			this.groupPanel5.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center;
			this.groupPanel5.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
			this.groupPanel5.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near;
			// 
			// 
			// 
			this.groupPanel5.StyleMouseDown.Class = "";
			this.groupPanel5.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			// 
			// 
			// 
			this.groupPanel5.StyleMouseOver.Class = "";
			this.groupPanel5.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.groupPanel5.TabIndex = 12;
			this.groupPanel5.Text = "步驟五：指定類組排名群組";
			// 
			// cmbPrefixList
			// 
			this.cmbPrefixList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbPrefixList.DisplayMember = "Text";
			this.cmbPrefixList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.cmbPrefixList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPrefixList.FormattingEnabled = true;
			this.cmbPrefixList.ItemHeight = 19;
			this.cmbPrefixList.Location = new System.Drawing.Point(143, 7);
			this.cmbPrefixList.Name = "cmbPrefixList";
			this.cmbPrefixList.Size = new System.Drawing.Size(167, 25);
			this.cmbPrefixList.TabIndex = 5;
			// 
			// frmMain
			// 
			this.ClientSize = new System.Drawing.Size(491, 643);
			this.Controls.Add(this.groupPanel5);
			this.Controls.Add(this.groupPanel4);
			this.Controls.Add(this.groupPanel3);
			this.Controls.Add(this.btnPrint);
			this.Controls.Add(this.groupPanel1);
			this.Controls.Add(this.groupPanel2);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "frmMain";
			this.Text = "班級成績單";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.groupPanel1.ResumeLayout(false);
			this.groupPanel2.ResumeLayout(false);
			this.groupPanel3.ResumeLayout(false);
			this.groupPanel4.ResumeLayout(false);
			this.panelEx1.ResumeLayout(false);
			this.panelEx1.PerformLayout();
			this.groupPanel5.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private SmartSchool.Common.ListViewEX lstSubject;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbExamList;
        private DevComponents.DotNetBar.ButtonX btnPrint;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel1;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel2;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel3;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel4;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox6;
        private DevComponents.DotNetBar.Controls.GroupPanel groupPanel5;
        private SmartSchool.Common.ListViewEX lstStudentCategory;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbPrefixList;
    }
}
