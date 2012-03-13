using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;
using System.Runtime.Remoting.Contexts;

namespace BusinessLogic
{
    public partial class Employee
    {
        private static FinancialEntities ObjectContext
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public static Employee GetById(int id)
        {
            var employee = ObjectContext.Employees.SingleOrDefault(entity => entity.PartyRoleId == id);
            return employee;
        }


        public static IQueryable<EmployeeHistoryModel> GetEmployeeHistory(int partyRoleId)
        {
            var receipts = from  r in ObjectContext.Receipts
                           join ra in ObjectContext.ReceiptPaymentAssocs on r.Id equals ra.ReceiptId
                           join p in ObjectContext.Payments on ra.PaymentId equals p.Id
                           where p.PaymentTypeId == PaymentType.Receipt.Id
                           && p.ProcessedToPartyRoleId == partyRoleId
                           select new EmployeeHistoryModel
                           {
                               Date = p.TransactionDate,
                               Particular = p.SpecificPaymentType.Name,
                               Received = p.TotalAmount,
                               Released = 0,
                               PaymentId = p.Id
                           };
            var disbursements = from d in ObjectContext.Disbursements
                                join p in ObjectContext.Payments on d.PaymentId equals p.Id
                                join par in ObjectContext.Parties on p.PartyRole.PartyId equals par.Id
                                where d.DisbursementTypeId != DisbursementType.OtherLoanDisbursementType.Id
                                && p.ProcessedToPartyRoleId == partyRoleId
                                && p.ParentPaymentId == null
                                select new EmployeeHistoryModel
                                {
                                    Date = p.TransactionDate,
                                    Particular = p.SpecificPaymentType.Name,
                                    Received = 0,
                                    Released = p.TotalAmount,
                                    PaymentId = p.Id
                                };

            var otherdisbursements = from d in ObjectContext.Disbursements
                                     join p in ObjectContext.Payments on d.PaymentId equals p.Id
                                     join par in ObjectContext.Parties on p.PartyRole.PartyId equals par.Id
                                     join di in ObjectContext.DisbursementItems on d.PaymentId equals di.PaymentId
                                     where d.DisbursementTypeId == DisbursementType.OtherLoanDisbursementType.Id
                                     && p.ProcessedToPartyRoleId == partyRoleId
                                     select new EmployeeHistoryModel
                                     {
                                         Date = p.TransactionDate,
                                         Particular = di.Particular,
                                         Received =0,
                                         Released = di.PerItemAmount,
                                         PaymentId = p.Id
                                     };

            ////Karaan pani.
            //var loanPayments = from p in ObjectContext.Payments
            //                   join lp in ObjectContext.LoanPayments on p.Id equals lp.PaymentId
            //                    where p.ProcessedToPartyRoleId == partyRoleId
            //                    && p.PaymentTypeId == PaymentType.LoanPayment.Id 
            //                    orderby p.TransactionDate ascending
            //                    select new EmployeeHistoryModel
            //                    {
            //                        Date = p.TransactionDate,
            //                        Particular = p.SpecificPaymentType.Name,
            //                        Amount = p.TotalAmount
            //                    };

            var feePayments = from fp in ObjectContext.FeePayments
                              join p in ObjectContext.Payments on fp.PaymentId equals p.Id
                              where p.ProcessedToPartyRoleId == partyRoleId
                              select new EmployeeHistoryModel
                              {
                                  Date = p.TransactionDate,
                                  Particular = fp.Particular,
                                  Received = p.TotalAmount,
                                  Released = 0,
                                  PaymentId = p.Id
                              };
            var query = receipts.Concat(disbursements).Concat(feePayments).Concat(otherdisbursements).OrderBy(e => e.Date).ThenBy( e =>e.PaymentId);

            return query;

        }
    }

    public class EmployeeHistoryModel
    {
        public int PaymentId { get; set; }
        public DateTime Date { get; set; }
        public string _Date { get { return Date.ToString("MM/dd/yyy"); } }
        public string Particular { get; set; }
        public decimal Received { get; set; }
        public decimal Released { get; set; }
        public string CurrencySymbol
        {
            get
            {
                    var currency = Currency.GetCurrencyByPaymentId(PaymentId);
                    if (currency != null)
                        return currency.Symbol;
                    else return "PHP";
              
            }
        }
    }
}
