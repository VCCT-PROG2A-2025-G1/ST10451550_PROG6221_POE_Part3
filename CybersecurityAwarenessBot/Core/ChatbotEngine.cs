using System;
using CybersecurityAwarenessBot.UI;
using CybersecurityAwarenessBot.Data;
using CybersecurityAwarenessBot.Audio;

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
            
            // This ensures the name is not empty
            while (string.IsNullOrWhiteSpace(name))
            {
                name = _ui.GetUserInput("Please enter a valid name:", ConsoleColor.Red);
            }
            
            _userName = name;
        }
        
        /// <summary>
        /// Runs the main conversation loop
        /// </summary>
        private void RunConversationLoop()
        {
            bool exitRequested = false;
            
            // This loops until the user requests to exit
            while (!exitRequested)
            {
                // This gets the user's input
                string userInput = _ui.GetUserInput(
                    "What would you like to know about cybersecurity? (Type 'exit' to quit)", 
                    ConsoleColor.Yellow
                );
                
                // This checks if the user wants to exit
                if (userInput.ToLower() == "exit" || userInput.ToLower() == "quit")
                {
                    _ui.DisplayGoodbyeMessage(_userName);
                    exitRequested = true;
                }
                else
                {
                    // This processes the input and displays the response
                    string response = _responseDb.GetResponse(userInput, _userName);
                    _ui.DisplayColoredText(response, ConsoleColor.White);
                }
            }
        }
    }
} 