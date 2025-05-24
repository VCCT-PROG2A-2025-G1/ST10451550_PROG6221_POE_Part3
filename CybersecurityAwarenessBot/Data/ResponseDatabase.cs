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
        
        // This stores follow-up questions for each topic
        private readonly Dictionary<string, List<string>> _followUpQuestions;
        
        // This stores detailed responses for common follow-up questions
        private readonly Dictionary<string, Dictionary<string, string>> _detailedResponses;
        
        // This stores direct answers to specific follow-up questions
        private readonly Dictionary<string, Dictionary<string, string>> _followUpResponses;
        
        // This creates a random number generator for selecting responses
        private readonly Random _random;
        
        // This tracks the last follow-up question asked
        private string _lastFollowUpQuestion = "";
        
        // This tracks whether we just answered a follow-up question
        private bool _isPostFollowUpState = false;
        
        // This stores the history of topics discussed by the user
        private List<string> _topicHistory = new List<string>();
        
        // This stores memory reference sentences to connect topics
        private List<string> _memoryReferences = new List<string>();
        
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
            
            // This loads follow-up questions for each topic
            _followUpQuestions = new Dictionary<string, List<string>>
            {
                { "password", new List<string> {
                    "Would you like to know how to create a strong password?",
                    "Would you like to learn about password managers?",
                    "Would you like to know about common password mistakes to avoid?"
                }},
                
                { "phishing", new List<string> {
                    "Would you like to know how to identify phishing emails?",
                    "Would you like to learn about common phishing tactics?",
                    "Would you like to know how to report phishing attempts?"
                }},
                
                { "malware", new List<string> {
                    "Would you like to know about different types of malware?",
                    "Would you like to learn about antivirus software recommendations?",
                    "Would you like to know about warning signs of malware infection?"
                }},
                
                { "social engineering", new List<string> {
                    "Would you like to learn about different social engineering tactics?",
                    "Would you like to know how organizations train against social engineering?",
                    "Would you like to see examples of real-world social engineering attacks?"
                }},
                
                { "data protection", new List<string> {
                    "Would you like to know more about encryption?",
                    "Would you like to learn about privacy settings for social media?",
                    "Would you like to know about data protection laws?"
                }},
                
                { "public wifi", new List<string> {
                    "Would you like to know more about VPNs?",
                    "Would you like to learn how to set up a VPN on your devices?",
                    "Would you like to know which activities to avoid on public WiFi?"
                }},
                
                { "updates", new List<string> {
                    "Would you like to know how to set up automatic updates?",
                    "Would you like to learn about zero-day vulnerabilities?",
                    "Would you like tips on managing updates across multiple devices?"
                }},
                
                { "backup", new List<string> {
                    "Would you like to know more about cloud backup options?",
                    "Would you like to learn how to create an automated backup schedule?",
                    "Would you like to know how to test your backups to ensure they're working?"
                }},
                
                { "2fa", new List<string> {
                    "Would you like to know about different types of two-factor authentication?",
                    "Would you like to learn which accounts should have 2FA enabled first?",
                    "Would you like to know how to set up 2FA on popular online services?"
                }}
            };
            
            // This stores detailed responses for common follow-up questions
            _detailedResponses = new Dictionary<string, Dictionary<string, string>>
            {
                { "password", new Dictionary<string, string> {
                    { "create", "To create a strong password: 1) Use at least 12 characters 2) Mix uppercase, lowercase, numbers and symbols 3) Avoid personal info like birthdays 4) Don't use dictionary words 5) Consider using a passphrase of random words with numbers and symbols mixed in" },
                    { "manager", "Password managers like LastPass, Bitwarden, or 1Password securely store all your passwords in an encrypted vault. You only need to remember one master password. They can generate strong unique passwords for each site and auto-fill them when needed." },
                    { "mistakes", "Common password mistakes include: 1) Reusing passwords across multiple sites 2) Using simple, easy-to-guess passwords 3) Writing passwords on sticky notes 4) Sharing passwords with others 5) Not changing passwords after a breach 6) Using personal information like birthdates" }
                }},
                
                { "phishing", new Dictionary<string, string> {
                    { "identify", "To identify phishing emails, look for: 1) Mismatched or suspicious sender addresses 2) Poor grammar/spelling 3) Urgent requests for action 4) Generic greetings like 'Dear Customer' 5) Suspicious attachments 6) Links that don't match the displayed text (hover to check)" },
                    { "report", "To report phishing: 1) Forward suspicious emails to your IT department if at work 2) Report to the company being impersonated through their official website 3) Forward phishing emails to the Anti-Phishing Working Group at reportphishing@apwg.org 4) Never reply to the suspicious email" },
                    { "received", "If you receive a phishing email: 1) Don't click any links or download attachments 2) Don't reply to it 3) Report it using the proper channels 4) Delete it from your inbox 5) If you did click a link, change your passwords immediately and run a security scan" }
                }},
                
                { "malware", new Dictionary<string, string> {
                    { "types", "Common malware types include: 1) Viruses - infect other files and spread 2) Trojans - disguised as legitimate software 3) Ransomware - encrypts your files and demands payment 4) Spyware - monitors your activities 5) Adware - displays unwanted ads 6) Worms - spread across networks automatically" },
                    { "remove", "To remove malware: 1) Disconnect from the internet 2) Boot in safe mode 3) Use reputable antivirus/anti-malware software to scan your system 4) Delete temporary files 5) If ransomware is involved, check nomoreransom.org for free decryption tools 6) As a last resort, restore from a clean backup" },
                    { "signs", "Warning signs of malware infection include: 1) Unexpected slowdowns 2) Frequent crashes or errors 3) Missing files 4) Strange pop-ups or browser redirects 5) Unfamiliar programs running 6) Unusual network activity 7) Disabled security software 8) Strange emails sent from your accounts" }
                }}
            };
            
            // This stores specific responses for each follow-up question
            _followUpResponses = new Dictionary<string, Dictionary<string, string>>
            {
                { "password", new Dictionary<string, string> {
                    { "Would you like to know how to create a strong password?", 
                      "To create a strong password: 1) Use at least 12-16 characters 2) Combine uppercase letters, lowercase letters, numbers, and symbols 3) Avoid sequential patterns and repeated characters 4) Don't use personal information 5) Consider using a passphrase (a series of random words with numbers and symbols). For example, 'Horse-Battery42-Staple!' is much stronger than 'Password123'." },
                    
                    { "Would you like to learn about password managers?", 
                      "Password managers are specialized applications that securely store all your passwords in an encrypted vault. The best ones (like LastPass, Bitwarden, 1Password) include features like: 1) Strong password generation 2) Auto-filling credentials 3) Secure sharing 4) Two-factor authentication 5) Security alerts for compromised passwords. Most offer free basic plans and premium features for a small subscription fee. They work across devices and browsers, so your passwords are always available but secure." },
                    
                    { "Would you like to know about common password mistakes to avoid?", 
                      "Common password mistakes to avoid: 1) Using simple, predictable passwords like 'password123' or 'qwerty' 2) Reusing the same password across multiple accounts 3) Sharing passwords with others 4) Using personal information like birthdays, names, or addresses 5) Not changing passwords after a security breach 6) Writing passwords on sticky notes near your computer 7) Storing passwords in unencrypted files 8) Using only the minimum required length 9) Not using multi-factor authentication when available." }
                }},
                
                { "phishing", new Dictionary<string, string> {
                    { "Would you like to know how to identify phishing emails?", 
                      "To identify phishing emails, look for: 1) Mismatched or suspicious sender addresses 2) Poor grammar/spelling 3) Urgent requests for action 4) Generic greetings like 'Dear Customer' 5) Suspicious attachments 6) Links that don't match the displayed text (hover to check) 7) Requests for personal information 8) Offers that seem too good to be true 9) Unexpected emails about accounts or deliveries 10) Threatening language about account closure or legal action. When in doubt, contact the company directly through their official website, not the email." },
                    
                    { "Would you like to learn about common phishing tactics?", 
                      "Common phishing tactics include: 1) Spoofing - impersonating trusted organizations like banks or tech companies 2) Spear phishing - personalized attacks targeting specific individuals with customized content 3) Business Email Compromise - impersonating executives to request fund transfers 4) Clone phishing - copying legitimate emails but replacing links with malicious ones 5) Vishing - voice phishing over phone calls 6) Smishing - SMS/text message phishing 7) Pharming - redirecting website traffic to fake sites 8) Whaling - targeting high-profile executives 9) Watering hole attacks - infecting websites frequently visited by targets." },
                    
                    { "Would you like to know how to report phishing attempts?", 
                      "To report phishing: 1) Forward suspicious emails to your IT department if at work 2) Report to the company being impersonated through their official website 3) Forward phishing emails to the Anti-Phishing Working Group at reportphishing@apwg.org 4) Report to your email provider (Gmail, Outlook, etc.) using their 'report phishing' feature 5) Submit to government agencies like the FTC at reportfraud.ftc.gov 6) Use your security software's reporting features 7) Never reply to the suspicious email or call numbers provided in it 8) Take screenshots for documentation if needed." }
                }},
                
                { "malware", new Dictionary<string, string> {
                    { "Would you like to know about different types of malware?", 
                      "Common malware types include: 1) Viruses - infect other files and spread when infected files are opened 2) Trojans - disguised as legitimate software but contain malicious code 3) Ransomware - encrypts your files and demands payment for decryption 4) Spyware - monitors your activities without consent 5) Adware - displays unwanted advertisements 6) Worms - spread across networks automatically without user interaction 7) Keyloggers - record keystrokes to steal passwords 8) Rootkits - gain privileged access while hiding their presence 9) Fileless malware - operates in memory without leaving files on disk 10) Cryptojackers - use your resources to mine cryptocurrency." },
                    
                    { "Would you like to learn about antivirus software recommendations?", 
                      "Recommended security tools: For paid antivirus: 1) Bitdefender - excellent protection with minimal system impact 2) Kaspersky - consistently high detection rates 3) Norton 360 - comprehensive suite with extras like VPN and password manager 4) ESET - lightweight with strong malware detection. For free options: 1) Microsoft Defender - built into Windows 10/11 and now quite effective 2) Avast Free - good protection with some privacy concerns 3) Malwarebytes Free - excellent for on-demand scanning 4) AVG Free - reliable basic protection. Consider supplementing with specialized tools like uBlock Origin (ad blocker) and Malwarebytes Anti-Exploit." },
                    
                    { "Would you like to know about warning signs of malware infection?", 
                      "Warning signs of malware infection: 1) Unexplained slowdowns or freezing 2) Programs crash frequently 3) Unfamiliar programs starting automatically 4) Browser redirects or changed homepage 5) Unusual pop-up advertisements 6) Antivirus or Task Manager won't open or has been disabled 7) Missing files or corrupted data 8) Unusual disk activity when you're not doing anything 9) Your contacts receive strange messages from your accounts 10) Battery drains quickly on mobile devices 11) Increased network activity even when not using the internet 12) Your webcam light activates without your knowledge. If you notice several of these signs, run a full security scan immediately." }
                }},
                
                { "social engineering", new Dictionary<string, string> {
                    { "Would you like to learn about different social engineering tactics?", 
                      "Common social engineering tactics include: 1) Pretexting - creating a fabricated scenario to extract information 2) Baiting - offering something enticing to trick victims into taking an action 3) Quid pro quo - offering a service or benefit in exchange for information 4) Tailgating - following someone into a secured area 5) Phishing - sending deceptive messages to trick people into revealing information 6) Vishing - voice phishing over phone calls 7) Impersonation - pretending to be someone else 8) Creating artificial urgency - pressuring victims to act quickly without thinking 9) Appeal to authority - claiming to be from management or law enforcement 10) Appeal to trust - exploiting existing relationships." },
                    
                    { "Would you like to know how organizations train against social engineering?", 
                      "Organizations train employees against social engineering through: 1) Simulated phishing campaigns - sending fake phishing emails to test awareness 2) Security awareness training - regular educational sessions on current threats 3) Clear security policies - documented procedures for information handling 4) Access control protocols - proper identification verification procedures 5) Incident response training - how to report and handle suspected attacks 6) Role-based training - specialized education for high-risk positions 7) Tabletop exercises - scenario-based practice sessions 8) Security champions programs - designating department-level security advocates 9) Regular security updates and newsletters 10) Rewards for reporting suspicious activities. The most effective programs combine regular training with real-world testing." },
                    
                    { "Would you like to see examples of real-world social engineering attacks?", 
                      "Notable real-world social engineering attacks include: 1) The 2020 Twitter hack where attackers called employees claiming to be from IT support to gain access to internal systems 2) The 2011 RSA breach where attackers sent employees emails with malicious Excel attachments labeled as 'recruitment plans' 3) The 2016 Ubiquiti Networks BEC scam where criminals impersonated executives and stole $46.7 million 4) The 2013 Target data breach that began with credentials phished from an HVAC contractor 5) The 2020 SolarWinds attack where attackers gained access through trojanized software updates 6) The 2018 case where a hacker called Apple and Amazon support to access a journalist's accounts 7) The classic case of Kevin Mitnick, who famously used social engineering to access corporate systems." }
                }},
                
                { "data protection", new Dictionary<string, string> {
                    { "Would you like to know more about encryption?", 
                      "Encryption is the process of converting information into a code to prevent unauthorized access. Key encryption concepts include: 1) Symmetric encryption - uses the same key to encrypt and decrypt (faster but key distribution is challenging) 2) Asymmetric encryption - uses public and private key pairs (slower but more secure for communications) 3) End-to-end encryption - only sender and recipient can read messages 4) Full disk encryption - protects all data on a storage device 5) File-level encryption - protects individual files 6) TLS/SSL - secures web connections 7) VPN encryption - protects internet traffic 8) Strong encryption uses algorithms like AES-256, RSA-2048, or ECC. For personal use, consider encrypted messaging apps (Signal, WhatsApp), password managers, and full-disk encryption options like BitLocker (Windows) or FileVault (Mac)." },
                    
                    { "Would you like to learn about privacy settings for social media?", 
                      "Essential social media privacy settings: For Facebook: 1) Set posts to 'Friends Only' 2) Review tagged photos before they appear on your profile 3) Limit past post visibility 4) Control app permissions 5) Use privacy checkup tool. For Instagram: 1) Set account to private 2) Control story sharing 3) Manage close friends list 4) Block unwanted users 5) Manage third-party app access. For Twitter: 1) Protect tweets 2) Disable location information 3) Control photo tagging 4) Limit data sharing. For LinkedIn: 1) Control connection viewing 2) Manage profile visibility 3) Control public profile visibility 4) Review data sharing settings. General tips: Regularly audit privacy settings, limit personal information shared, review connected apps, and disable location tracking when not needed." },
                    
                    { "Would you like to know about data protection laws?", 
                      "Major data protection laws include: 1) GDPR (European Union) - gives users control over personal data, requires explicit consent, and includes the right to be forgotten 2) CCPA/CPRA (California) - gives consumers rights to know what data is collected and opt-out of sales 3) POPIA (South Africa) - regulates how organizations process personal information 4) LGPD (Brazil) - similar to GDPR with focus on consent and user rights 5) PIPEDA (Canada) - requires consent for collection and disclosure of personal information 6) Privacy Act (Australia) - regulates handling of personal information by government agencies and private sector 7) HIPAA (US) - protects medical information 8) COPPA (US) - protects children's privacy online. These laws generally require organizations to implement data security measures, be transparent about data collection, and provide users with control over their information." }
                }},
                
                { "public wifi", new Dictionary<string, string> {
                    { "Would you like to know more about VPNs?", 
                      "VPNs (Virtual Private Networks) create an encrypted tunnel for your internet traffic, protecting your data from interception on public networks. Key VPN features to look for: 1) Strong encryption (OpenVPN, WireGuard, or IKEv2 protocols) 2) No-logs policy (verified by independent audits) 3) Kill switch (cuts internet if VPN disconnects) 4) DNS leak protection 5) Wide server network 6) Reliable speeds 7) Multi-device support. Reputable VPN providers include NordVPN, ExpressVPN, Surfshark, and ProtonVPN. Free VPNs often have limitations in security, speed, or data caps, and some sell your browsing data. Most quality VPNs cost $3-12 per month, with discounts for longer subscriptions." },
                    
                    { "Would you like to learn how to set up a VPN on your devices?", 
                      "Setting up a VPN is typically straightforward: 1) Choose and subscribe to a reputable VPN service 2) Download the provider's app from their official website or app store 3) Install the app following on-screen instructions 4) Launch the app and log in with your credentials 5) Select a server location (closer servers generally offer better speeds) 6) Click the connect button to establish the VPN connection. Most VPN apps allow you to enable auto-connect on untrusted networks and customize other security settings. For mobile devices, you can usually find VPN settings in iOS under Settings > General > VPN or in Android under Settings > Network & Internet > Advanced > VPN. Many VPN services allow connections on multiple devices simultaneously." },
                    
                    { "Would you like to know which activities to avoid on public WiFi?", 
                      "Activities to avoid on public WiFi without a VPN: 1) Online banking or accessing financial accounts 2) Making purchases or entering credit card information 3) Logging into work or business accounts 4) Accessing medical or insurance portals 5) Checking email (especially work email) 6) Using social media accounts 7) Filing taxes or accessing government services 8) Using cloud storage containing sensitive documents 9) Logging into password managers 10) Accessing cryptocurrency wallets. If you must perform these activities on public WiFi, always use a VPN, ensure the website uses HTTPS (look for the padlock icon), and consider using your phone's mobile data connection instead, which is generally more secure than public WiFi." }
                }},
                
                { "updates", new Dictionary<string, string> {
                    { "Would you like to know how to set up automatic updates?", 
                      "Setting up automatic updates: For Windows: 1) Go to Settings > Update & Security > Windows Update > Advanced options 2) Enable 'Receive updates for other Microsoft products' 3) Set active hours to avoid interruptions. For macOS: 1) Go to System Preferences > Software Update 2) Check 'Automatically keep my Mac up to date'. For Android: 1) Go to Settings > System > Advanced > System update 2) Enable auto-updates 3) Also update apps via Play Store > Settings > Auto-update apps. For iOS: 1) Go to Settings > General > Software Update > Automatic Updates. For browsers: 1) Chrome/Edge: Settings > About Chrome/Edge 2) Firefox: Options > General > Firefox Updates 3) Safari: Updates through macOS. For third-party software, look for update settings in individual applications or use update managers like Ninite (Windows) or MacUpdater (Mac)." },
                    
                    { "Would you like to learn about zero-day vulnerabilities?", 
                      "Zero-day vulnerabilities are software flaws unknown to the software creator that hackers discover and exploit before developers can create patches. Key points: 1) They're called 'zero-day' because developers have zero days to fix them before exploitation 2) They're highly valuable and sometimes sold on dark web markets for thousands of dollars 3) Nation-states and advanced threat actors often use them for targeted attacks 4) They represent a significant risk because traditional security tools often can't detect them 5) Defense requires layered security: keeping software minimized, implementing principle of least privilege, using behavior-based security tools, application whitelisting, and network segmentation 6) Regular backups are crucial for recovery if a zero-day attack succeeds 7) Once discovered, they become race conditions between attackers exploiting them and developers patching them." },
                    
                    { "Would you like tips on managing updates across multiple devices?", 
                      "Tips for managing updates across multiple devices: 1) Create an inventory of all devices and software requiring updates 2) Set a regular schedule for checking update status (monthly for less critical systems, weekly for critical ones) 3) Stagger updates rather than updating everything simultaneously to avoid widespread disruption 4) Test updates on non-critical systems before deploying widely 5) Use centralized update management tools like Microsoft WSUS for Windows, Jamf for Apple devices, or cross-platform solutions like ManageEngine 6) Consider auto-update for security patches but manual for feature updates 7) Keep offline backups before major updates 8) Document your update process and maintain change logs 9) Set calendar reminders for devices or software without auto-update capabilities 10) For IoT devices, check manufacturer websites regularly as many lack auto-update functionality." }
                }},
                
                { "backup", new Dictionary<string, string> {
                    { "Would you like to know more about cloud backup options?", 
                      "Popular cloud backup options include: 1) Microsoft OneDrive - Integrated with Windows, offers 5GB free, good for Office files 2) Google Drive - 15GB free (shared with Gmail), strong collaboration features 3) Dropbox - 2GB free, excellent sync capabilities 4) iCloud - Integrated with Apple devices, 5GB free 5) IDrive - 10GB free, supports unlimited devices 6) Backblaze - Unlimited storage for $7/month per computer, very simple interface 7) Carbonite - Unlimited storage with automatic backup 8) pCloud - One-time payment options available. Cloud backup advantages include offsite protection, automatic syncing, and accessibility from anywhere. Disadvantages include ongoing costs, privacy concerns, and dependency on internet connection. For sensitive data, look for services offering zero-knowledge encryption where only you hold the decryption key." },
                    
                    { "Would you like to learn how to create an automated backup schedule?", 
                      "Creating an automated backup schedule: For Windows: 1) Use File History: Settings > Update & Security > Backup > Add a drive > More options to set frequency 2) For full system backup: Control Panel > Backup and Restore (Windows 7) > Create a system image. For Mac: 1) Set up Time Machine: System Preferences > Time Machine > Select Backup Disk > Options to customize. For external devices: 1) Windows: Task Scheduler to run backup software at specific times 2) Mac: Automator or third-party apps like Carbon Copy Cloner. Cloud backup automation: 1) Most services like OneDrive, Google Drive have auto-sync options in their settings 2) Set specific folders to always backup automatically. Best practices: 1) Daily incremental backups for working files 2) Weekly full backups 3) Monthly system images 4) Verify backups regularly by testing restoration 5) Keep at least one backup disconnected from your network to protect against ransomware." },
                    
                    { "Would you like to know how to test your backups to ensure they're working?", 
                      "Testing your backups effectively: 1) Regular restoration tests - Attempt to restore random files monthly and complete test restores quarterly 2) Verification checks - Use backup software's verification feature to confirm data integrity 3) Test on different hardware - Ensure you can restore to different devices if your primary device fails 4) Document the restoration process - Create step-by-step instructions for restoration in emergency situations 5) Check for corruption - Open restored files to verify they function correctly 6) Test bootable backups - If you have system image backups, test if they can boot properly 7) Simulate disaster recovery - Practice a complete restoration scenario annually 8) Review backup logs - Check for errors or warnings in your backup software logs 9) Calculate restoration time - Measure how long full restores take to inform your disaster recovery planning 10) Audit backup coverage - Ensure all critical files and directories are included in your backup plan." }
                }},
                
                { "2fa", new Dictionary<string, string> {
                    { "Would you like to know about different types of two-factor authentication?", 
                      "The main types of two-factor authentication methods include: 1) SMS codes - messages sent to your phone (convenient but vulnerable to SIM swapping) 2) Authenticator apps - like Google Authenticator or Authy that generate time-based codes 3) Security keys - physical devices like YubiKey that you plug in or tap 4) Biometrics - fingerprints, face recognition, or voice identification 5) Push notifications - approve login attempts through a trusted app 6) Backup codes - one-time use codes for when other methods aren't available 7) Email verification codes - sent to your email address as a secondary factor. Authenticator apps and security keys offer the best balance of security and convenience, while SMS is better than no 2FA but has known vulnerabilities." },
                    
                    { "Would you like to learn which accounts should have 2FA enabled first?", 
                      "You should prioritize enabling 2FA on these accounts first: 1) Email accounts - they're often used for password resets and are the keys to your digital kingdom 2) Financial accounts - banks, credit cards, payment apps, cryptocurrency wallets 3) Cloud storage - where your sensitive files are stored 4) Social media - to prevent identity theft and reputation damage 5) Work accounts - especially those with access to sensitive company data 6) Password managers - they store all your other passwords 7) Government/tax accounts - contain highly sensitive personal information 8) Health and insurance portals - contain private medical information 9) Shopping and e-commerce accounts with saved payment methods 10) Gaming accounts with in-game purchases or valuable items. Start with your primary email account, as it's often the recovery method for other accounts." },
                    
                    { "Would you like to know how to set up 2FA on popular online services?", 
                      "To set up 2FA on popular services: 1) Google: Go to myaccount.google.com > Security > 2-Step Verification 2) Microsoft: Account.microsoft.com > Security > Advanced security > Two-step verification 3) Apple: appleid.apple.com > Sign in > Security > Two-Factor Authentication 4) Facebook: Settings > Security and Login > Two-Factor Authentication 5) Twitter: Settings and privacy > Security > Two-factor authentication 6) Instagram: Settings > Security > Two-factor authentication 7) Amazon: Account & Lists > Account > Login & security > Edit Two-Step Verification 8) PayPal: Settings > Security > Security Key 9) Dropbox: Settings > Security > Two-factor authentication 10) LinkedIn: Settings > Sign in & security > Two-step verification. Most services follow a similar pattern: access security settings, enable 2FA, and follow the setup wizard to add your phone or authenticator app." }
                }}
            };
            
            // Initialize memory reference sentences
            _memoryReferences = new List<string>
            {
                "I'm glad you're exploring both {0} and {1} - building strong cybersecurity knowledge!",
                "Great to see you're learning about {0} and now {1}!",
                "Excellent! After discussing {0}, let's cover {1}.",
                "Building on your {0} question, here's information about {1}.",
                "Since you asked about {0} earlier, you'll find {1} equally important!"
            };
        }
        
        /// <summary>
        /// Gets a response for the given user input
        /// </summary>
        /// <param name="userInput">The user's input</param>
        /// <param name="userName">The user's name for personalization</param>
        /// <param name="currentTopic">The current topic being discussed (if any)</param>
        /// <param name="lastFollowUp">The last follow-up question asked by the bot (if any)</param>
        /// <returns>A cybersecurity response based on the input and context</returns>
        public string GetResponse(string userInput, string userName, string currentTopic = "", string lastFollowUp = "")
        {
            // This converts the input to lowercase for case-insensitive matching
            userInput = userInput.ToLower();
            
            // This handles post-follow-up state when user says yes/no to "Would you like to learn more about [topic]?"
            if (_isPostFollowUpState)
            {
                // This resets the post-follow-up state
                _isPostFollowUpState = false;
                
                // This handles "yes" response (user wants more info on same topic)
                if (userInput.Contains("yes") || userInput.Contains("sure") || userInput.Contains("ok") || 
                    userInput.Contains("yeah") || userInput.Contains("yep"))
                {
                    // This gives a random response from the current topic
                    if (!string.IsNullOrEmpty(currentTopic) && _responses.ContainsKey(currentTopic))
                    {
                        int responseIndex = _random.Next(0, _responses[currentTopic].Count);
                        string selectedResponse = _responses[currentTopic][responseIndex];
                        return $"{userName}, {selectedResponse}\n\nType 'help' to see available topics, or ask me about cybersecurity.";
                    }
                }
                
                // This handles "no" response (user wants to change topics)
                // No need to check for "no" explicitly - we'll return the default message for any non-yes response
                return "Type 'help' to see available topics, or ask me about cybersecurity.";
            }
            
            // This saves the last follow-up question for future reference
            _lastFollowUpQuestion = lastFollowUp;
            
            // This checks if this is a direct response to a follow-up question
            if (!string.IsNullOrEmpty(lastFollowUp) && 
                (userInput.Contains("yes") || userInput.Contains("sure") || userInput.Contains("ok") || 
                 userInput.Contains("please") || userInput.Contains("tell me")))
            {
                // This looks for a direct answer to the specific follow-up question
                if (_followUpResponses.ContainsKey(currentTopic) && 
                    _followUpResponses[currentTopic].ContainsKey(lastFollowUp))
                {
                    string specificAnswer = _followUpResponses[currentTopic][lastFollowUp];
                    
                    // This sets the post-follow-up state so we can ask if they want more info on this topic
                    _isPostFollowUpState = true;
                    
                    return $"{userName}, {specificAnswer}\n\nWould you like to learn more about {currentTopic}?";
                }
            }
            
            // This checks if this is a follow-up to a previous topic
            if (!string.IsNullOrEmpty(currentTopic) && _followUpQuestions.ContainsKey(currentTopic))
            {
                // This checks for common follow-up subtopics
                if (_detailedResponses.ContainsKey(currentTopic))
                {
                    foreach (var subTopic in _detailedResponses[currentTopic].Keys)
                    {
                        if (userInput.Contains(subTopic))
                        {
                            string detailedResponse = _detailedResponses[currentTopic][subTopic];
                            return $"{userName}, {detailedResponse}. Is there something else about {currentTopic} you'd like to know?";
                        }
                    }
                }
                
                // This checks for topic switching
                string newTopic = FindTopicInInput(userInput);
                if (!string.IsNullOrEmpty(newTopic) && newTopic != currentTopic && newTopic != "help")
                {
                    // This handles switching to a new topic
                    return GetTopicResponse(newTopic, userName, true);
                }
                
                // This handles vague follow-up responses (moved lower in priority)
                if ((userInput.Contains("yes") || userInput.Contains("sure") || userInput.Contains("ok")) &&
                    string.IsNullOrEmpty(lastFollowUp))
                {
                    // This gives more information about the current topic
                    return GetTopicResponse(currentTopic, userName, false);
                }
                
                // This handles negative follow-up responses
                if (userInput.Contains("no") || userInput.Contains("not") || userInput.Contains("different"))
                {
                    return $"What cybersecurity topic would you like to learn about instead, {userName}?";
                }
            }
            
            // This handles requests for available topics
            if (userInput.Contains("help") || userInput.Contains("topics"))
            {
                // This returns the help message (only one variation)
                return _responses["help"][0];
            }
            
            // This creates a list to store found topics
            List<string> foundTopics = new List<string>();
            
            // This checks for specific topics in the user input
            foreach (var topic in _responses.Keys)
            {
                if (userInput.Contains(topic) && topic != "help")
                {
                    // This adds the topic to our found topics list
                    foundTopics.Add(topic);
                }
            }
            
            // This handles the case when multiple topics are found
            if (foundTopics.Count >= 2)
            {
                // This limits to first 2 topics to keep it simple
                var topic1 = foundTopics[0];
                var topic2 = foundTopics[1];
                return $"I noticed you mentioned both {topic1} and {topic2}. Which would you like to discuss first?";
            }
            
            // This handles when a single topic is found
            if (foundTopics.Count == 1)
            {
                string topic = foundTopics[0];
                return GetTopicResponse(topic, userName, false);
            }
            
            // This provides a default response when no matching topic is found
            return $"I'm sorry {userName}, I don't have information about that topic yet. Type 'help' to see available topics.";
        }
        
        /// <summary>
        /// Gets a response for a specific topic with optional follow-up question
        /// </summary>
        /// <param name="topic">The topic to get a response for</param>
        /// <param name="userName">The user's name for personalization</param>
        /// <param name="isTopicSwitch">Whether this is a topic switch in conversation</param>
        /// <returns>A response with follow-up question</returns>
        private string GetTopicResponse(string topic, string userName, bool isTopicSwitch)
        {
            // This randomly selects one of the available responses for the topic
            int responseIndex = _random.Next(0, _responses[topic].Count);
            string selectedResponse = _responses[topic][responseIndex];
            
            // This randomly selects a follow-up question if available
            string followUp = "";
            if (_followUpQuestions.ContainsKey(topic))
            {
                int followUpIndex = _random.Next(0, _followUpQuestions[topic].Count);
                followUp = _followUpQuestions[topic][followUpIndex];
                
                // This saves the selected follow-up question
                _lastFollowUpQuestion = followUp;
            }
            
            // This creates a transition phrase for topic switches
            string transition = isTopicSwitch ? $"Switching to {topic}. " : "";
            
            // Create memory reference if this isn't the first topic
            string memoryReference = "";
            if (topic != "help" && !_topicHistory.Contains(topic))
            {
                _topicHistory.Add(topic);
                
                // Add memory reference if this is at least the second topic
                if (_topicHistory.Count >= 2)
                {
                    string previousTopic = _topicHistory[_topicHistory.Count - 2];
                    int memoryIndex = _random.Next(0, _memoryReferences.Count);
                    memoryReference = string.Format(_memoryReferences[memoryIndex], previousTopic, topic) + " ";
                }
            }
            
            // This combines the response with follow-up question
            return $"{transition}{memoryReference}{userName}, {selectedResponse}\n\n{followUp}";
        }
        
        /// <summary>
        /// Finds a cybersecurity topic in the user input
        /// </summary>
        /// <param name="userInput">The user's input</param>
        /// <returns>The found topic or empty string if none found</returns>
        private string FindTopicInInput(string userInput)
        {
            foreach (var topic in _responses.Keys)
            {
                if (userInput.Contains(topic) && topic != "help")
                {
                    return topic;
                }
            }
            
            return "";
        }
        
        /// <summary>
        /// Gets the last follow-up question asked
        /// </summary>
        /// <returns>The last follow-up question</returns>
        public string GetLastFollowUpQuestion()
        {
            return _lastFollowUpQuestion;
        }
        
        /// <summary>
        /// Checks if we're in a post-follow-up state waiting for yes/no
        /// </summary>
        /// <returns>True if in post-follow-up state</returns>
        public bool IsInPostFollowUpState()
        {
            return _isPostFollowUpState;
        }
        
        /// <summary>
        /// Resets the post-follow-up state
        /// </summary>
        public void ResetPostFollowUpState()
        {
            _isPostFollowUpState = false;
        }
        
        /// <summary>
        /// Clears the topic history
        /// </summary>
        public void ClearTopicHistory()
        {
            _topicHistory.Clear();
        }
    }
} 