using System;

//------------------------------------------------------------------------------------------------------------------------

namespace CybersecurityAwarenessBot.Data
{
    /// <summary>
    /// Represents a cybersecurity task that users can create and manage
    /// </summary>
    public class CybersecurityTask
    {
        /// <summary>
        /// Gets or sets the task title
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Gets or sets the task description
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Gets or sets the reminder date for the task
        /// </summary>
        public DateTime? ReminderDate { get; set; }
        
        /// <summary>
        /// Gets or sets whether the task is completed
        /// </summary>
        public bool IsCompleted { get; set; }
        
        /// <summary>
        /// Gets or sets when the task was created
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Gets a formatted reminder text for display
        /// </summary>
        public string ReminderText
        {
            get
            {
                if (ReminderDate.HasValue)
                {
                    // This formats the reminder date for display
                    DateTime today = DateTime.Today;
                    DateTime reminderDay = ReminderDate.Value.Date;
                    
                    if (reminderDay == today)
                    {
                        return "⏰ Reminder: Today";
                    }
                    else if (reminderDay == today.AddDays(1))
                    {
                        return "⏰ Reminder: Tomorrow";
                    }
                    else if (reminderDay < today)
                    {
                        return "⚠️ Overdue";
                    }
                    else
                    {
                        return $"⏰ Reminder: {reminderDay:MMM dd, yyyy}";
                    }
                }
                return "📝 No reminder set";
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the CybersecurityTask class
        /// </summary>
        public CybersecurityTask()
        {
            // This sets the creation date to current time
            CreatedDate = DateTime.Now;
            IsCompleted = false;
        }
        
        /// <summary>
        /// Initialises a new instance of the CybersecurityTask class with specified values
        /// </summary>
        /// <param name="title">The task title</param>
        /// <param name="description">The task description</param>
        /// <param name="reminderDate">The optional reminder date</param>
        public CybersecurityTask(string title, string description, DateTime? reminderDate = null)
        {
            Title = title;
            Description = description;
            ReminderDate = reminderDate;
            CreatedDate = DateTime.Now;
            IsCompleted = false;
        }
    }
}

//--------------------------------------------------End of File--------------------------------------------------