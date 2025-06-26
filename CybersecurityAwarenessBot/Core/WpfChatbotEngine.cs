using System;
using CybersecurityAwarenessBot.Data;

namespace CybersecurityAwarenessBot.Core
{
    /// <summary>
    /// WPF-specific chatbot engine that integrates with the MainWindow
    /// </summary>
    public class WpfChatbotEngine
    {
        // This stores references to the components
        private readonly ResponseDatabase _responseDb;
        private readonly MainWindow _mainWindow;
        
        // This tracks the current conversation topic
        private string _currentTopic = "";
        
        // This tracks the last follow-up question asked
        private string _lastFollowUpQuestion = "";
        
        /// <summary>
        /// Initializes a new instance of the WpfChatbotEngine class
        /// </summary>
        /// <param name="responseDb">The response database</param>
        /// <param name="mainWindow">The main window for display operations</param>
        public WpfChatbotEngine(ResponseDatabase responseDb, MainWindow mainWindow)
        {
            _responseDb = responseDb ?? throw new ArgumentNullException(nameof(responseDb));
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
        }
        
        /// <summary>
        /// Gets a response for the user's input with enhanced task creation support
        /// </summary>
        /// <param name="userInput">The user's input text</param>
        /// <param name="userName">The user's name</param>
        /// <returns>The chatbot's response</returns>
        public string GetResponse(string userInput, string userName)
        {
            try
            {
                // This creates the task creation callback for the ResponseDatabase
                Func<string, string, DateTime?, string> taskCreationCallback = (title, description, reminderDate) =>
                {
                    return _mainWindow.CreateTaskFromActionKeywords(title, description, reminderDate, userName);
                };
                
                // This gets the response from the database with context and task creation support
                string response = _responseDb.GetResponse(userInput, userName, _currentTopic, _lastFollowUpQuestion, taskCreationCallback);
                
                // This updates the current topic based on the input
                UpdateCurrentTopic(userInput);
                
                // This updates the last follow-up question
                _lastFollowUpQuestion = _responseDb.GetLastFollowUpQuestion();
                
                // This prefixes the response with "Bot: " for clarity in the GUI
                return $"Bot: {response}";
            }
            catch (Exception ex)
            {
                // This handles any errors gracefully
                return $"Bot: I'm sorry, I encountered an error processing your request: {ex.Message}";
            }
        }
        
        /// <summary>
        /// Updates the current topic based on the user's input
        /// </summary>
        /// <param name="userInput">The user's input</param>
        private void UpdateCurrentTopic(string userInput)
        {
            // This checks for topic keywords in the user's input
            string lowerInput = userInput.ToLower();
            
            if (lowerInput.Contains("password"))
                _currentTopic = "password";
            else if (lowerInput.Contains("phishing"))
                _currentTopic = "phishing";
            else if (lowerInput.Contains("malware"))
                _currentTopic = "malware";
            else if (lowerInput.Contains("social engineering"))
                _currentTopic = "social engineering";
            else if (lowerInput.Contains("data protection"))
                _currentTopic = "data protection";
            else if (lowerInput.Contains("public wifi") || lowerInput.Contains("wifi"))
                _currentTopic = "public wifi";
            else if (lowerInput.Contains("update"))
                _currentTopic = "updates";
            else if (lowerInput.Contains("backup"))
                _currentTopic = "backup";
            else if (lowerInput.Contains("2fa") || lowerInput.Contains("two factor") || lowerInput.Contains("authentication"))
                _currentTopic = "2fa";
        }
        
        /// <summary>
        /// Gets the current conversation topic
        /// </summary>
        /// <returns>The current topic</returns>
        public string GetCurrentTopic()
        {
            return _currentTopic;
        }
        
        /// <summary>
        /// Resets the conversation state
        /// </summary>
        public void ResetConversation()
        {
            _currentTopic = "";
            _lastFollowUpQuestion = "";
            _responseDb.ResetPostFollowUpState();
            _responseDb.ClearTopicHistory();
        }
    }
} 