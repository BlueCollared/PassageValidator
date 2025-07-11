﻿using Avalonia.Controls;
using System;

namespace EtGate.UI
{
    internal class MaintenanceViewFactory : IViewFactory
    {
        public UserControl Create(Type viewModelType)
        {
            var viewType = viewModelType.Name.Replace("ViewModel", "View");
            var view = (UserControl)Activator.CreateInstance(Type.GetType($"EtGate.UI.Views.Maintenance.{viewType}"));
            return view;
        }
    }
}
