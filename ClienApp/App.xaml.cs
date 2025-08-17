using System.Configuration;
using System.Data;
using System.Windows;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private UpdateManager _updateManager;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize update manager
            InitializeUpdateManager();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Clean up update manager
            _updateManager?.Dispose();
            base.OnExit(e);
        }

        private void InitializeUpdateManager()
        {
            try
            {
                // Check if update system is properly configured
                if (UpdateManager.IsUpdateSystemConfigured())
                {
                    _updateManager = UpdateManager.CreateDefault();
                    
                    if (_updateManager != null)
                    {
                        // Enable auto-updates for this application
                        this.EnableAutoUpdates(_updateManager);
                    }
                }
                else
                {
                    // Update system not configured - this is normal during development
                    System.Diagnostics.Debug.WriteLine("Update system not configured. Skipping auto-update initialization.");
                }
            }
            catch (Exception ex)
            {
                // Log error but don't crash the application
                System.Diagnostics.Debug.WriteLine($"Failed to initialize update manager: {ex.Message}");
            }
        }
    }
}
