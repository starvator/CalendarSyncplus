﻿#region File Header

// /******************************************************************************
//  * 
//  *      Copyright (C) Ankesh Dave 2015 All Rights Reserved. Confidential
//  * 
//  ******************************************************************************
//  * 
//  *      Project:        CalendarSyncPlus
//  *      SubProject:     CalendarSyncPlus
//  *      Author:         Dave, Ankesh
//  *      Created On:     23-01-2015 1:55 PM
//  *      Modified On:    02-02-2015 2:50 PM
//  *      FileName:       App.xaml.cs
//  * 
//  *****************************************************************************/

#endregion

#region Imports

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Waf.Applications;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using CalendarSyncPlus.Analytics;
using CalendarSyncPlus.Application.Controllers;
using CalendarSyncPlus.Application.ViewModels;
using CalendarSyncPlus.Authentication.Google;
using CalendarSyncPlus.Common;
using CalendarSyncPlus.Common.Log;
using CalendarSyncPlus.ExchangeWebServices.Calendar;
using CalendarSyncPlus.GoogleServices.Calendar;
using CalendarSyncPlus.GoogleServices.Google;
using CalendarSyncPlus.OutlookServices.Calendar;
using CalendarSyncPlus.Presentation.Helpers;
using CalendarSyncPlus.Presentation.Services.SingleInstance;
using CalendarSyncPlus.Services.Calendars.Interfaces;
using CalendarSyncPlus.Services.Interfaces;
using CalendarSyncPlus.SyncEngine.Interfaces;
using WPFLocalizeExtension.Providers;

#endregion

namespace CalendarSyncPlus.Presentation
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : ISingleInstanceApp
    {
        static App()
        {
            DispatcherHelper.Initialize();
            ToolTipService.ShowDurationProperty.OverrideMetadata(
                typeof (DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));
            LocalizationInit();
        }

        public App(bool startMinimized = false)
        {
            _startMinimized = startMinimized;
        }

        #region ISingleInstanceApp Members

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            //Activate Hidden,Background Application
            Current.Dispatcher.BeginInvoke(((Action) (() => Utilities.BringToForeground(MainWindow))));
            return true;
        }

        #endregion

        private static void LocalizationInit()
        {
            ResxLocalizationProvider.Instance.FallbackAssembly = "CalendarSyncPlus.Common";
            ResxLocalizationProvider.Instance.FallbackDictionary = "Resources";
        }

        #region Fields

        private static ApplicationLogger _applicationLogger;
        private bool _startMinimized;
        private AggregateCatalog catalog;
        private CompositionContainer container;
        private IApplicationController controller;

        #endregion

        #region Properties

        #endregion

        #region Private Methods

        private void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception, e.IsTerminating);
        }

        private void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception, false);
        }

        private static void HandleException(Exception exception, bool isTerminating)
        {
            if (exception == null)
            {
                return;
            }

            Trace.TraceError(exception.ToString());

            if (!isTerminating)
            {
                MessageBox.Show(string.Format(CultureInfo.CurrentCulture,
                    "Unknown Error Occurred:{1}{0}", exception, Environment.NewLine)
                    , ApplicationInfo.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
                _applicationLogger.GetLogger(typeof (App)).Info(exception);
            }
        }

        #endregion

        #region Protected Methods

        /// <exception cref="ImportCardinalityMismatchException">
        ///     There are zero exported objects with the contract name derived from
        ///     <paramref name="T" /> in the <see cref="CompositionContainer" />
        ///     .-or-There is more than one exported object with the contract name
        ///     derived from <paramref name="T" /> in the
        ///     <see cref="CompositionContainer" /> .
        /// </exception>
        /// <exception cref="CompositionContractMismatchException">
        ///     The underlying exported object cannot be cast to
        ///     <paramref name="T" /> .
        /// </exception>
        /// <exception cref="CompositionException">
        ///     An error occurred during composition.
        ///     <see cref="System.ComponentModel.Composition.CompositionException.Errors" />
        ///     will contain a collection of errors that occurred.
        /// </exception>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;

            catalog = new AggregateCatalog();
            // Add the WpfApplicationFramework assembly to the catalog
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (ViewModel).Assembly));
            // Add the Common assembly to catalog
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ApplicationLogger).Assembly));
            //Add Services assembly to catalog
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (ICalendarService).Assembly));
            //Add Authentication.Google assembly to catalog
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(IAccountAuthenticationService).Assembly));
            //Add GoogleServices assembly to catalog
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (ICalendarService).Assembly));
            //Add OutlookServices assembly to catalog
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (ICalendarService).Assembly));
            //Add ExchangeWebServices assembly to catalog
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (IExchangeWebCalendarService).Assembly));
            //Add SyncEngine assembly to catalog
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (ICalendarSyncEngine).Assembly));
            //Add Analytics assembly to catalog
            catalog.Catalogs.Add(new AssemblyCatalog(typeof (SyncAnalyticsService).Assembly));
            // Add the Application assembly to the catalog
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(ShellViewModel).Assembly));
            // Add the Presentation assembly to the catalog
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            //Composition Container
            container = new CompositionContainer(catalog, true);
            var batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);

            //Load settings
            var settings = container.GetExportedValue<ISettingsProvider>().GetSettings();
            container.ComposeExportedValue(settings);

            //Load sync summary
            var syncSummary = container.GetExportedValue<ISyncSummaryProvider>().GetSyncSummary();
            container.ComposeExportedValue(syncSummary);

            //Get Application logger
            _applicationLogger = container.GetExportedValue<ApplicationLogger>();
            _applicationLogger.Setup();

            //Initialize Application Controller
            controller = container.GetExportedValue<IApplicationController>();

            controller.Initialize();
            if (settings.AppSettings.StartMinimized)
            {
                _startMinimized = true;
            }
            controller.Run(_startMinimized);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //Shutdown application Controller
            controller.Shutdown();

            //Dispose All parts
            container.Dispose();
            catalog.Dispose();
        }

        #endregion
    }
}