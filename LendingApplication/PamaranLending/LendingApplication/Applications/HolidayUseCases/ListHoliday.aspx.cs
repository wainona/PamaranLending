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
    public partial class ListHoliday : ActivityPageBase
    {
        protected void RefreshData(object sender, StoreRefreshDataEventArgs e)
        {
            DataSource dataSource = new DataSource();
            //dataSource.Name = "";

            e.Total = dataSource.Count;
            this.PageGridPanelStore.DataSource = dataSource.SelectAll(e.Start, e.Limit, entity => entity.Name);
            this.PageGridPanelStore.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (X.IsAjaxRequest == false && this.IsPostBack == false)
            {
                if (this.LoginInfo.UserType == UserAccountType.Teller.Name)
                {
                    btnOpen.Hidden = false;
                    btnAdd.Hidden = true;
                    btnDelete.Hidden = true;
                    btnDeleteSeparator.Hidden = true;
                    btnOpenSeparator.Hidden = true;
                }
            }
        }

        protected void btnDelete_Click(object sender, DirectEventArgs e)
        {
            using (var context = new FinancialEntities())
            {
                RowSelectionModel sm = this.PageGridPanelSelectionModel;
                SelectedRowCollection rows = sm.SelectedRows;
                foreach (SelectedRow row in rows)
                {
                    int id = int.Parse(row.RecordID);
                    Holiday holiday = context.Holidays.SingleOrDefault(entity => entity.Id == id);
                    if (holiday.Date < DateTime.Now)
                    {
                        X.Msg.Alert("Delete Failed!", "Date has already passed").Show();
                    }
                    else
                    {
                        X.Msg.Alert("Delete Success!", "Holiday has been successfully deleted").Show();
                        if (holiday != null) //other user may have del
                            context.Holidays.DeleteObject(holiday);
                        
                    }
                    
                }
                context.SaveChanges();
            }
            this.PageGridPanel.DeleteSelected();
            this.PageGridPanelStore.DataBind();
        }

        private class DataSource : IPageAbleDataSource<Holiday>
        {
            public string Name { get; set; }

            public DataSource()
            {
                this.Name = string.Empty;
            }

            public override int Count
            {
                get
                {
                    int count = 0;
                    using (var context = new FinancialEntities())
                    {
                        count = context.Holidays.Where(entity => entity.Name.Contains(Name)).Count();
                    }

                    return count;
                }
            }

            public override List<Holiday> SelectAll(int start, int limit, Func<Holiday, string> orderBy)
            {
                List<Holiday> holidays;
                using (var context = new FinancialEntities())
                {
                    var query = context.Holidays.Where(entity => entity.Name.Contains(Name)).OrderBy(orderBy).Skip(start).Take(limit);
                    holidays = query.ToList();
                }

                return holidays;
            }
        }
    }
}