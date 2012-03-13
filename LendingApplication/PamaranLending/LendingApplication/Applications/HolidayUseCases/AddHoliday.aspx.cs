using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using BusinessLogic;
using FirstPacific.UIFramework;

namespace LendingApplication.Applications.HolidayUseCases
{
    public partial class AddHoliday : ActivityPageBase
    {
        public override List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Super Admin");
                allowed.Add("Loan Clerk");
                allowed.Add("Admin");
                return allowed;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            dtHoliday.MinDate = DateTime.Now;
        }

        protected void btnSave_Click(object sender, DirectEventArgs e)
        {
            using (var context = new FinancialEntities())
            {
                Holiday holiday = new Holiday();
                holiday.Name = this.txtName.Text;
                holiday.Date = this.dtHoliday.SelectedDate;
                holiday.Description = this.txtDesciption.Text;
                holiday.Notes = this.txtNotes.Text;
                context.Holidays.AddObject(holiday);
                context.SaveChanges();
            }
        }
    }
}