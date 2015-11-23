﻿using System;
using System.Waf.Foundation;
using System.Xml.Serialization;

// this is a change for Tyler

namespace CalendarSyncPlus.Domain.Models.Preferences
{
    [Serializable]
    public class SyncFrequency : ValidatableModel
    {
        public string Name { get; protected set; }

        public virtual bool ValidateTimer(DateTime dateTimeNow)
        {
            return false;
        }

        public virtual DateTime GetNextSyncTime(DateTime dateTimeNow)
        {
            return dateTimeNow;
        }
    }
}