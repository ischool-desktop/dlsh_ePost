using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Aspose.Cells;
using Campus.Report;
using FISCA.Permission;
using ReportHelper;
using SHSchool.Data;
using K12.Data.Configuration;

namespace 班級考試成績單
{
    /// <summary>
    /// 學生考試成績單
    /// </summary>
    public class StudentExamScore
    {
        private StudentExamScoreHelper mHelper;
        private string mApplicationPath = string.Empty;

        /// <summary>
        /// 不列入各科平均計算之類別
        /// </summary>
        private List<string> mStudentCategories;
        /// <summary>
        /// 選取試別
        /// </summary>
        private string mSelectedExam;
        /// <summary>
        /// 選取科目
        /// </summary>
        private List<string> mSelectedSubjects;
        /// <summary>
        /// 選取類別前置
        /// </summary>
        private string mSelectedPrefix;

        /// <summary>
        /// 建構式
        /// </summary>
        public StudentExamScore()
        {
            K12.Presentation.NLDPanels.Class.RibbonBarItems["資料統計"]["報表"]["成績相關報表"]["個人考試成績單（含類組排名）"].Click += (sender, e) => Execute(true);
            K12.Presentation.NLDPanels.Class.RibbonBarItems["資料統計"]["報表"]["成績相關報表"]["個人考試成績單（含類組排名）"].Enable = FISCA.Permission.UserAcl.Current["SHEvaluation.Report1000010"].Executable;
            K12.Presentation.NLDPanels.Student.RibbonBarItems["資料統計"]["報表"]["成績相關報表"]["個人考試成績單（含類組排名）"].Click += (sender, e) => Execute(false);
            K12.Presentation.NLDPanels.Student.RibbonBarItems["資料統計"]["報表"]["成績相關報表"]["個人考試成績單（含類組排名）"].Enable = FISCA.Permission.UserAcl.Current["SHEvaluation.Report1000011"].Executable;

            Catalog ClassCatalog = FISCA.Permission.RoleAclSource.Instance["班級"]["報表"];
            Catalog StudentCatalog = FISCA.Permission.RoleAclSource.Instance["學生"]["報表"];

            ClassCatalog.Add(new RibbonFeature("SHEvaluation.Report1000010", "個人考試成績單（含類組排名）"));
            StudentCatalog.Add(new RibbonFeature("SHEvaluation.Report1000011", "個人考試成績單（含類組排名）"));
        }

        /// <summary>
        /// 實際執行
        /// </summary>
        /// <param name="IsSelectClass"></param>
        private void Execute(bool IsSelectClass)
        {
            mHelper = new StudentExamScoreHelper(IsSelectClass);

            frmHome frmStudentExamScore = new frmHome(mHelper);

            if (frmStudentExamScore.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mApplicationPath = frmStudentExamScore.ApplicationPath;
                mStudentCategories = frmStudentExamScore.SelectedStudentCategories;
                mSelectedExam = frmStudentExamScore.SelectedExam;
                mSelectedSubjects = frmStudentExamScore.SelectedSubjects;

                if (!string.IsNullOrEmpty(frmStudentExamScore.SelectedPrefix))
                    mSelectedPrefix = frmStudentExamScore.SelectedPrefix;

                BackgroundWorker bkw = new BackgroundWorker();
                bkw.WorkerReportsProgress = true;
                bkw.DoWork += new DoWorkEventHandler(bkw_DoWork);
                bkw.ProgressChanged += new ProgressChangedEventHandler(bkw_ProgressChanged);
                bkw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkw_RunWorkerCompleted);
                bkw.RunWorkerAsync();
            }
        }

        /// <summary>
        /// 產生報表資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bkw_DoWork(object sender, DoWorkEventArgs e)
        {         
            //宣告變數
            double progress = 0; //處理進度計數器

            BackgroundWorker bkw = ((BackgroundWorker)sender);

            if (!string.IsNullOrEmpty(mSelectedPrefix))
            {
                Stopwatch vStopwatch = new Stopwatch();
                vStopwatch.Start();
                mHelper.CalculateTagRank(mSelectedSubjects, mSelectedExam, mSelectedPrefix,bkw.ReportProgress);
                vStopwatch.Stop();
                //bkw.ReportProgress(1, vStopwatch.Elapsed.TotalSeconds.ToString("F2"));
            }
            else
                bkw.ReportProgress(1);

            #region 針對每個班級學生計算成績及排名
            foreach (SHClassRecord Class in mHelper.ClassList)
            {
                mHelper.CalculateClassScore(Class, mSelectedSubjects, mSelectedExam, mSelectedPrefix);
                bkw.ReportProgress((int)(++progress * 100.0d / mHelper.ClassList.Count), "班級考試成績單產生中...");
            }
            #endregion

            e.Result = mHelper.GetResult(mSelectedSubjects,mSelectedExam);
        }

        /// <summary>
        /// 進度改變
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bkw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage(""+e.UserState, e.ProgressPercentage);
        }

        /// <summary>
        /// 執行完成，產生報表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bkw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Tuple<int,Dictionary<string, List<DataSet>>> Result = e.Result as Tuple<int,Dictionary<string, List<DataSet>>>;

            FISCA.Presentation.MotherForm.SetStatusBarMessage("個人評量成績單產生完成");

            try
            {
                if (Result.Item1>20)
                  if (MessageBox.Show("提醒您學生的科目數最大為" + Result.Item1 + "個，預設樣版科目數為20個，建議您用自訂樣版調整後列印，是否繼續列印？", "個人考試成績單", MessageBoxButtons.YesNo) == DialogResult.No)
                      return;

                byte[] Byte = Resource.高中評量成績單; //將成績單先為預設

                #region 判斷是否用自訂範本，以及自訂範本是否有內容才套用
                ConfigData config = K12.Data.School.Configuration["高中個人評量成績單"];
                int _useTemplateNumber = 0;
                int.TryParse(config["TemplateNumber"], out _useTemplateNumber);
                string customize = config["CustomizeTemplate"];
                if (!string.IsNullOrEmpty(customize) && _useTemplateNumber==1)
                    Byte = Convert.FromBase64String(customize);
                #endregion

                MemoryStream Stream = new MemoryStream(Byte);

                Workbook wb = Report.Produce(Result.Item2,Stream);

                foreach(Worksheet sheet in wb.Worksheets)
                    sheet.PageSetup.CenterHorizontally = true;

                string mSaveFilePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Reports\\個人考試成績單.xls";

                ReportSaver.SaveWorkbook(wb, mSaveFilePath);

                mHelper.ExportClassInfo(mSelectedSubjects);
            }
            catch (Exception ve)
            {
                MessageBox.Show(ve.Message);
                SmartSchool.ErrorReporting.ReportingService.ReportException(ve);
            }
        }
    }
}