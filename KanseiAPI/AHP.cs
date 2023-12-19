using KanseiAPI.Models;

namespace KanseiAPI
{
    public class AHP
    {
        List<double> mWeights;
        List<Criteria> mCriteria;
        List<Student> mStudentsPoint;
        double[,] mCompareTable;
        List<List<double>> mTeachersCriteria;
        List<double> finalResult;

        public AHP(List<double> w, List<Criteria> criteria, List<Student> studentsPoint)
        {
            this.mWeights = w;
            this.mCriteria = criteria;
            this.mStudentsPoint = studentsPoint;
            this.mCompareTable = new double[studentsPoint.Count, studentsPoint.Count];
            this.mTeachersCriteria = new List<List<double>>();
            this.finalResult = new List<double>();
        }

        private List<double> Cal_mCompareTable(int criteriaIndex)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < this.mCompareTable.GetLength(0); ++i)
                for (int j = 0; j < this.mCompareTable.GetLength(1); ++j)
                {
                    if (i == j)
                        mCompareTable[i, j] = 1;
                    else if (i < j)
                    {

                        double val = (float)mStudentsPoint[i].PointForCriteria[criteriaIndex] / mStudentsPoint[j].PointForCriteria[criteriaIndex],
                               exten = (mStudentsPoint[i].PointForCriteria[criteriaIndex] % mStudentsPoint[j].PointForCriteria[criteriaIndex]) / (float)mStudentsPoint[j].PointForCriteria[criteriaIndex];

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

        public List<double> execute()
        {
            for (int i = 0; i < mCriteria.Count; i++)
            {
                mTeachersCriteria.Add(Cal_mCompareTable(i));
            }

            mTeachersCriteria.ForEach(criteria =>
            {
                double finalCriteria = 0.00f;
                for (int i = 0; i < criteria.Count; i++)
                {
                    finalCriteria += criteria[i] * mWeights[i];
                }
                finalResult.Add(finalCriteria);
            });

            finalResult.Sort((x, y) => y.CompareTo(x));

            return finalResult;
        }
    }
}
