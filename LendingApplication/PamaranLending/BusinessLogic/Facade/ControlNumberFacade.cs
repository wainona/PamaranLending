using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ControlNumberFacade
    {
        public static FinancialEntities Context
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        private static ControlNumber _Create(FormType formType)
        {
            int lastControlNum = 0;
            var lastControls = Context.ControlNumbers.Where(e => e.FormTypeId == formType.Id);
            if (lastControls.Count() == 0)
            {
                lastControlNum = 0;
            }
            else
            {
                lastControlNum = lastControls.Max(e => e.LastControlNumber) + 1;
            }

            ControlNumber controlnum = new ControlNumber();
            controlnum.FormType = formType;
            controlnum.LastControlNumber = lastControlNum;
            Context.ControlNumbers.AddObject(controlnum);

            return controlnum;
        }

        public static ControlNumber Create(FormType formType, Payment payment)
        {
            var controlNumber = _Create(formType);
            controlNumber.Payment = payment;
            return controlNumber;
        }

        public static ControlNumber GetByPaymentId(int paymentId, FormType formType)
        {
            return Context.ControlNumbers.FirstOrDefault(e => e.PaymentId == paymentId && e.FormTypeId == formType.Id);
        }

        public static ControlNumber GetByLoanAppId(int loanAppId)
        {
            return Context.ControlNumbers.FirstOrDefault(e => e.ApplicationId == loanAppId && e.FormTypeId == FormType.PromissoryNoteType.Id);
        }

        public static ControlNumber Create(LoanApplication loanapplication)
        {
            var controlNumber = _Create(FormType.PromissoryNoteType);
            controlNumber.LoanApplication = loanapplication;

            return controlNumber;
        }

    }
}
