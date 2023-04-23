using System.ComponentModel;

namespace SoundDesigner.Models;

public class CustomLabel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private double _opacity;
    public double Opacity
    {
        get { return _opacity; }
        set
        {
            _opacity = value;
            OnPropertyChanged(nameof(Opacity));
        }
    }

    private string _content;
    public string Content
    {
        get { return _content; }
        set
        {
            _content = value;
            OnPropertyChanged(nameof(Content));
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
