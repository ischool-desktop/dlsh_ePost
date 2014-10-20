using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Campus.Rating;

namespace 班級考試成績單
{
    /// <summary>
    /// 排名學生
    /// </summary>
    class RankStudent : IStudent
    {
        private Dictionary<string, decimal?> mScores = new Dictionary<string, decimal?>();

        /// <summary>
        /// 建構式，傳入學生系統編號及成績
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Score"></param>
        public RankStudent(string Id)
        {
            Places = new PlaceCollection();
            this.Id = Id;
        }

        /// <summary>
        /// 取得或設定學生成績
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public decimal? this[string Name]
        {
            get
            {
                return mScores.ContainsKey(Name) ? mScores[Name] : null;
            }
            set
            {
                if (mScores.ContainsKey(Name))
                    mScores[Name] = value;
                else
                    mScores.Add(Name, value);
            }
        }
        #region IStudent 成員

        /// <summary>
        /// 學生系統編號
        /// </summary>
        public string Id { get; private set;}


        /// <summary>
        /// 排名結果
        /// </summary>
        public PlaceCollection Places { get; private set;}
        #endregion
    }
}