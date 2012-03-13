using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class LoanDisbursementVcr
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

        public static LoanDisbursementVcr GetById(int id)
        {
            return Context.LoanDisbursementVcrs.SingleOrDefault(entity => entity.Id == id);
        }

        public static LoanDisbursementVcr Create(LoanApplication loanApplication, Agreement agreement, DateTime today)
        {
            LoanDisbursementVcr disbursement = new LoanDisbursementVcr();
            disbursement.Agreement = agreement;
            disbursement.Date = today;
            disbursement.Amount = loanApplication.LoanAmount;
            disbursement.Balance = loanApplication.LoanAmount;

            Context.LoanDisbursementVcrs.AddObject(disbursement);
            return disbursement;
        }

        public static LoanDisbursementVcr CreateForAdditionalLoan(LoanApplication loanApplication, Agreement agreement, decimal additionalAmount, DateTime today)
        {
            LoanDisbursementVcr disbursement = new LoanDisbursementVcr();
            disbursement.Agreement = agreement;
            disbursement.Date = today;
            disbursement.Amount = loanApplication.LoanAmount;
            disbursement.Balance = additionalAmount;

            Context.LoanDisbursementVcrs.AddObject(disbursement);
            return disbursement;
        }

        public DisbursementVcrStatu CurrentStatus
        {
            get
            {
                DisbursementVcrStatu disbursementStatus = this.DisbursementVcrStatus.SingleOrDefault(entity => entity.LoanDisbursementVoucherId == this.Id && entity.IsActive == true);
                if (disbursementStatus != null)
                    return disbursementStatus;
                return null;
            }
        }
    }
}
