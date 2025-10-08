using System.Windows;

namespace AudioEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialisation de l'application
            InitializeAudioEngine();
        }

        private void InitializeAudioEngine()
        {
            // Initialisation du moteur audio NAudio
            try
            {
                NAudio.MediaFoundation.MediaFoundationApi.Startup();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'initialisation du moteur audio: {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Nettoyage du moteur audio
            try
            {
                NAudio.MediaFoundation.MediaFoundationApi.Shutdown();
            }
            catch
            {
                // Ignorer les erreurs de nettoyage
            }

            base.OnExit(e);
        }
    }
}