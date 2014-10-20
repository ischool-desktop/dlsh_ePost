using System.Collections.Generic;
using SHSchool.Data;

namespace 班級考試成績單
{
    public class TagStudents
    {
        public List<string> StudentIDs { get; set; }

        public TagStudents()
        {
            StudentIDs = new List<string>();
        }
    }
}