﻿using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;

namespace Nitrox.Launcher.ViewModels.Abstract;

/// <summary>
///     Base class for (popup) dialog ViewModels.
/// </summary>
public abstract partial class ModalViewModelBase : ObservableValidator, IModalDialogViewModel
{
    [ObservableProperty] private bool? dialogResult;
    [ObservableProperty] private ButtonOptions? selectedOption;

    public static implicit operator bool(ModalViewModelBase self)
    {
        return self is { DialogResult: true } and not { SelectedOption: ButtonOptions.No };
    }

    [RelayCommand]
    public void Close()
    {
        ((IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime)?.Windows.FirstOrDefault(w => w.DataContext == this)?.Close(DialogResult);
    }
}