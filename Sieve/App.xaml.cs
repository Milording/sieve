using Microsoft.Practices.Unity;
using Prism.Unity.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Sieve.Services;

namespace Sieve
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : PrismUnityApplication
    {
        #region Constructor

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Overrided methods

        /// <summary>
        /// Invokes when application is initialized.
        /// </summary>
        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            Container.RegisterInstance(this.NavigationService);

            Container.RegisterType<IFilterService, FilterService>(new ContainerControlledLifetimeManager());

            return base.OnInitializeAsync(args);
        }


        /// <summary>
        /// Invokes when application is launched.
        /// </summary>
        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate("Main", null);

            return Task.FromResult<object>(null);
        }

        #endregion
    }
}
