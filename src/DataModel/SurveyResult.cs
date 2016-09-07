using System;
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
        Gratified = 3,

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
    /// The survey answer helper.
    /// </summary>
    public static class SurveyAnswerHelper
    {
        /// <summary>
        /// The to string.
        /// </summary>
        /// <param name="answer">
        /// The answer.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public static string ToString(SurveyAnswer answer)
        {
            switch (answer)
            {
                case SurveyAnswer.Perfect:
                    return "Perfekt";
                case SurveyAnswer.Good:
                    return "Gut";
                case SurveyAnswer.Gratified:
                    return "Befriedigend";
                case SurveyAnswer.Bad:
                    return "Schlecht";
                case SurveyAnswer.Worst:
                    return "Nicht Zufriedenstellend";
            }
            throw new ArgumentException("Unsupported answer result!");
        }

        public static SurveyAnswer ToEnum(string answer)
        {
            switch (answer)
            {
                case "Perfekt":
                case "Perfect":
                    return SurveyAnswer.Perfect;
                case "Gut":
                case "Good":
                    return SurveyAnswer.Good;
                case "Befriedigend":
                case "Gratified":
                    return SurveyAnswer.Gratified;
                case "Schlecht":
                case "Bad":
                    return SurveyAnswer.Bad;
                case "Nicht Zufriedenstellend":
                case "Worst":
                    return SurveyAnswer.Worst;
            }
            throw new ArgumentException("Unsupported answer result!");
        }

        public static SurveyAnswer ToEnum(int answer)
        {
            switch (answer)
            {
                case 1:
                    return SurveyAnswer.Perfect;
                case 2:
                    return SurveyAnswer.Good;
                case 3:
                    return SurveyAnswer.Gratified;
                case 4:
                    return SurveyAnswer.Bad;
                case 0:
                    return SurveyAnswer.Worst;
            }
            throw new ArgumentException("Unsupported answer result!");
        }
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