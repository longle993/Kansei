using KanseiAPI.Models;

namespace KanseiAPI
{
    // Cal TOPSIS for a teacher
    public class TOPSIS
    {
        private List<Student> mStudents;
        private List<Criteria> mCriteria;
        private double[] mAStar;
        private double[] mAMinus;
        private double[] mSStar;
        private double[] mSMinus;
        private List<double> mWeights;
        public TOPSIS(List<Student> students, List<Criteria> criteria, List<double> w)
        {
            this.mStudents = students;
            this.mCriteria = criteria;
            mAStar = new double[criteria.Count];
            mAMinus = new double[criteria.Count];
            mSStar = new double[students.Count];
            mSMinus = new double[students.Count];
            mWeights = w;
        }

        private void cal_StandardizedMatrix()
        {
            for (int i = 0; i < mCriteria.Count; ++i)
            {
                double sum = 0.0f;
                for (int j = 0; j < mStudents.Count; ++j)
                {
                    sum += mStudents[j].PointForCriteria[i] * mStudents[j].PointForCriteria[i];
                }

                sum = Math.Sqrt(sum);

                for (int j = 0; j < mStudents.Count; ++j)
                {
                    mStudents[j].Standardized[i] = mStudents[j].PointForCriteria[i] / sum;
                }
            }
        }

        private void cal_AStarAndAMinus()
        {
            for (int i = 0; i < mCriteria.Count; ++i)
                for (int j = 0; j < this.mStudents.Count; ++j)
                {
                    double valueWithWeight = this.mStudents[j].Standardized[i] * this.mWeights[i];
                    if (j == 0)
                    {
                        this.mAStar[i] = valueWithWeight;
                        this.mAMinus[i] = valueWithWeight;
                    }
                    else
                    {
                        if (valueWithWeight > this.mAStar[i])
                            this.mAStar[i] = valueWithWeight;
                        if (valueWithWeight < this.mAMinus[i])
                            this.mAMinus[i] = valueWithWeight;
                    }
                }
        }

        private void cal_SStarAndSMinus()
        {
            for (int i = 0; i < this.mStudents.Count; ++i)
            {
                for (int j = 0; j < mCriteria.Count; ++j)
                {
                    double val = this.mStudents[i].Standardized[j] * this.mWeights[j] - this.mAStar[j];
                    this.mSStar[i] += val * val;

                    val = this.mStudents[i].Standardized[j] * this.mWeights[j] - this.mAMinus[j];
                    this.mSMinus[i] += val * val;
                }

                this.mSStar[i] = Math.Sqrt(this.mSStar[i]);
                this.mSMinus[i] = Math.Sqrt(this.mSMinus[i]);
            }
        }

        private void cal_mCC()
        {
            for (int i = 0; i < this.mStudents.Count; ++i)
            {
                this.mStudents[i].mCC = this.mSMinus[i] / (this.mSMinus[i] + this.mSStar[i]);
            }
        }

        public Student execute()
        {
            cal_StandardizedMatrix();
            cal_AStarAndAMinus();
            cal_SStarAndSMinus();
            cal_mCC();
            mStudents.Sort();
            return mStudents[mStudents.Count / 2];
        }
    }
}
