using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using AudioEditor.Core.Models;

namespace AudioEditor.UI.Controls
{
    public partial class TrackControl : UserControl
    {
        private AudioTrack? _audioTrack;
        private bool _isDragging;
        private Point _dragStartPoint;
        private Rectangle? _selectedClip;
        private double _pixelsPerSecond = 100;
        private TimeSpan _totalDuration = TimeSpan.FromMinutes(5);

        public event EventHandler<AudioTrack>? TrackChanged;
        public event EventHandler<(AudioClip Clip, TimeSpan NewPosition)>? ClipMoved;
        public event EventHandler<AudioClip>? ClipSelected;
        public event EventHandler? ClipDeselected;

        public AudioTrack? Track
        {
            get => _audioTrack;
            set
            {
                _audioTrack = value;
                UpdateTrackDisplay();
            }
        }

        public double PixelsPerSecond
        {
            get => _pixelsPerSecond;
            set
            {
                _pixelsPerSecond = value;
                UpdateClipsDisplay();
                GenerateTimeGrid();
            }
        }

        public TimeSpan TotalDuration
        {
            get => _totalDuration;
            set
            {
                _totalDuration = value;
                UpdateCanvasWidth();
                GenerateTimeGrid();
            }
        }

        public TrackControl()
        {
            InitializeComponent();
            GenerateTimeGrid();
        }

        private void UpdateTrackDisplay()
        {
            if (_audioTrack == null) return;

            TrackNameTextBox.Text = _audioTrack.Name;
            VolumeSlider.Value = _audioTrack.Volume;
            PanSlider.Value = _audioTrack.Pan;
            MuteButton.Background = _audioTrack.IsMuted ? Brushes.Red : new SolidColorBrush(Color.FromRgb(80, 80, 80));

            UpdateVolumeLabel();
            UpdatePanLabel();
            UpdateClipsDisplay();
        }

        private void UpdateClipsDisplay()
        {
            // Effacer les clips existants (garder la grille)
            var clipsToRemove = ClipsCanvas.Children.OfType<Rectangle>().ToList();
            foreach (var clip in clipsToRemove)
            {
                ClipsCanvas.Children.Remove(clip);
            }

            if (_audioTrack?.Clips == null) return;

            foreach (var clip in _audioTrack.Clips)
            {
                CreateClipVisual(clip);
            }

            UpdateCanvasWidth();
        }

        private void CreateClipVisual(AudioClip clip)
        {
            var clipRect = new Rectangle
            {
                Fill = new SolidColorBrush(Color.FromRgb(74, 144, 226)),
                Stroke = new SolidColorBrush(Color.FromRgb(33, 113, 181)),
                StrokeThickness = 1,
                Height = 50,
                RadiusX = 2,
                RadiusY = 2,
                Cursor = Cursors.Hand,
                Tag = clip
            };

            // Calculer la position et la largeur
            var startX = clip.StartTime.TotalSeconds * _pixelsPerSecond;
            var width = clip.Duration.TotalSeconds * _pixelsPerSecond;

            clipRect.Width = Math.Max(width, 20); // Largeur minimale
            Canvas.SetLeft(clipRect, startX);
            Canvas.SetTop(clipRect, 5);

            // Ajouter le nom du fichier sur le clip
            var clipLabel = new TextBlock
            {
                Text = System.IO.Path.GetFileNameWithoutExtension(clip.FilePath),
                Foreground = Brushes.White,
                FontSize = 10,
                IsHitTestVisible = false
            };
            Canvas.SetLeft(clipLabel, startX + 3);
            Canvas.SetTop(clipLabel, 8);

            ClipsCanvas.Children.Add(clipRect);
            ClipsCanvas.Children.Add(clipLabel);

            // Événements de sélection
            clipRect.MouseLeftButtonDown += (s, e) => SelectClip(clipRect, clip);
        }

        private void SelectClip(Rectangle clipRect, AudioClip clip)
        {
            // Désélectionner le clip précédent
            if (_selectedClip != null)
            {
                _selectedClip.Stroke = new SolidColorBrush(Color.FromRgb(33, 113, 181));
                _selectedClip.StrokeThickness = 1;
            }

            // Sélectionner le nouveau clip
            _selectedClip = clipRect;
            clipRect.Stroke = Brushes.Yellow;
            clipRect.StrokeThickness = 2;

            ClipSelected?.Invoke(this, clip);
        }

        private void GenerateTimeGrid()
        {
            ClipTimeGridCanvas.Children.Clear();

            var canvasWidth = _totalDuration.TotalSeconds * _pixelsPerSecond;
            ClipsCanvas.MinWidth = Math.Max(400, canvasWidth);

            // Lignes de grille toutes les secondes
            for (var time = TimeSpan.Zero; time <= _totalDuration; time = time.Add(TimeSpan.FromSeconds(1)))
            {
                var x = time.TotalSeconds * _pixelsPerSecond;

                var gridLine = new Line
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = 60,
                    Stroke = new SolidColorBrush(Color.FromArgb(40, 255, 255, 255)),
                    StrokeThickness = 0.5
                };

                ClipTimeGridCanvas.Children.Add(gridLine);
            }
        }

        private void UpdateCanvasWidth()
        {
            var minWidth = 400.0;
            var contentWidth = _totalDuration.TotalSeconds * _pixelsPerSecond;

            if (_audioTrack?.Clips?.Any() == true)
            {
                var maxClipEnd = _audioTrack.Clips.Max(c => c.StartTime.Add(c.Duration));
                contentWidth = Math.Max(contentWidth, maxClipEnd.TotalSeconds * _pixelsPerSecond + 100);
            }

            ClipsCanvas.MinWidth = Math.Max(minWidth, contentWidth);
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_audioTrack == null) return;

            _audioTrack.IsMuted = !_audioTrack.IsMuted;
            MuteButton.Background = _audioTrack.IsMuted ? Brushes.Red : new SolidColorBrush(Color.FromRgb(80, 80, 80));

            TrackChanged?.Invoke(this, _audioTrack);
        }

        private void SoloButton_Click(object sender, RoutedEventArgs e)
        {
            // La logique solo sera gérée par le contrôle parent
            // car elle affecte les autres pistes
            if (_audioTrack != null)
                TrackChanged?.Invoke(this, _audioTrack);
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_audioTrack == null) return;

            _audioTrack.Volume = (float)e.NewValue;
            UpdateVolumeLabel();
            TrackChanged?.Invoke(this, _audioTrack);
        }

        private void PanSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_audioTrack == null) return;

            _audioTrack.Pan = (float)e.NewValue;
            UpdatePanLabel();
            TrackChanged?.Invoke(this, _audioTrack);
        }

        private void UpdateVolumeLabel()
        {
            var percentage = (int)Math.Round(VolumeSlider.Value * 100);
            VolumeLabel.Content = $"{percentage}%";
        }

        private void UpdatePanLabel()
        {
            var panValue = PanSlider.Value;
            string panText;

            if (Math.Abs(panValue) < 0.1)
                panText = "C";
            else if (panValue < 0)
                panText = $"L{Math.Round(Math.Abs(panValue) * 100)}";
            else
                panText = $"R{Math.Round(panValue * 100)}";

            PanLabel.Content = panText;
        }

        private void ClipsCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(ClipsCanvas);

            // Vérifier si on clique sur un clip
            var hitClip = ClipsCanvas.Children.OfType<Rectangle>()
                .FirstOrDefault(r => r.IsMouseOver);

            if (hitClip != null)
            {
                _isDragging = true;
                _dragStartPoint = position;
                ClipsCanvas.CaptureMouse();
            }
            else
            {
                // Clic dans l'espace vide - désélectionner
                if (_selectedClip != null)
                {
                    _selectedClip.Stroke = new SolidColorBrush(Color.FromRgb(33, 113, 181));
                    _selectedClip.StrokeThickness = 1;
                    _selectedClip = null;
                    ClipDeselected?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void ClipsCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(ClipsCanvas);

            if (_isDragging && _selectedClip?.Tag is AudioClip clip)
            {
                var deltaX = position.X - _dragStartPoint.X;
                var newStartX = Canvas.GetLeft(_selectedClip) + deltaX;

                // Contraindre à la zone valide
                newStartX = Math.Max(0, newStartX);

                Canvas.SetLeft(_selectedClip, newStartX);
                _dragStartPoint = position;

                // Mettre à jour le temps de début du clip
                var newStartTime = TimeSpan.FromSeconds(newStartX / _pixelsPerSecond);
                clip.StartTime = newStartTime;

                // Mettre à jour la position du label aussi
                var label = ClipsCanvas.Children.OfType<TextBlock>()
                    .FirstOrDefault(t => Math.Abs(Canvas.GetLeft(t) - Canvas.GetLeft(_selectedClip)) < 10);
                if (label != null)
                {
                    Canvas.SetLeft(label, newStartX + 3);
                }
            }
            else
            {
                // Afficher l'indicateur de position d'insertion
                InsertPositionIndicator.X1 = InsertPositionIndicator.X2 = position.X;
                InsertPositionIndicator.Visibility = Visibility.Visible;
            }
        }

        private void ClipsCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                ClipsCanvas.ReleaseMouseCapture();

                if (_selectedClip?.Tag is AudioClip clip)
                {
                    var newPosition = TimeSpan.FromSeconds(Canvas.GetLeft(_selectedClip) / _pixelsPerSecond);
                    ClipMoved?.Invoke(this, (clip, newPosition));
                }
            }

            InsertPositionIndicator.Visibility = Visibility.Collapsed;
        }

        private void ClipsCanvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var audioFile = files.FirstOrDefault(f =>
                    f.EndsWith(".wav", StringComparison.OrdinalIgnoreCase) ||
                    f.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) ||
                    f.EndsWith(".flac", StringComparison.OrdinalIgnoreCase));

                if (audioFile != null)
                {
                    var dropPosition = e.GetPosition(ClipsCanvas);
                    var startTime = TimeSpan.FromSeconds(dropPosition.X / _pixelsPerSecond);

                    // Créer un nouveau clip
                    var newClip = new AudioClip
                    {
                        FilePath = audioFile,
                        StartTime = startTime,
                        Duration = TimeSpan.FromSeconds(10), // Durée par défaut, sera mise à jour
                        Volume = 1.0f
                    };

                    _audioTrack?.Clips.Add(newClip);
                    CreateClipVisual(newClip);
                    UpdateCanvasWidth();
                }
            }
        }

        public void AddClip(AudioClip clip)
        {
            _audioTrack?.Clips.Add(clip);
            CreateClipVisual(clip);
            UpdateCanvasWidth();
        }

        public void RemoveSelectedClip()
        {
            if (_selectedClip?.Tag is AudioClip clip && _audioTrack != null)
            {
                _audioTrack.Clips.Remove(clip);
                UpdateClipsDisplay();
                _selectedClip = null;
                ClipDeselected?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}