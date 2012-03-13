using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;
using BusinessLogic;

namespace BusinessLogic.FullfillmentForms
{
    public class ForeignExchangeDetailForm : FullfillmentForm<FinancialEntities>
    {
        private List<ForExDenominationModel> ForExDenominationsFrom;
        private List<ForExDenominationModel> ForExDenominationsTo;

        public ForeignExchangeDetailForm()
        {
            ForExDenominationsFrom = new List<ForExDenominationModel>();
            ForExDenominationsTo = new List<ForExDenominationModel>();
        }
        
        public override void  Retrieve(int id)
        {

        }

        public override void  PrepareForSave()
        {
 	        
        }
    }
}
