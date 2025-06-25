using System;
using System.Windows;

namespace CybersecurityAwarenessBot
{
    /// <summary>
    /// Interaction logic for App.xaml - handles application startup and global events
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Handles application startup event
        /// </summary>
        /// <param name="e">Startup event arguments</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                // This calls the base startup method
                base.OnStartup(e);
                
                // This sets up global exception handling
                AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
                DispatcherUnhandledException += OnDispatcherUnhandledException;
            }
            catch (Exception ex)
            {
                // This handles any errors that may occur during startup 
                MessageBox.Show($"An error occurred during application startup: {ex.Message}", 
                                "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }
        
        /// <summary>
        /// Handles unhandled exceptions in the application domain
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Unhandled exception event arguments</param>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // This displays and logs critical errors
            if (e.ExceptionObject is Exception exception)
            {
                MessageBox.Show($"A critical error occurred: {exception.Message}", 
                                "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        /// <summary>
        /// Handles unhandled exceptions in the UI dispatcher
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Dispatcher unhandled exception event arguments</param>
        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // This handles UI thread exceptions gracefully
            MessageBox.Show($"An error occurred in the user interface: {e.Exception.Message}", 
                            "UI Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            
            // This marks the exception as handled to prevent application crash
            e.Handled = true;
        }
    }
} 
 