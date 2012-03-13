using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class Taxpayer
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

        public Ctc CurrentCtc
        {
            get
            {
                var ctc = this.Ctcs.OrderByDescending(entity => entity.DateIssued).FirstOrDefault(entity => entity.PartyRoleId == this.PartyRoleId);
                if (ctc != null)
                    return ctc;
                return null;
            }
        }

        public static void CreateOrUpdateTaxPayer(PartyRole partyRole, string tin)
        {
            var taxPayer = Context.Taxpayers.SingleOrDefault(entity => entity.PartyRoleId == partyRole.Id);
            if (taxPayer != null)
            {
                if (taxPayer.Tin == tin) return;
                //if (string.IsNullOrWhiteSpace(tin))
                //{
                //    Context.Taxpayers.DeleteObject(taxPayer);
                //    return;
                //}
                taxPayer.Tin = tin;
                return;
            }

            if (string.IsNullOrWhiteSpace(tin)) return;

            Taxpayer NewTaxPayer = new Taxpayer();
            NewTaxPayer.PartyRoleId = partyRole.Id;
            NewTaxPayer.Tin = tin;

            Context.Taxpayers.AddObject(NewTaxPayer);
        }
    }
}
