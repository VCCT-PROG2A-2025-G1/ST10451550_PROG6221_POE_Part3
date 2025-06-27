using System;
using System.Collections.Generic;
using System.Linq;
using CybersecurityAwarenessBot.Data;

//--------------------------------------------------------------------------------------------------------------------------------

namespace CybersecurityAwarenessBot.Core
{
    /// <summary>
    /// Manages the cybersecurity quiz game functionality
    /// </summary>
    public class QuizManager
    {
        // This stores all available quiz questions
        private readonly List<QuizQuestion> _allQuestions;
        
        // This stores the current quiz questions
        private List<QuizQuestion> _currentQuizQuestions;
        
        // This tracks the current question index
        private int _currentQuestionIndex;
        
        // This tracks the user's score
        private int _score;
        
        // This tracks the total questions in current quiz
        private const int QuestionsPerQuiz = 10;
        
        // This creates a random number generator
        private readonly Random _random;
        
        /// <summary>
        /// Gets the current question
        /// </summary>
        public QuizQuestion CurrentQuestion 
        { 
            get 
            { 
                if (_currentQuizQuestions != null && _currentQuestionIndex < _currentQuizQuestions.Count)
                    return _currentQuizQuestions[_currentQuestionIndex];
                return null;
            } 
        }
        
        /// <summary>
        /// Gets the current question number (1-based)
        /// </summary>
        public int CurrentQuestionNumber => _currentQuestionIndex + 1;
        
        /// <summary>
        /// Gets the total number of questions in the current quiz
        /// </summary>
        public int TotalQuestions => QuestionsPerQuiz;
        
        /// <summary>
        /// Gets the current score
        /// </summary>
        public int Score => _score;
        
        /// <summary>
        /// Gets whether the quiz is complete
        /// </summary>
        public bool IsQuizComplete => _currentQuizQuestions != null && _currentQuestionIndex >= _currentQuizQuestions.Count;
        
        /// <summary>
        /// Initializes a new instance of the QuizManager class
        /// </summary>
        public QuizManager()
        {
            _random = new Random();
            _allQuestions = CreateQuestionDatabase();
        }
        
        /// <summary>
        /// Starts a new quiz with random questions
        /// </summary>
        public void StartNewQuiz()
        {
            // This selects 10 random questions from the database
            _currentQuizQuestions = _allQuestions.OrderBy(x => _random.Next()).Take(QuestionsPerQuiz).ToList();
            _currentQuestionIndex = 0;
            _score = 0;
        }
        
        /// <summary>
        /// Submits an answer and returns whether it was correct
        /// </summary>
        /// <param name="answerIndex">The index of the selected answer</param>
        /// <returns>True if correct, false otherwise</returns>
        public bool SubmitAnswer(int answerIndex)
        {
            if (CurrentQuestion == null) return false;
            
            bool isCorrect = answerIndex == CurrentQuestion.CorrectAnswerIndex;
            if (isCorrect)
            {
                _score++;
            }
            
            return isCorrect;
        }
        
        /// <summary>
        /// Moves to the next question
        /// </summary>
        public void NextQuestion()
        {
            _currentQuestionIndex++;
        }
        
        /// <summary>
        /// Gets the final score percentage
        /// </summary>
        /// <returns>Score as a percentage</returns>
        public double GetScorePercentage()
        {
            return (double)_score / QuestionsPerQuiz * 100;
        }
        
        /// <summary>
        /// Gets the performance description based on score
        /// </summary>
        /// <returns>Performance feedback message</returns>
        public string GetPerformanceDescription()
        {
            double percentage = GetScorePercentage();
            
            if (percentage >= 90)
                return "Great job! You're a cybersecurity pro!";
            else if (percentage >= 70)
                return "Good work! Keep learning to stay safe online!";
            else
                return "Keep studying! Cybersecurity knowledge is important.";
        }
        
        /// <summary>
        /// Creates the database of quiz questions
        /// </summary>
        /// <returns>List of quiz questions</returns>
        private List<QuizQuestion> CreateQuestionDatabase()
        {
            return new List<QuizQuestion>
            {
                // Password Security Questions
                new QuizQuestion("A strong password should contain at least 12 characters with a mix of letters, numbers, and symbols.", 
                    true, "Strong passwords should be at least 12 characters long and include uppercase, lowercase, numbers, and special characters."),
                
                new QuizQuestion("Which of the following is the most secure password?", 
                    new List<string> { "password123", "P@ssw0rd2024!", "12345678", "qwerty" }, 
                    1, "P@ssw0rd2024! contains uppercase, lowercase, numbers, and special characters, making it the strongest option."),
                
                new QuizQuestion("You should use the same password for multiple accounts to make it easier to remember.", 
                    false, "Using unique passwords for each account prevents hackers from accessing multiple accounts if one is compromised."),
                
                new QuizQuestion("What is the primary benefit of using a password manager?", 
                    new List<string> { "It makes passwords shorter", "It generates and stores unique passwords", "It shares passwords with friends", "It eliminates the need for passwords" }, 
                    1, "Password managers generate strong, unique passwords for each account and store them securely."),
                
                // Phishing Questions
                new QuizQuestion("Phishing emails always come from obviously fake email addresses.", 
                    false, "Sophisticated phishing emails can appear to come from legitimate sources and may be very convincing."),
                
                new QuizQuestion("What should you do if you receive a suspicious email asking for personal information?", 
                    new List<string> { "Reply with the information requested", "Click the links to verify", "Delete the email and contact the company directly", "Forward it to friends" }, 
                    2, "Never provide personal information through email. Contact the company directly using their official website or phone number."),
                
                new QuizQuestion("Legitimate companies will ask for your password via email.", 
                    false, "Legitimate companies will never ask for passwords, PINs, or sensitive information via email."),
                
                new QuizQuestion("Which of these is a common sign of a phishing email?", 
                    new List<string> { "Professional formatting", "Correct spelling", "Urgent requests for action", "Company logo" }, 
                    2, "Phishing emails often create false urgency to pressure victims into acting without thinking carefully."),
                
                // Malware Questions
                new QuizQuestion("Antivirus software should be updated regularly to protect against new threats.", 
                    true, "Regular updates ensure antivirus software can detect and block the latest malware threats."),
                
                new QuizQuestion("What is ransomware?", 
                    new List<string> { "Free software", "Malware that encrypts files for ransom", "A type of firewall", "An email client" }, 
                    1, "Ransomware is malicious software that encrypts your files and demands payment for the decryption key."),
                
                new QuizQuestion("It's safe to download software from any website.", 
                    false, "Only download software from trusted, official sources to avoid malware infections."),
                
                new QuizQuestion("Which of these can help prevent malware infections?", 
                    new List<string> { "Clicking on pop-up ads", "Installing software from unknown sources", "Keeping software updated", "Disabling antivirus" }, 
                    2, "Keeping software updated patches security vulnerabilities that malware could exploit."),
                
                // Social Engineering Questions
                new QuizQuestion("Social engineering attacks target technology vulnerabilities rather than human psychology.", 
                    false, "Social engineering attacks exploit human psychology and trust rather than technical vulnerabilities."),
                
                new QuizQuestion("What should you do if someone calls claiming to be from IT and asks for your password?", 
                    new List<string> { "Give them the password immediately", "Hang up and call IT directly", "Ask for their employee ID", "Change your password first" }, 
                    1, "Legitimate IT staff will never ask for your password. Verify identity through official channels."),
                
                new QuizQuestion("Attackers may use personal information from social media for social engineering.", 
                    true, "Information shared on social media can be used to make social engineering attacks more convincing and targeted."),
                
                // Two-Factor Authentication Questions
                new QuizQuestion("Two-factor authentication (2FA) significantly improves account security.", 
                    true, "2FA adds an extra layer of security by requiring something you know (password) and something you have (phone)."),
                
                new QuizQuestion("Which is the most secure form of two-factor authentication?", 
                    new List<string> { "SMS text messages", "Email codes", "Authenticator apps", "Security questions" }, 
                    2, "Authenticator apps are more secure than SMS because they're not vulnerable to SIM swapping attacks."),
                
                new QuizQuestion("You only need 2FA on your most important accounts.", 
                    false, "2FA should be enabled on all accounts that support it, especially email, banking, and social media."),
                
                // Public WiFi Questions
                new QuizQuestion("Public WiFi networks are always secure.", 
                    false, "Public WiFi networks are often unsecured and can be dangerous for sensitive activities."),
                
                new QuizQuestion("What should you avoid doing on public WiFi?", 
                    new List<string> { "Reading news", "Checking weather", "Online banking", "Browsing social media" }, 
                    2, "Avoid accessing sensitive accounts like banking on public WiFi as data can be intercepted."),
                
                new QuizQuestion("A VPN (Virtual Private Network) can help protect your data on public WiFi.", 
                    true, "VPNs encrypt your internet connection, protecting your data from being intercepted on public networks."),
                
                // Software Updates Questions
                new QuizQuestion("Software updates often contain important security fixes.", 
                    true, "Updates frequently include patches for security vulnerabilities that could be exploited by attackers."),
                
                new QuizQuestion("What's the best practice for software updates?", 
                    new List<string> { "Never update software", "Only update once a year", "Enable automatic updates when possible", "Wait for others to test updates first" }, 
                    2, "Automatic updates ensure you receive critical security patches as soon as they're available."),
                
                new QuizQuestion("It's safe to ignore security updates if your computer seems to be working fine.", 
                    false, "Security vulnerabilities may not be immediately obvious, but they can be exploited by attackers."),
                
                // Data Backup Questions
                new QuizQuestion("The 3-2-1 backup rule means having 3 copies of data on 2 different media with 1 offsite.", 
                    true, "The 3-2-1 rule is a best practice: 3 copies, 2 different storage types, 1 stored offsite."),
                
                new QuizQuestion("How often should you test your backups?", 
                    new List<string> { "Never", "Once a year", "Regularly", "Only when needed" }, 
                    2, "Regular testing ensures your backups are working properly and can be restored when needed."),
                
                new QuizQuestion("Cloud storage alone is sufficient for backup protection.", 
                    false, "While cloud storage is helpful, it's best to have multiple backup methods following the 3-2-1 rule."),
                
                // Privacy and Data Protection Questions
                new QuizQuestion("You should regularly review privacy settings on your social media accounts.", 
                    true, "Regular reviews help ensure you're only sharing information with intended audiences and maintaining your privacy."),
                
                new QuizQuestion("What information should you avoid sharing on social media?", 
                    new List<string> { "Your favorite movies", "Your vacation photos", "Your full birthdate and address", "Your hobbies" }, 
                    2, "Personal details like full birthdate, address, and location information can be used for identity theft."),
                
                new QuizQuestion("It's safe to click 'Accept All Cookies' on every website.", 
                    false, "Accepting all cookies can allow extensive tracking of your online activities across websites.")
            };
        }
    }
}

//--------------------------------------------------End of File--------------------------------------------------