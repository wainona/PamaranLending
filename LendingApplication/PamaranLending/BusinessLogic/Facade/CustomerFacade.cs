using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class Customer
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

        public CustomerStatu CurrentStatus
        {
            get
            {
                return this.CustomerStatus.FirstOrDefault(entity => entity.IsActive);
            }
        }

        public CustomerCategory CurrentCustomerCategory
        {
            get
            {
                return this.CustomerCategories.FirstOrDefault(entity => entity.EndDate == null && entity.PartyRoleId == this.PartyRoleId);
            }
        }

        public CustomerClassification CurrentClassification
        {
            get
            {
                var classification = this.CustomerClassifications.FirstOrDefault(entity => entity.PartyRoleId == this.PartyRoleId && entity.EndDate == null);
                if (classification != null)
                    return classification;

                return null;
            }
        }

        public static Customer GetById(int id)
        {
            return Context.Customers.SingleOrDefault(entity => entity.PartyRoleId == id);
        }
        public static void UpdateCustomerStatus(DateTime date)
        {
            ///Deliquent -> Active If that customer no longer has an overdue account
            ///Subprime -> Active If customer has no under litigation account && no deliquent account
            ///Subprime -> Deliquent Active If customer has no under litigation account but has deliquent account
            ///Active -> Deliquent if customer has no under litigation account but has deliquent account 
            var customers = from c in Context.Customers
                            join pr in Context.PartyRoles on c.PartyRoleId equals pr.Id
                            join cs in Context.CustomerStatus on c.PartyRoleId equals cs.PartyRoleId
                            where cs.IsActive == true && (cs.CustomerStatusTypeId == CustomerStatusType.DelinquentType.Id
                            || cs.CustomerStatusTypeId == CustomerStatusType.SubprimeType.Id || cs.CustomerStatusTypeId == CustomerStatusType.ActiveType.Id
                            )
                            select new { cs, pr };
                            
            foreach (var customer in customers)
            {
                var loanAccountStatus = Party.RetrieveOutstandingLoans(customer.pr.PartyId);

                var countOfUnderLitigation = loanAccountStatus.Where(entity => entity.StatusTypeId == LoanAccountStatusType.UnderLitigationType.Id).Count();
                var countOfDelinquent = loanAccountStatus.Where(entity => entity.StatusTypeId == LoanAccountStatusType.DelinquentType.Id).Count();

                if (customer.cs.CustomerStatusTypeId == CustomerStatusType.DelinquentType.Id)
                {
                    if (countOfDelinquent == 0 && customer.cs.CustomerStatusTypeId != CustomerStatusType.ActiveType.Id) 
                        CustomerStatu.ChangeStatus(customer.pr, CustomerStatusType.ActiveType, date);
                }
                else if (customer.cs.CustomerStatusTypeId == CustomerStatusType.SubprimeType.Id)
                {
                    if (countOfUnderLitigation == 0 && countOfDelinquent > 0 && customer.cs.CustomerStatusTypeId != CustomerStatusType.DelinquentType.Id)
                        CustomerStatu.ChangeStatus(customer.pr, CustomerStatusType.DelinquentType, date);
                    else if (countOfUnderLitigation == 0 && countOfDelinquent == 0 &&customer.cs.CustomerStatusTypeId != CustomerStatusType.ActiveType.Id)
                        CustomerStatu.ChangeStatus(customer.pr, CustomerStatusType.ActiveType, date);
                }
                else if (customer.cs.CustomerStatusTypeId == CustomerStatusType.ActiveType.Id)
                {
                    if (countOfUnderLitigation == 0 && countOfDelinquent > 0&&customer.cs.CustomerStatusTypeId != CustomerStatusType.DelinquentType.Id)
                        CustomerStatu.ChangeStatus(customer.pr, CustomerStatusType.DelinquentType, date);
                }
            }

        }
    }
}
