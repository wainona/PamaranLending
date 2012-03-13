using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Web;

namespace FirstPacific.UIFramework
{
    /// <summary>
    /// Creates one ObjectContext instance per HTTP request. This instance is then
    /// shared during the lifespan of the HTTP request.
    /// </summary>
    public sealed class AspNetObjectContextManager<T> where T : ObjectContext, new()
    {

        /// <summary>
        /// Returns a shared ObjectContext instance.
        /// </summary>
        public static T ObjectContext
        {
            get
            {
                string ocKey = "ocm_" + HttpContext.Current.GetHashCode().ToString("x");

                if (HttpContext.Current.Items.Contains(ocKey) == false)
                {
                    HttpContext.Current.Items.Add(ocKey, new T());
                }
                return HttpContext.Current.Items[ocKey] as T;
            }
        }

        static AspNetObjectContextManager()
        {
            if (HttpContext.Current == null)
                throw new InvalidOperationException("An AspNetObjectContextManager can only be used in a HTTP context.");
        }
    }
}
