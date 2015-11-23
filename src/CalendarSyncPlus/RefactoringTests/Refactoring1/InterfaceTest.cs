using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalendarSyncPlus.Domain.Models.Preferences;
using CalendarSyncPlus.Services.Tasks.Interfaces;
using CalendarSyncPlus.Services.Calendars.Interfaces;
using CalendarSyncPlus.Services.Contacts.Interfaces;
using CalendarSyncPlus.OutlookServices.Task;
using CalendarSyncPlus.OutlookServices.Calendar;
using CalendarSyncPlus.OutlookServices.Contact;
using CalendarSyncPlus.GoogleServices.Tasks;
using CalendarSyncPlus.GoogleServices.Contacts;
using CalendarSyncPlus.GoogleServices.Calendar;
using CalendarSyncPlus.Authentication.Google;
using CalendarSyncPlus.Common.Log;

namespace RefactoringTests
{
    [TestClass]
    public class InterfaceTest
    {
        [TestMethod]
        public void TestTaskInterfaceHierarchy()
        {
            ITaskService outlookTask = new OutlookTaskService(new ApplicationLogger());
            ITaskService googleTask = new GoogleTaskService(new IAccountAuthenticationService(), new ApplicationLogger());
        }

        [TestMethod]
        public void TestCalendarInterfaceHierarchy()
        {
            ICalendarService outlookCalendar = new OutlookCalendarService(new ApplicationLogger());
            ICalendarService googleCalendar = new GoogleCalendarService(new IAccountAuthenticationService(), new ApplicationLogger());
        }

        [TestMethod]
        public void TestContactInterfaceHierarchy()
        {
            IContactService outlookContact = new OutlookContactService();
            IContactService googleContact = new GoogleContactService();
        }
    }
}
