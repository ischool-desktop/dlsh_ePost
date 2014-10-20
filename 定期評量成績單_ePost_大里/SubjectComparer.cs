using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 班級考試成績單
{
    public class SubjectComparer : IComparer<string>
    {
        private static int SortBySubjectName(string a, string b)
        {
            string a1 = a.Length > 0 ? a.Substring(0, 1) : "";
            string b1 = b.Length > 0 ? b.Substring(0, 1) : "";
            #region 第一個字一樣的時候
            if (a1 == b1)
            {
                if (a.Length > 1 && b.Length > 1)
                    return SortBySubjectName(a.Substring(1), b.Substring(1));
                else
                    return a.Length.CompareTo(b.Length);
            }
            #endregion
            #region 第一個字不同，分別取得在設定順序中的數字，如果都不在設定順序中就用單純字串比較
            int ai = getIntForSubject(a1), bi = getIntForSubject(b1);
            if (ai > 0 || bi > 0)
                return ai.CompareTo(bi);
            else
                return a1.CompareTo(b1);
            #endregion
        }

        private static int getIntForSubject(string a1)
        {
            List<string> list = new List<string>();
            list.AddRange(new string[] { "國", "英", "數", "物", "化", "生", "基", "歷", "地", "公", "文", "礎", "世" });

            int x = list.IndexOf(a1);
            if (x < 0)
                return list.Count;
            else
                return x;
        }

        #region IComparer<string> Members

        public int Compare(string a, string b)
        {
            return SortBySubjectName(a, b);
        }

        #endregion
    }
}