using Avalonia.Controls;
using System;

namespace EtGate.UI
{
    public interface IViewFactory
    {
        UserControl Create(Type viewModelType);
    }
}
