using System.Collections.Generic;

//------------------------------------------------------------------------------------------------------------------------

namespace CybersecurityAwarenessBot.Data
{
    /// <summary>
    /// Represents a cybersecurity quiz question
    /// </summary>
    public class QuizQuestion
    {
        /// <summary>
        /// Gets or sets the question text
        /// </summary>
        public string Question { get; set; }
        
        /// <summary>
        /// Gets or sets the list of answer options
        /// </summary>
        public List<string> Options { get; set; }
        
        /// <summary>
        /// Gets or sets the index of the correct answer (0-based)
        /// </summary>
        public int CorrectAnswerIndex { get; set; }
        
        /// <summary>
        /// Gets or sets the explanation for the correct answer
        /// </summary>
        public string Explanation { get; set; }
        
        /// <summary>
        /// Gets or sets whether this is a true/false question
        /// </summary>
        public bool IsTrueFalse { get; set; }
        
        /// <summary>
        /// Gets the correct answer text
        /// </summary>
        public string CorrectAnswer
        {
            get
            {
                if (Options != null && CorrectAnswerIndex >= 0 && CorrectAnswerIndex < Options.Count)
                {
                    return Options[CorrectAnswerIndex];
                }
                return "";
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the QuizQuestion class
        /// </summary>
        public QuizQuestion()
        {
            Options = new List<string>();
        }
        
        /// <summary>
        /// Initializes a new instance of the QuizQuestion class for multiple choice
        /// </summary>
        /// <param name="question">The question text</param>
        /// <param name="options">The answer options</param>
        /// <param name="correctIndex">The index of the correct answer</param>
        /// <param name="explanation">The explanation for the correct answer</param>
        public QuizQuestion(string question, List<string> options, int correctIndex, string explanation)
        {
            Question = question;
            Options = options ?? new List<string>();
            CorrectAnswerIndex = correctIndex;
            Explanation = explanation;
            IsTrueFalse = false;
        }
        
        /// <summary>
        /// Initializes a new instance of the QuizQuestion class for true/false
        /// </summary>
        /// <param name="question">The question text</param>
        /// <param name="isTrue">Whether the correct answer is true</param>
        /// <param name="explanation">The explanation for the correct answer</param>
        public QuizQuestion(string question, bool isTrue, string explanation)
        {
            Question = question;
            Options = new List<string> { "True", "False" };
            CorrectAnswerIndex = isTrue ? 0 : 1;
            Explanation = explanation;
            IsTrueFalse = true;
        }
    }
}

//--------------------------------------------------End of File--------------------------------------------------