using KanseiAPI.NewModel;

namespace KanseiAPI
{
    public class Algorithm
    {
        private List<Teacher> mTeachers;
        private List<Evaluation> mStudentPoints;
        private List<Criteria> mCriteria;
        private List<Kansei> mListKansei;

        public Algorithm(List<Evaluation> students, List<Criteria> criteria, List<Teacher> teachers, List<Kansei> listKansei)
        {
            this.mTeachers = teachers;
            this.mCriteria = criteria;
            this.mStudentPoints = students;
            this.mListKansei = listKansei;
        }

        public void execute()
        {

        }
    }
}
