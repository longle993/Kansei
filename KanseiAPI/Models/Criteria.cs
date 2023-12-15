namespace KanseiAPI.Models
{
    public class Criteria
    {
        string criteriaName;
        double totalPoint;

        public Criteria() { }

        public Criteria(string criteriaName, double totalPoint)
        {
            this.criteriaName = criteriaName;
            this.totalPoint = totalPoint;
        }

        public string CriteriaName { get => criteriaName; set => criteriaName = value; }
        public double TotalPoint { get => totalPoint; set => totalPoint = value; }
        public static double sumTotalPoint(List<Kansei> kanseiMultiWeight, string type)
        {
            return kanseiMultiWeight.Where(p => p.Type == type).Sum(p => p.Point);
        }
        public static List<Criteria> averagePoint(List<Criteria> teacher, int quantity1, int quantity2, int quantity3)
        {
            teacher.ForEach(item =>
            {
                switch (item.CriteriaName)
                {
                    case "TL01": item.TotalPoint = item.TotalPoint / quantity1; break;
                    case "TL02": item.TotalPoint = item.TotalPoint / quantity2; break;
                    case "TL03": item.TotalPoint = item.TotalPoint / quantity3; break;

                }
            });
            return teacher;
        }
    }
}
