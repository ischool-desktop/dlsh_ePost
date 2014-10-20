using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using K12.Data.Configuration;

namespace 班級考試成績單
{
    /// <summary>
    /// 主要表單
    /// </summary>
    public partial class frmHome :  FISCA.Presentation.Controls.BaseForm
    {
        private StudentExamScoreHelper mHelper;
        private string mVersionMessage = string.Empty;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="vHelper"></param>
        public frmHome(StudentExamScoreHelper vHelper)
        {
            InitializeComponent();

            mHelper = vHelper;

            mVersionMessage = "『";

            foreach (Assembly Assembly in AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(x => x.GetName().Name.Equals("ExamReport")))
            
            mVersionMessage += Assembly.GetName().Version;

            mVersionMessage += "』";

            this.TitleText += mVersionMessage;

            int SchoolYear = K12.Data.Int.Parse(K12.Data.School.DefaultSchoolYear);
            int Semester = K12.Data.Int.Parse(K12.Data.School.DefaultSemester);

            for (int i = SchoolYear - 3; i <= SchoolYear; i++)
                cmbSchoolYear.Items.Add("" + i);
            cmbSchoolYear.SelectedIndex = 3;

            cmbSemester.Items.Add("1");
            cmbSemester.Items.Add("2");
            cmbSemester.SelectedIndex = Semester - 1;
        }

        /// <summary>
        /// 載入表單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            cmbSchoolYear.SelectedIndexChanged += (vsender, ve) => Download();
            cmbSemester.SelectedIndexChanged += (vsender, ve) => Download();

            Download();
        }

        /// <summary>
        /// 下載資料
        /// </summary>
        private void Download()
        {
            this.TitleText = "個人考試成績單" + mVersionMessage + "資料下載中...";
            btnPrint.Enabled = false;

            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += (vsender, ve) =>
            {
                List<string> vSchoolYearSemester = ve.Argument as List<string>;

                mHelper.Initial(vSchoolYearSemester[0], vSchoolYearSemester[1]);
            };

            worker.RunWorkerCompleted += (vsender, ve) =>
            {
                LoadExamList();

                this.TitleText = "個人考試成績單" + mVersionMessage;
                btnPrint.Enabled = true;
            };

            string Message = ValidateSchoolYearSemester();

            if (!string.IsNullOrWhiteSpace(Message))
            {
                MessageBox.Show(Message);
                return;
            }

            List<string> SchoolYearSemester = new List<string>() { SchoolYear, Semester };

            worker.RunWorkerAsync(SchoolYearSemester); 
        }

        /// <summary>
        /// 檢查學年度學期
        /// </summary>
        /// <returns></returns>
        private string ValidateSchoolYearSemester()
        {
            StringBuilder strBuilder = new StringBuilder();

            int a;

            if (!int.TryParse(SchoolYear, out a))
                strBuilder.AppendLine("學年度必須為數字");

            if (!(Semester.Equals("1") || Semester.Equals("2")))
                strBuilder.AppendLine("學期必須為1或2");

            return strBuilder.ToString();
        }


        /// <summary>
        /// 載入試別清單
        /// </summary>
        private void LoadExamList()
        {
            try
            {
                this.UseWaitCursor = true;

                cmbExamList.SelectedIndexChanged -= cmbExamList_SelectedIndexChanged;
                cmbExamList.Items.Add("資料下載中...");
                cmbExamList.SelectedIndex = 0;
                cmbExamList.SelectedItem = null;
                cmbExamList.Items.Clear();
                cmbExamList.Items.AddRange(mHelper.GetSelectClassesExamList().ToArray());
                cmbExamList.SelectedIndexChanged += cmbExamList_SelectedIndexChanged;
                cmbPrefixList.SelectedItem = null;
                cmbPrefixList.Items.Clear();
                lstSubject.Clear();

                mHelper.StudentPrefixList.ForEach
                (x =>
                    {
                        if (!string.IsNullOrEmpty(x))
                            cmbPrefixList.Items.Add(x);
                    }
                );
            }
            catch (Exception e)
            {

            }
            finally
            {
                this.UseWaitCursor = false;
            }
        }

        private void cmbExamList_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> Subjects = mHelper.GetSelectExamSubjectList(cmbExamList.Text);

            lstSubject.Clear();

            for (int i = 0; i < Subjects.Count; i++)
                lstSubject.Items.Add(Subjects[i]);
        }

        /// <summary>
        /// 應用程式路徑
        /// </summary>
        public string ApplicationPath
        {
            get
            {
                return Application.StartupPath;
            }
        }

        /// <summary>
        /// 選取學生類別
        /// </summary>
        public List<string> SelectedStudentCategories
        {
            get
            {
                //List<string> StudentCategories = new List<string>();

                //StudentCategories.Clear();

                //foreach (ListViewItem lvi in lstStudentCategory.CheckedItems)
                //    StudentCategories.Add(lvi.Text);

                //return StudentCategories;

                return new List<string>();
            }
        }

        /// <summary>
        /// 選取考試科目
        /// </summary>
        public List<string> SelectedSubjects
        {
            get
            {
                List<string> Subjects = new List<string>();

                Subjects.Clear();

                foreach (ListViewItem lvi in lstSubject.CheckedItems)
                    Subjects.Add(lvi.Text);

                return Subjects;
            }
        }

        /// <summary>
        /// 選取試別
        /// </summary>
        public string SelectedExam
        {
            get
            {
                return cmbExamList.Text;
            }
        }

        /// <summary>
        /// 選取類別前置
        /// </summary>
        public string SelectedPrefix
        {
            get
            {
                return cmbPrefixList.Text;
            }
        }

        /// <summary>
        /// 選取學年度
        /// </summary>
        public string SchoolYear
        {
            get { return "" + cmbSchoolYear.SelectedItem; }
        }

        /// <summary>
        /// 選取學期
        /// </summary>
        public string Semester 
        {
            get { return "" + cmbSemester.SelectedItem; }
        }

        /// <summary>
        /// 設定樣版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTemplate_Click(object sender, EventArgs e)
        {
            #region 讀取 Preference

            ConfigData config = K12.Data.School.Configuration["高中個人評量成績單"];

            if (config != null)
            {
                int _useTemplateNumber = 0;
                int.TryParse(config["TemplateNumber"], out _useTemplateNumber);

                string customize = config["CustomizeTemplate"];
                byte[] _buffer = Resource.高中評量成績單;

                if (!string.IsNullOrEmpty(customize))
                     _buffer = Convert.FromBase64String(customize);

                int _custodian = 0;
                int.TryParse(config["Custodian"], out _custodian);

                int _address = 0;
                int.TryParse(config["Address"], out _address);

                frmTemplateConfig frmConfig = new frmTemplateConfig(_buffer, _useTemplateNumber, _custodian, _address);

                frmConfig.ShowDialog();
            }
            #endregion
        }

        /// <summary>
        /// 列印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}