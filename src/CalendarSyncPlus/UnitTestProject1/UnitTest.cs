using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalendarSyncPlus.Domain.Models.Preferences;

namespace RefactoringTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void DailySyncTest()
        {
            DateTime dt = new DateTime(2015, 11, 23);
            SyncFrequency a = new DailySyncFrequency();
            System.Console.WriteLine(a.GetNextSyncTime(dt));
            System.Console.WriteLine(a.ValidateTimer(dt));
        }
        public void WeeklySyncTest()
        {
            DateTime dt = new DateTime(2015, 11, 23);
            SyncFrequency a = new WeeklySyncFrequency();
            System.Console.WriteLine(a.GetNextSyncTime(dt));
            System.Console.WriteLine(a.ValidateTimer(dt));
        }
        public void IntervalSyncTest()
        {
            DateTime dt = new DateTime(2015, 11, 23);
            SyncFrequency a = new IntervalSyncFrequency();
            System.Console.WriteLine(a.GetNextSyncTime(dt));
            System.Console.WriteLine(a.ValidateTimer(dt));
        }
    }
}
