using System;
using System.Media;
using System.IO;

//------------------------------------------------------------------------------------------------------------------------

namespace CybersecurityAwarenessBot.Audio
{
    /// <summary>
    /// Manages audio playback for the Cybersecurity Awareness Chatbot
    /// </summary>
    public class AudioManager
    {
        // This stores the path to the audio files directory
        private readonly string _audioDirectory;
        
        /// <summary>
        /// Initializes a new instance of the AudioManager class
        /// </summary>
        /// <param name="audioDirectory">Directory containing audio files (defaults to application directory)</param>
        public AudioManager(string audioDirectory = null)
        {
            // This sets up the audio directory path with a default if not provided
            _audioDirectory = audioDirectory ?? AppDomain.CurrentDomain.BaseDirectory;
        }
        
        /// <summary>
        /// Plays a voice greeting when the application starts
        /// </summary>
        /// <returns>True if played successfully, false otherwise</returns>
        public bool PlayVoiceGreeting()
        {
            // This attempts to play the greeting audio file from the Audio subfolder
            bool result = false;
            
            try
            {
                // This builds the path to the greeting file in the Audio subfolder
                string audioFilePath = Path.Combine(_audioDirectory, "Audio", "greeting.wav");
                
                // This checks if the file exists before attempting to play it
                if (File.Exists(audioFilePath))
                {
                    using (SoundPlayer player = new SoundPlayer(audioFilePath))
                    {
                        player.Play();
                    }
                    result = true;
                }
            }
            catch (Exception)
            {
                // This silently handles any errors during audio playback
                result = false;
            }
            
            // This provides a fallback message if audio fails
            if (!result)
            {
                Console.WriteLine("Welcome to the Cybersecurity Awareness Chatbot!");
            }
            
            return result;
        }
        
        /// <summary>
        /// Plays an audio file from the audio directory
        /// </summary>
        /// <param name="fileName">Name of the audio file to play</param>
        /// <param name="useAudioSubfolder">Whether to first look in the Audio subfolder</param>
        /// <returns>True if played successfully, false otherwise</returns>
        public bool PlayAudioFile(string fileName, bool useAudioSubfolder = false)
        {
            try
            {
                string filePath;
                
                // This determines the file path based on the subfolder preference
                if (useAudioSubfolder)
                {
                    filePath = Path.Combine(_audioDirectory, "Audio", fileName);
                }
                else
                {
                    filePath = Path.Combine(_audioDirectory, fileName);
                }
                
                // This checks if the file exists before attempting to play it
                if (!File.Exists(filePath))
                {
                    return false;
                }
                
                // This creates a new SoundPlayer and plays the audio file
                using (SoundPlayer player = new SoundPlayer(filePath))
                {
                    player.Play();
                }
                
                return true;
            }
            catch (Exception)
            {
                // This silently handles any errors during audio playback
                return false;
            }
        }
        
        /// <summary>
        /// Plays an audio file synchronously (waits for completion)
        /// </summary>
        /// <param name="fileName">Name of the audio file to play</param>
        /// <param name="useAudioSubfolder">Whether to first look in the Audio subfolder</param>
        /// <returns>True if played successfully, false otherwise</returns>
        public bool PlayAudioFileSync(string fileName, bool useAudioSubfolder = false)
        {
            // This maintains the public interface but just calls the regular method
            // since synchronous playback isn't needed for this assignment
            return PlayAudioFile(fileName, useAudioSubfolder);
        }
    }
}

//--------------------------------------------------End of File--------------------------------------------------