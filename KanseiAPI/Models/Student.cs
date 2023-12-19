namespace KanseiAPI.Models
{
    public class Student : IComparable
    {
        public String name;
        public List<int> PointForCriteria;
        public List<double> Standardized;
        public double mCC;

        public Student()
        {
            name = null;
            PointForCriteria = new List<int>();
            Standardized = new List<double>();
            mCC = 0.0f;
        }

        public int CompareTo(object obj)
        {
            return this.mCC.CompareTo(((Student)obj).mCC);
        }
    }
}
