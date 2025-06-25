using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using CybersecurityAwarenessBot.Core;
using CybersecurityAwarenessBot.Data;
using CybersecurityAwarenessBot.Audio;

namespace CybersecurityAwarenessBot
{
    /// <summary>
    /// Main window for the Cybersecurity Awareness Chatbot WPF application
    /// </summary>
    public partial class MainWindow : Window
    {
        // This stores the chatbot components
        private readonly ResponseDatabase _responseDb;
        private readonly AudioManager _audioManager;
        private WpfChatbotEngine _chatbotEngine;
        
        // This stores the task list for the Task Assistant
        private readonly ObservableCollection<CybersecurityTask> _tasks;
        
        // This tracks the user's name
        private string _userName = "";
        
        // This stores predefined cybersecurity tasks
        private readonly List<CybersecurityTask> _predefinedTasks;
        
        // This tracks if we're waiting for a reminder response
        private bool _waitingForReminderResponse = false;
        
        // This stores the last created task for reminder setup
        private CybersecurityTask _lastCreatedTask = null;
        
        /// <summary>
        /// Initialises a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            
            // This initializes the chatbot components
            _responseDb = new ResponseDatabase();
            _audioManager = new AudioManager();
            
            // This initializes the task collection
            _tasks = new ObservableCollection<CybersecurityTask>();
            TaskListBox.ItemsSource = _tasks;
            
            // This creates predefined example tasks
            _predefinedTasks = CreatePredefinedTasks();
            
            // This loads the predefined tasks
            LoadPredefinedTasks();
            
            // This initializes the chatbot interface
            InitializeChatbot();
        }
        
        /// <summary>
        /// Initializes the chatbot interface and displays welcome message
        /// </summary>
        private void InitializeChatbot()
        {
            // This displays the ASCII logo
            DisplayLogo();
            
            // This plays the voice greeting
            _audioManager.PlayVoiceGreeting();
            
            // This gets the user's name
            GetUserName();
            
            // This creates the WPF chatbot engine
            _chatbotEngine = new WpfChatbotEngine(_responseDb, this);
            
            // This displays the welcome message
            DisplayWelcomeMessage();
            
            // This displays initial instructions
            AppendToChatDisplay("Bot: Type 'help' to see available topics, or ask me about cybersecurity.", Brushes.Yellow);
            AppendToChatDisplay("Bot: You can also ask me to help with tasks like 'create a task' or 'remind me to update passwords'.", Brushes.Cyan);
            AppendDividerToChat();
        }
        
        /// <summary>
        /// Displays the ASCII art logo in the interface
        /// </summary>
        private void DisplayLogo()
        {
            // This sets the ASCII art logo text
            string logo = @"          .-""-.          
         / .--. \         
        / /    \ \        
        | |    | |        
        | |.-""-.|        
       ///`.::::.'\\       
      ||| ::/  \:: ;      
      ||; ::\\__/:: ;      
       \\\  '::::'/       
        `=':-..-'`        ";
        
            LogoDisplay.Text = logo;
        }
        
        /// <summary>
        /// Gets the user's name through a simple dialog
        /// </summary>
        private void GetUserName()
        {
            // This gets the user's name with validation
            while (string.IsNullOrWhiteSpace(_userName))
            {
                string input = Microsoft.VisualBasic.Interaction.InputBox(
                    "Please enter your name:", 
                    "Cybersecurity Awareness Bot", 
                    "");
                
                if (string.IsNullOrWhiteSpace(input))
                {
                    MessageBox.Show("Please enter a valid name.", "Name Required", 
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (!IsValidName(input))
                {
                    MessageBox.Show("Please enter a name with letters only.", "Invalid Name", 
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    _userName = input;
                }
            }
        }
        
        /// <summary>
        /// Validates that a name contains only letters and spaces
        /// </summary>
        /// <param name="name">The name to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        private bool IsValidName(string name)
        {
            // This checks if the name contains only letters and spaces
            return name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
        }
        
        /// <summary>
        /// Displays a welcome message for the user
        /// </summary>
        private void DisplayWelcomeMessage()
        {
            AppendToChatDisplay($"Bot: Hello, {_userName}! I'm here to help you learn about cybersecurity.", Brushes.LightGreen);
            AppendDividerToChat();
        }
        
        /// <summary>
        /// Handles the Send button click event
        /// </summary>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }
        
        /// <summary>
        /// Handles the Enter key press in the input textbox
        /// </summary>
        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUserInput();
            }
        }
        
        /// <summary>
        /// Processes the user's input and generates appropriate responses
        /// </summary>
        private void ProcessUserInput()
        {
            string input = UserInput.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;
            
            // This displays the user's input
            AppendToChatDisplay($"You: {input}", Brushes.White);
            
            // This clears the input field
            UserInput.Text = "";
            
            // This checks if we're waiting for a reminder response
            if (_waitingForReminderResponse)
            {
                if (ProcessReminderResponse(input))
                {
                    AppendDividerToChat();
                    return;
                }
            }
            
            // This processes task-related commands first
            if (ProcessTaskCommands(input))
            {
                AppendDividerToChat();
                return;
            }
            
            // This handles help command
            if (input.ToLower() == "help" || input.ToLower() == "topics")
            {
                DisplayHelpMenu();
                AppendDividerToChat();
                return;
            }
            
            // This uses the chatbot engine to get responses
            string response = _chatbotEngine.GetResponse(input, _userName);
            AppendToChatDisplay(response, Brushes.White);
            
            AppendDividerToChat();
        }
        
        /// <summary>
        /// Processes task-related commands and returns true if handled
        /// </summary>
        /// <param name="input">The user's input</param>
        /// <returns>True if the input was a task command, false otherwise</returns>
        private bool ProcessTaskCommands(string input)
        {
            string lowerInput = input.ToLower();
            
            // This checks for natural language task creation
            if (TryParseTaskCreation(input))
            {
                return true;
            }
            
            // This checks for general task creation commands
            if (lowerInput.Contains("create task") || lowerInput.Contains("add task") || 
                lowerInput.Contains("new task") || lowerInput.Contains("make task"))
            {
                AppendToChatDisplay("Bot: I can help you create a task! You can either:", Brushes.Cyan);
                AppendToChatDisplay("Bot: • Use the Task Assistant panel on the right, or", Brushes.Cyan);
                AppendToChatDisplay("Bot: • Type 'Add task - [your task title]' here in the chat", Brushes.Yellow);
                AppendToChatDisplay("Bot: For example: 'Add task - Review privacy settings'", Brushes.Yellow);
                return true;
            }
            
            // This checks for task viewing commands
            if (lowerInput.Contains("show tasks") || lowerInput.Contains("list tasks") || 
                lowerInput.Contains("my tasks") || lowerInput.Contains("view tasks"))
            {
                AppendToChatDisplay($"Bot: You currently have {_tasks.Count} tasks in your list:", Brushes.Cyan);
                if (_tasks.Count == 0)
                {
                    AppendToChatDisplay("Bot: No tasks yet. Try typing 'Add task - [your task]' or use the Task Assistant panel!", Brushes.Yellow);
                }
                else
                {
                    foreach (var task in _tasks.Take(3)) // Show first 3 tasks
                    {
                        AppendToChatDisplay($"• {task.Title}", Brushes.White);
                    }
                    if (_tasks.Count > 3)
                    {
                        AppendToChatDisplay($"... and {_tasks.Count - 3} more. Check the Task Assistant panel for details.", Brushes.Gray);
                    }
                }
                return true;
            }
            
            // This checks for reminder-related commands
            if (lowerInput.Contains("remind me") || lowerInput.Contains("reminder"))
            {
                AppendToChatDisplay("Bot: I can help you set reminders! After creating a task, I'll ask if you want a reminder.", Brushes.Cyan);
                AppendToChatDisplay("Bot: Try: 'Add task - Update antivirus software'", Brushes.Yellow);
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Tries to parse natural language task creation
        /// </summary>
        /// <param name="input">The user's input</param>
        /// <returns>True if a task was created, false otherwise</returns>
        private bool TryParseTaskCreation(string input)
        {
            // This matches patterns like "Add task - Review privacy settings"
            var taskMatch = Regex.Match(input, @"add task\s*-\s*(.+)", RegexOptions.IgnoreCase);
            if (taskMatch.Success)
            {
                string taskTitle = taskMatch.Groups[1].Value.Trim();
                if (!string.IsNullOrEmpty(taskTitle))
                {
                    CreateTaskFromChat(taskTitle);
                    return true;
                }
            }
            
            // This matches patterns like "Create task to review privacy settings"
            var createMatch = Regex.Match(input, @"create task to (.+)", RegexOptions.IgnoreCase);
            if (createMatch.Success)
            {
                string taskTitle = createMatch.Groups[1].Value.Trim();
                if (!string.IsNullOrEmpty(taskTitle))
                {
                    CreateTaskFromChat(taskTitle);
                    return true;
                }
            }
            
            return false;
        }
        
        /// <summary>
        /// Creates a task from chat input and offers reminder setup
        /// </summary>
        /// <param name="taskTitle">The title of the task</param>
        private void CreateTaskFromChat(string taskTitle)
        {
            // This generates a description based on the title
            string description = GenerateTaskDescription(taskTitle);
            
            // This creates the task without a reminder initially
            var newTask = new CybersecurityTask(taskTitle, description, null);
            _tasks.Add(newTask);
            
            // This provides feedback and offers reminder setup
            AppendToChatDisplay($"Bot: Great! I've created the task '{taskTitle}' with the description '{description}'", Brushes.LightGreen);
            AppendToChatDisplay("Bot: Would you like me to set a reminder for this task? You can say:", Brushes.Cyan);
            AppendToChatDisplay("• 'Yes, remind me tomorrow'", Brushes.Yellow);
            AppendToChatDisplay("• 'Yes, remind me in 3 days'", Brushes.Yellow);
            AppendToChatDisplay("• 'Yes, remind me next week'", Brushes.Yellow);
            AppendToChatDisplay("• 'No thanks' (to skip the reminder)", Brushes.Yellow);
            
            // This sets a flag to expect a reminder response
            _waitingForReminderResponse = true;
            _lastCreatedTask = newTask;
        }
        
        /// <summary>
        /// Generates a cybersecurity-focused description based on the task title
        /// </summary>
        /// <param name="title">The task title</param>
        /// <returns>A generated description</returns>
        private string GenerateTaskDescription(string title)
        {
            string lowerTitle = title.ToLower();
            
            if (lowerTitle.Contains("password"))
                return "Review and strengthen passwords to ensure account security.";
            else if (lowerTitle.Contains("privacy") || lowerTitle.Contains("setting"))
                return "Check and update privacy settings to protect personal information.";
            else if (lowerTitle.Contains("update") || lowerTitle.Contains("software"))
                return "Install the latest security updates and software patches.";
            else if (lowerTitle.Contains("backup"))
                return "Create or verify backups of important data and files.";
            else if (lowerTitle.Contains("2fa") || lowerTitle.Contains("authentication") || lowerTitle.Contains("two factor"))
                return "Enable two-factor authentication for enhanced account security.";
            else if (lowerTitle.Contains("antivirus") || lowerTitle.Contains("virus"))
                return "Update and run antivirus software to protect against malware.";
            else if (lowerTitle.Contains("email") || lowerTitle.Contains("phishing"))
                return "Review email security settings and learn to identify phishing attempts.";
            else if (lowerTitle.Contains("wifi") || lowerTitle.Contains("network"))
                return "Secure wireless network connections and avoid public WiFi risks.";
            else
                return $"Complete the cybersecurity task: {title.ToLower()}.";
        }
        
        /// <summary>
        /// Processes reminder setup responses
        /// </summary>
        /// <param name="input">The user's input</param>
        /// <returns>True if the input was processed as a reminder response</returns>
        private bool ProcessReminderResponse(string input)
        {
            string lowerInput = input.ToLower();
            
            // This handles "no" responses
            if (lowerInput.Contains("no") || lowerInput.Contains("skip") || lowerInput.Contains("later"))
            {
                AppendToChatDisplay("Bot: No problem! The task has been created without a reminder.", Brushes.Cyan);
                AppendToChatDisplay("Bot: You can always set a reminder later using the Task Assistant panel.", Brushes.Cyan);
                _waitingForReminderResponse = false;
                _lastCreatedTask = null;
                return true;
            }
            
            // This handles "yes" responses with time specifications
            if (lowerInput.Contains("yes") || lowerInput.Contains("remind"))
            {
                DateTime? reminderDate = ParseReminderTime(input);
                if (reminderDate.HasValue && _lastCreatedTask != null)
                {
                    // This updates the task with the reminder date
                    _lastCreatedTask.ReminderDate = reminderDate;
                    
                    // This removes and re-adds the task to refresh the display
                    int taskIndex = _tasks.IndexOf(_lastCreatedTask);
                    if (taskIndex >= 0)
                    {
                        _tasks.RemoveAt(taskIndex);
                        _tasks.Insert(taskIndex, _lastCreatedTask);
                    }
                    
                    AppendToChatDisplay($"Bot: Perfect! I'll remind you about '{_lastCreatedTask.Title}' on {reminderDate.Value:dddd, MMMM dd, yyyy}.", Brushes.LightGreen);
                }
                else
                {
                    AppendToChatDisplay("Bot: I didn't quite understand the time. The task has been created without a reminder.", Brushes.Orange);
                    AppendToChatDisplay("Bot: You can set a reminder later using the Task Assistant panel.", Brushes.Cyan);
                }
                
                _waitingForReminderResponse = false;
                _lastCreatedTask = null;
                return true;
            }
            
            // This handles unclear responses
            AppendToChatDisplay("Bot: I didn't understand. Please say 'yes' to set a reminder or 'no' to skip it.", Brushes.Yellow);
            return true;
        }
        
        /// <summary>
        /// Parses reminder time from natural language input
        /// </summary>
        /// <param name="input">The user's input</param>
        /// <returns>The parsed reminder date or null if not understood</returns>
        private DateTime? ParseReminderTime(string input)
        {
            string lowerInput = input.ToLower();
            
            if (lowerInput.Contains("tomorrow"))
                return DateTime.Today.AddDays(1);
            else if (lowerInput.Contains("next week") || lowerInput.Contains("1 week"))
                return DateTime.Today.AddDays(7);
            else if (lowerInput.Contains("2 weeks"))
                return DateTime.Today.AddDays(14);
            else if (lowerInput.Contains("next month") || lowerInput.Contains("1 month"))
                return DateTime.Today.AddMonths(1);
            
            // This tries to parse specific day numbers
            var dayMatch = Regex.Match(input, @"(\d+)\s*days?", RegexOptions.IgnoreCase);
            if (dayMatch.Success && int.TryParse(dayMatch.Groups[1].Value, out int days))
            {
                return DateTime.Today.AddDays(days);
            }
            
            // This tries to parse specific week numbers
            var weekMatch = Regex.Match(input, @"(\d+)\s*weeks?", RegexOptions.IgnoreCase);
            if (weekMatch.Success && int.TryParse(weekMatch.Groups[1].Value, out int weeks))
            {
                return DateTime.Today.AddDays(weeks * 7);
            }
            
            return null;
        }
        
        /// <summary>
        /// Displays the help menu with available topics
        /// </summary>
        private void DisplayHelpMenu()
        {
            AppendToChatDisplay("Bot: Here are the available cybersecurity topics I can help with:", Brushes.Yellow);
            AppendToChatDisplay("• passwords - Learn about creating strong passwords", Brushes.Cyan);
            AppendToChatDisplay("• phishing - Understand and avoid phishing attacks", Brushes.Cyan);
            AppendToChatDisplay("• malware - Protect against malicious software", Brushes.Cyan);
            AppendToChatDisplay("• social engineering - Recognize manipulation tactics", Brushes.Cyan);
            AppendToChatDisplay("• data protection - Keep your information safe", Brushes.Cyan);
            AppendToChatDisplay("• public wifi - Stay secure on public networks", Brushes.Cyan);
            AppendToChatDisplay("• updates - Keep software current and secure", Brushes.Cyan);
            AppendToChatDisplay("• backup - Protect against data loss", Brushes.Cyan);
            AppendToChatDisplay("• 2fa - Two-factor authentication security", Brushes.Cyan);
            AppendToChatDisplay("", Brushes.White);
            AppendToChatDisplay("Bot: TASK ASSISTANT COMMANDS:", Brushes.Yellow);
            AppendToChatDisplay("• 'Add task - [task title]' - Create a new task", Brushes.LightGreen);
            AppendToChatDisplay("• 'Create task to [task description]' - Alternative creation", Brushes.LightGreen);
            AppendToChatDisplay("• 'Show my tasks' or 'List tasks' - View current tasks", Brushes.LightGreen);
            AppendToChatDisplay("• After creating a task, I'll ask about reminders", Brushes.LightGreen);
            AppendToChatDisplay("• Use the Edit button in the Task Assistant panel to modify tasks", Brushes.LightGreen);
            AppendToChatDisplay("", Brushes.White);
            AppendToChatDisplay("Bot: EXAMPLE TASK COMMANDS:", Brushes.Yellow);
            AppendToChatDisplay("• 'Add task - Review privacy settings'", Brushes.White);
            AppendToChatDisplay("• 'Add task - Enable two-factor authentication'", Brushes.White);
            AppendToChatDisplay("• 'Add task - Update antivirus software'", Brushes.White);
            AppendToChatDisplay("• 'Add task - Backup important files'", Brushes.White);
            AppendToChatDisplay("", Brushes.White);
            AppendToChatDisplay("Bot: REMINDER OPTIONS:", Brushes.Yellow);
            AppendToChatDisplay("• 'Yes, remind me tomorrow'", Brushes.Orange);
            AppendToChatDisplay("• 'Yes, remind me in 3 days'", Brushes.Orange);
            AppendToChatDisplay("• 'Yes, remind me next week'", Brushes.Orange);
            AppendToChatDisplay("• 'No thanks' (skip reminder)", Brushes.Orange);
            AppendToChatDisplay("", Brushes.White);
            AppendToChatDisplay("Bot: You can also use the Task Assistant panel on the right for a visual interface!", Brushes.Cyan);
        }
        
        /// <summary>
        /// Handles the Help button click event
        /// </summary>
        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayHelpMenu();
            AppendDividerToChat();
        }
        
        /// <summary>
        /// Handles the Clear Chat button click event
        /// </summary>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // This clears the chat display
            ChatDisplay.Document.Blocks.Clear();
            
            // This displays a confirmation message
            AppendToChatDisplay("Bot: Chat cleared. Type 'help' to see available topics.", Brushes.Yellow);
            AppendDividerToChat();
        }
        
        /// <summary>
        /// Appends text to the chat display with specified color
        /// </summary>
        /// <param name="text">The text to append</param>
        /// <param name="color">The color for the text</param>
        public void AppendToChatDisplay(string text, Brush color)
        {
            // This checks if it's a bot response to apply typing effect
            if (text.StartsWith("Bot:"))
            {
                AppendToChatDisplayWithTyping(text, color);
            }
            else
            {
                AppendToChatDisplayInstantly(text, color);
            }
        }
        
        /// <summary>
        /// Appends text instantly without typing effect
        /// </summary>
        /// <param name="text">The text to append</param>
        /// <param name="color">The color for the text</param>
        private void AppendToChatDisplayInstantly(string text, Brush color)
        {
            // This creates a new paragraph for the text
            Paragraph paragraph = new Paragraph();
            Run run = new Run(text) { Foreground = color };
            paragraph.Inlines.Add(run);
            
            // This adds the paragraph to the chat display
            ChatDisplay.Document.Blocks.Add(paragraph);
            
            // This scrolls to the bottom to show the latest message
            ChatScrollViewer.ScrollToEnd();
        }
        
        /// <summary>
        /// Appends text with typing effect for bot responses
        /// </summary>
        /// <param name="text">The text to append</param>
        /// <param name="color">The color for the text</param>
        private async void AppendToChatDisplayWithTyping(string text, Brush color)
        {
            // This creates a new paragraph for the typing effect
            Paragraph paragraph = new Paragraph();
            Run run = new Run("") { Foreground = color };
            paragraph.Inlines.Add(run);
            
            // This adds the paragraph to the chat display
            ChatDisplay.Document.Blocks.Add(paragraph);
            
            // This types each character with a small delay
            foreach (char c in text)
            {
                run.Text += c;
                
                // This scrolls to the bottom to show the latest character
                ChatScrollViewer.ScrollToEnd();
                
                // This creates a small delay between characters (15ms like original console version)
                await Task.Delay(15);
            }
        }
        
        /// <summary>
        /// Appends a visual divider to the chat display
        /// </summary>
        private void AppendDividerToChat()
        {
            AppendToChatDisplayInstantly("─".PadRight(50, '─'), Brushes.DarkCyan);
        }
        
        /// <summary>
        /// Handles the Add Task button click event
        /// </summary>
        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // This gets the input values
            string title = TaskTitleInput.Text.Trim();
            string description = TaskDescriptionInput.Text.Trim();
            DateTime? reminderDate = TaskReminderDate.SelectedDate;
            
            // This validates the input
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a task title.", "Title Required", 
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                TaskTitleInput.Focus();
                return;
            }
            
            if (string.IsNullOrEmpty(description))
            {
                MessageBox.Show("Please enter a task description.", "Description Required", 
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                TaskDescriptionInput.Focus();
                return;
            }
            
            // This validates the reminder date if provided
            if (reminderDate.HasValue && reminderDate.Value.Date < DateTime.Today)
            {
                var result = MessageBox.Show("The reminder date is in the past. Do you want to continue?", 
                                             "Past Date Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            
            // This creates the new task
            var newTask = new CybersecurityTask(title, description, reminderDate);
            
            // This adds the task to the collection
            _tasks.Add(newTask);
            
            // This clears the input fields
            TaskTitleInput.Text = "";
            TaskDescriptionInput.Text = "";
            TaskReminderDate.SelectedDate = null;
            DateDisplayBox.Text = "Click calendar icon to select date";
            DateDisplayBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
            
            // This shows a confirmation message
            string dateInfo = reminderDate.HasValue ? $" with reminder for {reminderDate.Value:MMM dd, yyyy}" : "";
            MessageBox.Show($"Task '{title}' has been added successfully{dateInfo}!", "Task Added", 
                            MessageBoxButton.OK, MessageBoxImage.Information);
            
            // This also adds a message to the chat
            AppendToChatDisplay($"Bot: Great! I've added the task '{title}' to your list{dateInfo}.", Brushes.LightGreen);
            AppendDividerToChat();
        }
        
        /// <summary>
        /// Handles the task list selection changed event
        /// </summary>
        private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This enables/disables the action buttons based on selection
            bool hasSelection = TaskListBox.SelectedItem != null;
            EditTaskButton.IsEnabled = hasSelection;
            CompleteTaskButton.IsEnabled = hasSelection;
            DeleteTaskButton.IsEnabled = hasSelection;
        }
        
        /// <summary>
        /// Handles the date picker selection changed event
        /// </summary>
        private void TaskReminderDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // This updates the display box when a date is selected
            if (TaskReminderDate.SelectedDate.HasValue)
            {
                DateDisplayBox.Text = TaskReminderDate.SelectedDate.Value.ToString("dddd, MMMM dd, yyyy");
                DateDisplayBox.Foreground = Brushes.White;
            }
            else
            {
                DateDisplayBox.Text = "Click calendar icon to select date";
                DateDisplayBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
            }
        }
        
        /// <summary>
        /// Handles the Edit Task button click event
        /// </summary>
        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is CybersecurityTask selectedTask)
            {
                // This loads the selected task into the edit fields
                TaskTitleInput.Text = selectedTask.Title;
                TaskDescriptionInput.Text = selectedTask.Description;
                TaskReminderDate.SelectedDate = selectedTask.ReminderDate;
                
                // This updates the display box
                if (selectedTask.ReminderDate.HasValue)
                {
                    DateDisplayBox.Text = selectedTask.ReminderDate.Value.ToString("dddd, MMMM dd, yyyy");
                    DateDisplayBox.Foreground = Brushes.White;
                }
                                 else
                 {
                     DateDisplayBox.Text = "Click calendar icon to select date";
                     DateDisplayBox.Foreground = new SolidColorBrush(Color.FromRgb(136, 136, 136));
                 }
                
                // This removes the original task from the list
                _tasks.Remove(selectedTask);
                
                // This provides feedback to the user
                AppendToChatDisplay($"Bot: I've loaded '{selectedTask.Title}' into the edit form. Make your changes and click 'Add Task' to save.", Brushes.Orange);
                AppendDividerToChat();
            }
        }
        
        /// <summary>
        /// Handles the Complete Task button click event
        /// </summary>
        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is CybersecurityTask selectedTask)
            {
                // This marks the task as completed and removes it from the list
                selectedTask.IsCompleted = true;
                _tasks.Remove(selectedTask);
                
                // This shows a confirmation message
                MessageBox.Show($"Task '{selectedTask.Title}' has been marked as completed!", "Task Completed", 
                                MessageBoxButton.OK, MessageBoxImage.Information);
                
                // This adds a message to the chat
                AppendToChatDisplay($"Bot: Excellent! You've completed the task '{selectedTask.Title}'. Keep up the great cybersecurity habits!", Brushes.LightGreen);
                AppendDividerToChat();
            }
        }
        
        /// <summary>
        /// Handles the Delete Task button click event
        /// </summary>
        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is CybersecurityTask selectedTask)
            {
                // This confirms the deletion with the user
                var result = MessageBox.Show($"Are you sure you want to delete the task '{selectedTask.Title}'?", 
                                             "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    // This removes the task from the collection
                    _tasks.Remove(selectedTask);
                    
                    // This adds a message to the chat
                    AppendToChatDisplay($"Bot: I've removed the task '{selectedTask.Title}' from your list.", Brushes.Orange);
                    AppendDividerToChat();
                }
            }
        }
        
        /// <summary>
        /// Creates a list of predefined cybersecurity tasks
        /// </summary>
        /// <returns>List of predefined tasks</returns>
        private List<CybersecurityTask> CreatePredefinedTasks()
        {
            return new List<CybersecurityTask>
            {
                new CybersecurityTask("Enable Two-Factor Authentication", 
                    "Set up 2FA on all important accounts (email, banking, social media)", 
                    DateTime.Today.AddDays(1)),
                
                new CybersecurityTask("Update All Software", 
                    "Check for and install updates for operating system, antivirus, and applications", 
                    DateTime.Today.AddDays(7)),
                
                new CybersecurityTask("Review Privacy Settings", 
                    "Check and update privacy settings on social media and online accounts", 
                    DateTime.Today.AddDays(3)),
                
                new CybersecurityTask("Password Security Audit", 
                    "Review and strengthen weak passwords, consider using a password manager", 
                    DateTime.Today.AddDays(2)),
                
                new CybersecurityTask("Backup Important Data", 
                    "Create or verify backups of important files and documents", 
                    DateTime.Today.AddDays(5))
            };
        }
        
        /// <summary>
        /// Loads the predefined tasks into the task list
        /// </summary>
        private void LoadPredefinedTasks()
        {
            // This adds the predefined tasks to help users get started
            foreach (var task in _predefinedTasks)
            {
                _tasks.Add(task);
            }
        }
    }
} 