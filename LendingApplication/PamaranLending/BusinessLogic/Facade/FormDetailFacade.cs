using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class FormDetail
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

        public static FormDetail GetByPaymentId(int id)
        {
            return Context.FormDetails.SingleOrDefault(entity => entity.PaymentId == id);
        }

        public static IQueryable<FormDetail> GetByLoanAppIdAndType(int id, FormType type)
        {
            return Context.FormDetails.Where(entity => entity.LoanAppId == id && entity.FormTypeId == type.Id);
        }

        public static FormDetail Create(int formTypeId, LoanApplication loanApp, string RoleString, string Person, string Signature)
        {
            FormDetail form = new FormDetail();
            form.FormTypeId = formTypeId;
            form.LoanApplication = loanApp;
            form.RoleString = RoleString;
            if (string.IsNullOrWhiteSpace(Person) == false)
                form.PersonString = Person;
            if (string.IsNullOrWhiteSpace(Signature) == false)
                form.Signature = Signature;
            return form;
        }
        public static FormDetail Create(int formTypeId, Payment payment, string RoleString, string Person, string Signature)
        {
            FormDetail form = new FormDetail();
            form.FormTypeId = formTypeId;
            form.Payment = payment;
            form.RoleString = RoleString;
            if (string.IsNullOrWhiteSpace(Person) == false)
                form.PersonString = Person;
            if (string.IsNullOrWhiteSpace(Signature) == false)
                form.Signature = Signature;
            return form;
        }
    }
}
