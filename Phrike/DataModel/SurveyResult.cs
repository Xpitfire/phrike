using System.ComponentModel.DataAnnotations;

namespace DataModel
{
    public class SurveyResult : BaseEntity
    {
        [Required]
        public SurveyQuestion SurveyQuestion { get; set; }
        [Required]
        public Test Test { get; set; }

        [Required]
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