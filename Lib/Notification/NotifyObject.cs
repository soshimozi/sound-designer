﻿using System.Collections.Generic;
using System.ComponentModel;

namespace SoundDesigner.Lib.Notification;

public class NotifyObject : INotifyPropertyChanged
{
    public bool SetField<T>(ref T fieldReference, T value, string name)
    {
        if (EqualityComparer<T>.Default.Equals(fieldReference, value)) return false;

        fieldReference = value;
        OnPropertyChanged(name);
        return true;
    }

    protected void OnPropertyChanged(string name)
    {
        var handler = PropertyChanged;
        if (handler != null)
        {
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
