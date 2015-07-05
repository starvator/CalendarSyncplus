﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Waf.Applications;
using System.Waf.Foundation;
using System.Xml.Serialization;

namespace CalendarSyncPlus.Domain.Models.Preferences
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Settings : Model
    {
        /// <summary>
        /// 
        /// </summary>
        public string SettingsVersion { get; set; }

        /// <summary>
        /// </summary>
        public bool AllowManualAuthentication { get; set; }

        /// <summary>
        /// </summary>
        [XmlIgnore]
        public bool IsFirstSave { get; set; }

        /// <summary>
        /// </summary>
        public ObservableCollection<GoogleAccount> GoogleAccounts { get; set; }

        /// <summary>
        /// </summary>
        public ObservableCollection<CalendarSyncProfile> CalendarSyncProfiles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ContactsSyncProfile> ContactSyncProfiles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<TaskSyncProfile> TaskSyncProfiles { get; set; }
        /// <summary>
        /// </summary>
        public AppSettings AppSettings { get; set; }
        /// <summary>
        /// </summary>
        public LogSettings LogSettings { get; set; }
       
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static Settings GetDefaultSettings()
        {
            var settings = new Settings
            {
                IsFirstSave = true,
                SettingsVersion = ApplicationInfo.Version,
                AppSettings = new AppSettings
                {
                    MinimizeToSystemTray = true,
                    CheckForUpdates = true,
                    PeriodicSyncOn = true,
                    RunApplicationAtSystemStartup = true,
                    ProxySettings = new ProxySetting
                    {
                        ProxyType = ProxyType.Auto
                    }
                },
                CalendarSyncProfiles = new ObservableCollection<CalendarSyncProfile>
                {
                    CalendarSyncProfile.GetDefaultSyncProfile()
                },
                GoogleAccounts = new ObservableCollection<GoogleAccount>()
            };
            return settings;
        }
       
    }
}