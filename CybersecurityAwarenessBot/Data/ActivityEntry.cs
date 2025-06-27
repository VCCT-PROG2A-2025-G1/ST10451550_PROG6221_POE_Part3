using System;

//------------------------------------------------------------------------------------------------------------------------

namespace CybersecurityAwarenessBot.Data
{
    /// <summary>
    /// Represents a user activity entry in the activity log system
    /// </summary>
    public class ActivityEntry
    {
        /// <summary>
        /// Gets or sets when the activity occurred
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// Gets or sets the type of activity (Task, Quiz, Chat, System)
        /// </summary>
        public string ActivityType { get; set; }
        
        /// <summary>
        /// Gets or sets the user-friendly description of the activity
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets a formatted timestamp for display purposes
        /// </summary>
        public string FormattedTimestamp
        {
            get
            {
                DateTime now = DateTime.Now;
                DateTime today = DateTime.Today;
                DateTime yesterday = today.AddDays(-1);
                
                if (Timestamp.Date == today)
                {
                    return $"Today {Timestamp:h:mm tt}";
                }
                else if (Timestamp.Date == yesterday)
                {
                    return $"Yesterday {Timestamp:h:mm tt}";
                }
                else if (Timestamp.Date >= today.AddDays(-7))
                {
                    return $"{Timestamp:dddd h:mm tt}";
                }
                else
                {
                    return $"{Timestamp:MMM dd h:mm tt}";
                }
            }
        }
        
        /// <summary>
        /// Gets a formatted log entry for display
        /// </summary>
        public string FormattedEntry
        {
            get
            {
                return $"[{FormattedTimestamp}] {Description}";
            }
        }
        
        /// <summary>
        /// Gets an icon based on the activity type for UI display
        /// </summary>
        public string ActivityTypeIcon
        {
            get
            {
                switch (ActivityType?.ToLower())
                {
                    case "task":
                        return "📝";
                    case "quiz":
                        return "🧠";
                    case "chat":
                        return "💬";
                    case "system":
                        return "⚙️";
                    default:
                        return "📋";
                }
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the ActivityEntry class
        /// </summary>
        public ActivityEntry()
        {
            Timestamp = DateTime.Now;
        }
        
        /// <summary>
        /// Initializes a new instance of the ActivityEntry class with specified values
        /// </summary>
        /// <param name="activityType">The type of activity</param>
        /// <param name="description">The description of the activity</param>
        public ActivityEntry(string activityType, string description)
        {
            Timestamp = DateTime.Now;
            ActivityType = activityType;
            Description = description;
        }
    }
}

//--------------------------------------------------End of File-------------------------------------------------- 