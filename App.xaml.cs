using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SoundDesigner.Event;
using SoundDesigner.Extension;
using SoundDesigner.Lib;
using SoundDesigner.ViewModel;
using Syncfusion.UI.Xaml.Diagram;

namespace SoundDesigner;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    static App()
    {
        var licenseKey = Environment.GetEnvironmentVariable("syncfusion-licensekey");
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(licenseKey);

        ConfigureServices();
    }

    private static void ConfigureServices()
    {
        Ioc.Default.ConfigureServices(
            new ServiceCollection()
                .AddSingleton<IEventAggregator, EventAggregator>()
                .AddViewModels<ViewModelBase>()
                .AddViewModels<DiagramViewModel>()
                .AddViewModels<AudioGraphViewModel>()
                .BuildServiceProvider()
        );

    }
}