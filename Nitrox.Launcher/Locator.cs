using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Nitrox.Launcher.ViewModels;
using Nitrox.Launcher.ViewModels.Abstract;
using Nitrox.Launcher.Views;
using ReactiveUI;

namespace Nitrox.Launcher;

internal sealed class Locator : IViewLocator
{
    private static readonly ConcurrentDictionary<Type, RoutableViewModelBase> viewModelCache = new();
    private static readonly ConcurrentDictionary<Type, byte> attachedListeners = new();

    private static MainWindow? mainWindow;

    private static RoutingState? mainRouter;

    public static MainWindow MainWindow
    {
        get
        {
            if (mainWindow != null)
            {
                return mainWindow;
            }

            if (Application.Current?.ApplicationLifetime is not ClassicDesktopStyleApplicationLifetime desktop)
            {
                throw new NotSupportedException("This Avalonia application is only supported on desktop environments.");
            }
            return mainWindow = (MainWindow)desktop.MainWindow;
        }
    }

    public static RoutingState MainRouter => mainRouter ??= MainWindow.ViewModel?.Router ?? throw new Exception($"Tried to get {nameof(MainRouter)} before {nameof(Views.MainWindow)} was initialized");

    public static RoutableViewModelBase GetSharedViewModel<TModel>() where TModel : RoutableViewModelBase
    {
        Type key = typeof(TModel);
        if (viewModelCache.TryGetValue(key, out RoutableViewModelBase? vm))
        {
            return vm;
        }

        TModel viewModel = (TModel)key.GetConstructor(BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance, new[] { typeof(IScreen) })!.Invoke(new[] { MainWindow.ViewModel });
        viewModelCache.TryAdd(typeof(TModel), viewModel);
        return viewModel;
    }

    public IViewFor ResolveView<T>(T? viewModel, string? contract = null)
    {
        AttachListenersForViewChange(viewModel);
        // If you change a condition in this switch, make sure to listen for it in the method above.
        return viewModel switch
        {
            PlayViewModel => new PlayView(),
            ServersViewModel vm when vm.Servers.Any() => new ServersView(),
            ServersViewModel vm when !vm.Servers.Any() => new EmptyServersView(),
            _ => new PlayView()
        };
    }

    /// <summary>
    ///     Any conditions used in mapping method <see cref="ResolveView{T}" /> should be listened to, so the view updates when the condition changes.
    /// </summary>
    /// <remarks>
    ///     TODO: Use source generators to implement this method based on member access operations after ViewModel types in <see cref="ResolveView{T}"/>.
    /// </remarks>
    private void AttachListenersForViewChange<T>(T viewModel)
    {
        Type vmType = viewModel?.GetType() ?? throw new Exception($"Unable to get type of {nameof(viewModel)}");
        if (attachedListeners.ContainsKey(vmType))
        {
            return;
        }
        attachedListeners.TryAdd(vmType, 0);

        switch (viewModel)
        {
            case ServersViewModel vm:
                vm.Servers.PropertyChanged += (_, _) => MainRouter.Navigate.Execute(vm);
                break;
        }
    }
}


