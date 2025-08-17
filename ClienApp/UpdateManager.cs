using NetSparkleUpdater;
using NetSparkleUpdater.Enums;
using NetSparkleUpdater.SignatureVerifiers;
using NetSparkleUpdater.UI.WPF;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;

namespace ClientApp
{
    /// <summary>
    /// Manages automatic updates for the ProfessionalClientApp using NetSparkle
    /// </summary>
    public class UpdateManager
    {
        private SparkleUpdater _sparkle;
        private readonly string _appCastUrl;
        private readonly string _publicKey;
        private bool _isInitialized = false;

        // Configuration - Update these values for your deployment
        private const string DEFAULT_APPCAST_URL = "https://your-domain.com/appcast.xml";
        private const string DEFAULT_PUBLIC_KEY = "YOUR_BASE64_PUBLIC_KEY_HERE";

        public UpdateManager(string appCastUrl = null, string publicKey = null)
        {
            _appCastUrl = appCastUrl ?? DEFAULT_APPCAST_URL;
            _publicKey = publicKey ?? DEFAULT_PUBLIC_KEY;
        }

        /// <summary>
        /// Initialize the update manager
        /// </summary>
        /// <param name="applicationIcon">Application icon for update dialogs</param>
        /// <returns>True if initialization was successful</returns>
        public bool Initialize(Icon applicationIcon = null)
        {
            try
            {
                // Create the SparkleUpdater instance
                _sparkle = new SparkleUpdater(
                    _appCastUrl,
                    new Ed25519Checker(SecurityMode.Strict, _publicKey)
                )
                {
                    UIFactory = new UIFactory(applicationIcon),
                    RelaunchAfterUpdate = true, // Automatically restart after update
                    ShowsUIOnMainThread = true,
                    CheckFrequency = TimeSpan.FromHours(24), // Check daily
                    CustomInstallerArguments = "/SILENT" // Silent installer arguments
                };

                // Subscribe to events
                _sparkle.UpdateDetected += OnUpdateDetected;
                _sparkle.UpdateCheckFinished += OnUpdateCheckFinished;
                _sparkle.PreparingToExit += OnPreparingToExit;
                _sparkle.UserSkippedVersion += OnUserSkippedVersion;

                _isInitialized = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize update manager: {ex.Message}", 
                    "Update Manager Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }

        /// <summary>
        /// Start the automatic update checking loop
        /// </summary>
        /// <param name="doInitialCheck">Whether to perform an initial check immediately</param>
        public void StartUpdateLoop(bool doInitialCheck = false)
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("UpdateManager must be initialized before starting update loop");
            }

            _sparkle.StartLoop(doInitialCheck);
        }

        /// <summary>
        /// Stop the automatic update checking loop
        /// </summary>
        public void StopUpdateLoop()
        {
            if (_isInitialized)
            {
                _sparkle?.StopLoop();
            }
        }

        /// <summary>
        /// Manually check for updates (user-initiated)
        /// </summary>
        public async Task CheckForUpdatesAsync()
        {
            if (!_isInitialized)
            {
                MessageBox.Show("Update manager is not initialized", "Update Check", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                await Task.Run(() => _sparkle.CheckForUpdatesAtUserRequest());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking for updates: {ex.Message}", 
                    "Update Check Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Check for updates quietly (no UI shown if no updates)
        /// </summary>
        /// <returns>Update information if available</returns>
        public async Task<UpdateInfo> CheckForUpdatesQuietlyAsync()
        {
            if (!_isInitialized)
            {
                return null;
            }

            try
            {
                return await Task.Run(() => _sparkle.CheckForUpdatesQuietly());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Silent update check failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Get the current application version
        /// </summary>
        public string GetCurrentVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
        }

        /// <summary>
        /// Dispose of resources
        /// </summary>
        public void Dispose()
        {
            if (_sparkle != null)
            {
                _sparkle.UpdateDetected -= OnUpdateDetected;
                _sparkle.UpdateCheckFinished -= OnUpdateCheckFinished;
                _sparkle.PreparingToExit -= OnPreparingToExit;
                _sparkle.UserSkippedVersion -= OnUserSkippedVersion;

                _sparkle.Dispose();
                _sparkle = null;
            }
            _isInitialized = false;
        }

        #region Event Handlers

        private void OnUpdateDetected(object sender, UpdateDetectedEventArgs e)
        {
            // Log update detection
            System.Diagnostics.Debug.WriteLine($"Update detected: {e.NextVersion}");
            
            // You can add custom logic here before showing the update UI
            // For example, check if the user is in the middle of important work
        }

        private void OnUpdateCheckFinished(object sender, bool updateRequired)
        {
            System.Diagnostics.Debug.WriteLine($"Update check finished. Update required: {updateRequired}");
            
            if (!updateRequired)
            {
                // Optionally log that no updates were found
                System.Diagnostics.Debug.WriteLine("No updates available");
            }
        }

        private void OnPreparingToExit(object sender, CancelEventArgs e)
        {
            // This event is fired when the application is about to exit for an update
            // You can use this to save user data, close connections, etc.
            
            var result = MessageBox.Show(
                "The application needs to restart to complete the update. Any unsaved work will be lost. Continue?",
                "Update Restart Required",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true; // Cancel the update process
            }
            else
            {
                // Perform any necessary cleanup here
                // Save user preferences, close database connections, etc.
                System.Diagnostics.Debug.WriteLine("Preparing to exit for update...");
            }
        }

        private void OnUserSkippedVersion(object sender, SkipVersionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"User skipped version: {e.Version}");
        }

        #endregion

        #region Static Helper Methods

        /// <summary>
        /// Create and configure an UpdateManager instance with default settings
        /// </summary>
        /// <param name="appIcon">Application icon for dialogs</param>
        /// <returns>Configured UpdateManager instance</returns>
        public static UpdateManager CreateDefault(Icon appIcon = null)
        {
            var updateManager = new UpdateManager();
            updateManager.Initialize(appIcon);
            return updateManager;
        }

        /// <summary>
        /// Check if updates are configured and available
        /// </summary>
        /// <returns>True if update system is properly configured</returns>
        public static bool IsUpdateSystemConfigured()
        {
            // Check if the default values have been replaced with actual configuration
            return !DEFAULT_APPCAST_URL.Contains("your-domain.com") && 
                   !DEFAULT_PUBLIC_KEY.Contains("YOUR_BASE64_PUBLIC_KEY_HERE");
        }

        #endregion
    }

    /// <summary>
    /// Extension methods for easier integration
    /// </summary>
    public static class UpdateManagerExtensions
    {
        /// <summary>
        /// Add update checking to a WPF application
        /// </summary>
        /// <param name="app">The WPF Application instance</param>
        /// <param name="updateManager">The UpdateManager instance</param>
        public static void EnableAutoUpdates(this Application app, UpdateManager updateManager)
        {
            if (updateManager != null)
            {
                // Start update checking when application starts
                app.Startup += (sender, e) =>
                {
                    updateManager.StartUpdateLoop(doInitialCheck: true);
                };

                // Clean up when application exits
                app.Exit += (sender, e) =>
                {
                    updateManager.Dispose();
                };
            }
        }
    }
}

