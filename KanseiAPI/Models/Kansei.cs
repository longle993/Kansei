namespace KanseiAPI.Models
{
    public class Kansei
    {
        string kanseiWord;
        double point;
        string type;
        public Kansei(string kanseiWord, double point, string type)
        {
            this.kanseiWord = kanseiWord;
            this.point = point;
            this.type = type;
        }
        public string Type { get => type; set => type = value; }
        public string KanseiWord { get => kanseiWord; set => kanseiWord = value; }
        public double Point { get => point; set => point = value; }
        public static List<double> tinhTrongSo(List<double> listKanseiPoint)
        {
            List<double> result = new List<double>();
            double sum = listKanseiPoint.Sum();
            for (int i = 0; i < listKanseiPoint.Count; i++)
            {
                if (i == result.Count - 1)
                {
                    result.Add(1 - listKanseiPoint.Sum());
                }
                else
                    result.Add((double)listKanseiPoint[i] / sum);
            }
            return result;
        }
    }
}
