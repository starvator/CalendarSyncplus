using System;
using System.Waf.Foundation;
using System.Xml.Serialization;

namespace CalendarSyncPlus.Domain.Models.Preferences
{
    [Serializable]

    interface SyncFrequency
    {
        public virtual bool ValidateTimer(DateTime dateTimeNow);
        public virtual DateTime GetNextSyncTime(DateTime dateTimeNow);

    }

    /*
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
       */
}