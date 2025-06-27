# ST10451550_PROG6221_POE

VIDEO LINK: https://youtu.be/Xzm56LtCzlA
# Cybersecurity Awareness Chatbot
This is a comprehensive WPF-based educational chatbot designed to raise cybersecurity awareness among South African citizens with intelligent conversation capabilities, task management, quiz system, and activity tracking.

# Project Description
This cybersecurity awareness chatbot provides dynamic information on various cybersecurity topics, including phishing, password safety, and safe browsing practices. It features an intelligent conversation system, interactive quiz game, task management with reminders, activity logging, and enhanced natural language processing that adapts to user interactions.

# Features
- Interactive conversation with personalised responses
- Information on 9+ cybersecurity topics with response variations
- Intelligent conversation flow with relevant follow-up questions and specific answers
- Memory system that recalls and connects previously discussed topics
- Sentiment detection with empathetic responses to user emotions
- Enhanced natural language processing with action-based keyword detection
- Task management system with reminders for cybersecurity activities
- Interactive cybersecurity quiz with 30+ questions and immediate feedback
- Comprehensive activity logging and progress tracking
- Professional WPF interface with tabbed navigation
- Voice greeting with ASCII art branding
- Natural language task creation using chat commands
- Multiple keyword detection for complex queries
- Enhanced error handling and edge case management

# How to Use
1. Run the application
2. Enter your name when prompted in the welcome dialogue
3. Use the Chat tab to ask questions about cybersecurity topics
4. Create tasks using the Task Assistant tab or natural language commands
5. Take the cybersecurity quiz to test your knowledge
6. View your activity history in the Activity Log tab
7. Type 'help' in chat to see available topics and commands
8. Use natural language for task creation (e.g., "remind me to backup files tomorrow")
9. Navigate between tabs for different features

# Technical Details
- Built in C# as a WPF application using XAML
- Enhanced modular architecture with conversation state management
- Advanced input validation and keyword recognition
- Memory system for topic tracking and recall with natural transitions
- Sentiment detection using keyword-based emotion recognition
- Natural language processing for action-based task creation
- Dictionary-based response variations with random selection
- Comprehensive error handling for robust user experience
- Activity logging system with timestamped entries
- Quiz system with scoring and feedback mechanisms
- Task management with reminder calculations and date parsing

# Project Structure
- Core: Contains the chatbot engines, quiz manager, and conversation logic
- UI: Handles user interface and display functionality (both console and WPF)
- Data: Manages cybersecurity response content, quiz questions, tasks, and activity entries
- Audio: Manages voice greeting functionality
- Utilities: Activity logging and tracking systems
- MainWindow.xaml/cs: Primary WPF interface and application logic
- App.xaml/cs: WPF application configuration and startup