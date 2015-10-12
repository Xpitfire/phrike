namespace DataModel
{
    public class SurveyResult : BaseEntity
    {
        public SurveyQuestion SurveyQuestion { get; set; }
        public int Answer { get; set; }
    }
}