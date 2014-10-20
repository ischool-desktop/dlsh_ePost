using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Aspose.Words;
using FISCA.Permission;
using SHSchool.Data;

namespace 班級考試成績單
{
    /// <summary>
    /// 班級考試成績單主要處理類別
    /// </summary>
    public class ClassExamScore
    {
        private ClassExamScoreHelper mHelper;
        private string mApplicationPath = "";

        /// <summary>
        /// 計算欄位
        /// </summary>
        private List<string> mCalculateField;
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
        /// 無參數建構式
        /// </summary>
        public ClassExamScore()
        {
            RibbonFeature Feature = new RibbonFeature("SHEvaluation.Report1000020", "班級考試成績單（含類組排名）");
            Catalog ClassCatalog = FISCA.Permission.RoleAclSource.Instance["班級"]["報表"];
            ClassCatalog.Add(Feature);

            K12.Presentation.NLDPanels.Class.RibbonBarItems["資料統計"]["報表"]["成績相關報表"]["班級考試成績單（含類組排名）"].Enable = FISCA.Permission.UserAcl.Current["SHEvaluation.Report1000020"].Executable;
            K12.Presentation.NLDPanels.Class.RibbonBarItems["資料統計"]["報表"]["成績相關報表"]["班級考試成績單（含類組排名）"].Click += (sender, e) =>
            {
                mHelper = new ClassExamScoreHelper();

                frmMain frmClassExamScore = new frmMain(mHelper);

                if (frmClassExamScore.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    mApplicationPath = frmClassExamScore.ApplicationPath;
                    mCalculateField = frmClassExamScore.CalculateField;
                    mStudentCategories = frmClassExamScore.SelectedStudentCategories;
                    mSelectedExam = frmClassExamScore.SelectedExam;
                    mSelectedSubjects = frmClassExamScore.SelectedSubjects;

                    if (!string.IsNullOrEmpty(frmClassExamScore.SelectedPrefix))
                        mSelectedPrefix = frmClassExamScore.SelectedPrefix;    

                    BackgroundWorker bkw = new BackgroundWorker();
                    bkw.WorkerReportsProgress = true;
                    bkw.DoWork += new DoWorkEventHandler(bkw_DoWork);
                    bkw.ProgressChanged += new ProgressChangedEventHandler(bkw_ProgressChanged);
                    bkw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkw_RunWorkerCompleted);
                    bkw.RunWorkerAsync();
                }
            };
        }

        /// <summary>
        /// 背景執行
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
                mHelper.CalculateTagRank(mSelectedSubjects, mSelectedExam, mSelectedPrefix);
                vStopwatch.Stop();
                bkw.ReportProgress(1, vStopwatch.Elapsed.TotalSeconds.ToString("F2"));
            }
            else
                bkw.ReportProgress(1);
            

            Document doc = new Document();

            doc.Sections.Clear();


            #region 針對每個班級產生報表
            foreach (SHClassRecord Class in mHelper.ClassList)
            {
                mHelper.CalculateClassScore(Class, mSelectedSubjects, mSelectedExam,mSelectedPrefix);

                Document each_page = new Document(mApplicationPath+"\\班級考試成績單.doc", LoadFormat.Doc, "");

                #region 執行合併列印
                string[] merge_keys = new string[] { "學校名稱", "班級名稱", "考試名稱", "考試成績" };

                object[] merge_values = new object[] { 
                    K12.Data.School.ChineseName ,//對應"學校名稱"欄
                    Class.Name, //對應"班級名稱"欄
                    mSelectedExam, //對應"考式名稱"欄
                    new object[] { mHelper.SubjectKeyList , mHelper.ScoreTable , mCalculateField , mStudentCategories} };//對應"考試成績"欄

                //註冊合併列印事件，當合併的欄位是考試成績時，必須用特別的方法做合併
                each_page.MailMerge.MergeField += new Aspose.Words.Reporting.MergeFieldEventHandler(MailMerge_MergeField);

                //執行合併動作
                each_page.MailMerge.Execute(merge_keys, merge_values);
                #endregion

                //合併至doc
                doc.Sections.Add(doc.ImportNode(each_page.Sections[0], true));
                bkw.ReportProgress((int)(++progress * 100.0d / mHelper.ClassList.Count));
            }
            #endregion

            e.Result = doc;
        }

        /// <summary>
        /// 合併列印儲存格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MailMerge_MergeField(object sender, Aspose.Words.Reporting.MergeFieldEventArgs e)
        {
            if (e.FieldName == "考試成績")
            {
                //針對"考試成績"這個合併欄位做特殊的合併(PS. 在合併欄位所在的表格中畫出整個班級的考試成績單)
                #region 針對"考試成績"這個合併欄未做特殊的合併(再合併欄位所屬的表格中畫出整個班級的考試成績單)
                e.Text = string.Empty;
                //整理列印科目+級別+學分數
                List<string> groups = (List<string>)((object[])e.FieldValue)[0];
                //全班學生成績資料
                Dictionary<SHStudentRecord, Dictionary<string, string>> classExamScoreTable = (Dictionary<SHStudentRecord, Dictionary<string, string>>)((object[])e.FieldValue)[1];
                //其它統計欄位
                List<string> otherList = (List<string>)((object[])e.FieldValue)[2];

                //其它統計欄位
                List<string> StudentCategories = (List<string>)((object[])e.FieldValue)[3];

                //每個科目的總分
                Dictionary<string, decimal> groupSum = new Dictionary<string, decimal>();
                //每個科目有分數的人數
                Dictionary<string, int> groupCount = new Dictionary<string, int>();

                DocumentBuilder builder = new DocumentBuilder(e.Document);
                Cell currentCell;
                builder.RowFormat.AllowBreakAcrossPages = true;
                builder.MoveToField(e.Field, false);
                #region 取得外框寬度並計算欄寬
                Cell SCell = (Cell)builder.CurrentParagraph.ParentNode;
                double Swidth = SCell.CellFormat.Width;
                double microUnit = Swidth / (groups.Count + otherList.Count + 3); //姓名*2、座號跟每科成績各一份
                #endregion
                Table table = builder.StartTable();

                builder.CellFormat.ClearFormatting();
                builder.CellFormat.Borders.LineWidth = 0.5;

                builder.RowFormat.HeightRule = HeightRule.Auto;
                builder.RowFormat.Height = builder.Font.Size * 1.2d;
                builder.RowFormat.Alignment = RowAlignment.Center;
                builder.CellFormat.VerticalAlignment = CellVerticalAlignment.Center;
                builder.CellFormat.LeftPadding = 3.0;
                builder.CellFormat.RightPadding = 3.0;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.ParagraphFormat.LineSpacingRule = LineSpacingRule.Exactly;
                builder.ParagraphFormat.LineSpacing = 10;
                #region 填表頭
                builder.InsertCell().CellFormat.Width = microUnit;
                builder.CellFormat.Borders.Right.LineWidth = 0.25;
                builder.Write("座號");

                builder.InsertCell().CellFormat.Width = microUnit * 2;
                builder.CellFormat.Borders.Right.LineWidth = 0.25;
                builder.Write("姓名");

                foreach (string key in groups)
                {
                    #region 每科給一欄
                    builder.InsertCell().CellFormat.Width = microUnit;
                    builder.CellFormat.VerticalMerge = CellMerge.None;
                    builder.CellFormat.Borders.Right.LineWidth = 0.25;
                    string[] list = key.Split(new string[] { "^_^" }, StringSplitOptions.None);
                    builder.Write(list[0] + Common.GetNumberString(list[1]));
                    #endregion
                }
                foreach (string key in otherList)
                {
                    #region 統計欄位
                    builder.InsertCell().CellFormat.Width = microUnit;
                    builder.CellFormat.VerticalMerge = CellMerge.First;
                    if (otherList[otherList.Count - 1] != key)
                        builder.CellFormat.Borders.Right.LineWidth = 0.25;
                    builder.Write(key);
                    #endregion
                }
                builder.EndRow();
                #endregion

                #region 填學分數
                currentCell = builder.InsertCell();
                currentCell.CellFormat.Width = microUnit * 3;
                builder.CellFormat.Borders.Right.LineWidth = 0.25;
                currentCell.FirstParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                builder.Write("學分數");
                foreach (string key in groups)
                {
                    #region 每科給一欄
                    currentCell = builder.InsertCell();
                    currentCell.CellFormat.Width = microUnit;
                    builder.CellFormat.VerticalMerge = CellMerge.None;
                    currentCell.FirstParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    if (otherList.Count > 0 || groups[groups.Count - 1] != key)
                        builder.CellFormat.Borders.Right.LineWidth = 0.25;
                    string[] list = key.Split(new string[] { "^_^" }, StringSplitOptions.None);
                    builder.Write(list[2]);
                    #endregion
                }
                foreach (string key in otherList)
                {
                    #region 統計欄位

                    currentCell = builder.InsertCell();
                    currentCell.CellFormat.Width = microUnit;
                    builder.CellFormat.VerticalMerge = CellMerge.Previous;
                    currentCell.FirstParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    if (otherList[otherList.Count - 1] != key)
                        builder.CellFormat.Borders.Right.LineWidth = 0.25;
                    #endregion
                }
                builder.EndRow();
                #endregion
                //畫雙線
                foreach (Cell cell in table.LastRow.Cells)
                    cell.CellFormat.Borders.Bottom.LineStyle = LineStyle.Double;

                builder.CellFormat.VerticalMerge = CellMerge.None;
                foreach (SHStudentRecord studentRec in classExamScoreTable.Keys)
                {
                    #region 填學生資料
                    //座號
                    builder.InsertCell().CellFormat.Width = microUnit;
                    builder.CellFormat.Borders.Right.LineWidth = 0.25;
                    builder.Write(K12.Data.Int.GetString(studentRec.SeatNo));

                    //姓名
                    builder.InsertCell().CellFormat.Width = microUnit * 2;
                    builder.CellFormat.Borders.Right.LineWidth = 0.25;
                    builder.Write(studentRec.Name);
                    foreach (string key in groups)
                    {
                        #region 各科成績
                        builder.InsertCell().CellFormat.Width = microUnit;
                        if (otherList.Count > 0 || groups[groups.Count - 1] != key)
                            builder.CellFormat.Borders.Right.LineWidth = 0.25;
                        if (classExamScoreTable[studentRec].ContainsKey(key))
                        {
                            builder.Write(classExamScoreTable[studentRec][key]);
                            #region 算各科平均
                            decimal score;
                            if (decimal.TryParse(classExamScoreTable[studentRec][key], out score)  && !mHelper.IsInStudentCategories(studentRec,StudentCategories))
                            {
                                if (!groupSum.ContainsKey(key))
                                    groupSum.Add(key, 0m);
                                if (!groupCount.ContainsKey(key))
                                    groupCount.Add(key, 0);
                                groupCount[key]++;
                                groupSum[key] += score;
                            }
                            #endregion
                        }
                        else
                            builder.Write("--");
                        #endregion
                    }
                    foreach (string key in otherList)
                    {
                        #region 統計欄位
                        builder.InsertCell().CellFormat.Width = microUnit;
                        if (otherList[otherList.Count - 1] != key)
                            builder.CellFormat.Borders.Right.LineWidth = 0.25;
                        if (classExamScoreTable[studentRec].ContainsKey(key))
                        {
                            builder.Write(classExamScoreTable[studentRec][key]);
                        }
                        else
                            builder.Write("--");
                        #endregion
                    }
                    builder.EndRow();
                    #endregion
                }
                //畫雙線
                foreach (Cell cell in table.LastRow.Cells)
                    cell.CellFormat.Borders.Bottom.LineStyle = LineStyle.Double;

                #region 填平均
                currentCell = builder.InsertCell();
                currentCell.CellFormat.Width = microUnit * 3;
                currentCell.FirstParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Right;
                builder.CellFormat.Borders.Right.LineWidth = 0.25;
                builder.Write("平均");
                foreach (string key in groups)
                {
                    #region 各科平均
                    currentCell = builder.InsertCell();
                    currentCell.CellFormat.Width = microUnit;
                    currentCell.FirstParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    if (otherList.Count > 0 || groups[groups.Count - 1] != key)
                        builder.CellFormat.Borders.Right.LineWidth = 0.25;
                    if (groupSum.ContainsKey(key))
                    {
                        builder.Write((groupSum[key] / (groupCount[key] == 0 ? 1 : groupCount[key])).ToString(".0"));
                    }
                    else
                        builder.Write("--");
                    #endregion
                }
                if (otherList.Count > 0)
                {
                    builder.InsertCell().CellFormat.Width = microUnit * otherList.Count;
                    builder.CellFormat.Borders.Right.LineWidth = 0.25;
                }
                builder.EndRow();
                #endregion
                #region 去除表格四邊的線
                foreach (Cell cell in table.FirstRow.Cells)
                    cell.CellFormat.Borders.Top.LineStyle = LineStyle.None;

                //foreach ( Cell cell in table.LastRow.Cells )
                //    cell.CellFormat.Borders.Bottom.LineStyle = LineStyle.None;

                foreach (Row row in table.Rows)
                {
                    row.FirstCell.CellFormat.Borders.Left.LineStyle = LineStyle.None;
                    row.LastCell.CellFormat.Borders.Right.LineStyle = LineStyle.None;
                }
                #endregion
                #endregion
            }
        }

        /// <summary>
        /// 進度回報
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bkw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string CalculateTime = "" + e.UserState;

            if (!string.IsNullOrEmpty(CalculateTime))
                FISCA.Presentation.MotherForm.SetStatusBarMessage("計算類組排名時間共"+CalculateTime+"秒",e.ProgressPercentage);
            else
                FISCA.Presentation.MotherForm.SetStatusBarMessage("班級考試成績單產生中...", e.ProgressPercentage);
        }

        /// <summary>
        /// 完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bkw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("班級考試成績單產生完成");
            Document doc = (Document)e.Result;
            #region 儲存並開啟檔案

            string reportName = "班級考試成績單";
            string path = Path.Combine(Application.StartupPath, "Reports");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, reportName + ".doc");

            if (File.Exists(path))
            {
                int i = 1;
                while (true)
                {
                    string newPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + (i++) + Path.GetExtension(path);
                    if (!File.Exists(newPath))
                    {
                        path = newPath;
                        break;
                    }
                }
            }

            try
            {
                doc.Save(path, SaveFormat.Doc);
                System.Diagnostics.Process.Start(path);
            }
            catch
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.Title = "另存新檔";
                sd.FileName = reportName + ".doc";
                sd.Filter = "Word檔案 (*.doc)|*.doc|所有檔案 (*.*)|*.*";
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        doc.Save(sd.FileName, SaveFormat.AsposePdf);
                    }
                    catch
                    {
                        
                        MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            #endregion
        } 
    }
}