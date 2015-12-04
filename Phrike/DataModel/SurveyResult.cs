using System.ComponentModel.DataAnnotations;

namespace DataModel
{
    /// <summary>
    /// The survey answer.
    /// </summary>
    public enum SurveyAnswer : byte
    {
        /// <summary>
        /// The perfect.
        /// </summary>
        Perfect = 1,

        /// <summary>
        /// The good.
        /// </summary>
        Good = 2,

        /// <summary>
        /// The gratifying.
        /// </summary>
        Gratifying = 3,

        /// <summary>
        /// The bad.
        /// </summary>
        Bad = 4,

        /// <summary>
        /// The worst.
        /// </summary>
        Worst = 5
    }

    /// <summary>
    /// The survey result.
    /// </summary>
    public class SurveyResult : BaseEntity
    {
        /// <summary>
        /// Gets or sets the survey question.
        /// </summary>
        [Required]
        public SurveyQuestion SurveyQuestion { get; set; }

        /// <summary>
        /// Gets or sets the test.
        /// </summary>
        [Required]
        public Test Test { get; set; }

        /// <summary>
        /// Gets or sets the answer.
        /// </summary>
        [Required]
        public SurveyAnswer Answer { get; set; }
    }
}