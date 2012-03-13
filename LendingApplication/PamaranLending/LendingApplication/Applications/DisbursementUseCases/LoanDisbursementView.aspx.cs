using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using FirstPacific.UIFramework;
using Ext.Net;

namespace LendingApplication.Applications.DisbursementUseCases
{
    public partial class LoanDisbursementView : ActivityPageBase
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
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Teller");
                allowed.Add("Admin");
                //allowed.Add("Cashier");
                return allowed;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {


                int id = int.Parse(Request.QueryString["id"]);
                hiddenDisbursementID.Text = id.ToString();

                var list = ObjectContext.LoanDisbursementViewLists.FirstOrDefault(entity => entity.PaymentId == id);

                var breakdown = DisbursementFacade.GetBreakdown(id);
                strBreakdown.DataSource = breakdown.ToList();
                strBreakdown.DataBind();

                txtAmountDisbursed.Text = list.AmountDisbursed.ToString("N");
                txtDateDisbursed.Text = list.DateDisbursed.ToString();
                txtDisbursedBy.Text = list.DisbursedBy;
                txtDisbursedTo.Text = list.DisbursedTo;
                txtLoanAgreementId.Text = list.LoanAgreementID.ToString();
                var disbursedToName = ObjectContext.Disbursements.FirstOrDefault(entity => entity.PaymentId == id);
                if (string.IsNullOrEmpty(disbursedToName.DisbursedToName) == false)
                    txtReceivedBy.Text = disbursedToName.DisbursedToName;
                else txtReceivedBy.Text = list.DisbursedTo;

                var agreementItems = ObjectContext.AgreementItems.Where(entity => entity.AgreementId == list.LoanAgreementID);
                var loanDisbursementVcr = ObjectContext.LoanDisbursementVcrs.FirstOrDefault(entity => entity.AgreementId == list.LoanAgreementID);
                var disbursements = ObjectContext.PaymentApplications.Where(entity => entity.LoanDisbursementVoucherId == loanDisbursementVcr.Id);
                var latestDisbursement = disbursements.OrderByDescending(entity => entity.PaymentId).FirstOrDefault();
                var oldestDisbursement = disbursements.OrderBy(entity => entity.PaymentId).FirstOrDefault();
                hiddenOldestDisbursementID.Text = oldestDisbursement.PaymentId.ToString(); 
                if (agreementItems.Count() == 1)
                {
                    // No additional loan
                    var partialstatus = from pa in ObjectContext.PaymentApplications
                                join lv in ObjectContext.LoanDisbursementVcrs on pa.LoanDisbursementVoucherId equals lv.Id
                                join lvs in ObjectContext.DisbursementVcrStatus on lv.Id equals lvs.LoanDisbursementVoucherId
                                where pa.PaymentId == id && lvs.DisbursementVoucherStatTypId == DisbursementVcrStatusEnum.PartiallyDisbursedType.Id
                                && lvs.IsActive == true
                                select lvs;
                    if (partialstatus.Count() != 0)
                    {
                        btnPrintRelease.Hidden = true;
                        btnPrintReleaseSeparator.Hidden = true;
                        //btnTransactionSlipSeparator.Hidden = true;
                        //btnPrintTransactionSlip.Hidden = true;
                    }
                    else
                    {
                        //check if this is the most current disbursement and if it does not have a status partial disbursement
                        if (latestDisbursement.PaymentId != id)
                        {
                            btnPrintRelease.Hidden = true;
                            btnPrintReleaseSeparator.Hidden = true;
                            //btnTransactionSlipSeparator.Hidden = true;
                            //btnPrintTransactionSlip.Hidden = true;
                        }
                    }
                }
                else
                {
                    //with additional loan
                    var partial = from pa in ObjectContext.PaymentApplications
                                  join lv in ObjectContext.LoanDisbursementVcrs on pa.LoanDisbursementVoucherId equals lv.Id
                                  join lvs in ObjectContext.DisbursementVcrStatus on lv.Id equals lvs.LoanDisbursementVoucherId
                                  join fa in ObjectContext.FinancialAccounts on lv.AgreementId equals fa.AgreementId
                                  join las in ObjectContext.LoanAccountStatus on fa.Id equals las.FinancialAccountId
                                  where pa.PaymentId == id && lvs.DisbursementVoucherStatTypId == DisbursementVcrStatusEnum.PartiallyDisbursedType.Id
                                  && lvs.TransitionDateTime == pa.Payment.EntryDate
                                  select lvs;
                  
                
                    //meaning has additional loan
                    if (partial.Count() != 0)
                    {
                        // has partiallydisbursedstatus after additional loan
                        btnPrintRelease.Hidden = true;
                        btnPrintReleaseSeparator.Hidden = true;
                        //btnTransactionSlipSeparator.Hidden = true;
                        //btnPrintTransactionSlip.Hidden = true;
                    }
                    else
                    {
                        //check if this is the most current disbursement and if it does not have a status partial disbursement
                        if (latestDisbursement.PaymentId != id) 
                        {
                            btnPrintRelease.Hidden = true;
                            btnPrintReleaseSeparator.Hidden = true;
                            //btnTransactionSlipSeparator.Hidden = true;
                            //btnPrintTransactionSlip.Hidden = true;
                        }
                    }
                }

              
            }

        }
    }
}