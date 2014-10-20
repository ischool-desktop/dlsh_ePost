using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.Rating;

namespace 班級考試成績單
{
    /// <summary>
    /// 成績解析器
    /// </summary>
    class ScoreParser : IScoreParser<RankStudent>
    {
        public ScoreParser(string Name)
        {
            this.Name = Name;
        }

        #region IScoreParser<RankStudent> 成員

        public decimal? GetScore(RankStudent student)
        {
            return student[Name];
        }

        public string Name { get; private set; }
        #endregion
    }
}