using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Campus.Rating;
using FISCA.Data;
using K12.Data.Configuration;
using ReportHelper;
using SHSchool.Data;

namespace 班級考試成績單
{
    /// <summary>
    /// 初始化資料時，若採用多執行緒會有問題，故先將執行緒數量改為1。
    /// </summary>
    public class StudentExamScoreHelper
    {
        internal Dictionary<string, Dictionary<string, string>> mStudentsScore;
        internal Dictionary<string, TagStudents> TagStudents;
        private List<string> SubjectKeys;
        private string mCurrentExamID;
        private bool mIsSelectClass;
        private string mSchoolYear;
        private string mSemester;

        /// <summary>
        /// 是否選取班級，不是的話就選取學生
        /// </summary>
        /// <param name="IsSelectClass"></param>
        public StudentExamScoreHelper(bool IsSelectClass)
        {
            this.mIsSelectClass = IsSelectClass;
            this.mStudentsScore = new Dictionary<string, Dictionary<string, string>>();
            this.TagStudents = new Dictionary<string, TagStudents>();
        }

        /// <summary>
        /// 取得班級考試成績單所需基本資料 
        /// Ⅰ選取班級學生名單 
        /// Ⅱ學生修課清單 
        /// Ⅲ學生修課考試資訊
        /// </summary>
        public void Initial(string SchoolYear,string Semester)
        {
            mSchoolYear = SchoolYear;
            mSemester = Semester;

            try
            {
                #region 取得選取學生及班級
                if (mIsSelectClass)
                {
                    //取得選取班級
                    ClassList = SHClass
                        .SelectByIDs(K12.Presentation.NLDPanels.Class.SelectedSource);

                    //取得選取班級的學生，並且為一般生
                    ClassStudentList = SHStudent
                        .SelectByClassIDs(K12.Presentation.NLDPanels.Class.SelectedSource)
                        .FindAll(x => x.Status == K12.Data.StudentRecord.StudentStatus.一般);

                    //若選取為班級，則選取學生與班級學生為一樣
                    SelectedStudentList = ClassStudentList;
                }
                else
                {
                    //取得選取學生，狀態為一般生
                    SelectedStudentList = SHStudent
                        .SelectByIDs(K12.Presentation.NLDPanels.Student.SelectedSource)
                        .FindAll(x => x.Status == K12.Data.StudentRecord.StudentStatus.一般);

                    //取得選取學生的班級
                    ClassList = SHClass.SelectByIDs(SelectedStudentList.Select(x => x.RefClassID).Distinct());

                    //取得班級學生
                    ClassStudentList = SHStudent
                        .SelectByClassIDs(ClassList.Select(x => x.ID))
                        .FindAll(x=>x.Status == K12.Data.StudentRecord.StudentStatus.一般).ToList();
                }
                #endregion

                #region 取得學生基本資料
                //取得父母及監護人
                ParentList = SHParent.SelectByStudentIDs(ClassStudentList.Select(x => x.ID));

                //取得地址
                AddressList = SHAddress.SelectByStudentIDs(ClassStudentList.Select(x => x.ID));

                //取得班級班導師
                TeacherList = SHTeacher.SelectByIDs(ClassList.Select(x => x.RefTeacherID).Distinct());

                //取得學生標籤
                StudentTagList = SHStudentTag
                    .SelectByStudentIDs(ClassStudentList.Select(x => x.ID));

                //取得學生標籤『全名』列表
                StudentCategoryList = StudentTagList
                    .Select(x => x.FullName)
                    .Distinct()
                    .ToList();

                //取得學生標籤『前置詞』列表
                StudentPrefixList = StudentTagList
                    .Select(x => x.Prefix)
                    .Distinct()
                    .ToList();
                #endregion

                #region 取得試別及評量設定
                //取得所有試別
                ExamList = SHExam.SelectAll();

                //取得所有『評量設定』包含『試別』
                AEIncludeList = SHAEInclude.SelectAll();
                #endregion

                #region 分批取得學生修課
                FunctionSpliter<string, SHSCAttendRecord> SCAttendSpliter = new FunctionSpliter<string, SHSCAttendRecord>(1000, 1);

                SCAttendSpliter.Function = SelectSCAttend;

                SCAttendList = SCAttendSpliter.Execute(ClassStudentList.Select(x => x.ID).ToList());
                #endregion

                #region 分批取得課程清單
                FunctionSpliter<string, SHCourseRecord> CourseSpliter = new FunctionSpliter<string, SHCourseRecord>(500, 1);

                CourseSpliter.Function = SHCourse.SelectByIDs;

                List<string> CourseIDs = SCAttendList.Select(x => x.RefCourseID).Distinct().ToList();

                CourseList = CourseSpliter.Execute(CourseIDs);
                #endregion
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 依目前年度及學期取得學生修課
        /// </summary>
        /// <param name="StudentIDs"></param>
        /// <returns></returns>
        private List<SHSCAttendRecord> SelectSCAttend(List<string> StudentIDs)
        {
            return SHSCAttend.Select(StudentIDs, null, null, mSchoolYear , mSemester);
        }

        /// <summary>
        /// 依學生修課取得評量成績
        /// </summary>
        /// <param name="SCAttendIDs"></param>
        /// <returns></returns>
        private List<SHSCETakeRecord> SelectSCETake(List<string> SCAttendIDs)
        {
            return SHSCETake.Select(null, null, new List<string>() { mCurrentExamID }, null, SCAttendIDs);
        }

        /// <summary>
        /// 學生類別排名
        /// </summary>
        public Dictionary<string, string> StudentTagRanks { get; private set; }

        /// <summary>
        /// 地址列表
        /// </summary>
        public List<SHAddressRecord> AddressList { get; private set; }

        /// <summary>
        /// 家長及監護人列表
        /// </summary>
        public List<SHParentRecord> ParentList { get; private set; }

        /// <summary>
        /// 評量設定列表
        /// </summary>
        public List<SHAEIncludeRecord> AEIncludeList { get; private set; }

        /// <summary>
        /// 學生類別列表
        /// </summary>
        public List<string> StudentCategoryList { get; private set; }

        /// <summary>
        /// 學生前置前列表
        /// </summary>
        public List<string> StudentPrefixList { get; private set; }

        /// <summary>
        /// 學生標籤列表
        /// </summary>
        public List<SHStudentTagRecord> StudentTagList { get; private set; }

        /// <summary>
        /// 選取班級列表
        /// </summary>
        public List<SHClassRecord> ClassList { get; private set; }

        /// <summary>
        /// 選取班級學生列表
        /// </summary>
        public List<SHStudentRecord> ClassStudentList { get; private set; }

        /// <summary>
        /// 實際選取學生
        /// </summary>
        public List<SHStudentRecord> SelectedStudentList { get; private set; }

        /// <summary>
        /// 班級教師列表
        /// </summary>
        public List<SHTeacherRecord> TeacherList { get; private set; }

        /// <summary>
        /// 取得選取班級考清單，例如期中考、期末考…等。
        /// </summary>
        /// <returns></returns>
        public List<SHExamRecord> ExamList { get; private set; }

        /// <summary>
        /// 學生修課清單
        /// </summary>
        public List<SHSCAttendRecord> SCAttendList { get; private set; }

        /// <summary>
        /// 課程清單
        /// </summary>
        public List<SHCourseRecord> CourseList { get; private set; }

        /// <summary>
        /// 取得選取試別考試科目資訊，例如期中考科目為國文、英文、數學…等。 
        /// </summary>
        /// <param name="vExamName">試別名稱</param>
        /// <returns></returns>
        public List<string> GetSelectExamSubjectList(string vExamName)
        {
            List<string> Subjects = new List<string>();

            foreach (SHCourseRecord Course in CourseList)
            {
                List<string> CourseExamList = AEIncludeList
                    .FindAll(x => x.RefAssessmentSetupID.Equals(Course.RefAssessmentSetupID))
                    .Select(x => x.ExamName)
                    .ToList();

                if (CourseExamList.Contains(vExamName) && !Subjects.Contains(Course.Subject))
                    Subjects.Add(Course.Subject);
            }

            Subjects.Sort(new SubjectComparer() { });

            return Subjects;
        }


        /// <summary>
        /// 學生是否包含指定類別
        /// </summary>
        /// <param name="Student"></param>
        /// <param name="StudentCategories"></param>
        /// <returns></returns>
        public bool IsInStudentCategories(SHStudentRecord Student, List<string> StudentCategories)
        {
            foreach (SHStudentTagRecord StudentTag in StudentTagList.FindAll(x => x.RefStudentID.Equals(Student.ID)))
                if (StudentCategories.Contains(StudentTag.FullName))
                    return true;
            return false;
        }

        /// <summary>
        /// 取得選取班級考試資訊，例如期中考、期末考…等。 
        /// </summary>
        /// <returns></returns>
        public List<string> GetSelectClassesExamList()
        {
            //取得評量設定系統編號
            List<string> AssessmentSetupIDs = CourseList
                .Select(x => x.RefAssessmentSetupID)
                .Distinct()
                .ToList();

            //取得試別清單
            List<string> Exams = AEIncludeList
                .FindAll(x => AssessmentSetupIDs.Contains(x.RefAssessmentSetupID))
                .Select(x => x.ExamName).ToList();

            Exams = Exams
                .Distinct()
                .ToList();
            Exams.Sort();

            return Exams;
        }

        /// <summary>
        /// 每位學生資料列表
        /// </summary>
        public Dictionary<string,Dictionary<string,string>> StudentsScore
        {
            get
            {
                return mStudentsScore;
            }
        }

        /// <summary>
        /// 科目列表
        /// </summary>
        public List<string> SubjectKeyList
        {
            get
            {
                return SubjectKeys;
            }
        }

        /// <summary>
        /// 計算類別排名
        /// </summary>
        /// <param name="vSubjects"></param>
        /// <param name="vExam"></param>
        public void CalculateTagRank(List<string> vSubjects, string vExam, string vCategory,Action<int,object> ReportProgress)
        {
            int RankProgressCount = 0;
            int RandkTotalCount =0;

            SubjectKeys = new List<string>();

            List<string> StudentIDs = new List<string>();
            Dictionary<string, string> TotalRankStudents = new Dictionary<string, string>();        //所有排名學生
            TagStudents = new Dictionary<string, TagStudents>();                                    //標籤下的學生

            #region 設定目前試別，用來取得評量成績用（少撈點資料）
            SHExamRecord ExamRecord = ExamList.Find(x => x.Name.Equals(vExam));

            if (ExamRecord != null)
                mCurrentExamID = ExamRecord.ID;
            #endregion

            #region 根據群組取得學生標籤清單
            QueryHelper Helper = new QueryHelper();
            DataTable Table = Helper.Select("select tag_student.ref_student_id,class.grade_year,tag.prefix,tag.name from tag_student inner join tag on tag_student.ref_tag_id=tag.id inner join student on student.id=tag_student.ref_student_id left outer join class on class.id = student.ref_class_id where student.status=1 and prefix='" + vCategory + "'");

            List<string> SelectedStudentTags = new List<string>();

            List<string> SelectedStudentIDs = ClassStudentList
                .Select(x => x.ID)
                .ToList();
            #endregion

            #region 找出選取學生的完整標籤名稱有哪些，並且標籤是在指定的群組下
            foreach (DataRow Row in Table.Rows)
            {
                string RefStudentID = "" + Row["ref_student_id"];   //學生系統編號
                string PrefixName = "" + Row["prefix"];             //標籤群組
                string Name = "" + Row["name"];                     //標籤名稱
                string GradeYear = "" + Row["grade_year"];          //年級
                string TagFullName = PrefixName + Name + GradeYear; //標籤完整名稱

                if (SelectedStudentIDs.Contains(RefStudentID))
                    if (!SelectedStudentTags.Contains(TagFullName))
                        SelectedStudentTags.Add(TagFullName);
            }
            #endregion

            #region 依學生標籤分類
            foreach (DataRow Row in Table.Rows)
            {
                string RefStudentID = "" + Row["ref_student_id"];   //學生系統編號
                string PrefixName = "" + Row["prefix"];             //標籤群組
                string Name = "" + Row["name"];                     //標籤名稱
                string GradeYear = "" + Row["grade_year"];          //年級
                string TagFullName = PrefixName + Name + GradeYear; //標籤完整名稱                

                #region 只將選取學生標籤進行分類
                if (SelectedStudentTags.Contains(TagFullName))
                {
                    if (!TagStudents.ContainsKey(TagFullName))
                        TagStudents.Add(TagFullName, new TagStudents());

                    if (!TagStudents[TagFullName].StudentIDs.Contains(RefStudentID))
                    {
                       TagStudents[TagFullName].StudentIDs.Add(RefStudentID);
                        RandkTotalCount ++;
                    }
                    if (!StudentIDs.Contains(RefStudentID))
                        StudentIDs.Add(RefStudentID);
                }
                #endregion
            }


            #endregion

            #region 分批取得學生修課
            FunctionSpliter<string, SHSCAttendRecord> SCAttendSpliter = new FunctionSpliter<string, SHSCAttendRecord>(1000, 3);
            SCAttendSpliter.Function = SelectSCAttend;
            List<SHSCAttendRecord> SCAttends = SCAttendSpliter.Execute(StudentIDs);
            #endregion

            #region 分批取得課程
            FunctionSpliter<string, SHCourseRecord> CourseSpliter = new FunctionSpliter<string, SHCourseRecord>(500, 3);
            CourseSpliter.Function = SHCourse.SelectByIDs;
            List<SHCourseRecord> Courses = CourseSpliter.Execute(SCAttends.Select(x => x.RefCourseID).Distinct().ToList());
            #endregion

            #region 分批取得評量成績
            FunctionSpliter<string, SHSCETakeRecord> SCETakeSpliter = new FunctionSpliter<string, SHSCETakeRecord>(1000, 3);
            SCETakeSpliter.Function = SelectSCETake;
            List<SHSCETakeRecord> SCETakes = SCETakeSpliter.Execute(SCAttends.Select(x => x.ID).ToList());
            #endregion

            #region 根據分類學生做加權總分及排名
            foreach (TagStudents CurrentTagStudents in TagStudents.Values)
            {
                //建立學生範圍
                RatingScope<RankStudent> RankScope = new RatingScope<RankStudent>("排名學生");

                //計算班級平均
                Dictionary<string, AverageCount> AverageCount = new Dictionary<string, AverageCount>();

                #region 針對群組下每位學生
                foreach (string StudentID in CurrentTagStudents.StudentIDs)
                {
                    if (ReportProgress != null)
                        ReportProgress.Invoke((int)(++RankProgressCount * 100.0d / RandkTotalCount), "計算類組排名中...");

                    //加入table
                    if (!mStudentsScore.ContainsKey(StudentID))
                        mStudentsScore.Add(StudentID, new Dictionary<string, string>());

                    //排名學生
                    RankStudent RankStudent = new RankStudent(StudentID);
                    RankScope.Add(RankStudent);

                    bool canRank = true;      //是否可排名
                    decimal WeiSum = 0;       //加權總分
                    decimal Sum = 0;          //總分
                    decimal CreditCount = 0;  //總學分數
                    decimal SubjectCount = 0; //總分加總的科目數

                    foreach (SHSCAttendRecord Attendance in SCAttends.FindAll(x => x.RefStudentID.Equals(StudentID)))
                    {
                        SHCourseRecord StudentCourse = Courses.Find(x => x.ID.Equals(Attendance.RefCourseID));

                        List<string> CourseExamList = new List<string>();

                        if (StudentCourse != null)
                            CourseExamList = AEIncludeList
                            .FindAll(x => x.RefAssessmentSetupID.Equals(StudentCourse.RefAssessmentSetupID))
                            .Select(x => x.ExamName)
                            .ToList();

                        if (StudentCourse != null && CourseExamList.Count > 0 && vSubjects.Contains(StudentCourse.Subject) && CourseExamList.Contains(vExam))
                        {
                            //把科目、級別、學分數兜成 "_科目_級別_學分數_"的字串，這個字串在不同科目級別學分數會成為唯一值
                            string SubjectKey = StudentCourse.Subject + "^_^" + K12.Data.Decimal.GetString(StudentCourse.Level) + "^_^" + K12.Data.Decimal.GetString(StudentCourse.Credit);
                            bool hasScore = false;

                            #region 檢查這個KEY有沒有評分同時計算總分平均及是否可排名
                            foreach (SHSCETakeRecord ExamScore in SCETakes.FindAll(x => x.RefStudentID.Equals(Attendance.RefStudentID)))
                            {
                                SHCourseRecord SCETakeCourse = Courses.Find(x => x.ID.Equals(ExamScore.RefCourseID));
                                SHExamRecord SCETakeExam = ExamList.Find(x => x.ID.Equals(ExamScore.RefExamID));

                                if (SCETakeCourse != null &&
                                    SCETakeExam != null &&
                                    SCETakeExam.Name.Equals(vExam) &&
                                    SubjectKey.Equals(SCETakeCourse.Subject + "^_^" + SCETakeCourse.Level + "^_^" + SCETakeCourse.Credit))
                                {
                                    //是要列印的科目
                                    if (!SubjectKeys.Contains(SubjectKey))
                                        SubjectKeys.Add(SubjectKey);

                                    hasScore = true;

                                    if (StudentCourse.Credit.HasValue)
                                    {
                                        WeiSum += ExamScore.Score * StudentCourse.Credit.Value; //加權總分
                                        CreditCount += StudentCourse.Credit.Value; //學分
                                        RankStudent[SubjectKey] = ExamScore.Score;

                                        if (!AverageCount.ContainsKey(SubjectKey))
                                            AverageCount.Add(SubjectKey, new AverageCount());
                                        AverageCount[SubjectKey].Score += ExamScore.Score;
                                        AverageCount[SubjectKey].Count++;
                                    }

                                    Sum += ExamScore.Score; //總分
                                    SubjectCount++; //科目數
                                }
                            }
                            #endregion
                            //發現沒有評分
                            if (!hasScore)
                                canRank = false;
                        }
                    }

                    decimal WeiAverage = WeiSum / (CreditCount == 0 ? 1 : CreditCount);
                    decimal Average = Sum / (SubjectCount == 0 ? 1 : SubjectCount);

                    mStudentsScore[StudentID].Add("加權總分", WeiSum.Round().GetFormat());
                    mStudentsScore[StudentID].Add("加權平均", WeiAverage.Round().GetFormat());
                    mStudentsScore[StudentID].Add("總分", Sum.Round().GetFormat());
                    mStudentsScore[StudentID].Add("平均", Average.Round().GetFormat());

                    RankStudent["總分"] = Sum;
                    RankStudent["平均"] = Average;
                    RankStudent["加權總分"] = WeiSum;
                    RankStudent["加權平均"] = WeiAverage;

                    if (!AverageCount.ContainsKey("總分"))
                        AverageCount.Add("總分", new AverageCount());

                    if (!AverageCount.ContainsKey("平均"))
                        AverageCount.Add("平均", new AverageCount());

                    if (!AverageCount.ContainsKey("加權總分"))
                        AverageCount.Add("加權總分", new AverageCount());

                    if (!AverageCount.ContainsKey("加權平均"))
                        AverageCount.Add("加權平均", new AverageCount());

                    AverageCount["總分"].Score += Sum;
                    AverageCount["總分"].Count++;
                    AverageCount["平均"].Score += Average;
                    AverageCount["平均"].Count++;
                    AverageCount["加權總分"].Score += WeiSum;
                    AverageCount["加權總分"].Count++;
                    AverageCount["加權平均"].Score += WeiAverage;
                    AverageCount["加權平均"].Count++;
                }
                #endregion

                RankScope.Rank(new ScoreParser("總分"), PlaceOptions.Unsequence);
                RankScope.Rank(new ScoreParser("平均"), PlaceOptions.Unsequence);
                RankScope.Rank(new ScoreParser("加權總分"), PlaceOptions.Unsequence);
                RankScope.Rank(new ScoreParser("加權平均"), PlaceOptions.Unsequence);

                SubjectKeys.ForEach(x => RankScope.Rank(new ScoreParser(x), PlaceOptions.Unsequence));

                foreach (string Key in AverageCount.Keys)
                    if (SubjectKeys.Contains(Key))
                        AverageCount[Key].Score /= AverageCount[Key].Count;

                foreach (string StudentID in CurrentTagStudents.StudentIDs)
                {
                    if (!mStudentsScore[StudentID].ContainsKey("分組人數"))
                        mStudentsScore[StudentID].Add("分組人數", ""+CurrentTagStudents.StudentIDs.Count);

                    if (!mStudentsScore[StudentID].ContainsKey("分組總分排名"))
                        mStudentsScore[StudentID].Add("分組總分排名", RankScope.Contains(StudentID) && RankScope[StudentID].Places.Contains("總分") ? "" + RankScope[StudentID].Places["總分"].Level : string.Empty);

                    if (!mStudentsScore[StudentID].ContainsKey("分組平均排名"))
                        mStudentsScore[StudentID].Add("分組平均排名", RankScope.Contains(StudentID) && RankScope[StudentID].Places.Contains("平均") ? "" + RankScope[StudentID].Places["平均"].Level : string.Empty);

                    if (!mStudentsScore[StudentID].ContainsKey("分組加權總分排名"))
                        mStudentsScore[StudentID].Add("分組加權總分排名", RankScope.Contains(StudentID) && RankScope[StudentID].Places.Contains("加權總分") ? "" + RankScope[StudentID].Places["加權總分"].Level : string.Empty);

                    if (!mStudentsScore[StudentID].ContainsKey("分組加權平均排名"))
                        mStudentsScore[StudentID].Add("分組加權平均排名", RankScope.Contains(StudentID) && RankScope[StudentID].Places.Contains("加權平均") ? "" + RankScope[StudentID].Places["加權平均"].Level : string.Empty);

                    foreach (string x in SubjectKeys)
                    {
                        if (RankScope[StudentID].Places.Contains(x))
                        {
                            if (!mStudentsScore[StudentID].ContainsKey("分組排名^_^" + x))
                            {
                                string SubjectKey = "分組排名^_^" + x;
                                string SubjectRank = RankScope[StudentID].Places.Contains(x) ? "" + RankScope[StudentID].Places[x].Level : string.Empty;

                                mStudentsScore[StudentID].Add(SubjectKey , SubjectRank);
                            }
                            if (!mStudentsScore[StudentID].ContainsKey("分組平均^_^" + x))
                                mStudentsScore[StudentID].Add("分組平均^_^" + x, "" + AverageCount[x].Score.ToString("0.00"));
                        } 
                    }
                }
            }
            #endregion
        }


        private Tuple<decimal, decimal, decimal, decimal> GetClassAverageScore(Dictionary<string,string> StudentScore,Dictionary<string,AverageCount> ClassAverageScore)
        {
            decimal Sum = 0;
            decimal Avg = 0;
            decimal WeiSum = 0;
            decimal WeiAvg = 0;
            decimal Count = 0;
            decimal CreditCount = 0;

            foreach (string Key in StudentScore.Keys)
            {
                string[] list = Key.Split(new string[] { "^_^" }, StringSplitOptions.None);                

                if (list.Count() == 3)
                {
                    if (ClassAverageScore.ContainsKey(Key))
                    {
                        int Credit = K12.Data.Int.Parse(list[2]);
                        Sum += ClassAverageScore[Key].Score;
                        WeiSum += Credit * ClassAverageScore[Key].Score;
                        Count++;
                        CreditCount += Credit;
                    }
                }
            }

            if (Count > 0)
                Avg = (Sum / Count).Round();
            if (CreditCount > 0)
                WeiAvg = (WeiSum / CreditCount).Round();

            return new Tuple<decimal, decimal, decimal, decimal>(Sum, Avg, WeiSum, WeiAvg);
        }

        /// <summary>
        /// 計算班級學生成績
        /// </summary>
        /// <param name="vClass"></param>
        /// <param name="vSubjects"></param>
        /// <param name="vExam"></param>
        /// <param name="vCategory"></param>
        public void CalculateClassScore(SHClassRecord vClass,List<string> vSubjects, string vExam, string vCategory)
        {
            //建立學生範圍
            RatingScope<RankStudent> RankScope = new RatingScope<RankStudent>("排名學生");

            //計算班級平均
            Dictionary<string, AverageCount> AverageCount = new Dictionary<string, AverageCount>();

            //整理列印科目+級別+學分數
            SubjectKeys = new List<string>();

            #region 設定目前試別，用來取得評量成績用（少撈點資料）
            SHExamRecord ExamRecord = ExamList.Find(x => x.Name.Equals(vExam));

            if (ExamRecord != null)
                mCurrentExamID = ExamRecord.ID;
            #endregion

            #region 分批取得評量成績
            FunctionSpliter<string, SHSCETakeRecord> SCETakeSpliter = new FunctionSpliter<string, SHSCETakeRecord>(1000, 3);
            SCETakeSpliter.Function = SelectSCETake;
            List<SHSCETakeRecord> SCETakeList = SCETakeSpliter.Execute(SCAttendList.Select(x => x.ID).ToList());
            #endregion

            List<SHStudentRecord> CalculateStudentList = ClassStudentList
                .FindAll(x => x.RefClassID.Equals(vClass.ID))
                .OrderBy(x => x.SeatNo)
                .Distinct()
                .ToList();

            #region 針對班級學生
            foreach (SHStudentRecord Student in CalculateStudentList)
            {
                //排名學生
                RankStudent RankStudent = new RankStudent(Student.ID);
                RankScope.Add(RankStudent);

                //加入table
                if (!mStudentsScore.ContainsKey(Student.ID))
                    mStudentsScore.Add(Student.ID,new Dictionary<string,string>());
                //加權總分
                decimal WeiSum = 0;
                //參加排名
                bool canRank = true;
                //總學分數
                decimal CreditCount = 0;
                //總分
                decimal Sum = 0;
                //總分加總的科目數
                decimal SubjectCount = 0;

                List<SHSCAttendRecord> StudentSCAttendList = SCAttendList.FindAll(x => x.RefStudentID.Equals(Student.ID));

                foreach (SHSCAttendRecord Attendance in StudentSCAttendList)
                {
                    SHCourseRecord StudentCourse = CourseList.Find(x => x.ID.Equals(Attendance.RefCourseID));

                    List<string> CourseExamList = new List<string>();

                    if (StudentCourse != null)
                        CourseExamList = AEIncludeList
                        .FindAll(x => x.RefAssessmentSetupID.Equals(StudentCourse.RefAssessmentSetupID))
                        .Select(x => x.ExamName)
                        .ToList();

                    if (StudentCourse != null && CourseExamList.Count > 0 && vSubjects.Contains(StudentCourse.Subject) && CourseExamList.Contains(vExam))
                    {
                        //把科目、級別、學分數兜成 "_科目_級別_學分數_"的字串，這個字串在不同科目級別學分數會成為唯一值
                        string SubjectKey = StudentCourse.Subject + "^_^" + K12.Data.Decimal.GetString(StudentCourse.Level) + "^_^" + K12.Data.Decimal.GetString(StudentCourse.Credit);
                        bool hasScore = false;

                        #region 檢查這個KEY有沒有評分同時計算總分平均及是否可排名
                        foreach (SHSCETakeRecord ExamScore in SCETakeList.FindAll(x => x.RefSCAttendID.Equals(Attendance.ID)))
                        {
                            SHCourseRecord SCETakeCourse = CourseList.Find(x => x.ID.Equals(ExamScore.RefCourseID));

                            SHExamRecord SCETakeExam = ExamList.Find(x => x.ID.Equals(ExamScore.RefExamID));

                            if (SCETakeCourse != null &&
                                SCETakeExam != null &&
                                SCETakeExam.Name.Equals(vExam) &&
                                SubjectKey.Equals(SCETakeCourse.Subject + "^_^" + SCETakeCourse.Level + "^_^" + SCETakeCourse.Credit))
                            {
                                //是要列印的科目
                                if (!SubjectKeys.Contains(SubjectKey))
                                    SubjectKeys.Add(SubjectKey);

                                hasScore = true;

                                if (!mStudentsScore[Student.ID].ContainsKey(SubjectKey))
                                    mStudentsScore[Student.ID].Add(SubjectKey, ExamScore.Score.Round().GetFormat());
                                else
                                    mStudentsScore[Student.ID][SubjectKey] = ExamScore.Score.Round().GetFormat();

                                if (StudentCourse.Credit.HasValue)
                                {
                                    WeiSum += ExamScore.Score * StudentCourse.Credit.Value; //加權總分
                                    CreditCount += StudentCourse.Credit.Value; //學分
                                    RankStudent[SubjectKey] = ExamScore.Score;

                                    if (!AverageCount.ContainsKey(SubjectKey))
                                        AverageCount.Add(SubjectKey, new AverageCount());
                                    AverageCount[SubjectKey].Score += ExamScore.Score;
                                    AverageCount[SubjectKey].Count++;
                                }

                                Sum += ExamScore.Score; //總分
                                SubjectCount++; //科目數
                            }
                        }
                        #endregion
                        //發現沒有評分
                        if (!hasScore)
                        {
                            canRank = false; //若是其中有一科沒有評分，就不能排名；必須要所有科目都有才能排名
                            if (!mStudentsScore[Student.ID].ContainsKey(SubjectKey))
                                mStudentsScore[Student.ID].Add(SubjectKey, "未輸入");
                        }
                    }
                }

                decimal WeiAverage = WeiSum / (CreditCount == 0 ? 1 : CreditCount);
                decimal Average = Sum / (SubjectCount == 0 ? 1 : SubjectCount);

                if (!mStudentsScore[Student.ID].ContainsKey("加權總分"))
                    mStudentsScore[Student.ID].Add("加權總分", WeiSum.Round().GetFormat());
                if (!mStudentsScore[Student.ID].ContainsKey("加權平均"))
                    mStudentsScore[Student.ID].Add("加權平均", WeiAverage.Round().GetFormat());
                if (!mStudentsScore[Student.ID].ContainsKey("總分"))
                    mStudentsScore[Student.ID].Add("總分", Sum.Round().GetFormat());
                if (!mStudentsScore[Student.ID].ContainsKey("平均"))
                    mStudentsScore[Student.ID].Add("平均", Average.Round().GetFormat());

                //if (canRank)
                //{
                    RankStudent["總分"] = Sum;
                    RankStudent["平均"] = Average;
                    RankStudent["加權總分"] = WeiSum;
                    RankStudent["加權平均"] = WeiAverage;
                //}
            }
            #endregion

            RankScope.Rank(new ScoreParser("總分"), PlaceOptions.Unsequence);
            RankScope.Rank(new ScoreParser("平均"), PlaceOptions.Unsequence);
            RankScope.Rank(new ScoreParser("加權總分"), PlaceOptions.Unsequence);
            RankScope.Rank(new ScoreParser("加權平均"), PlaceOptions.Unsequence);

            SubjectKeys.ForEach(x=> RankScope.Rank(new ScoreParser(x),PlaceOptions.Unsequence));

            //計算班級科目平均
            foreach (string Key in AverageCount.Keys)
                if (SubjectKeys.Contains(Key))
                    AverageCount[Key].Score = (AverageCount[Key].Score / AverageCount[Key].Count).Round();

            foreach (SHStudentRecord stuRec in CalculateStudentList)
            {
                Tuple<decimal,decimal,decimal,decimal> Scores = GetClassAverageScore(mStudentsScore[stuRec.ID],AverageCount);

                if (!mStudentsScore[stuRec.ID].ContainsKey("班級總分"))
                    mStudentsScore[stuRec.ID].Add("班級總分", Scores.Item1.GetFormat());

                if (!mStudentsScore[stuRec.ID].ContainsKey("班級平均"))
                    mStudentsScore[stuRec.ID].Add("班級平均", Scores.Item2.GetFormat());

                if (!mStudentsScore[stuRec.ID].ContainsKey("班級加權總分"))
                    mStudentsScore[stuRec.ID].Add("班級加權總分", Scores.Item3.GetFormat());
                
                if (!mStudentsScore[stuRec.ID].ContainsKey("班級加權平均"))
                    mStudentsScore[stuRec.ID].Add("班級加權平均", Scores.Item4.GetFormat());

                if (!mStudentsScore[stuRec.ID].ContainsKey("總分排名"))
                    mStudentsScore[stuRec.ID].Add("總分排名", RankScope.Contains(stuRec.ID) && RankScope[stuRec.ID].Places.Contains("總分") ? "" + RankScope[stuRec.ID].Places["總分"].Level : string.Empty);
                if (!mStudentsScore[stuRec.ID].ContainsKey("平均排名"))
                    mStudentsScore[stuRec.ID].Add("平均排名", RankScope.Contains(stuRec.ID) && RankScope[stuRec.ID].Places.Contains("平均") ? "" + RankScope[stuRec.ID].Places["平均"].Level : string.Empty);
                if (!mStudentsScore[stuRec.ID].ContainsKey("加權總分排名"))
                    mStudentsScore[stuRec.ID].Add("加權總分排名", RankScope.Contains(stuRec.ID) && RankScope[stuRec.ID].Places.Contains("加權總分") ? "" + RankScope[stuRec.ID].Places["加權總分"].Level : string.Empty);
                if (!mStudentsScore[stuRec.ID].ContainsKey("加權平均排名"))
                    mStudentsScore[stuRec.ID].Add("加權平均排名", RankScope.Contains(stuRec.ID) && RankScope[stuRec.ID].Places.Contains("加權平均") ? "" + RankScope[stuRec.ID].Places["加權平均"].Level : string.Empty);

                SubjectKeys.ForEach
                (x=>
                    {
                        if (mStudentsScore[stuRec.ID].ContainsKey(x))
                        {
                            if (!mStudentsScore[stuRec.ID].ContainsKey("排名^_^" + x))
                                mStudentsScore[stuRec.ID].Add("排名^_^" + x,RankScope[stuRec.ID].Places.Contains(x)?""+RankScope[stuRec.ID].Places[x].Level:string.Empty);
                            if (!mStudentsScore[stuRec.ID].ContainsKey("平均^_^" + x))
                                mStudentsScore[stuRec.ID].Add("平均^_^" + x, "" + AverageCount[x].Score.GetFormat());
                        }
                    }
                );
            }

            //排序要列印的科目
            SubjectKeys.Sort(new SubjectComparer() { });
        }


        private Tuple<string, string> GetReceiver(string ReveiverIndex,string ReceiverAddressIndex,SHStudentRecord Student,SHParentRecord Parent,SHAddressRecord Address)
        {
            string ReceiverName = string.Empty;
            string ReceiverAddress = string.Empty;

            switch(ReveiverIndex)
            {
                case "0": ReceiverName = Student.Name; break;
                case "1": ReceiverName = Parent.CustodianName; break;
                case "2": ReceiverName = Parent.FatherName; break;
                case "3": ReceiverName = Parent.MotherName; break;
                default: ReceiverName = Parent.CustodianName; break; //預設為監護人姓名
            }

            switch (ReceiverAddressIndex)
            {
                case "0": ReceiverAddress = Address.PermanentAddress; break;
                case "1": ReceiverAddress = Address.MailingAddress; break;
                case "2": ReceiverAddress = Address.Address1Address; break;
                default: ReceiverAddress = Address.PermanentAddress; break; //預設為戶籍地址
            }

            return new Tuple<string, string>(ReceiverName, ReceiverAddress);
        }

        /// <summary>
        /// 計算成績
        /// </summary>
        /// <param name="vClass"></param>
        /// <param name="vSubjects"></param>
        /// <param name="vExam"></param>
        public Tuple<int,Dictionary<string, List<DataSet>>> GetResult(List<string> vSubjects,string vExam)
        {
            Dictionary<string, List<DataSet>> Result = new Dictionary<string, List<DataSet>>();

            Dictionary<string, string> ClassStudentCounts = new Dictionary<string, string>();

            int MaxSubjectCount = 0;

            SelectedStudentList.Select(x => x.RefClassID).Distinct().ToList().ForEach
            (x =>
                {
                    int Count = ClassStudentList.FindAll(y => y.RefClassID.Equals(x)).Count;

                    ClassStudentCounts.Add(x, "" + Count);
                }
            );

            foreach (SHStudentRecord Student in SelectedStudentList.OrderBy(x=>x.SeatNo))
            {
                #region 初始化資料
                DataSet StudentResult = new DataSet("DataSection");
                SHClassRecord Class = ClassList.Find(x => x.ID.Equals(Student.RefClassID));
                SHTeacherRecord Teacher = Class != null ? TeacherList.Find(x => x.ID.Equals(Class.RefTeacherID)):null;
                SHParentRecord Parent = ParentList.Find(x => x.RefStudentID.Equals(Student.ID));
                SHAddressRecord Address = AddressList.Find(x => x.RefStudentID.Equals(Student.ID));

                string vClassName = Class != null ? Class.Name : "《未分班》";
                string vTeacherName = Teacher != null ? Teacher.Name : "《無班導師》";
                string CustodianName = Parent != null ? Parent.CustodianName : string.Empty;
                string FatherName = Parent != null ? Parent.FatherName : string.Empty;
                string MotherName = Parent != null ? Parent.MotherName : string.Empty;
                string PermanentAddress = Address != null ? Address.PermanentAddress : string.Empty;
                string MailingAddress = Address != null ? Address.MailingAddress : string.Empty;
                string ClassStudentCount = !string.IsNullOrEmpty(Student.RefClassID) ? ClassStudentCounts[Student.RefClassID] : "0";

                if (!Result.ContainsKey(vClassName))
                    Result.Add(vClassName, new List<DataSet>());
                #endregion 

                #region 基本資料
                DataTable SchoolName = K12.Data.School.ChineseName.ToDataTable("學校名稱", "學校名稱");
                DataTable SchoolYear = K12.Data.School.DefaultSchoolYear.ToDataTable("學年度", "學年度");
                DataTable Semester = K12.Data.School.DefaultSemester.ToDataTable("學期", "學期");
                DataTable ExamName = vExam.ToDataTable("試別名稱", "試別名稱");

                StudentResult.Tables.Add(SchoolName);
                StudentResult.Tables.Add(SchoolYear);
                StudentResult.Tables.Add(Semester);
                StudentResult.Tables.Add(ExamName);
                #endregion

                #region 學生資料
                DataTable StudentName = Student.Name.ToDataTable("學生姓名", "學生姓名");
                DataTable StudentNumber = Student.StudentNumber.ToDataTable("學號", "學號");
                DataTable StudentSeatNo = K12.Data.Int.GetString(Student.SeatNo).ToDataTable("座號", "座號");
                DataTable ClassName = vClassName.ToDataTable("班級名稱", "班級名稱");
                DataTable MyTeacher = vTeacherName.ToDataTable("導師姓名", "導師姓名");

                StudentResult.Tables.Add(StudentName);
                StudentResult.Tables.Add(StudentNumber);
                StudentResult.Tables.Add(StudentSeatNo);
                StudentResult.Tables.Add(ClassName);
                StudentResult.Tables.Add(MyTeacher);
                StudentResult.Tables.Add(CustodianName.ToDataTable("監護人姓名", "監護人姓名"));
                StudentResult.Tables.Add(MotherName.ToDataTable("母親姓名", "母親姓名"));
                StudentResult.Tables.Add(FatherName.ToDataTable("父親姓名", "父親姓名"));
                StudentResult.Tables.Add(PermanentAddress.ToDataTable("永久地址", "永久地址"));
                StudentResult.Tables.Add(MailingAddress.ToDataTable("郵寄地址", "郵寄地址"));
                StudentResult.Tables.Add(K12.Data.School.Address.ToDataTable("學校地址", "學校地址"));
                StudentResult.Tables.Add(ClassStudentCount.ToDataTable("班級學生人數", "班級學生人數"));

                ConfigData config = K12.Data.School.Configuration["高中個人評量成績單"];

                string ReceiverIndex = config["Custodian"];
                string ReceiverAddressIndex = config["Address"];

                Tuple<string, string> ReceiverResult = GetReceiver(ReceiverIndex, ReceiverAddressIndex, Student, Parent, Address);
                StudentResult.Tables.Add(ReceiverResult.Item1.ToDataTable("收件人姓名", "收件人姓名"));
                StudentResult.Tables.Add(ReceiverResult.Item2.ToDataTable("收件人地址", "收件人地址"));
                #endregion

                #region 成績資料
                if (StudentsScore.ContainsKey(Student.ID))
                {
                    Dictionary<string, string> vStudentScores = StudentsScore[Student.ID];

                    StudentResult.Tables.Add(vStudentScores["加權平均"].ToDataTable("加權平均", "加權平均"));
                    StudentResult.Tables.Add(vStudentScores["加權總分"].ToDataTable("加權總分", "加權總分"));
                    StudentResult.Tables.Add(vStudentScores["平均"].ToDataTable("平均", "平均"));
                    StudentResult.Tables.Add(vStudentScores["總分"].ToDataTable("總分", "總分"));

                    StudentResult.Tables.Add(vStudentScores["班級平均"].ToDataTable("班級平均", "班級平均"));
                    StudentResult.Tables.Add(vStudentScores["班級總分"].ToDataTable("班級總分", "班級總分"));
                    StudentResult.Tables.Add(vStudentScores["班級加權平均"].ToDataTable("班級加權平均", "班級加權平均"));
                    StudentResult.Tables.Add(vStudentScores["班級加權總分"].ToDataTable("班級加權總分", "班級加權總分"));

                    StudentResult.Tables.Add(vStudentScores["加權平均排名"].ToDataTable("加權平均排名", "加權平均排名"));
                    StudentResult.Tables.Add(vStudentScores["加權總分排名"].ToDataTable("加權總分排名", "加權總分排名"));
                    StudentResult.Tables.Add(vStudentScores["平均排名"].ToDataTable("平均排名", "平均排名"));
                    StudentResult.Tables.Add(vStudentScores["總分排名"].ToDataTable("總分排名", "總分排名"));

                    if (vStudentScores.ContainsKey("分組人數"))
                        StudentResult.Tables.Add(vStudentScores["分組人數"].ToDataTable("分組人數", "分組人數"));
                    if (vStudentScores.ContainsKey("分組加權平均排名"))
                        StudentResult.Tables.Add(vStudentScores["分組加權平均排名"].ToDataTable("分組加權平均排名", "分組加權平均排名"));
                    if (vStudentScores.ContainsKey("分組加權總分排名"))
                        StudentResult.Tables.Add(vStudentScores["分組加權總分排名"].ToDataTable("分組加權總分排名", "分組加權總分排名"));
                    if (vStudentScores.ContainsKey("分組平均排名"))
                        StudentResult.Tables.Add(vStudentScores["分組平均排名"].ToDataTable("分組平均排名", "分組平均排名"));
                    if (vStudentScores.ContainsKey("分組總分排名"))
                        StudentResult.Tables.Add(vStudentScores["分組總分排名"].ToDataTable("分組總分排名", "分組總分排名"));

                    int SubjectCount = 0;

                    List<string> SubjectKeys = vStudentScores.Keys.ToList();

                    SubjectKeys.Sort();
                    SubjectKeys.Sort(new SubjectComparer() { });

                    foreach(string z in SubjectKeys)
                    {
                        string[] list = z.Split(new string[] { "^_^" }, StringSplitOptions.None);

                        if (vSubjects.Contains(list[0]))
                        {
                            SubjectCount++;
                            string SubjectAverageKey = "平均^_^" + z;
                            string SubjectRankKey = "排名^_^" + z;
                            string ClassSubjectRankKey = "分組排名^_^" + z;

                            //StudentResult.Tables.Add((x + Common.GetNumberString(list[1])).ToDataTable("科目名稱" + SubjectIndex, "科目名稱"));
                            StudentResult.Tables.Add((list[0] + Common.GetNumberString(list[1])).ToDataTable("科目名稱" + SubjectCount, "科目名稱"));
                            StudentResult.Tables.Add((list[2]).ToDataTable("科目學分數" + SubjectCount, "科目學分數"));
                            StudentResult.Tables.Add(vStudentScores[z].ToDataTable("科目成績" + SubjectCount, "科目成績"));

                            string SubjectRankValue = vStudentScores.ContainsKey(SubjectRankKey) ? vStudentScores[SubjectRankKey] : string.Empty;
                            string ClassSubjectRankValue = vStudentScores.ContainsKey(ClassSubjectRankKey) ? vStudentScores[ClassSubjectRankKey] : string.Empty;
                            string SubjectAverageValue = vStudentScores.ContainsKey(SubjectAverageKey) ? vStudentScores[SubjectAverageKey] : string.Empty;

                            StudentResult.Tables.Add(SubjectRankValue.ToDataTable("科目排名" + SubjectCount, "科目排名"));
                            StudentResult.Tables.Add(ClassSubjectRankValue.ToDataTable("科目分組排名" + SubjectCount, "科目分組排名"));
                            StudentResult.Tables.Add(SubjectAverageValue.ToDataTable("科目班級平均" + SubjectCount, "科目班級平均"));
                        }
                     }

                    if (SubjectCount > MaxSubjectCount)
                        MaxSubjectCount = SubjectCount;
                }
                #endregion

                Result[vClassName].Add(StudentResult);
            }

            return new Tuple<int, Dictionary<string, List<DataSet>>>(MaxSubjectCount, Result);
        }
    }
}