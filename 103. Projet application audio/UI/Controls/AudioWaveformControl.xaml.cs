using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using NAudio.Wave;

namespace AudioEditor.UI.Controls
{
    public partial class AudioWaveformControl : UserControl
    {
        private bool _isSelecting;
        private Point _selectionStartPoint;
        private Point _selectionEndPoint;
        private double _zoomLevel = 1.0;
        private float[]? _audioData;
        private WaveFormat? _waveFormat;
        private TimeSpan _audioDuration;
        private double _samplesPerPixel = 100;

        public event EventHandler<TimeSpan>? PositionChanged;
        public event EventHandler<(TimeSpan Start, TimeSpan End)>? SelectionChanged;

        public AudioWaveformControl()
        {
            InitializeComponent();
            InitializeEventHandlers();
        }

        private void InitializeEventHandlers()
        {
            ZoomInButton.Click += (s, e) => ZoomIn();
            ZoomOutButton.Click += (s, e) => ZoomOut();
            ZoomFitButton.Click += (s, e) => ZoomToFit();
        }

        public void LoadAudioData(float[] audioData, WaveFormat waveFormat)
        {
            _audioData = audioData;
            _waveFormat = waveFormat;
            _audioDuration = TimeSpan.FromSeconds((double)audioData.Length / waveFormat.SampleRate / waveFormat.Channels);

            UpdateAudioInfo();
            GenerateWaveform();
            GenerateTimeGrid();
            ZoomToFit();
        }

        private void UpdateAudioInfo()
        {
            if (_waveFormat != null)
            {
                AudioInfoLabel.Content = $"{_waveFormat.SampleRate}Hz, {_waveFormat.Channels}ch, {_audioDuration:mm\\:ss\\.ff}";
            }
        }

        private void GenerateWaveform()
        {
            if (_audioData == null || _waveFormat == null) return;

            AudioDataCanvas.Children.Clear();

            var canvasWidth = Math.Max(800, _audioData.Length / _samplesPerPixel);
            var canvasHeight = WaveformCanvas.ActualHeight > 0 ? WaveformCanvas.ActualHeight : 150;

            WaveformCanvas.Width = canvasWidth;
            WaveformCanvas.Height = canvasHeight;

            var centerY = canvasHeight / 2;
            var maxAmplitude = canvasHeight / 2 - 5;

            // Dessiner la forme d'onde
            var waveformPath = new Path
            {
                Stroke = new SolidColorBrush(Color.FromRgb(100, 200, 255)),
                StrokeThickness = 1,
                Fill = new SolidColorBrush(Color.FromArgb(80, 100, 200, 255))
            };

            var geometry = new PathGeometry();
            var figure = new PathFigure { StartPoint = new Point(0, centerY) };

            // Calculer les points de la forme d'onde
            for (int x = 0; x < canvasWidth; x++)
            {
                var sampleIndex = (int)(x * _samplesPerPixel);
                if (sampleIndex >= _audioData.Length) break;

                // Calculer l'amplitude moyenne pour cette colonne de pixels
                var amplitude = CalculateAmplitudeAtPixel(x);
                var y1 = centerY - (amplitude * maxAmplitude);
                var y2 = centerY + (amplitude * maxAmplitude);

                figure.Segments.Add(new LineSegment(new Point(x, y1), true));
            }

            // Compléter le chemin pour créer une forme fermée
            for (int x = (int)canvasWidth - 1; x >= 0; x--)
            {
                var amplitude = CalculateAmplitudeAtPixel(x);
                var y2 = centerY + (amplitude * maxAmplitude);
                figure.Segments.Add(new LineSegment(new Point(x, y2), true));
            }

            geometry.Figures.Add(figure);
            waveformPath.Data = geometry;
            AudioDataCanvas.Children.Add(waveformPath);
        }

        private double CalculateAmplitudeAtPixel(int pixelX)
        {
            if (_audioData == null) return 0;

            var startSample = (int)(pixelX * _samplesPerPixel);
            var endSample = Math.Min((int)((pixelX + 1) * _samplesPerPixel), _audioData.Length);

            var maxAmplitude = 0f;
            for (int i = startSample; i < endSample; i++)
            {
                maxAmplitude = Math.Max(maxAmplitude, Math.Abs(_audioData[i]));
            }

            return maxAmplitude;
        }

        private void GenerateTimeGrid()
        {
            if (_audioDuration == TimeSpan.Zero) return;

            TimeGridCanvas.Children.Clear();
            TimeRulerCanvas.Children.Clear();

            var canvasWidth = WaveformCanvas.Width;
            var canvasHeight = WaveformCanvas.Height;
            var pixelsPerSecond = canvasWidth / _audioDuration.TotalSeconds;

            // Déterminer l'intervalle approprié pour les marqueurs
            var interval = CalculateTimeInterval(pixelsPerSecond);

            for (var time = TimeSpan.Zero; time <= _audioDuration; time = time.Add(interval))
            {
                var x = time.TotalSeconds * pixelsPerSecond;

                // Ligne de grille verticale
                var gridLine = new Line
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = canvasHeight,
                    Stroke = new SolidColorBrush(Color.FromArgb(80, 255, 255, 255)),
                    StrokeThickness = 0.5
                };
                TimeGridCanvas.Children.Add(gridLine);

                // Marqueur de temps sur la règle
                var timeLabel = new TextBlock
                {
                    Text = time.ToString(@"mm\:ss"),
                    Foreground = Brushes.White,
                    FontSize = 10
                };
                Canvas.SetLeft(timeLabel, x + 2);
                Canvas.SetTop(timeLabel, 5);
                TimeRulerCanvas.Children.Add(timeLabel);
            }
        }

        private TimeSpan CalculateTimeInterval(double pixelsPerSecond)
        {
            var intervals = new[] { 0.1, 0.5, 1, 5, 10, 30, 60 }; // secondes

            foreach (var interval in intervals)
            {
                if (interval * pixelsPerSecond >= 50) // Au moins 50 pixels entre les marqueurs
                {
                    return TimeSpan.FromSeconds(interval);
                }
            }

            return TimeSpan.FromMinutes(1);
        }

        public void SetPlayheadPosition(TimeSpan position)
        {
            if (_audioDuration == TimeSpan.Zero) return;

            var x = (position.TotalSeconds / _audioDuration.TotalSeconds) * WaveformCanvas.Width;
            PlayheadLine.X1 = PlayheadLine.X2 = x;
            PlayheadLine.Y2 = WaveformCanvas.Height;
            PlayheadLine.Visibility = Visibility.Visible;
        }

        private void ZoomIn()
        {
            _zoomLevel *= 1.5;
            _samplesPerPixel /= 1.5;
            GenerateWaveform();
            GenerateTimeGrid();
        }

        private void ZoomOut()
        {
            _zoomLevel /= 1.5;
            _samplesPerPixel *= 1.5;
            GenerateWaveform();
            GenerateTimeGrid();
        }

        private void ZoomToFit()
        {
            if (_audioData == null) return;

            var availableWidth = WaveformScrollViewer.ActualWidth - 20;
            _samplesPerPixel = _audioData.Length / availableWidth;
            _zoomLevel = 1.0;
            GenerateWaveform();
            GenerateTimeGrid();
        }

        private void WaveformCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isSelecting = true;
            _selectionStartPoint = e.GetPosition(WaveformCanvas);
            WaveformCanvas.CaptureMouse();

            SelectionRectangle.Visibility = Visibility.Collapsed;
            e.Handled = true;
        }

        private void WaveformCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isSelecting)
            {
                _selectionEndPoint = e.GetPosition(WaveformCanvas);
                UpdateSelectionRectangle();
            }
        }

        private void WaveformCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isSelecting)
            {
                _isSelecting = false;
                WaveformCanvas.ReleaseMouseCapture();

                var startTime = PixelToTime(_selectionStartPoint.X);
                var endTime = PixelToTime(_selectionEndPoint.X);

                if (Math.Abs(_selectionEndPoint.X - _selectionStartPoint.X) > 5)
                {
                    SelectionChanged?.Invoke(this, (
                        TimeSpan.FromSeconds(Math.Min(startTime, endTime)),
                        TimeSpan.FromSeconds(Math.Max(startTime, endTime))
                    ));
                    UpdateSelectionInfo();
                }
                else
                {
                    // Simple clic - changer la position de lecture
                    var clickTime = TimeSpan.FromSeconds(startTime);
                    PositionChanged?.Invoke(this, clickTime);
                    SetPlayheadPosition(clickTime);
                    SelectionRectangle.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void UpdateSelectionRectangle()
        {
            var left = Math.Min(_selectionStartPoint.X, _selectionEndPoint.X);
            var width = Math.Abs(_selectionEndPoint.X - _selectionStartPoint.X);

            Canvas.SetLeft(SelectionRectangle, left);
            SelectionRectangle.Width = width;
            SelectionRectangle.Height = WaveformCanvas.Height;
            SelectionRectangle.Visibility = Visibility.Visible;
        }

        private void UpdateSelectionInfo()
        {
            var startTime = TimeSpan.FromSeconds(Math.Min(PixelToTime(_selectionStartPoint.X), PixelToTime(_selectionEndPoint.X)));
            var endTime = TimeSpan.FromSeconds(Math.Max(PixelToTime(_selectionStartPoint.X), PixelToTime(_selectionEndPoint.X)));
            var duration = endTime - startTime;

            SelectionInfoLabel.Content = $"{startTime:mm\\:ss\\.ff} - {endTime:mm\\:ss\\.ff} ({duration:ss\\.ff}s)";
        }

        private double PixelToTime(double pixelX)
        {
            if (WaveformCanvas.Width == 0) return 0;
            return (pixelX / WaveformCanvas.Width) * _audioDuration.TotalSeconds;
        }

        private void WaveformCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                ZoomIn();
            else
                ZoomOut();

            e.Handled = true;
        }

        public void ClearSelection()
        {
            SelectionRectangle.Visibility = Visibility.Collapsed;
            SelectionInfoLabel.Content = "None";
        }
    }
}