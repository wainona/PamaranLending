using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class CustomerSourceOfIncome
    {
        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
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

        public static CustomerSourceOfIncome GetById(int id)
        {
            return Context.CustomerSourceOfIncomes.SingleOrDefault(entity => entity.Id == id);
        }

        public static CustomerSourceOfIncome GetByPartyRoleId(int id)
        {
            return Context.CustomerSourceOfIncomes.SingleOrDefault(entity => entity.PartyRoleId == id);
        }

        public static List<CustomerSourceOfIncome> GetAllActive(Customer customer)
        {
            return customer.CustomerSourceOfIncomes.Where(entity => entity.EndDate == null && entity.PartyRoleId == customer.PartyRoleId).ToList();
        }

        public static CustomerSourceOfIncome GetActive(Customer customer, SourceOfIncome sourceOfIncome)
        {
            return customer.CustomerSourceOfIncomes.FirstOrDefault(entity => entity.EndDate == null && entity.SourceOfIncomeId == sourceOfIncome.Id);
        }

        public static CustomerSourceOfIncome Create(SourceOfIncome sourceOfIncome, Customer customer, DateTime today)
        {
            CustomerSourceOfIncome customerSourceOfIncome = new CustomerSourceOfIncome();
            customerSourceOfIncome.Customer = customer;
            customerSourceOfIncome.EffectiveDate = today;
            customerSourceOfIncome.SourceOfIncome = sourceOfIncome;

            Context.CustomerSourceOfIncomes.AddObject(customerSourceOfIncome);
            return customerSourceOfIncome;
        }

        public static CustomerSourceOfIncome CreateOrUpdate(SourceOfIncome sourceOfIncome, Customer customer, DateTime today)
        {
            CustomerSourceOfIncome current = GetActive(customer, sourceOfIncome);
            if (current != null)
                current.EndDate = today;

            CustomerSourceOfIncome customerSourceOfIncome = new CustomerSourceOfIncome();
            customerSourceOfIncome.Customer = customer;
            customerSourceOfIncome.EffectiveDate = today;
            customerSourceOfIncome.SourceOfIncome = sourceOfIncome;

            Context.CustomerSourceOfIncomes.AddObject(customerSourceOfIncome);
            return customerSourceOfIncome;
        }


    }
}
