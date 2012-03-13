using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using FirstPacific.UIFramework;
using BusinessLogic.FullfillmentForms;

namespace LendingApplication.Applications.LoanRestructureUseCases
{
    public partial class ListAdditionalLoan : ActivityPageBase
    {
        //public override List<string> UserTypesAllowed
        //{
        //    get
        //    {
        //        List<string> allowed = new List<string>();
        //        allowed.Add("Super Admin");
        //        allowed.Add("Loan Clerk");
        //        allowed.Add("Admin");
        //        return allowed;
        //    }
        //}

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

        public string HiddenCustomerId
        {
            get
            {
                return string.IsNullOrWhiteSpace(hiddenCustomerID.Value.ToString()) ? "0" : hiddenCustomerID.Value.ToString();
            }
        }

        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            int id = int.Parse(HiddenCustomerId);
            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            form.LoanRestructuredView.Clear();
            form.RetrieveLoanRestructureList(id);

            PageGridPanelStore.DataSource = form.LoanRestructuredView.ToList();
            PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
                hiddenResourceGuid.Value = this.ResourceGuid;

            }
        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            //using (var context = new BusinessModelEntities())
            //{
            //    RowSelectionModel sm = this.PageGridPanelSelectionModel;
            //    SelectedRowCollection rows = sm.SelectedRows;
            //    foreach (SelectedRow row in rows)
            //    {
            //        int id = int.Parse(row.RecordID);
            //        Customer customer = context.Customers.SingleOrDefault(entity => entity.ID == id);
            //        if (customer != null) //other user may have del
            //            context.Customers.DeleteObject(customer);
            //    }
            //    context.SaveChanges();
            //}
            //this.PageGridPanel.DeleteSelected();
            //this.PageGridPanelStore.DataBind();
        }

        [DirectMethod]
        public void FillLoanRestructureGrid(int id)
        {
            Fill(id);
        }

        protected void Fill(int id)
        {
            LoanRestructureForm form = this.CreateOrRetrieve<LoanRestructureForm>();
            form.LoanRestructuredView.Clear();
            form.RetrieveLoanRestructureList(id);

            PageGridPanelStore.DataSource = form.LoanRestructuredView.ToList();
            PageGridPanelStore.DataBind();
        }
    }
}