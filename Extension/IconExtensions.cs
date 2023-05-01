﻿using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System;

namespace SoundDesigner.Extension;

public static class IconExtensions
{
    [DllImport("gdi32.dll", SetLastError = true)]
    private static extern bool DeleteObject(IntPtr hObject);


    public static ImageSource ToImageSource(this Icon icon)
    {
        var bitmap = icon.ToBitmap();
        var hBitmap = bitmap.GetHbitmap();

        ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
            hBitmap,
            IntPtr.Zero,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());

        if (!DeleteObject(hBitmap))
        {
            throw new Win32Exception();
        }

        return wpfBitmap;
    }

}