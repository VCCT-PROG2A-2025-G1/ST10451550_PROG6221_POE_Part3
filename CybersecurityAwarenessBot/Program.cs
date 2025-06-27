// Luke Manley
// ST10451550
// Group 1

// My References
// ASCII Lock Art - https://ascii.co.uk/art/lock
// C# Exception Handling - https://www.geeksforgeeks.org/c-sharp-exception/
// C# Exception Handling - https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/
// Password Security Information - https://www.cisa.gov/secure-our-world/use-strong-passwords
// Phishing Information - https://www.business.hsbc.co.za/en-gb/campaigns/cybercrime/phishing
// Malware Information - https://www.techtarget.com/searchsecurity/definition/malware
// Social Engineering Information- https://www.imperva.com/learn/application-security/social-engineering-attack/
// Data Protection Information - https://cloudian.com/guides/data-protection/data-protection-and-privacy-7-ways-to-protect-user-data/
// Two-Factor Authentication Information - https://www.microsoft.com/en-za/security/business/security-101/what-is-two-factor-authentication-2fa
// Backup Information - https://www.techtarget.com/searchdatabackup/definition/3-2-1-Backup-Strategy
// C# Access Modifiers Guide - https://www.youtube.com/watch?v=jcn5uCZAk2w
// C# Dictionaries Guide - https://www.youtube.com/watch?v=R94JHIXdTk0
// C# XML Documentation Guide - https://www.youtube.com/watch?v=z448dPJNXNs
// AI Assistant - https://chatgpt.com/
// C# Soundplayer Class - https://learn.microsoft.com/en-us/dotnet/api/system.media.soundplayer?view=windowsdesktop-9.0
// Path.Combine Method - https://learn.microsoft.com/en-us/dotnet/api/system.io.path.combine?view=net-9.0
// Where I got my Text-to-speech from - https://console.cloud.google.com/speech/overview?project=noble-freehold-457120-s5
// Thread.Sleep Method - https://learn.microsoft.com/en-us/dotnet/api/system.threading.thread.sleep?view=net-9.0
// C# Dictionary Collections - https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2
// List<T> Collections - https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1
// C# Dictionary Collections - https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/how-to-initialize-a-dictionary-with-a-collection-initializer
// String Contains Method - https://learn.microsoft.com/en-us/dotnet/api/system.string.contains
// C# Lists - https://www.geeksforgeeks.org/c-sharp-list-class/
// Random Class - https://learn.microsoft.com/en-us/dotnet/api/system.random?view=net-9.0
// Natural Language Processing Fundamentals - https://www.geeksforgeeks.org/natural-language-processing-nlp-tutorial/
// Keyword Extraction Methods in NLP - https://www.geeksforgeeks.org/nlp/keyword-extraction-methods-in-nlp/
// Activity Logging Best Practices - https://betterstack.com/community/guides/logging/logging-best-practices/
// Application Logging Development Guide - https://coralogix.com/guides/application-performance-monitoring/application-logging-best-practices/
// WPF User Interface Best Practices - https://blog.postsharp.net/wpf-best-practices-2024
// WPF Design Guidelines and Controls - https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/guidelines-for-designing-stylable-controls
// Task Automation and UI Design Patterns - https://www.codemag.com/Article/1905031/A-Design-Pattern-for-Building-WPF-Business-Applications-Part-1
// Youtube playlist I used to understand and practice WPF - https://www.youtube.com/watch?v=t9ivUosw_iI&list=PLih2KERbY1HHOOJ2C6FOrVXIwg4AZ-hk1

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

// This file contains the original console application entry point
// It has been preserved for reference but is no longer used in the WPF version
// The WPF application now starts through App.xaml and MainWindow.xaml

/*
using System;
using CybersecurityAwarenessBot.Core;
using CybersecurityAwarenessBot.UI;
using CybersecurityAwarenessBot.Data;
using CybersecurityAwarenessBot.Audio;

namespace CybersecurityAwarenessBot
{
    /// <summary>
    /// Original console entry point - preserved for reference
    /// The application now starts as a WPF application through App.xaml
    /// </summary>
    class Program
    {
        /// <summary>
        /// Original console main method - no longer used
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                // This creates the component instances
                UserInterface ui = new UserInterface();
                ResponseDatabase responseDb = new ResponseDatabase();
                AudioManager audioManager = new AudioManager();
                
                // This creates the chatbot engine with dependencies
                ChatbotEngine chatbot = new ChatbotEngine(ui, responseDb, audioManager);
                
                // This starts the chatbot
                chatbot.Start();
            }
            catch (Exception ex)
            {
                // This handles any top-level exceptions
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"A critical error occurred: {ex.Message}");
                Console.ResetColor();
                
                // This waits for user input before closing
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
*/

//--------------------------------------------------End of File--------------------------------------------------
