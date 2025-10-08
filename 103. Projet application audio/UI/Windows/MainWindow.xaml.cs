using System.Windows;
using System.Windows.Controls;

namespace AudioEditor.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPlaying = false;
        private int trackCount = 0;

        public MainWindow()
        {
            InitializeComponent();
            UpdateUI();
        }

        private void UpdateUI()
        {
            TrackCountDisplay.Text = trackCount.ToString();
            StatusText.Text = isPlaying ? "Lecture en cours..." : "Prêt";
        }

        #region Menu Events

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Nouveau projet créé!", "Anomaly Recorder", MessageBoxButton.OK, MessageBoxImage.Information);
            trackCount = 0;
            UpdateUI();
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fonction Ouvrir à implémenter", "Anomaly Recorder", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Projet sauvegardé!", "Anomaly Recorder", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveProjectAs_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fonction Sauvegarder sous à implémenter", "Anomaly Recorder", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ImportAudio_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fonction Import Audio à implémenter", "Anomaly Recorder", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportMix_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fonction Export à implémenter", "Anomaly Recorder", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        #region Track Events

        private void AddAudioTrack_Click(object sender, RoutedEventArgs e)
        {
            trackCount++;

            // Créer un panel pour la nouvelle piste
            var trackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(0, 5, 0, 5),
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(63, 63, 70))
            };

            // Titre de la piste
            var trackTitle = new TextBlock
            {
                Text = $"Piste Audio {trackCount}",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(5),
                Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White)
            };

            // Contrôles de la piste
            var controlsPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(5)
            };

            var muteButton = new Button
            {
                Content = "M",
                Width = 25,
                Height = 25,
                Margin = new Thickness(2),
                ToolTip = "Muet"
            };

            var soloButton = new Button
            {
                Content = "S",
                Width = 25,
                Height = 25,
                Margin = new Thickness(2),
                ToolTip = "Solo"
            };

            var volumeSlider = new Slider
            {
                Minimum = 0,
                Maximum = 100,
                Value = 75,
                Width = 80,
                Margin = new Thickness(5, 0, 0, 0)
            };

            controlsPanel.Children.Add(muteButton);
            controlsPanel.Children.Add(soloButton);
            controlsPanel.Children.Add(volumeSlider);

            trackPanel.Children.Add(trackTitle);
            trackPanel.Children.Add(controlsPanel);

            TracksPanel.Children.Add(trackPanel);
            UpdateUI();
        }

        private void AddMidiTrack_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fonction Piste MIDI à implémenter", "Anomaly Recorder", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteTrack_Click(object sender, RoutedEventArgs e)
        {
            if (trackCount > 0)
            {
                trackCount--;
                if (TracksPanel.Children.Count > 1) // Garder le titre
                {
                    TracksPanel.Children.RemoveAt(TracksPanel.Children.Count - 1);
                }
                UpdateUI();
            }
        }

        #endregion

        #region Transport Events

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            isPlaying = !isPlaying;
            PlayPauseButton.Content = isPlaying ? "⏸" : "▶";
            UpdateUI();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            isPlaying = false;
            PlayPauseButton.Content = "▶";
            UpdateUI();
        }

        private void Record_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fonction Enregistrement à implémenter", "Anomaly Recorder", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MasterVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MasterVolumeDisplay != null)
            {
                MasterVolumeDisplay.Text = $"{(int)e.NewValue}%";
            }
        }

        #endregion

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow
            {
                Owner = this
            };
            aboutWindow.ShowDialog();
        }
    }
}