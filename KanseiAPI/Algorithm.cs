using KanseiAPI.NewModel;
using System.Collections.Generic;

namespace KanseiAPI
{
    public class Algorithm
    {
        private List<Teacher> mTeachers;
        private List<Evaluation> mStudentPointsList;
        private List<Evaluation> mStudentPoints;
        private List<Criteria> mCriteria;
        private List<Kansei> mListKansei;
        private Dictionary<string, string> mMapResult;

        /*  
            students: Danh sách điểm đánh giá
            listKansei: Danh sách điểm lúc tư vấn sinh viên chọn
         */
        public Algorithm(List<Evaluation> students, List<Criteria> criteria, List<Teacher> teachers, List<Kansei> listKansei)
        {
            this.mTeachers = teachers;
            this.mCriteria = criteria;
            this.mStudentPointsList = students;
            this.mListKansei = listKansei;
            this.mStudentPoints = new List<Evaluation>();
            this.mMapResult = new Dictionary<string, string>();
        }

        public Dictionary<string, string> MMapResult { get => mMapResult; }

        public void execute()
        {

            List<double> w = new AHP(mListKansei.Select(p => p.Point).ToList()).Cal_mCompareTable();

            for (int i = 0; i < mTeachers.Count; i++)
            {
                mStudentPoints.Add(new TOPSIS(mStudentPointsList, w, mCriteria).execute());
            }

            List<double> points = new List<double>();

            mStudentPoints.ForEach(evaluation =>
            {
                evaluation.ListKansei.ForEach(kansei => points.Add(kansei.Point));
            });

            List<double> ranking = new AHP(points).Cal_mCompareTable();

            for(int i = 0; i< mTeachers.Count; i++)
            {
                mMapResult.Add(mTeachers[i].Name, ranking[i].ToString());
            }
        }
    }
}
