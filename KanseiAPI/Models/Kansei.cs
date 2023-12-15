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
        public static List<double> tinhTrongSo(List<double> listKanseiPount)
        {
            List<double> result = new List<double>();
            double sum = listKanseiPount.Sum();
            for (int i = 0; i < listKanseiPount.Count; i++)
            {
                if (i == result.Count - 1)
                {
                    result.Add(1 - listKanseiPount.Sum());
                }
                else
                    result.Add((double)listKanseiPount[i] / sum);
            }
            return result;
        }
    }
}
