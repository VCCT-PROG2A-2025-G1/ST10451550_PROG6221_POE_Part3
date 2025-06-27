using System;
using System.Collections.Generic;
using CybersecurityAwarenessBot.UI;
using CybersecurityAwarenessBot.Data;
using CybersecurityAwarenessBot.Audio;

//------------------------------------------------------------------------------------------------------------------------

namespace CybersecurityAwarenessBot.Core
{
    /// <summary>
    /// Core engine for the Cybersecurity Awareness Chatbot
    /// </summary>
    public class ChatbotEngine
    {
        // This stores references to the dependent components
        private readonly UserInterface _ui;
        private readonly ResponseDatabase _responseDb;
        private readonly AudioManager _audioManager;
        
        // This keeps track of the user's name
        private string _userName;
        
        // This tracks the current conversation topic
        private string _currentTopic = "";
        
        // This tracks the last follow-up question asked
        private string _lastFollowUpQuestion = "";
        
        // This defines available topics for the help menu
        private readonly string[] _availableTopics = {
            "passwords", "phishing", "malware", "social engineering", 
            "data protection", "public wifi", "updates", "backup", "2fa"
        };
        
        /// <summary>
        /// Initializes a new instance of the ChatbotEngine class
        /// </summary>
        /// <param name="ui">User interface component</param>
        /// <param name="responseDb">Response database component</param>
        /// <param name="audioManager">Audio manager component</param>
        public ChatbotEngine(UserInterface ui, ResponseDatabase responseDb, AudioManager audioManager)
        {
            // This sets up the component references
            _ui = ui ?? throw new ArgumentNullException(nameof(ui));
            _responseDb = responseDb ?? throw new ArgumentNullException(nameof(responseDb));
            _audioManager = audioManager ?? throw new ArgumentNullException(nameof(audioManager));
        }
        
        /// <summary>
        /// Starts the chatbot and runs the main conversation loop
        /// </summary>
        public void Start()
        {
            try
            {
                // This initializes the console interface
                _ui.InitializeConsole();
                
                // This displays the welcome logo
                _ui.DisplayLogo();
                
                // This plays the voice greeting (with fallback to text if audio fails)
                _audioManager.PlayVoiceGreeting();
                
                // This gets the user's name
                GetUserName();
                
                // This shows the welcome message
                _ui.DisplayWelcomeMessage(_userName);
                _ui.DisplayDivider(ConsoleColor.Cyan);
                
                // This displays initial instructions
                _ui.DisplayTextInstantly("Type 'help' to see available topics, or ask me about cybersecurity.", ConsoleColor.Yellow);
                
                // This runs the main conversation loop
                RunConversationLoop();
            }
            catch (Exception ex)
            {
                // This handles any unexpected errors
                _ui.DisplayErrorMessage($"An error occurred: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Gets and validates the user's name
        /// </summary>
        private void GetUserName()
        {
            // This gets the user name with validation
            string name = _ui.GetUserInput("Please enter your name:", ConsoleColor.Yellow);
            
            // This ensures the name is not empty and contains only letters
            while (string.IsNullOrWhiteSpace(name) || !IsValidName(name))
            {
                name = _ui.GetUserInput("Please enter a valid name (letters only):", ConsoleColor.Red);
            }
            
            _userName = name;
        }
        
        /// <summary>
        /// Validates that a name contains only letters
        /// </summary>
        /// <param name="name">The name to validate</param>
        /// <returns>True if the name contains only letters, false otherwise</returns>
        private bool IsValidName(string name)
        {
            // This checks if the name contains only letters
            foreach (char c in name)
            {
                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    return false;
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// Runs the main conversation loop
        /// </summary>
        private void RunConversationLoop()
        {
            bool exitRequested = false;
            bool waitingForFollowUpResponse = false;
            
            // This loops until the user requests to exit
            while (!exitRequested)
            {
                // This creates a context-aware prompt based on current topic
                string prompt = GetContextAwarePrompt(waitingForFollowUpResponse || _responseDb.IsInPostFollowUpState());
                
                // This gets the user's input
                string userInput = _ui.GetUserInput(prompt, ConsoleColor.Yellow);
                
                // This checks if the user wants to exit
                if (userInput.ToLower() == "exit" || userInput.ToLower() == "quit")
                {
                    _ui.DisplayGoodbyeMessage(_userName);
                    exitRequested = true;
                }
                // This displays the help menu if requested
                else if (userInput.ToLower() == "help" || userInput.ToLower() == "topics")
                {
                    _ui.DisplayHelpMenu(_availableTopics);
                    // This resets the current topic when user asks for help
                    _currentTopic = "";
                    _lastFollowUpQuestion = "";
                    _responseDb.ResetPostFollowUpState();
                    _responseDb.ClearTopicHistory(); // Clear topic history when help is requested
                    waitingForFollowUpResponse = false;
                }
                else
                {
                    // This processes the input and displays the response with follow-up context
                    string response = _responseDb.GetResponse(userInput, _userName, _currentTopic, _lastFollowUpQuestion);
                    
                    _ui.DisplayColoredText(response, ConsoleColor.White);
                    
                    // This checks if we need to update the prompt based on the response type
                    if (response.Contains("Type 'help' to see available topics"))
                    {
                        // This resets the conversation to the default state after completing a post-follow-up flow
                        _currentTopic = "";
                        _lastFollowUpQuestion = "";
                        waitingForFollowUpResponse = false;
                    }
                    else
                    {
                        // This checks if the response was to a specific follow-up question
                        bool wasFollowUpResponse = !string.IsNullOrEmpty(_lastFollowUpQuestion) && 
                            (userInput.Contains("yes") || userInput.Contains("sure") || userInput.Contains("ok") || 
                             userInput.Contains("please") || userInput.Contains("tell me"));
                        
                        // This updates the current topic based on the input
                        UpdateCurrentTopic(userInput);
                        
                        // This updates the last follow-up question
                        _lastFollowUpQuestion = _responseDb.GetLastFollowUpQuestion();
                        
                        // This checks if we're waiting for a follow-up response
                        waitingForFollowUpResponse = !string.IsNullOrEmpty(_lastFollowUpQuestion);
                        
                        // This resets the follow-up question if we just answered one
                        if (wasFollowUpResponse && !_responseDb.IsInPostFollowUpState())
                        {
                            _lastFollowUpQuestion = "";
                            waitingForFollowUpResponse = false;
                        }
                    }
                    
                    // This adds a divider after each response for better readability
                    _ui.DisplayDivider(ConsoleColor.Cyan);
                }
            }
        }
        
        /// <summary>
        /// Creates a context-aware prompt based on the current topic
        /// </summary>
        /// <param name="isInFollowUpFlow">Whether we're in a follow-up question flow</param>
        /// <returns>A prompt that reflects the current conversation context</returns>
        private string GetContextAwarePrompt(bool isInFollowUpFlow = false)
        {
            // This returns a blank prompt when we're in a follow-up flow to avoid confusion
            if (isInFollowUpFlow)
            {
                return "";
            }
            
            if (string.IsNullOrEmpty(_currentTopic))
            {
                return "What would you like to learn about cybersecurity? (Type 'help' for topics or 'exit' to quit)";
            }
            else
            {
                return $"Would you like to know more about {_currentTopic}? (Type 'help' for other topics, 'exit' to quit)";
            }
        }
        
        /// <summary>
        /// Updates the current topic based on user input
        /// </summary>
        /// <param name="userInput">The user's input</param>
        private void UpdateCurrentTopic(string userInput)
        {
            // This normalizes the input to lowercase
            string input = userInput.ToLower();
            
            // This resets the topic if the user indicates they want to change topics
            if (input.Contains("different topic") || input.Contains("another topic") || 
                input.Contains("something else") || input.Contains("change topic"))
            {
                _currentTopic = "";
                _lastFollowUpQuestion = "";
                _responseDb.ResetPostFollowUpState();
                _responseDb.ClearTopicHistory(); // Clear topic history when explicitly changing topics
                return;
            }
            
            // This checks if the input contains a topic keyword
            foreach (string topic in _availableTopics)
            {
                // This normalizes the topic for comparison (e.g., "passwords" to "password")
                string normalizedTopic = topic;
                if (topic == "passwords") normalizedTopic = "password";
                if (topic == "updates") normalizedTopic = "update";
                
                if (input.Contains(normalizedTopic))
                {
                    _currentTopic = normalizedTopic;
                    // This resets the follow-up question when switching topics
                    _lastFollowUpQuestion = "";
                    _responseDb.ResetPostFollowUpState();
                    return;
                }
            }
            
            // This keeps the existing topic if no new topic is detected and we're not in a topic switch request
            // No change to _currentTopic if we don't detect a new topic
        }
    }
}

//--------------------------------------------------End of File--------------------------------------------------