using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FirstPacific.UIFramework;
using BusinessLogic;
using Ext.Net;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication.Applications.CollectionUseCases
{
    public partial class CustomerChecksPickList : ActivityPageBase
	{
        public string ParentResourceGuid
        {
            get
            {
                if (ViewState["ParentResourceGuid"] != null)
                    return ViewState["ParentResourceGuid"].ToString();
                else
                    return null;
            }
            protected set
            {
                ViewState["ParentResourceGuid"] = value;
            }
        }

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

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            var querylist = CreateQuery();
            PageGridPanelStore.DataSource = querylist;
            PageGridPanelStore.DataBind();
        }

		protected void Page_Load(object sender, EventArgs e)
		{
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                this.ParentResourceGuid = Request.QueryString["ResourceGuid"];
                hiddenResourceGUID.Value = this.ParentResourceGuid;
                AddLoanPaymentForm form = this.Retrieve<AddLoanPaymentForm>(ParentResourceGuid);
                hiddenRandomKey.Value = Request.QueryString["RandomKey"];
                hiddenType.Value = Request.QueryString["RoleType"];
                AddLoanPaymentForm newForm = this.CreateOrRetrieve<AddLoanPaymentForm>();
                hiddenPatyRoleId.Text = form.CustomerId.ToString();
            }
		}

        private IEnumerable<ChequeViewModel> CreateQuery()
        {
            var resourceGUID = hiddenResourceGUID.Value.ToString();

            AddLoanPaymentForm form = this.Retrieve<AddLoanPaymentForm>(resourceGUID);
            var partyRoleId = int.Parse(hiddenPatyRoleId.Text);

            var partyRole = ObjectContext.PartyRoles.SingleOrDefault(entity => entity.Id == partyRoleId);
            var customerPartyRole = PartyRole.GetByPartyIdAndRole(partyRole.PartyId, RoleType.CustomerType);

            var chequesInModel = from ac in form.AvailableCheques select ac.CheckNumber;
            var models = ReceiptPaymentFacade.RetrieveChequeModelOfCustomerWithBalance(customerPartyRole);

            var browsableCheques = from c in models
                                   where chequesInModel.Contains(c.CheckNumber) == false
                                   select c;
            return browsableCheques;
        }
	}
}