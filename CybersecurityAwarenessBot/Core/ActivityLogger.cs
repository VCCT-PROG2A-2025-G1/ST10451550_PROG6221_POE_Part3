using System;
using System.Collections.Generic;
using System.Linq;
using CybersecurityAwarenessBot.Data;

//------------------------------------------------------------------------------------------------------------------------

namespace CybersecurityAwarenessBot.Core
{
    /// <summary>
    /// Manages activity logging and display for the cybersecurity chatbot
    /// </summary>
    public class ActivityLogger
    {
        // This stores all activity entries (limited to last 50 for memory management)
        private readonly List<ActivityEntry> _activities;
        
        // This defines the maximum number of activities to store
        private const int MaxActivities = 50;
        
        //------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the ActivityLogger class
        /// </summary>
        public ActivityLogger()
        {
            _activities = new List<ActivityEntry>();
        }
        
        //------------------------------------------------------------------------------------------------------------------------

        #region Core Logging Methods

        /// <summary>
        /// Logs a new activity entry
        /// </summary>
        /// <param name="activityType">The type of activity (Task, Quiz, Chat, System)</param>
        /// <param name="description">The user-friendly description</param>
        public void LogActivity(string activityType, string description)
        {
            // This creates a new activity entry
            var activity = new ActivityEntry(activityType, description);
            
            // This adds the activity to the beginning of the list (newest first)
            _activities.Insert(0, activity);
            
            // This maintains the maximum activity limit
            if (_activities.Count > MaxActivities)
            {
                _activities.RemoveAt(_activities.Count - 1);
            }
        }
        
        /// <summary>
        /// Logs a task-related activity
        /// </summary>
        /// <param name="action">The task action (added, completed, deleted, edited)</param>
        /// <param name="taskTitle">The task title</param>
        /// <param name="additionalInfo">Optional additional information</param>
        public void LogTaskActivity(string action, string taskTitle, string additionalInfo = "")
        {
            string description = $"Task {action}: '{taskTitle}'";
            if (!string.IsNullOrEmpty(additionalInfo))
            {
                description += $" ({additionalInfo})";
            }
            
            LogActivity("Task", description);
        }
        
        /// <summary>
        /// Logs a quiz-related activity
        /// </summary>
        /// <param name="action">The quiz action (started, completed)</param>
        /// <param name="additionalInfo">Optional additional information like score</param>
        public void LogQuizActivity(string action, string additionalInfo = "")
        {
            string description = $"Quiz {action}";
            if (!string.IsNullOrEmpty(additionalInfo))
            {
                description += $": {additionalInfo}";
            }
            
            LogActivity("Quiz", description);
        }
        
        /// <summary>
        /// Logs a chat-related activity (topic discussions)
        /// </summary>
        /// <param name="topic">The cybersecurity topic discussed</param>
        public void LogChatActivity(string topic)
        {
            string description = $"Discussed {FormatTopicForLog(topic)} security";
            LogActivity("Chat", description);
        }
        
        /// <summary>
        /// Logs a system-related activity
        /// </summary>
        /// <param name="action">The system action (help accessed, tab switched, etc.)</param>
        public void LogSystemActivity(string action)
        {
            LogActivity("System", action);
        }
        
        /// <summary>
        /// Gets recent activities for display
        /// </summary>
        /// <param name="count">Number of activities to retrieve (default 5)</param>
        /// <returns>List of recent activities</returns>
        public List<ActivityEntry> GetRecentActivities(int count = 5)
        {
            return _activities.Take(count).ToList();
        }
        
        /// <summary>
        /// Gets all activities for comprehensive display
        /// </summary>
        /// <returns>List of all stored activities</returns>
        public List<ActivityEntry> GetAllActivities()
        {
            return _activities.ToList();
        }
        
        /// <summary>
        /// Gets the count of total activities
        /// </summary>
        /// <returns>Total number of activities</returns>
        public int GetActivityCount()
        {
            return _activities.Count;
        }
        
        /// <summary>
        /// Clears all activities from the log
        /// </summary>
        public void ClearAllActivities()
        {
            _activities.Clear();
        }
        
        /// <summary>
        /// Formats activity log for chat display
        /// </summary>
        /// <param name="activities">The activities to format</param>
        /// <param name="showMoreAvailable">Whether more activities are available</param>
        /// <returns>Formatted activity log string</returns>
        public string FormatActivitiesForChat(List<ActivityEntry> activities, bool showMoreAvailable = false)
        {
            if (activities.Count == 0)
            {
                return "No recent activities to display. Start using the chatbot to see your activity history!";
            }
            
            string result = "═══ RECENT ACTIVITY LOG ═══\n";
            
            for (int i = 0; i < activities.Count; i++)
            {
                result += $"{i + 1}. {activities[i].FormattedEntry}\n";
            }
            
            if (showMoreAvailable)
            {
                result += "\nType 'show more' to see additional activity history.";
            }
            
            return result.TrimEnd();
        }
        
        /// <summary>
        /// Checks if input contains activity log keywords
        /// </summary>
        /// <param name="input">User input to check</param>
        /// <returns>True if input requests activity log</returns>
        public bool IsActivityLogRequest(string input)
        {
            string lowerInput = input.ToLower();
            
            // This checks for various activity log trigger phrases
            return lowerInput.Contains("what have you done for me") ||
                   lowerInput.Contains("recent activity") ||
                   lowerInput.Contains("history") ||
                   lowerInput.Contains("log") ||
                   lowerInput.Contains("show more");
        }
        
        /// <summary>
        /// Determines if user wants extended activity history
        /// </summary>
        /// <param name="input">User input to check</param>
        /// <returns>True if user wants more activities</returns>
        public bool IsShowMoreRequest(string input)
        {
            string lowerInput = input.ToLower();
            return lowerInput.Contains("show more") || lowerInput.Contains("more activities") || lowerInput.Contains("full history");
        }
        
        /// <summary>
        /// Formats topic names for activity log display
        /// </summary>
        /// <param name="topic">The raw topic name</param>
        /// <returns>Formatted topic name</returns>
        private string FormatTopicForLog(string topic)
        {
            switch (topic.ToLower())
            {
                case "password":
                case "passwords":
                    return "password";
                case "2fa":
                case "two-factor":
                    return "two-factor authentication";
                case "phishing":
                    return "phishing";
                case "malware":
                    return "malware";
                case "social engineering":
                    return "social engineering";
                case "data protection":
                    return "data protection";
                case "public wifi":
                case "wifi":
                    return "WiFi";
                case "updates":
                    return "software update";
                case "backup":
                    return "backup";
                case "antivirus":
                case "virus":
                    return "antivirus";
                default:
                    return topic;
            }
        }

        #endregion
    }
}

//--------------------------------------------------End of File-------------------------------------------------- 