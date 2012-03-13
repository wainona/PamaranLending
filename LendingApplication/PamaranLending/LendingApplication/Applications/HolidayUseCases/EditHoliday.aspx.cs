using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.FullfillmentForms;
using Ext.Net;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.HolidayUseCases
{
    public partial class EditHoliday : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Loan Clerk");
                allowed.Add("Admin");
                allowed.Add("Teller");
                return allowed;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                if (this.LoginInfo.UserType == UserAccountType.Teller.Name)
                {
                    btnEdit.Hidden = true;
                    btnSave.Hidden = true;
                    btnOpen.Hidden = true;
                }
             
                int id = int.Parse(Request.QueryString["id"]);
                hiddenId.Text = id.ToString();
                using (var context = new FinancialEntities())
                {
                    Holiday holiday = context.Holidays.SingleOrDefault(entity => entity.Id == id);
                    if (holiday != null)
                    {
                        this.txtName.Value = holiday.Name;
                        this.dtHoliday.SelectedDate = holiday.Date;
                        this.txtDesciption.Text = holiday.Description;
                        this.txtNotes.Text = holiday.Notes;
                    }
                    else
                    {
                        // TODO:: Don't know how to handle for now.
                    }
                }

                if (Holiday.GetById(id) == null)
                    throw new AccessToDeletedRecordException("The selected holiday has already been deleted by another user.");
            }
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var context = new FinancialEntities())
            {
                int id = int.Parse(this.hiddenId.Text);
                Holiday holiday = context.Holidays.SingleOrDefault(entity => entity.Id == id);
                holiday.Name = this.txtName.Text;
                holiday.Date = this.dtHoliday.SelectedDate;
                holiday.Description = this.txtDesciption.Text;
                holiday.Notes = this.txtNotes.Text;
                context.SaveChanges();
            }
        }

        public void btnOpen_Click(object sender, DirectEventArgs e)
        {
            btnOpen.Hidden = true;
            btnEdit.Hidden = false;
            pnlHoliday.Disabled = true;
            btnSave.Disabled = true;
        }

        protected void btnEdit_Click(object sender, DirectEventArgs e)
        {
            btnEdit.Hidden = true;
            btnOpen.Hidden = false;
            pnlHoliday.Disabled = false;
            btnSave.Disabled = false;
        }
    }
}