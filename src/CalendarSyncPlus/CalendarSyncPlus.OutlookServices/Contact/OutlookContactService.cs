using System.ComponentModel.Composition;
using CalendarSyncPlus.Common.MetaData;
using CalendarSyncPlus.Services.Contacts.Interfaces;

namespace CalendarSyncPlus.OutlookServices.Contact
{
    [Export(typeof(IContactService))]
    [ExportMetadata("ServiceType", ServiceType.OutlookDesktop)]
    public class OutlookContactService : IContactService
    {
    }
}
