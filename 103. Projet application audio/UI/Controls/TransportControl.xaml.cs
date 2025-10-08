using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace AudioEditor.UI.Controls
{
    public partial class TransportControl : UserControl
    {
        private bool _isPlaying;
        private bool _isRecording;
        private bool _isPaused;
        private bool _isLooping;
        private bool _isMetronomeEnabled;
        private TimeSpan _currentPosition = TimeSpan.Zero;
        private TimeSpan _totalDuration = TimeSpan.Zero;
        private DispatcherTimer? _updateTimer;

        public event EventHandler? PlayRequested;
        public event EventHandler? PauseRequested;
        public event EventHandler? StopRequested;
        public event EventHandler? RecordRequested;
        public event EventHandler? RewindRequested;
        public event EventHandler<bool>? LoopToggled;
        public event EventHandler<bool>? MetronomeToggled;
        public event EventHandler<TimeSpan>? PositionChanged;
        public event EventHandler<int>? BpmChanged;

        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                UpdatePlayPauseButton();
                if (value && !_isPaused)
                    StartUpdateTimer();
                else if (!value)
                    StopUpdateTimer();
            }
        }

        public bool IsRecording
        {
            get => _isRecording;
            set
            {
                _isRecording = value;
                UpdateRecordButton();
            }
        }

        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                _isPaused = value;
                UpdatePlayPauseButton();
            }
        }

        public bool IsLooping
        {
            get => _isLooping;
            set
            {
                _isLooping = value;
                UpdateLoopButton();
            }
        }

        public bool IsMetronomeEnabled
        {
            get => _isMetronomeEnabled;
            set
            {
                _isMetronomeEnabled = value;
                UpdateMetronomeButton();
            }
        }

        public TimeSpan CurrentPosition
        {
            get => _currentPosition;
            set
            {
                _currentPosition = value;
                UpdateTimeDisplay();
                UpdateProgressBar();
            }
        }

        public TimeSpan TotalDuration
        {
            get => _totalDuration;
            set
            {
                _totalDuration = value;
                UpdateTimeDisplay();
                UpdateProgressBar();
            }
        }

        public int Bpm
        {
            get
            {
                if (int.TryParse(BpmTextBox.Text, out int bpm))
                    return bpm;
                return 120;
            }
            set
            {
                BpmTextBox.Text = value.ToString();
            }
        }

        public TransportControl()
        {
            InitializeComponent();
            InitializeTimer();
            UpdateTimeDisplay();
            UpdateProgressBar();

            // Événement pour le BPM
            BpmTextBox.TextChanged += (s, e) => BpmChanged?.Invoke(this, Bpm);
        }

        private void InitializeTimer()
        {
            _updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(50) // 20 FPS
            };
            _updateTimer.Tick += UpdateTimer_Tick;
        }

        private void UpdateTimer_Tick(object? sender, EventArgs e)
        {
            if (_isPlaying && !_isPaused)
            {
                // La position sera mise à jour par le service audio
                // Ici on ne fait que rafraîchir l'affichage
                UpdateTimeDisplay();
                UpdateProgressBar();
            }
        }

        private void StartUpdateTimer()
        {
            _updateTimer?.Start();
        }

        private void StopUpdateTimer()
        {
            _updateTimer?.Stop();
        }

        private void UpdatePlayPauseButton()
        {
            if (_isPlaying && !_isPaused)
            {
                // Afficher l'icône Pause
                PlayIcon.Visibility = Visibility.Collapsed;
                PauseIcon.Visibility = Visibility.Visible;
                PlayPauseButton.ToolTip = "Pause";
            }
            else
            {
                // Afficher l'icône Play
                PlayIcon.Visibility = Visibility.Visible;
                PauseIcon.Visibility = Visibility.Collapsed;
                PlayPauseButton.ToolTip = "Lecture";
            }
        }

        private void UpdateRecordButton()
        {
            RecordButton.Background = _isRecording
                ? System.Windows.Media.Brushes.Red
                : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(139, 69, 19));
        }

        private void UpdateLoopButton()
        {
            LoopButton.Background = _isLooping
                ? System.Windows.Media.Brushes.Orange
                : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(80, 80, 80));
        }

        private void UpdateMetronomeButton()
        {
            MetronomeButton.Background = _isMetronomeEnabled
                ? System.Windows.Media.Brushes.LightBlue
                : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(80, 80, 80));
        }

        private void UpdateTimeDisplay()
        {
            CurrentTimeLabel.Content = _currentPosition.ToString(@"mm\:ss\.ff");
            TotalTimeLabel.Content = _totalDuration.ToString(@"mm\:ss\.ff");
        }

        private void UpdateProgressBar()
        {
            if (_totalDuration.TotalSeconds > 0)
            {
                var progressWidth = ProgressCanvas.ActualWidth > 0 ? ProgressCanvas.ActualWidth : 300;
                var percentage = _currentPosition.TotalSeconds / _totalDuration.TotalSeconds;
                var newWidth = Math.Max(0, Math.Min(progressWidth, percentage * progressWidth));

                ProgressBar.Width = newWidth;
                ProgressIndicator.X1 = ProgressIndicator.X2 = newWidth;
            }
            else
            {
                ProgressBar.Width = 0;
                ProgressIndicator.X1 = ProgressIndicator.X2 = 0;
            }
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlaying && !_isPaused)
            {
                // En cours de lecture -> Pause
                PauseRequested?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                // Arrêté ou en pause -> Lecture
                PlayRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StopRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RecordButton_Click(object sender, RoutedEventArgs e)
        {
            RecordRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RewindButton_Click(object sender, RoutedEventArgs e)
        {
            RewindRequested?.Invoke(this, EventArgs.Empty);
        }

        private void LoopButton_Click(object sender, RoutedEventArgs e)
        {
            IsLooping = !IsLooping;
            LoopToggled?.Invoke(this, IsLooping);
        }

        private void MetronomeButton_Click(object sender, RoutedEventArgs e)
        {
            IsMetronomeEnabled = !IsMetronomeEnabled;
            MetronomeToggled?.Invoke(this, IsMetronomeEnabled);
        }

        private void ProgressCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_totalDuration.TotalSeconds > 0)
            {
                var clickX = e.GetPosition(ProgressCanvas).X;
                var progressWidth = ProgressCanvas.ActualWidth;
                var percentage = clickX / progressWidth;
                var newPosition = TimeSpan.FromSeconds(_totalDuration.TotalSeconds * percentage);

                // Contraindre la position
                if (newPosition < TimeSpan.Zero)
                    newPosition = TimeSpan.Zero;
                else if (newPosition > _totalDuration)
                    newPosition = _totalDuration;

                CurrentPosition = newPosition;
                PositionChanged?.Invoke(this, newPosition);
            }
        }

        public void Reset()
        {
            IsPlaying = false;
            IsRecording = false;
            IsPaused = false;
            CurrentPosition = TimeSpan.Zero;
        }

        public void SetDuration(TimeSpan duration)
        {
            TotalDuration = duration;
        }

        public void SetPosition(TimeSpan position)
        {
            CurrentPosition = position;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            // Mettre à jour la barre de progression quand la taille change
            Dispatcher.BeginInvoke(() => UpdateProgressBar());
        }

        public void Dispose()
        {
            _updateTimer?.Stop();
            _updateTimer = null;
        }
    }
}