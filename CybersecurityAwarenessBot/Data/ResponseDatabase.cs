using System;
using System.Collections.Generic;

namespace CybersecurityAwarenessBot.Data
{
    /// <summary>
    /// Manages cybersecurity response content for the chatbot
    /// </summary>
    public class ResponseDatabase
    {
        // This stores all the cybersecurity topic responses (multiple per topic)
        private readonly Dictionary<string, List<string>> _responses;
        
        // This creates a random number generator for selecting responses
        private readonly Random _random;
        
        /// <summary>
        /// Initializes a new instance of the ResponseDatabase class
        /// </summary>
        public ResponseDatabase()
        {
            // This initializes the random number generator
            _random = new Random();
            
            // This loads all predefined responses into the dictionary
            _responses = new Dictionary<string, List<string>>
            {
                { "password", new List<string> {
                    "You should use strong, unique passwords for each account and consider using a password manager. A strong password has at least 12 characters, mixed case, numbers, and symbols.",
                    "Password security is crucial for protecting your accounts. Use different passwords for each service, make them complex, and change them regularly. Consider using a reputable password manager.",
                    "Creating secure passwords involves using a mix of letters, numbers, and symbols. Avoid using personal information, dictionary words, or common patterns. A password manager can help you manage multiple complex passwords."
                }},
                
                { "phishing", new List<string> {
                    "Phishing is when attackers trick you into revealing personal information. Always check email addresses, don't click suspicious links, and be wary of urgent requests. Legitimate organizations won't ask for sensitive information via email.",
                    "Phishing attacks use deceptive emails or messages to steal your information. Look for suspicious sender addresses, grammatical errors, and unusual requests. When in doubt, contact the company directly using their official website.",
                    "To avoid phishing scams, never click links in unexpected emails. Hover over links to see the actual URL, and be suspicious of messages creating urgency or fear. Legitimate companies never request passwords or financial details via email."
                }},
                
                { "malware", new List<string> {
                    "Malware is malicious software that can damage your device or steal information. Protect yourself by installing antivirus software, keeping your system updated, and avoiding suspicious downloads or websites.",
                    "Malicious software (malware) can infect your devices in many ways. Keep your security software updated, be careful about what you download, and regularly scan your system for threats.",
                    "Malware comes in many forms including viruses, trojans, and ransomware. Practice safe browsing habits, don't open attachments from unknown sources, and use reputable security software to protect your devices."
                }},
                
                { "social engineering", new List<string> {
                    "Social engineering is when attackers manipulate people into revealing confidential information. Be suspicious of unsolicited contacts, verify identities independently, and don't share sensitive information without verification.",
                    "Social engineering attacks exploit human psychology rather than technical vulnerabilities. Always verify the identity of anyone requesting sensitive information, even if they appear legitimate or friendly.",
                    "In social engineering attacks, criminals use manipulation to gain your trust. Be wary of unexpected phone calls, too-good-to-be-true offers, or anyone creating pressure to act quickly without verification."
                }},
                
                { "data protection", new List<string> {
                    "Protect your data by using encryption, backing up regularly, being cautious about sharing information online, and reviewing privacy settings on your accounts and devices.",
                    "Data protection involves safeguarding your personal information from unauthorized access. Use strong encryption, keep software updated, and be mindful of what information you share online.",
                    "To protect your data, regularly review app permissions, use privacy-focused services when possible, encrypt sensitive files, and maintain awareness of how your information is being collected and used."
                }},
                
                { "public wifi", new List<string> {
                    "Public WiFi networks are often insecure. Avoid accessing sensitive accounts on public WiFi, use a VPN for encryption, and ensure websites use HTTPS before entering personal information.",
                    "When using public WiFi, your data may be vulnerable to interception. Use a VPN service to encrypt your connection, avoid financial transactions, and disable auto-connect features on your devices.",
                    "Public WiFi security risks include man-in-the-middle attacks and fake networks. Always verify the network name before connecting, use a VPN, and save sensitive activities for secure networks."
                }},
                
                { "updates", new List<string> {
                    "Software updates often contain security fixes. Always keep your operating system, applications, and antivirus software up-to-date to protect against known vulnerabilities.",
                    "Regular software updates are critical for security. Enable automatic updates when possible, and promptly install security patches to protect against exploits targeting known vulnerabilities.",
                    "Updating your software closes security holes that hackers could exploit. Make updating a regular habit - this includes your operating system, apps, browsers, and especially security software."
                }},
                
                { "backup", new List<string> {
                    "Regular backups protect against ransomware and data loss. Use the 3-2-1 rule: have 3 copies of your data on 2 different types of media with 1 copy stored offsite or in the cloud.",
                    "Data backups are your insurance against ransomware, hardware failure, and accidental deletion. Automate your backups when possible and regularly test that they can be successfully restored.",
                    "Creating reliable backups means having multiple copies of your important files. Use external drives, cloud storage, or both, and set a regular schedule for backing up to avoid data loss disasters."
                }},
                
                { "2fa", new List<string> {
                    "Two-factor authentication (2FA) adds an extra layer of security by requiring something you know (password) and something you have (like your phone). Enable it on all important accounts.",
                    "2FA significantly improves your account security by requiring a second verification step. Even if your password is compromised, attackers still can't access your account without the second factor.",
                    "Enable two-factor authentication wherever available to protect your accounts. It adds a verification step using your phone, an authenticator app, or a security key that prevents unauthorized access even if your password is stolen."
                }},
                
                { "help", new List<string> {
                    "You can ask me about: passwords, phishing, malware, social engineering, data protection, public wifi, updates, backup, or 2fa (two-factor authentication)."
                }}
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
            
            // This creates a list to store found topics
            List<string> foundTopics = new List<string>();
            
            // This checks for specific topics in the user input
            foreach (var topic in _responses.Keys)
            {
                if (userInput.Contains(topic))
                {
                    // This adds the topic to our found topics list
                    foundTopics.Add(topic);
                }
            }
            
            // This handles requests for available topics
            if (userInput.Contains("help") || userInput.Contains("topics"))
            {
                // This returns the help message (only one variation)
                return _responses["help"][0];
            }
            
            // This handles the case when multiple topics are found (excluding "help")
            if (foundTopics.Count >= 2 && !foundTopics.Contains("help"))
            {
                // This limits to first 2 topics to keep it simple
                var topic1 = foundTopics[0];
                var topic2 = foundTopics[1];
                return $"I noticed you mentioned both {topic1} and {topic2}. Which would you like to discuss first?";
            }
            
            // This handles when a single topic is found
            if (foundTopics.Count == 1 && foundTopics[0] != "help")
            {
                string topic = foundTopics[0];
                // This randomly selects one of the available responses for the topic
                int responseIndex = _random.Next(0, _responses[topic].Count);
                string selectedResponse = _responses[topic][responseIndex];
                
                // This personalizes the response with the user's name
                return $"{userName}, {selectedResponse}";
            }
            
            // This provides a default response when no matching topic is found
            return $"I'm sorry {userName}, I don't have information about that topic yet. Type 'help' to see available topics.";
        }
    }
} 