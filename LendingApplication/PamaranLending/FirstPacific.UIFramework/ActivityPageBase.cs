using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Data.Objects;

namespace FirstPacific.UIFramework
{
    public class ActivityPageBase : Page
    {
        public ActivityPageBase()
        {
            base.PreInit += new EventHandler(ActivityPageBase_PreInit);
            this.ResourceGuid = null;
        }

        public string ResourceGuid 
        {
            get
            {
                if (ViewState["ResourceGuid"] != null)
                    return ViewState["ResourceGuid"].ToString();
                else
                    return null;
            }
            protected set
            {
                ViewState["ResourceGuid"] = value;
            }
        }

        public virtual List<string> UserTypesAllowed
        {
            get
            {
                List<string> allowed = new List<string>();
                allowed.Add("Admin");
                allowed.Add("Super Admin");
                allowed.Add("Accountant");
                allowed.Add("Teller");
                return allowed;
            }
        }

        public LoginSessionInformation LoginInfo
        {
            get
            {
                return LoginSessionInformation.Instance;
            }
        }

        public F CreateOrRetrieve<F>()
            where F :FullfillmentForm, new ()
        {
            F form = default(F);
            if (this.ResourceGuid == null)
            {
                form = new F();
                this.ResourceGuid = SessionResourceManager.Instance.AddTimedResources(form);
            }
            else
            {
                form = (F)SessionResourceManager.Instance.RetrieveResource(this.ResourceGuid);
                SessionResourceManager.Instance.ExtendLifetimeOfResource(this.ResourceGuid);
                if (form == null)
                {
                    form = new F();
                    this.ResourceGuid = SessionResourceManager.Instance.AddTimedResources(form);
                }
            }
            return form;
        }

        public F Retrieve<F>(string resourceGuid)
            where F : FullfillmentForm, new()
        {
            F form = (F)SessionResourceManager.Instance.RetrieveResource(resourceGuid);
            SessionResourceManager.Instance.ExtendLifetimeOfResource(resourceGuid);
            return form;
        }

        public F Retrieve<F>()
            where F : FullfillmentForm, new()
        {
            F form = (F)SessionResourceManager.Instance.RetrieveResource(this.ResourceGuid);
            SessionResourceManager.Instance.ExtendLifetimeOfResource(this.ResourceGuid);
            return form;
        }

        public F CreateOrRetrieveBOM<F>()
            where F : BusinessObjectModel, new()
        {
            F form = default(F);
            if (this.ResourceGuid == null)
            {
                form = new F();
                this.ResourceGuid = SessionResourceManager.Instance.AddTimedResources(form);
            }
            else
            {
                form = (F)SessionResourceManager.Instance.RetrieveResource(this.ResourceGuid);
                SessionResourceManager.Instance.ExtendLifetimeOfResource(this.ResourceGuid);
                if (form == null)
                {
                    form = new F();
                    this.ResourceGuid = SessionResourceManager.Instance.AddTimedResources(form);
                }
            }
            return form;
        }

        public F RetrieveBOM<F>(string resourceGuid)
            where F : BusinessObjectModel, new()
        {
            F form = (F)SessionResourceManager.Instance.RetrieveResource(resourceGuid);
            SessionResourceManager.Instance.ExtendLifetimeOfResource(resourceGuid);
            return form;
        }

        public F RetrieveBOM<F>()
            where F : BusinessObjectModel, new()
        {
            F form = (F)SessionResourceManager.Instance.RetrieveResource(this.ResourceGuid);
            SessionResourceManager.Instance.ExtendLifetimeOfResource(this.ResourceGuid);
            return form;
        }

        public void Register(BusinessObjectModel resource)
        {
            this.ResourceGuid = SessionResourceManager.Instance.AddTimedResources(resource);
        }

        public void Register(FullfillmentForm resource)
        {
            this.ResourceGuid = SessionResourceManager.Instance.AddTimedResources(resource);
        }

        public bool HasPermissionToPerformActivityType(string activityType)
        {
            return true;
        }

        private void ActivityPageBase_PreInit(object sender, EventArgs e)
        {
            SessionResourceManager.Instance.DisposeTimedOutResources();
            if (LoginInfo.IsAuthenticated == false)
                FormsAuthentication.RedirectToLoginPage();
            else if (LoginInfo.IsAuthenticated && UserTypesAllowed.Contains(LoginInfo.UserType) == false)
                FormsAuthentication.RedirectToLoginPage();
        }
    }
}