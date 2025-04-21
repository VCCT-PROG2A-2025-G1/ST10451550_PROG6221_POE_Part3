using System;
using System.Threading;

namespace CybersecurityAwarenessBot.UI
{
    /// <summary>
    /// Handles all display-related functionality for the Cybersecurity Awareness Chatbot
    /// </summary>
    public class UserInterface
    {
        // This defines the delay between characters for the typing effect (in milliseconds)
        private const int TypeWriterDelayMs = 15;
        
        /// <summary>
        /// Initializes the console interface settings
        /// </summary>
        public void InitializeConsole()
        {
            // This sets up the console appearance
            Console.Title = "Cybersecurity Awareness Chatbot";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
        }

        /// <summary>
        /// Displays text in a specified color
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="color">The color to use</param>
        public void DisplayColoredText(string text, ConsoleColor color)
        {
            // This saves the current color, changes it, and restores it after
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            
            // This displays text with typing effect
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(TypeWriterDelayMs);
            }
            Console.WriteLine();
            
            Console.ForegroundColor = originalColor;
        }
        
        /// <summary>
        /// Displays text instantly without typing effect
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="color">The color to use</param>
        public void DisplayTextInstantly(string text, ConsoleColor color)
        {
            // This saves the current color, changes it, and restores it after
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            
            // This displays text without delay
            Console.WriteLine(text);
            
            Console.ForegroundColor = originalColor;
        }

        /// <summary>
        /// Gets input from the user with a specified prompt
        /// </summary>
        /// <param name="prompt">The prompt to display</param>
        /// <param name="promptColor">Color for the prompt</param>
        /// <returns>User input as string</returns>
        public string GetUserInput(string prompt, ConsoleColor promptColor)
        {
            // This displays the prompt and gets the user's response
            Console.WriteLine();
            DisplayTextInstantly(prompt, promptColor);
            Console.Write("> ");
            return Console.ReadLine().Trim();
        }
        
        /// <summary>
        /// Displays a decorative border around text
        /// </summary>
        /// <param name="text">The text to display inside the border</param>
        /// <param name="color">The color to use for the border and text</param>
        public void DisplayBorderedText(string text, ConsoleColor color)
        {
            // This saves the original color
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            
            // This creates a border based on the text length
            string border = new string('-', text.Length + 4);
            
            // This displays the bordered text
            Console.WriteLine(border);
            Console.WriteLine($"| {text} |");
            Console.WriteLine(border);
            
            // This restores the original color
            Console.ForegroundColor = originalColor;
        }
        
        /// <summary>
        /// Displays a section divider to separate conversation segments
        /// </summary>
        /// <param name="color">The color to use for the divider</param>
        public void DisplayDivider(ConsoleColor color)
        {
            // This displays a simple section divider
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            
            Console.WriteLine("\n" + new string('-', 50) + "\n");
            
            Console.ForegroundColor = originalColor;
        }
        
        /// <summary>
        /// Displays formatted help information
        /// </summary>
        /// <param name="topics">Array of available topics</param>
        public void DisplayHelpMenu(string[] topics)
        {
            // This displays a formatted help menu
            DisplayBorderedText("Available Topics", ConsoleColor.Yellow);
            Console.WriteLine();
            
            // This displays each topic with a bullet point
            foreach (string topic in topics)
            {
                DisplayTextInstantly($"• {topic}", ConsoleColor.Cyan);
            }
            
            Console.WriteLine();
            DisplayTextInstantly("Type a topic name to learn more about it.", ConsoleColor.Yellow);
            DisplayTextInstantly("Type 'exit' to quit the application.", ConsoleColor.Yellow);
            Console.WriteLine();
        }

        /// <summary>
        /// Displays the ASCII art logo of the chatbot
        /// </summary>
        public void DisplayLogo()
        {
            // This saves the original console colors to restore later
            ConsoleColor originalForeground = Console.ForegroundColor;
            ConsoleColor originalBackground = Console.BackgroundColor;

            Console.Clear();
            
            // This draws the improved lock ASCII art with colors
            Console.ForegroundColor = ConsoleColor.Cyan;
            
            // This centers the logo
            Console.WriteLine();
            Console.WriteLine("          .-\"\"-.          ");
            Console.WriteLine("         / .--. \\         ");
            Console.WriteLine("        / /    \\ \\        ");
            Console.WriteLine("        | |    | |        ");
            Console.WriteLine("        | |.-\"\"-.|        ");
            
            // This highlights internal parts in yellow
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("       ///`.::::.'\\       ");
            Console.WriteLine("      ||| ::/  \\:: ;      ");
            Console.WriteLine("      ||; ::\\__/:: ;      ");
            Console.WriteLine("       \\\\\\  '::::'/      ");
            Console.WriteLine("        `=':-..-'`        ");
            
            // This displays the title with a white color for emphasis
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("    CYBERSECURITY AWARENESS BOT    ");
            Console.WriteLine();
            
            // This displays the subtitle with a green color
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("   Protecting South African Citizens");
            Console.WriteLine("     Through Security Education     ");
            Console.WriteLine();
            
            // This restores the original console colors
            Console.ForegroundColor = originalForeground;
            Console.BackgroundColor = originalBackground;
        }

        /// <summary>
        /// Displays a welcome message for the user
        /// </summary>
        /// <param name="userName">The user's name</param>
        public void DisplayWelcomeMessage(string userName)
        {
            // This welcomes the user with a personalized message
            Console.WriteLine();
            DisplayColoredText($"Hello, {userName}! I'm here to help you learn about cybersecurity.", ConsoleColor.Green);
        }

        /// <summary>
        /// Displays a goodbye message to the user
        /// </summary>
        /// <param name="userName">The user's name</param>
        public void DisplayGoodbyeMessage(string userName)
        {
            // This provides a friendly exit message
            DisplayDivider(ConsoleColor.Cyan);
            DisplayColoredText($"Goodbye, {userName}! Stay safe online!", ConsoleColor.Green);
        }

        /// <summary>
        /// Displays an error message
        /// </summary>
        /// <param name="message">The error message to display</param>
        public void DisplayErrorMessage(string message)
        {
            // This shows error messages in red
            DisplayTextInstantly($"Error: {message}", ConsoleColor.Red);
        }
    }
} 