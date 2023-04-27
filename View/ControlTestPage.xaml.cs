using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SoundDesigner.Controls;
using SoundDesigner.Models;
using SoundDesigner.ViewModel;

namespace SoundDesigner.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ControlTestPage : UserControl
    {

        protected Canvas? PanelCanvas;

        private bool _isDragging = false;
        private Point? offset = null;
        private AudioJack? _draggingFrom = null;
        private Cable? _draggingCable = null;

        public ControlTestPage()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var presenter = this.FindChild<ContentPresenter>("PartPanelCanvas");
        }

        private void ControlPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var from = _draggingFrom;
            var cable = _draggingCable;

            _isDragging = false;
            _draggingFrom = null;
            _draggingCable = null;

            ReleaseMouseCapture();

            if (from == null || PanelCanvas == null || cable == null) return;

            // we are done with this cable for now
            PanelCanvas.Children.Remove(cable);

            var audioJack = FindAudioJackFromHitPoint(e.GetPosition(PanelCanvas), PanelCanvas);
            if (audioJack == null || audioJack == from) return;
            if (audioJack.ToY != from.ToY)
            {
                //Connect(from, audioJack);
            }
        }

        private void ControlPanel_MouseMove(object sender, MouseEventArgs e)
        {

            if (!_isDragging || PanelCanvas == null || _draggingCable == null || _draggingFrom == null) return;

            _draggingCable.EndPoint = e.GetPosition(this);
            _draggingCable.DraggableState = Cable.DraggableStateEnum.DraggingNoDrop;

            var audioJack = FindAudioJackFromHitPoint(e.GetPosition(PanelCanvas), PanelCanvas);
            if (audioJack == null) return;

            // can only drop inputs on outputs and vice versa
            if (audioJack.ToY != _draggingFrom.ToY)
            {
                _draggingCable.DraggableState = Cable.DraggableStateEnum.DraggingCanDrop;
            }

        }

        private static AudioJack? FindAudioJackFromHitPoint(Point hitPoint, Canvas canvas)
        {
            var hitResults = new List<HitTestResult>();
            VisualTreeHelper.HitTest(canvas, null, result =>
            {
                hitResults.Add(result);
                return HitTestResultBehavior.Continue;
            }, new PointHitTestParameters(hitPoint));

            var audioJack = hitResults.Select(result => result.VisualHit)
                .OfType<AudioJack>().FirstOrDefault();

            if (audioJack != null) return audioJack;

            var image = hitResults.Select(result => result.VisualHit)
                .OfType<Image>().FirstOrDefault();

            return image?.FindAncestor<AudioJack>();
        }

    }
}
