namespace KanseiAPI.Models
{
    public class Teacher
    {
        private string id;
        private string name;
        private List<Criteria> criteria;
        public Teacher() { }

        public Teacher(string id,string name, List<Criteria> criteria)
        {
            this.id = id;
            this.name = name;
            this.criteria = criteria;
        }

        public string Name { get => name; set => name = value; }
        public List<Criteria> Criteria { get => criteria; set => criteria = value; }
        public string Id { get => id; set => id = value; }
    }
}
