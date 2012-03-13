using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class CustomerStatu
    {
        private static FinancialEntities Context
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public static CustomerStatu GetActive(PartyRole partyRole)
        {
            return Context.CustomerStatus.FirstOrDefault(entity => entity.IsActive && entity.PartyRoleId == partyRole.Id);
        }

        public static bool CanChangeStatusTo(PartyRole partyRole, CustomerStatusType statusTo)
        {
            CustomerStatu status = GetActive(partyRole);

            if (status == null)
                return true;

            if (status.CustomerStatusTypeId == statusTo.Id)
                return false;

            if (status.CustomerStatusTypeId == CustomerStatusType.NewType.Id)
                return statusTo.Id == CustomerStatusType.ActiveType.Id;
            else if (status.CustomerStatusTypeId == CustomerStatusType.ActiveType.Id)
                return statusTo.Id != CustomerStatusType.NewType.Id;
            else if (status.CustomerStatusTypeId == CustomerStatusType.InactiveType.Id)
                return statusTo.Id == CustomerStatusType.ActiveType.Id;
            else if (status.CustomerStatusTypeId == CustomerStatusType.DelinquentType.Id)
                return statusTo.Id == CustomerStatusType.ActiveType.Id;
            else if (status.CustomerStatusTypeId == CustomerStatusType.SubprimeType.Id)
                return statusTo.Id == CustomerStatusType.ActiveType.Id;
            else
                return false;

            //if (status.CustomerStatusType.Id == CustomerStatusType.ActiveType.Id)
            //{
            //    return statusTo.Id != CustomerStatusType.ActiveType.Id;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public static CustomerStatu ChangeStatus(PartyRole partyRole, CustomerStatusType statusTo, DateTime today)
        {
            CustomerStatu status = GetActive(partyRole);
            if (CanChangeStatusTo(partyRole, statusTo))
            {
                return CreateOrUpdateCurrent(partyRole, statusTo, today);
            }
            return status;
        }

        public static CustomerStatu CreateOrUpdateCurrent(PartyRole partyRole, CustomerStatusType statusTo, DateTime today)
        {
            CustomerStatu customerAppStatus = GetActive(partyRole);
            if (customerAppStatus != null && customerAppStatus.CustomerStatusType.Id != statusTo.Id)
                customerAppStatus.IsActive = false;

            if (customerAppStatus == null || customerAppStatus.CustomerStatusType.Id != statusTo.Id)
            {
                CustomerStatu status = new CustomerStatu();
                status.CustomerStatusType = statusTo;
                status.PartyRoleId = partyRole.Id;
                status.TransitionDateTime = today;
                status.IsActive = true;
                return status;
            }
            return customerAppStatus;
        }
    }
}
