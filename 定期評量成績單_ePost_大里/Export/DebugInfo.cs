using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Aspose.Cells;
using K12.Data;
using SHSchool.Data;

namespace 班級考試成績單
{
    public static class DebugInfo
    {
        public static void ExportClassInfo(this StudentExamScoreHelper Helper,List<string> Subjects)
        {
            Workbook book = new Workbook();

            book.Worksheets.Clear();

            foreach (ClassRecord ClassRec in Helper.ClassList)
            {
                int SubjectIndex = book.Worksheets.Add();
                int SatIndex = book.Worksheets.Add();

                book.Worksheets[SubjectIndex].Name = ClassRec.Name +"-科目";
                book.Worksheets[SatIndex].Name = ClassRec.Name + "-統計";

                List<SHStudentRecord> ClassStudents = Helper.ClassStudentList.FindAll(x => x.RefClassID.Equals(ClassRec.ID));

                int SubjectRecordIndex = 1;

                book.Worksheets[SubjectIndex].Cells[0, 0].PutValue("學號");
                book.Worksheets[SubjectIndex].Cells[0, 1].PutValue("姓名");
                book.Worksheets[SubjectIndex].Cells[0, 2].PutValue("科目名稱");
                book.Worksheets[SubjectIndex].Cells[0, 3].PutValue("科目級別");
                book.Worksheets[SubjectIndex].Cells[0, 4].PutValue("科目學分");
                book.Worksheets[SubjectIndex].Cells[0, 5].PutValue("科目成績");
                book.Worksheets[SubjectIndex].Cells[0, 6].PutValue("班級排名");
                book.Worksheets[SubjectIndex].Cells[0, 7].PutValue("班級平均");

                book.Worksheets[SatIndex].Cells[0, 0].PutValue("學號");
                book.Worksheets[SatIndex].Cells[0, 1].PutValue("姓名");

                book.Worksheets[SatIndex].Cells[0, 2].PutValue("總分");
                book.Worksheets[SatIndex].Cells[0, 3].PutValue("總分排名");

                book.Worksheets[SatIndex].Cells[0, 4].PutValue("平均");
                book.Worksheets[SatIndex].Cells[0, 5].PutValue("平均排名");

                book.Worksheets[SatIndex].Cells[0, 6].PutValue("加權總分");
                book.Worksheets[SatIndex].Cells[0, 7].PutValue("加權總分排名");

                book.Worksheets[SatIndex].Cells[0, 8].PutValue("加權平均");
                book.Worksheets[SatIndex].Cells[0, 9].PutValue("加權平均排名");

                book.Worksheets[SatIndex].Cells[0, 10].PutValue("班級平均");
                book.Worksheets[SatIndex].Cells[0, 11].PutValue("班級總分");
                book.Worksheets[SatIndex].Cells[0, 12].PutValue("班級加權平均");
                book.Worksheets[SatIndex].Cells[0, 13].PutValue("班級加權總分");

                
                for (int i = 0; i < ClassStudents.Count; i++)
                {
                    if (Helper.mStudentsScore.ContainsKey(ClassStudents[i].ID))
                    {
                        book.Worksheets[SatIndex].Cells[i+1, 0].PutValue(ClassStudents[i].StudentNumber);
                        book.Worksheets[SatIndex].Cells[i + 1, 1].PutValue(ClassStudents[i].Name);

                        if (Helper.mStudentsScore[ClassStudents[i].ID].ContainsKey("總分"))
                            book.Worksheets[SatIndex].Cells[i+1, 2].PutValue(Helper.mStudentsScore[ClassStudents[i].ID]["總分"]);

                        if (Helper.mStudentsScore[ClassStudents[i].ID].ContainsKey("總分排名"))
                            book.Worksheets[SatIndex].Cells[i + 1, 3].PutValue(Helper.mStudentsScore[ClassStudents[i].ID]["總分排名"]);

                        if (Helper.mStudentsScore[ClassStudents[i].ID].ContainsKey("平均"))
                            book.Worksheets[SatIndex].Cells[i + 1, 4].PutValue(Helper.mStudentsScore[ClassStudents[i].ID]["平均"]);

                        if (Helper.mStudentsScore[ClassStudents[i].ID].ContainsKey("平均排名"))
                            book.Worksheets[SatIndex].Cells[i + 1, 5].PutValue(Helper.mStudentsScore[ClassStudents[i].ID]["平均排名"]);

                        if (Helper.mStudentsScore[ClassStudents[i].ID].ContainsKey("加權總分"))
                            book.Worksheets[SatIndex].Cells[i + 1, 6].PutValue(Helper.mStudentsScore[ClassStudents[i].ID]["加權總分"]);

                        if (Helper.mStudentsScore[ClassStudents[i].ID].ContainsKey("加權總分排名"))
                            book.Worksheets[SatIndex].Cells[i + 1, 7].PutValue(Helper.mStudentsScore[ClassStudents[i].ID]["加權總分排名"]);

                        if (Helper.mStudentsScore[ClassStudents[i].ID].ContainsKey("加權平均"))
                            book.Worksheets[SatIndex].Cells[i + 1, 8].PutValue(Helper.mStudentsScore[ClassStudents[i].ID]["加權平均"]);

                        if (Helper.mStudentsScore[ClassStudents[i].ID].ContainsKey("加權平均排名"))
                            book.Worksheets[SatIndex].Cells[i + 1, 9].PutValue(Helper.mStudentsScore[ClassStudents[i].ID]["加權平均排名"]);

                        if (Helper.mStudentsScore[ClassStudents[i].ID].ContainsKey("班級平均"))
                            book.Worksheets[SatIndex].Cells[i + 1, 10].PutValue(Helper.mStudentsScore[ClassStudents[i].ID]["班級平均"]);

                        if (Helper.mStudentsScore[ClassStudents[i].ID].ContainsKey("班級總分"))
                            book.Worksheets[SatIndex].Cells[i + 1, 11].PutValue(Helper.mStudentsScore[ClassStudents[i].ID]["班級總分"]);

                        if (Helper.mStudentsScore[ClassStudents[i].ID].ContainsKey("班級加權平均"))
                            book.Worksheets[SatIndex].Cells[i + 1, 12].PutValue(Helper.mStudentsScore[ClassStudents[i].ID]["班級加權平均"]);

                        if (Helper.mStudentsScore[ClassStudents[i].ID].ContainsKey("班級加權總分"))
                            book.Worksheets[SatIndex].Cells[i + 1, 13].PutValue(Helper.mStudentsScore[ClassStudents[i].ID]["班級加權總分"]);

                        foreach (string Key in Helper.mStudentsScore[ClassStudents[i].ID].Keys)
                        {
                            string[] list = Key.Split(new string[] { "^_^" }, StringSplitOptions.None);

                            if (list.Count() == 3)
                            {
                                book.Worksheets[SubjectIndex].Cells[SubjectRecordIndex, 0].PutValue(ClassStudents[i].StudentNumber);
                                book.Worksheets[SubjectIndex].Cells[SubjectRecordIndex, 1].PutValue(ClassStudents[i].Name);

                                book.Worksheets[SubjectIndex].Cells[SubjectRecordIndex, 2].PutValue(list[0]);
                                book.Worksheets[SubjectIndex].Cells[SubjectRecordIndex, 3].PutValue(list[1]);
                                book.Worksheets[SubjectIndex].Cells[SubjectRecordIndex, 4].PutValue(list[2]);
                                book.Worksheets[SubjectIndex].Cells[SubjectRecordIndex, 5].PutValue(Helper.mStudentsScore[ClassStudents[i].ID][Key]);

                                if (Helper.mStudentsScore[ClassStudents[i].ID].ContainsKey("排名^_^" + Key))
                                    book.Worksheets[SubjectIndex].Cells[SubjectRecordIndex, 6].PutValue(Helper.mStudentsScore[ClassStudents[i].ID]["排名^_^" + Key]);

                                if (Helper.mStudentsScore[ClassStudents[i].ID].ContainsKey("平均^_^" + Key))
                                    book.Worksheets[SubjectIndex].Cells[SubjectRecordIndex, 7].PutValue(Helper.mStudentsScore[ClassStudents[i].ID]["平均^_^" + Key]);

                                SubjectRecordIndex++;
                            }
                        }


                    }
                }

                book.Worksheets[SubjectIndex].AutoFitColumns();
                book.Worksheets[SatIndex].AutoFitColumns();
            }

            List<string> StudentIDs = new List<string>();

            foreach (string TagName in Helper.TagStudents.Keys)
                StudentIDs.AddRange(Helper.TagStudents[TagName].StudentIDs);

            List<SHStudentRecord> Students = SHStudent.SelectByIDs(StudentIDs);

            foreach (string TagName in Helper.TagStudents.Keys)
            {
                int RankSubjectIndex = book.Worksheets.Add();
                int RankSatIndex = book.Worksheets.Add();
                int RankRecordIndex = 1;

                book.Worksheets[RankSubjectIndex].Name = "(分組)"+TagName + "-科目";
                book.Worksheets[RankSubjectIndex].Cells[0, 0].PutValue("學號");
                book.Worksheets[RankSubjectIndex].Cells[0, 1].PutValue("姓名");
                book.Worksheets[RankSubjectIndex].Cells[0, 2].PutValue("科目名稱");
                book.Worksheets[RankSubjectIndex].Cells[0, 3].PutValue("科目級別");
                book.Worksheets[RankSubjectIndex].Cells[0, 4].PutValue("科目學分");
                book.Worksheets[RankSubjectIndex].Cells[0, 5].PutValue("科目成績");
                book.Worksheets[RankSubjectIndex].Cells[0, 6].PutValue("分組排名");

                book.Worksheets[RankSatIndex].Name = "(分組)" + TagName + "-統計";

                book.Worksheets[RankSatIndex].Cells[0, 0].PutValue("學號");
                book.Worksheets[RankSatIndex].Cells[0, 1].PutValue("姓名");

                book.Worksheets[RankSatIndex].Cells[0, 2].PutValue("總分");
                book.Worksheets[RankSatIndex].Cells[0, 3].PutValue("總分排名");

                book.Worksheets[RankSatIndex].Cells[0, 4].PutValue("平均");
                book.Worksheets[RankSatIndex].Cells[0, 5].PutValue("平均排名");

                book.Worksheets[RankSatIndex].Cells[0, 6].PutValue("加權總分");
                book.Worksheets[RankSatIndex].Cells[0, 7].PutValue("加權總分排名");

                book.Worksheets[RankSatIndex].Cells[0, 8].PutValue("加權平均");
                book.Worksheets[RankSatIndex].Cells[0, 9].PutValue("加權平均排名");


                List<string> CurrentStudentIDs = Helper.TagStudents[TagName].StudentIDs;

                for (int i = 0; i < CurrentStudentIDs.Count; i++)
                {
                    string StudentID = CurrentStudentIDs[i];
                    SHStudentRecord Student = Students.Find(x => x.ID.Equals(StudentID));

                    if (Helper.mStudentsScore.ContainsKey(StudentID))
                    {
                        book.Worksheets[RankSatIndex].Cells[i + 1, 0].PutValue(Student.StudentNumber);
                        book.Worksheets[RankSatIndex].Cells[i + 1, 1].PutValue(Student.Name);

                        if (Helper.mStudentsScore[StudentID].ContainsKey("總分"))
                            book.Worksheets[RankSatIndex].Cells[i + 1, 2].PutValue(Helper.mStudentsScore[StudentID]["總分"]);

                        if (Helper.mStudentsScore[StudentID].ContainsKey("分組總分排名"))
                            book.Worksheets[RankSatIndex].Cells[i + 1, 3].PutValue(Helper.mStudentsScore[StudentID]["分組總分排名"]);

                        if (Helper.mStudentsScore[StudentID].ContainsKey("平均"))
                            book.Worksheets[RankSatIndex].Cells[i + 1, 4].PutValue(Helper.mStudentsScore[StudentID]["平均"]);

                        if (Helper.mStudentsScore[StudentID].ContainsKey("分組平均排名"))
                            book.Worksheets[RankSatIndex].Cells[i + 1, 5].PutValue(Helper.mStudentsScore[StudentID]["分組平均排名"]);

                        if (Helper.mStudentsScore[StudentID].ContainsKey("加權總分"))
                            book.Worksheets[RankSatIndex].Cells[i + 1, 6].PutValue(Helper.mStudentsScore[StudentID]["加權總分"]);

                        if (Helper.mStudentsScore[StudentID].ContainsKey("分組加權總分排名"))
                            book.Worksheets[RankSatIndex].Cells[i + 1, 7].PutValue(Helper.mStudentsScore[StudentID]["分組加權總分排名"]);

                        if (Helper.mStudentsScore[StudentID].ContainsKey("加權平均"))
                            book.Worksheets[RankSatIndex].Cells[i + 1, 8].PutValue(Helper.mStudentsScore[StudentID]["加權平均"]);

                        if (Helper.mStudentsScore[StudentID].ContainsKey("分組加權平均排名"))
                            book.Worksheets[RankSatIndex].Cells[i + 1, 9].PutValue(Helper.mStudentsScore[StudentID]["分組加權平均排名"]);


                        foreach (string Key in Helper.mStudentsScore[StudentID].Keys)
                        {
                            string[] list = Key.Split(new string[] { "^_^" }, StringSplitOptions.None);

                            if (list.Count() == 3)
                            {
                                book.Worksheets[RankSubjectIndex].Cells[RankRecordIndex, 0].PutValue(Student.StudentNumber);
                                book.Worksheets[RankSubjectIndex].Cells[RankRecordIndex, 1].PutValue(Student.Name);

                                book.Worksheets[RankSubjectIndex].Cells[RankRecordIndex, 2].PutValue(list[0]);
                                book.Worksheets[RankSubjectIndex].Cells[RankRecordIndex, 3].PutValue(list[1]);
                                book.Worksheets[RankSubjectIndex].Cells[RankRecordIndex, 4].PutValue(list[2]);
                                book.Worksheets[RankSubjectIndex].Cells[RankRecordIndex, 5].PutValue(Helper.mStudentsScore[StudentID][Key]);

                                if (Helper.mStudentsScore[StudentID].ContainsKey("分組排名^_^" + Key))
                                    book.Worksheets[RankSubjectIndex].Cells[RankRecordIndex, 6].PutValue(Helper.mStudentsScore[StudentID]["排名^_^" + Key]);

                                RankRecordIndex++;
                            }
                        }

                        book.Worksheets[RankSubjectIndex].AutoFitColumns();
                        book.Worksheets[RankSatIndex].AutoFitColumns();
                    }
                }
            }

            try
            {
               book.Save(Application.StartupPath + "\\個人考試成績單(詳細).xls", Aspose.Cells.FileFormatType.Excel2003);
            }
            catch (Exception ex)
            {

            }
        }
    }
}