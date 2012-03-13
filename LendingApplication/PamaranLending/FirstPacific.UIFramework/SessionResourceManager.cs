using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FirstPacific.UIFramework
{
    public class SessionResourceManager
    {
        private Dictionary<string, TimeManagedResources> forms;

        public SessionResourceManager()
        {
            this.forms = new Dictionary<string, TimeManagedResources>();
        }

        public string AddTimedResources(object resource)
        {
            return AddTimedResources(resource, TimeSpan.FromHours(8));
        }

        public string AddTimedResources(object resource, TimeSpan timeout)
        {
            TimeManagedResources timeManageResource = new TimeManagedResources() ;
            timeManageResource.TimeOut = timeout;
            timeManageResource.Resource = resource;
            forms.Add(timeManageResource.RandomKey, timeManageResource);
            return timeManageResource.RandomKey;
        }

        public object RetrieveResource(string guid)
        {
            object resource = null;
            if (forms.ContainsKey(guid))
            {
                TimeManagedResources timeManageResource = forms[guid];
                resource = timeManageResource.Resource;
            }
            return resource;
        }

        public void ExtendLifetimeOfResource(string guid)
        {
            if (forms.ContainsKey(guid))
            {
                TimeManagedResources timeManageResource = forms[guid];
                timeManageResource.Extend();
            }
        }

        public void DisposeTimedOutResources()
        {
            List<TimeManagedResources> toRemove = new List<TimeManagedResources>();
            foreach (TimeManagedResources resource in this.forms.Values)
            {
                if (resource.HasTimedOut)
                    toRemove.Add(resource);
            }

            foreach (TimeManagedResources item in toRemove)
            {
                this.forms.Remove(item.RandomKey);
            }
        }

        public static SessionResourceManager Instance
        {
            get
            {
                SessionResourceManager form =
                    HttpContext.Current.Session["SessionFullfillmentFormsManager"] as SessionResourceManager;
                if (form == null)
                {
                    form = new SessionResourceManager();
                    HttpContext.Current.Session["SessionFullfillmentFormsManager"] = form;
                }
                return form;
            }
        }
    }

    public class TimeManagedResources
    {
        public DateTime StartTime { get; private set; }

        public TimeSpan TimeOut { get; set; }

        public string RandomKey { get;private set; }

        public object Resource { get; set; }

        public TimeManagedResources ()
	    {
            this.StartTime = DateTime.Now;
            this.RandomKey = Guid.NewGuid().ToString().GetHashCode().ToString("x");
        }

        public void Extend()
        {
            this.StartTime = DateTime.Now;
        }

        public bool HasTimedOut
        {
            get
            {
                return (DateTime.Now - this.StartTime) > TimeOut;
            }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (obj is TimeManagedResources)
            {
                TimeManagedResources other = obj as TimeManagedResources;
                return this.RandomKey == other.RandomKey;
            }
            return base.Equals(obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
