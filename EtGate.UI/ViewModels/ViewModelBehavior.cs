using Avalonia;
using Avalonia.Controls;
using System;

namespace EtGate.UI.ViewModels;
public static class ContentControlHelper
{
    public static readonly AttachedProperty<INavigationService> NavigationServiceProperty =
        AvaloniaProperty.RegisterAttached<ContentControl, INavigationService>("NavigationService", typeof(ContentControlHelper));

    public static INavigationService GetNavigationService(ContentControl control) => control.GetValue(NavigationServiceProperty);
    public static void SetNavigationService(ContentControl control, INavigationService value) => control.SetValue(NavigationServiceProperty, value);

    static ContentControlHelper()
    {
        NavigationServiceProperty.Changed.Subscribe(OnNavigationServiceChanged);
    }

    private static void OnNavigationServiceChanged(AvaloniaPropertyChangedEventArgs<INavigationService> e)
    {
        if (e.Sender is ContentControl control && e.NewValue.Value is INavigationService navigationService)
        {
            navigationService.NavigateTo<AgentLoginViewModel>();
        }
    }
}
