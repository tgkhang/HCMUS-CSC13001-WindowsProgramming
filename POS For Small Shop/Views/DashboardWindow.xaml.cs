using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;
using Microsoft.UI.Windowing;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_For_Small_Shop.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardWindow : Window
    {
        public static DashboardWindow Instance { get; private set; }
        private string _username;
        public Frame CurrentFrame { get; private set; }
        public DashboardWindow()
        {
            this.InitializeComponent();
            Instance = this;
            //_username = username;
            this.Activated += DashboardWindow_Activated;
            //          App.MainWindow = this;
            CurrentFrame = this.MainFrame;
            MainFrame.Navigate(typeof(HomePage));
        }

        public void NavigateToPage(Type pageType)
        {
            MainFrame.Navigate(pageType);
        }

        private void DashboardWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            // Detach the event after first use
            this.Activated -= DashboardWindow_Activated;

            IntPtr hwnd = WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            if (appWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.Maximize();
            }
        }

    }
}
