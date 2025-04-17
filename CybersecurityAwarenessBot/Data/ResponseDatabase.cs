using System;
using System.Collections.Generic;

namespace CybersecurityAwarenessBot.Data
{
    /// <summary>
    /// Manages cybersecurity response content for the chatbot
    /// </summary>
    public class ResponseDatabase
    {
        // This stores all the cybersecurity topic responses
        private readonly Dictionary<string, string> _responses;
        
        /// <summary>
        /// Initializes a new instance of the ResponseDatabase class
        /// </summary>
        public ResponseDatabase()
        {
            // This loads all predefined responses into the dictionary
            _responses = new Dictionary<string, string>
            {
                { "password", "You should use strong, unique passwords for each account and consider using a password manager. A strong password has at least 12 characters, mixed case, numbers, and symbols." },
                
                { "phishing", "Phishing is when attackers trick you into revealing personal information. Always check email addresses, don't click suspicious links, and be wary of urgent requests. Legitimate organizations won't ask for sensitive information via email." },
                
                { "malware", "Malware is malicious software that can damage your device or steal information. Protect yourself by installing antivirus software, keeping your system updated, and avoiding suspicious downloads or websites." },
                
                { "social engineering", "Social engineering is when attackers manipulate people into revealing confidential information. Be suspicious of unsolicited contacts, verify identities independently, and don't share sensitive information without verification." },
                
                { "data protection", "Protect your data by using encryption, backing up regularly, being cautious about sharing information online, and reviewing privacy settings on your accounts and devices." },
                
                { "public wifi", "Public WiFi networks are often insecure. Avoid accessing sensitive accounts on public WiFi, use a VPN for encryption, and ensure websites use HTTPS before entering personal information." },
                
                { "updates", "Software updates often contain security fixes. Always keep your operating system, applications, and antivirus software up-to-date to protect against known vulnerabilities." },
                
                { "backup", "Regular backups protect against ransomware and data loss. Use the 3-2-1 rule: have 3 copies of your data on 2 different types of media with 1 copy stored offsite or in the cloud." },
                
                { "2fa", "Two-factor authentication (2FA) adds an extra layer of security by requiring something you know (password) and something you have (like your phone). Enable it on all important accounts." },
                
                { "help", "You can ask me about: passwords, phishing, malware, social engineering, data protection, public wifi, updates, backup, or 2fa (two-factor authentication)." }
            };
        }
        
        /// <summary>
        /// Gets a response for the given user input
        /// </summary>
        /// <param name="userInput">The user's input</param>
        /// <param name="userName">The user's name for personalization</param>
        /// <returns>A cybersecurity response based on the input</returns>
        public string GetResponse(string userInput, string userName)
        {
            // This converts the input to lowercase for case-insensitive matching
            userInput = userInput.ToLower();
            
            // This checks for specific topics in the user input
            foreach (var topic in _responses.Keys)
            {
                if (userInput.Contains(topic))
                {
                    // This personalizes the response with the user's name
                    return $"{userName}, {_responses[topic]}";
                }
            }
            
            // This handles requests for available topics
            if (userInput.Contains("help") || userInput.Contains("topics"))
            {
                return _responses["help"];
            }
            
            // This provides a default response when no matching topic is found
            return $"I'm sorry {userName}, I don't have information about that topic yet. Type 'help' to see available topics.";
        }
    }
} 