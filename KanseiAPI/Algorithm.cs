using KanseiAPI.Models;

namespace KanseiAPI
{
    public class Algorithm
    {
        private List<Teacher> mTeachers;
        private List<Student> mStudentsPoint;
        private List<Criteria> mCriteria;
        private List<Kansei> mListKansei;
        private List<List<double>> listType;
        private static readonly List<string> type = new List<string>() { "TL01", "TL02", "TL03", "TL04" };
        public Algorithm(List<Student> students, List<Criteria> criteria,
            List<Teacher> teachers, List<Kansei> listKansei)
        {
            this.mTeachers = teachers;
            this.mCriteria = criteria;
            this.mStudentsPoint = students;
            this.mListKansei = listKansei;
            listType = new List<List<double>>();
        }

        private List<double> Cal_mCompareTable(List<double> criteria)
        {
            List<double> result = new List<double>();
            double[,] mCompareTable = new double[criteria.Count, criteria.Count];
            for (int i = 0; i < criteria.Count; ++i)
                for (int j = 0; j < criteria.Count; ++j)
                {
                    if (i == j)
                        mCompareTable[i, j] = 1;
                    else if (i < j)
                    {

                        double val = (float)criteria[i] / criteria[j],
                               exten = (criteria[i] % criteria[j]) / (float)criteria[j];

                        if (exten < 0.5f)
                        {
                            val = (int)val;
                        }
                        else
                        {
                            val = (int)val + 1.0f;
                        }

                        mCompareTable[i, j] = val;
                    }
                    else
                    {
                        mCompareTable[i, j] = 1 / mCompareTable[j, i];
                    }
                }
            for (int i = 0; i < mCompareTable.GetLength(0); i++)
            {
                double ava = 0;
                for (int j = 0; j < mCompareTable.GetLength(1); j++)
                {
                    ava += mCompareTable[i, j] / mCompareTable.GetLength(1);
                }
                result.Add(ava);
            }
            return result;
        }

        private void execute()
        {
            for (int i = 0; i < type.Count; i++)
                listType[i] = Kansei.tinhTrongSo(mListKansei.Where(p => p.Type == type[i]).Select(p => p.Point).ToList());

            List<double> kanseiCriteria = new List<double>();
            List<double> w = new List<double>();
            List<double> criteriaFinal = new List<double>();
            int typeIndex = 0;

            listType.ForEach(type =>
            {
                double sum = 0;
                for (int i = 0; i < type.Count; i++)
                    sum += type[i];
                for (int i = 0; i < type.Count; i++)
                {
                    double temp = sum / type.Count;
                    kanseiCriteria[i] = temp * mListKansei[i].Point;
                }
                kanseiCriteria.ForEach(kansei => criteriaFinal[typeIndex] += kansei / kanseiCriteria.Count);
            });

            w = Cal_mCompareTable(criteriaFinal);

            for (int i = 0; i < mTeachers.Count; i++)
            {
                mStudentsPoint.Add(new TOPSIS(mStudentsPoint, mCriteria, w).execute());
            }

            List<double> ranking = new AHP(w, mCriteria, mStudentsPoint).execute();
        }
    }
}
