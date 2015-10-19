namespace DataModel
{
    public class SurveyResult : BaseEntity
    {
        public SurveyQuestion SurveyQuestion { get; set; }
        public Test Test { get; set; }

        public SurveyAnswer Answer { get; set; }
    }

    public enum SurveyAnswer : byte
    {
        Perfect = 1,
        Good = 2,
        Gratifying = 3,
        Bad = 4,
        Worst = 5
    }
}