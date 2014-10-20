using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace �Z�ŦҸզ��Z��
{
    /// <summary>
    /// �D�n���
    /// </summary>
    public partial class frmMain :  FISCA.Presentation.Controls.BaseForm
    {
        private ClassExamScoreHelper mHelper;

        public frmMain()
        {
            InitializeComponent();
        }

        public frmMain(ClassExamScoreHelper vHelper)
        {
            InitializeComponent();

            mHelper = vHelper;
        }

        /// <summary>
        /// ���J�էO�M��
        /// </summary>
        private void LoadExamList()
        {
            this.UseWaitCursor = true;
            cmbExamList.Items.Add("��ƤU����...");
            cmbExamList.SelectedIndex = 0;

            cmbExamList.SelectedItem = null;
            cmbExamList.Items.Clear();
            cmbExamList.Items.AddRange(mHelper.GetSelectClassesExamList().ToArray());

            cmbPrefixList.SelectedItem = null;
            cmbPrefixList.Items.Clear();

            mHelper.StudentPrefixList.ForEach
            (x =>
            {
                if (!string.IsNullOrEmpty(x))
                    cmbPrefixList.Items.Add(x);
            }
            );

            this.UseWaitCursor = false;
        }

        /// <summary>
        /// ���J�ǥ����O
        /// </summary>
        private void LoadStudentCategory()
        {
            foreach (string value in mHelper.StudentCategoryList)
                lstStudentCategory.Items.Add(value);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            
        }

        private void cmbExamList_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> Subjects = mHelper.GetSelectExamSubjectList(cmbExamList.Text);

            lstSubject.Clear();

            for (int i = 0; i < Subjects.Count; i++)
                lstSubject.Items.Add(Subjects[i]);
        }

        public List<string> CalculateField
        {
            get
            {
                List<string> List = new List<string>();
                foreach (CheckBox var in new CheckBox[] { checkBox1, checkBox2, checkBox3, checkBox4, checkBox5,checkBox6 })
                    if (var.Checked)
                        List.Add(var.Text);
                return List;
            }
        }

        public string ApplicationPath
        {
            get
            {
                return Application.StartupPath;
            }
        }

        /// <summary>
        /// ����ǥ����O
        /// </summary>
        public List<string> SelectedStudentCategories
        {
            get
            {
                List<string> StudentCategories = new List<string>();

                StudentCategories.Clear();

                foreach (ListViewItem lvi in lstStudentCategory.CheckedItems)
                    StudentCategories.Add(lvi.Text);

                return StudentCategories;
            }
        }

        /// <summary>
        /// ����Ҹլ��
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
        /// ����էO
        /// </summary>
        public string SelectedExam
        {
            get
            {
                return cmbExamList.Text;
            }
        }

        /// <summary>
        /// ������O�e�m
        /// </summary>
        public string SelectedPrefix
        {
            get
            {
                return cmbPrefixList.Text;
            }
        }

        /// <summary>
        /// ���J���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = "�C�L�Z�Ŧ��Z�� ��ƤU����...";
            btnPrint.Enabled = false;

            BackgroundWorker worker = new BackgroundWorker();

            worker.DoWork += (vsender, ve) => mHelper.InitialClassExamScore();

            worker.RunWorkerCompleted += (vsender, ve) =>
            {
                LoadExamList();
                LoadStudentCategory();

                this.Text = "�C�L�Z�Ŧ��Z��";
                btnPrint.Enabled = true;
            };

            worker.RunWorkerAsync();
        }
    }
}