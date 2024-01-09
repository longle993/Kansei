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
            List<double> w = new List<double>();
            List<double> criteriaPoint = new List<double>();

            for (int i = 0; i < mCriteria.Count; i++)
            {
                List<double> kanseiPointWithCriteria = mListKansei.Where(w => w.Type == mCriteria[i].Id).Select(p => p.Point).ToList();
                List<double> temp = new AHP(kanseiPointWithCriteria).Cal_mCompareTable();
                double valTemp = 0.0f;
                for (int j = 0; j < temp.Count; j++)
                    valTemp += temp[j] * kanseiPointWithCriteria[j] / temp.Count;
                criteriaPoint.Add(valTemp);
            }

            w = new AHP(criteriaPoint).Cal_mCompareTable();

            for (int i = 0; i < mTeachers.Count; i++)
            {
                mStudentPoints.Add(new TOPSIS(mStudentPointsList.Where(p => p.TeacherId == mTeachers[i].Id).ToList(),
                                   w, mCriteria).execute());
            }

            List<List<double>> pointsCriteria = new List<List<double>>();

            for (int i = 0; i < mStudentPoints.Count; i++)
            {
                List<double> points = new List<double>();
                foreach (var criteria in mCriteria)
                {
                    List<double> evalPoint = mStudentPoints[i].ListKansei.Where(kansei => kansei.Type == criteria.Id).ToList().Select(k => k.Point).ToList();
                    List<double> temp = new AHP(evalPoint).Cal_mCompareTable();
                    double average = 0;
                    for (int j = 0; j < evalPoint.Count; j++)
                        average += evalPoint[j] * temp[j] / evalPoint.Count;
                    points.Add(average);
                }
                pointsCriteria.Add(points);
            }

            List<List<double>> finalTeachersCriteria = new List<List<double>>();

            for (int i = 0; i < mCriteria.Count; i++)
            {
                List<double> teachersPoint = new List<double>();
                for (int j = 0; j < pointsCriteria.Count; j++)
                    teachersPoint.Add(pointsCriteria[j][i]);
                finalTeachersCriteria.Add(new AHP(teachersPoint).Cal_mCompareTable());
            }

            // this is the final ranking point
            double[] teachersFinalPoint = new double[finalTeachersCriteria.Count];

            for (int i = 0; i < finalTeachersCriteria.Count; i++)
                for (int j = 0; j < w.Count; j++)
                    teachersFinalPoint[i] += finalTeachersCriteria[i][j] * w[j];

            // Multiply matrix finalCriteriasPoint with Weight (w array)

            //List<double> ranking = new AHP(points).Cal_mCompareTable();

            //for (int i = 0; i < mTeachers.Count; i++)
            //{
            //    mMapResult.Add(mTeachers[i].Name, ranking[i].ToString());
            //}
        }
    }
}
