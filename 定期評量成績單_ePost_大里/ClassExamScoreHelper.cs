using System.Collections.Generic;
using System.Data;
using System.Linq;
using FISCA.Data;
using SHSchool.Data;

namespace 班級考試成績單
{
    /// <summary>
    /// 實際計算班級考試成績
    /// </summary>
    public class ClassExamScoreHelper
    {
        private Dictionary<SHStudentRecord, Dictionary<string, string>> classExamScoreTable;
        private List<string> SubjectKeys;
        private string mCurrentExamID;

        /// <summary>
        /// 無參數建構式
        /// </summary>
        public ClassExamScoreHelper()
        {

        }

        /// <summary>
        /// 取得班級考試成績單所需基本資料 
        /// Ⅰ選取班級學生名單 
        /// Ⅱ學生修課清單 
        /// Ⅲ學生修課考試資訊
        /// </summary>
        public void InitialClassExamScore()
        {
            ClassList = SHClass
                .SelectByIDs(K12.Presentation.NLDPanels.Class.SelectedSource);      //取得選取班級

            StudentList = SHStudent
                .SelectByClassIDs(K12.Presentation.NLDPanels.Class.SelectedSource)
                .FindAll(x=>x.Status == K12.Data.StudentRecord.StudentStatus.一般); //取得選取班級的學生，並且為一般生。

            StudentTagList = SHStudentTag
                .SelectByStudentIDs(StudentList.Select(x=>x.ID)); //取得學生標籤

            StudentCategoryList = StudentTagList
                .Select(x=>x.FullName)
                .Distinct()
                .ToList();

            StudentPrefixList = StudentTagList
                .Select(x => x.Prefix)
                .Distinct()
                .ToList();

            ExamList = SHExam.SelectAll();

            AEIncludeList = SHAEInclude.SelectAll();

            #region 分批取得學生修課
            FunctionSpliter<string, SHSCAttendRecord> SCAttendSpliter = new FunctionSpliter<string, SHSCAttendRecord>(1000, 3);

            SCAttendSpliter.Function = SelectSCAttend;

            SCAttendList = SCAttendSpliter.Execute(StudentList.Select(x => x.ID).ToList());
            #endregion

            #region 分批取得課程清單
            FunctionSpliter<string,SHCourseRecord> CourseSpliter = new FunctionSpliter<string,SHCourseRecord>(500, 3);

            CourseSpliter.Function = SHCourse.SelectByIDs;

            CourseList = CourseSpliter.Execute(SCAttendList.Select(x => x.RefCourseID).Distinct().ToList());
            #endregion
        }

        /// <summary>
        /// 依目前年度及學期取得學生修課
        /// </summary>
        /// <param name="StudentIDs"></param>
        /// <returns></returns>
        private List<SHSCAttendRecord> SelectSCAttend(List<string> StudentIDs)
        {
            return SHSCAttend.Select(StudentIDs, null, null, K12.Data.School.DefaultSchoolYear, K12.Data.School.DefaultSemester);
        }

        /// <summary>
        /// 依學生修課取得評量成績
        /// </summary>
        /// <param name="SCAttendIDs"></param>
        /// <returns></returns>
        private List<SHSCETakeRecord> SelectSCETake(List<string> SCAttendIDs)
        {
            return SHSCETake.Select(null, null, new List<string>(){mCurrentExamID}, null,SCAttendIDs);
        }

        /// <summary>
        /// 學生類別排名
        /// </summary>
        public Dictionary<string,string> StudentTagRanks { get; private set; }

        /// <summary>
        /// 評量設定列表
        /// </summary>
        public List<SHAEIncludeRecord> AEIncludeList { get; private set; }

        /// <summary>
        /// 學生類別列表
        /// </summary>
        public List<string> StudentCategoryList { get ; private set; }

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
        public List<SHStudentRecord> StudentList { get; private set; }

        /// <summary>
        /// 取得選取班級考清單，例如期中考、期末考…等。
        /// </summary>
        /// <returns></returns>
        public List<SHExamRecord> ExamList { get; private set;}

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
            foreach(SHStudentTagRecord StudentTag in StudentTagList.FindAll(x=>x.RefStudentID.Equals(Student.ID)))
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
                .Select(x=>x.RefAssessmentSetupID)
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

        public Dictionary<SHStudentRecord, Dictionary<string, string>> ScoreTable
        {
            get
            {
                return classExamScoreTable;
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
        public void CalculateTagRank(List<string> vSubjects, string vExam,string vCategory)
        {
            SubjectKeys = new List<string>();

            List<string> StudentIDs = new List<string>();
            Dictionary<string, string> TotalRankStudents = new Dictionary<string, string>();        //所有排名學生
            Dictionary<string, TagStudents> TagStudents = new Dictionary<string, TagStudents>();    //標籤下的學生           

            #region 設定目前試別，用來取得評量成績用（少撈點資料）
            SHExamRecord ExamRecord = ExamList.Find(x => x.Name.Equals(vExam));

            if (ExamRecord != null)
                mCurrentExamID = ExamRecord.ID;
            #endregion

            #region 根據群組取得學生標籤清單
            QueryHelper Helper = new QueryHelper();
            DataTable Table = Helper.Select("select tag_student.ref_student_id,class.grade_year,tag.prefix,tag.name from tag_student inner join tag on tag_student.ref_tag_id=tag.id inner join student on student.id=tag_student.ref_student_id left outer join class on class.id = student.ref_class_id where prefix='" + vCategory + "'");

            List<string> SelectedStudentTags = new List<string>();

            List<string> SelectedStudentIDs = StudentList
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

                    TagStudents[TagFullName].StudentIDs.Add(RefStudentID);
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
                //最後學生排名
                Dictionary<string, string> StudentRanks = new Dictionary<string, string>();
                Dictionary<string, decimal> StudentScores = new Dictionary<string, decimal>();

                //參與排序的學生
                Dictionary<string, decimal> canRankList = new Dictionary<string, decimal>();

                //記錄每個學生有未評分的KEY(如果整班都未評分就不管，但如果發現有評分就要把學生從可排序清單中移除)
                Dictionary<string, List<string>> nonScoreKeys = new Dictionary<string, List<string>>();

                #region 針對群組下每位學生
                foreach(string StudentID in CurrentTagStudents.StudentIDs)
                {
                    bool canRank = true;    //是否可排名
                    decimal scoreCount = 0; //加權總分
                    decimal CreditCount = 0;    //總學分數
                    StudentRanks.Add(StudentID, "");

                    foreach(SHSCAttendRecord Attendance in SCAttends.FindAll(x=>x.RefStudentID.Equals(StudentID)))
                    {
                        SHCourseRecord StudentCourse = Courses.Find(x => x.ID.Equals(Attendance.RefCourseID));

                        List<string> CourseExamList = new List<string>();

                        if (StudentCourse!=null)
                            CourseExamList = AEIncludeList
                            .FindAll(x => x.RefAssessmentSetupID.Equals(StudentCourse.RefAssessmentSetupID))
                            .Select(x => x.ExamName)
                            .ToList();

                        if (StudentCourse!=null && CourseExamList.Count>0 && vSubjects.Contains(StudentCourse.Subject) && CourseExamList.Contains(vExam))
                        {
                            //把科目、級別、學分數兜成 "_科目_級別_學分數_"的字串，這個字串在不同科目級別學分數會成為唯一值
                            string key = StudentCourse.Subject + "^_^" + K12.Data.Decimal.GetString(StudentCourse.Level) + "^_^" + K12.Data.Decimal.GetString(StudentCourse.Credit);
                            bool hasScore = false;

                            #region 檢查這個KEY有沒有評分同時計算總分平均及是否可排名
                            foreach (SHSCETakeRecord ExamScore in SCETakes.FindAll(x=>x.RefStudentID.Equals(Attendance.RefStudentID)))
                            {
                                SHCourseRecord SCETakeCourse = Courses.Find(x => x.ID.Equals(ExamScore.RefCourseID));

                                SHExamRecord SCETakeExam = ExamList.Find(x => x.ID.Equals(ExamScore.RefExamID));

                                if (SCETakeCourse != null &&
                                    SCETakeExam != null &&
                                    SCETakeExam.Name.Equals(vExam) &&
                                    key.Equals(SCETakeCourse.Subject + "^_^" + SCETakeCourse.Level + "^_^" + SCETakeCourse.Credit))
                                {
                                    //是要列印的科目
                                    if (!SubjectKeys.Contains(key))
                                        SubjectKeys.Add(key);

                                    hasScore = true;

                                    if (StudentCourse.Credit.HasValue)
                                    {
                                        scoreCount += ExamScore.Score * StudentCourse.Credit.Value; //加權總分
                                        CreditCount += StudentCourse.Credit.Value; //學分
                                    }
                                }
                            }
                            #endregion
                            //發現沒有評分
                            if (!hasScore)
                            {
                                #region 加入學生未評分科目
                                if (!nonScoreKeys.ContainsKey(StudentID))
                                    nonScoreKeys.Add(StudentID, new List<string>());
                                if (!nonScoreKeys[StudentID].Contains(key))
                                    nonScoreKeys[StudentID].Add(key);
                                #endregion
                            }
                        }
                    }

                    if (!StudentScores.ContainsKey(StudentID))
                        StudentScores.Add(StudentID, scoreCount);

                    if (canRank)
                        canRankList.Add(StudentID, scoreCount);
                }
                #endregion

                #region 如果學生在要列印科目中發現未評分項目則從可排名清單中移除
                foreach(string StudentID in nonScoreKeys.Keys)
                {
                    foreach (string Key in nonScoreKeys[StudentID])
                    {
                         
                        if ((SubjectKeys.Contains(Key)) && canRankList.ContainsKey(StudentID))
                            canRankList.Remove(StudentID);
                    }
                }
                #endregion

                List<decimal> rankScoreList = new List<decimal>();
                rankScoreList.AddRange(canRankList.Values);
                rankScoreList.Sort();
                rankScoreList.Reverse();

                foreach (string StudentID in canRankList.Keys)
                {
                    StudentRanks[StudentID] = "" + (rankScoreList.IndexOf(StudentScores[StudentID]) + 1);
                }

                foreach (string StudentID in StudentRanks.Keys)
                {
                    if (!TotalRankStudents.ContainsKey(StudentID))
                        TotalRankStudents.Add(StudentID, StudentRanks[StudentID]);
                }
            }
            #endregion

            StudentTagRanks = TotalRankStudents;
        }

        /// <summary>
        /// 計算成績
        /// </summary>
        /// <param name="vClass"></param>
        /// <param name="vSubjects"></param>
        /// <param name="vExam"></param>
        public void CalculateClassScore(SHClassRecord vClass,List<string> vSubjects,string vExam,string vCategory)
        {
            //學生的各欄位資料
            classExamScoreTable = new Dictionary<SHStudentRecord, Dictionary<string, string>>();

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

            //參予排序的學生
            Dictionary<SHStudentRecord, decimal> canRankList = new Dictionary<SHStudentRecord, decimal>();
            Dictionary<SHStudentRecord, decimal> canRankList2 = new Dictionary<SHStudentRecord, decimal>();

            //記錄每個學生有未評分的KEY(如果整班都未評分就不管，但如果發現有評分就要把學生從可排序清單中移除)
            Dictionary<SHStudentRecord, List<string>> nonScoreKeys = new Dictionary<SHStudentRecord, List<string>>();

            #region 針對班級學生
            foreach (SHStudentRecord Student in StudentList
                .FindAll(x=>x.RefClassID.Equals(vClass.ID))
                .OrderBy(x=>x.SeatNo))
            {
                Dictionary<SHStudentRecord, string> StudentRank = new Dictionary<SHStudentRecord, string>();

                //加入table
                classExamScoreTable.Add(Student, new Dictionary<string, string>());
                //加權總分
                decimal scoreCount = 0;
                //參加排名
                bool canRank = true;
                //總學分數
                decimal CreditCount = 0;
                //總分
                decimal sum = 0;

                foreach(SHSCAttendRecord Attendance in SCAttendList.FindAll(x=>x.RefStudentID.Equals(Student.ID)))
                {
                    SHCourseRecord StudentCourse = CourseList.Find(x => x.ID.Equals(Attendance.RefCourseID));

                    List<string> CourseExamList = new List<string>();

                    if (StudentCourse!=null)
                        CourseExamList = AEIncludeList
                        .FindAll(x => x.RefAssessmentSetupID.Equals(StudentCourse.RefAssessmentSetupID))
                        .Select(x => x.ExamName)
                        .ToList();

                    if (StudentCourse!=null && CourseExamList.Count>0 && vSubjects.Contains(StudentCourse.Subject) && CourseExamList.Contains(vExam))
                    {
                        //把科目、級別、學分數兜成 "_科目_級別_學分數_"的字串，這個字串在不同科目級別學分數會成為唯一值
                        string key = StudentCourse.Subject + "^_^" + K12.Data.Decimal.GetString(StudentCourse.Level) + "^_^" + K12.Data.Decimal.GetString(StudentCourse.Credit);
                        bool hasScore = false;

                        #region 檢查這個KEY有沒有評分同時計算總分平均及是否可排名                            
                        foreach (SHSCETakeRecord ExamScore in SCETakeList.FindAll(x=>x.RefSCAttendID.Equals(Attendance.ID)))
                        {
                            SHCourseRecord SCETakeCourse = CourseList.Find(x => x.ID.Equals(ExamScore.RefCourseID));

                            SHExamRecord SCETakeExam = ExamList.Find(x => x.ID.Equals(ExamScore.RefExamID));

                            if (SCETakeCourse!=null &&
                                SCETakeExam !=null &&
                                SCETakeExam.Name.Equals(vExam) && 
                                key.Equals(SCETakeCourse.Subject + "^_^" + SCETakeCourse.Level + "^_^" + SCETakeCourse.Credit))
                            {
                                //是要列印的科目
                                if (!SubjectKeys.Contains(key))
                                    SubjectKeys.Add(key);

                                hasScore = true;

                                if (!classExamScoreTable[Student].ContainsKey(key))
                                    classExamScoreTable[Student].Add(key, K12.Data.Decimal.GetString(ExamScore.Score));
                                else
                                    classExamScoreTable[Student][key] = K12.Data.Decimal.GetString(ExamScore.Score);

                                if (StudentCourse.Credit.HasValue)
                                {
                                    scoreCount += ExamScore.Score * StudentCourse.Credit.Value; //加權總分
                                    CreditCount += StudentCourse.Credit.Value; //學分
                                }

                                sum += ExamScore.Score; //總分
                            }
                        }
                        #endregion
                        //發現沒有評分
                        if (!hasScore)
                        {
                            #region 加入學生未評分科目
                            if (!nonScoreKeys.ContainsKey(Student))
                                nonScoreKeys.Add(Student, new List<string>());
                            if (!nonScoreKeys[Student].Contains(key))
                                nonScoreKeys[Student].Add(key);
                            #endregion
                            classExamScoreTable[Student].Add(key, "未輸入");
                        }
                    }
                }
                    
                classExamScoreTable[Student].Add("加權總分", scoreCount.ToString());
                classExamScoreTable[Student].Add("加權平均", (scoreCount / (CreditCount == 0 ? 1 : CreditCount)).ToString(".00"));
                classExamScoreTable[Student].Add("總分", sum.ToString());
                if (canRank)
                {
                    canRankList.Add(Student, decimal.Parse((scoreCount / (CreditCount == 0 ? 1 : CreditCount)).ToString(".00")));
                    canRankList2.Add(Student, sum);
                }
            }
            #endregion

            #region 如果學生在要列印科目中發現未評分項目則從可排名清單中移除
            foreach (SHStudentRecord stu in nonScoreKeys.Keys)
            {
                foreach (string key in nonScoreKeys[stu])
                {
                    if (SubjectKeys.Contains(key) && canRankList.ContainsKey(stu))
                    {
                        canRankList.Remove(stu);
                        canRankList2.Remove(stu);
                    }
                }
            }
            #endregion
            
            List<decimal> rankWeiAverageScoreList = new List<decimal>();
            rankWeiAverageScoreList.AddRange(canRankList.Values);
            rankWeiAverageScoreList.Sort();
            rankWeiAverageScoreList.Reverse();
               
            List<decimal> rankTotalScoreList = new List<decimal>();
            rankTotalScoreList.AddRange(canRankList2.Values);
            rankTotalScoreList.Sort();
            rankTotalScoreList.Reverse();
            foreach (SHStudentRecord stuRec in classExamScoreTable.Keys)
            {
                if (canRankList.ContainsKey(stuRec))
                    classExamScoreTable[stuRec].Add("加權平均排名", "" + (rankWeiAverageScoreList.IndexOf(decimal.Parse(classExamScoreTable[stuRec]["加權平均"])) + 1));
                else
                    classExamScoreTable[stuRec].Add("加權平均排名", "");

                if (canRankList2.ContainsKey(stuRec))
                    classExamScoreTable[stuRec].Add("總分排名", "" + (rankTotalScoreList.IndexOf(decimal.Parse(classExamScoreTable[stuRec]["總分"])) + 1));
                else
                    classExamScoreTable[stuRec].Add("總分排名", "");

                if (!string.IsNullOrEmpty(vCategory))
                {
                    if (StudentTagRanks.ContainsKey(stuRec.ID))
                        classExamScoreTable[stuRec].Add("類組排名",StudentTagRanks[stuRec.ID]);
                    else 
                      classExamScoreTable[stuRec].Add("類組排名","");
                }
            }
            //排序要列印的科目
            SubjectKeys.Sort(new SubjectComparer() { });
          }
    }
}