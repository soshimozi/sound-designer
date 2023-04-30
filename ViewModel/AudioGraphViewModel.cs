using System;
using System.Windows;
using Syncfusion.UI.Xaml.Diagram;

namespace SoundDesigner.ViewModel;

public class AudioGraphViewModel : DiagramViewModel
{
    ResourceDictionary resourceDictionary = new ResourceDictionary()
    {
        Source = new Uri(@"/Syncfusion.SfDiagram.Wpf;component/Resources/BasicShapes.xaml", UriKind.RelativeOrAbsolute)
    };

    
}