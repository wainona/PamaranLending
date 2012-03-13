﻿using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Web;

using FirstPacific.UIFramework;

namespace FirstPacific.UIFramework.HttpModules
{
    /// <summary>
    /// Disposes an ObjectContext created by an AspNetObjectContextManager instance.
    /// </summary>
    public class AspNetObjectContextDisposalModule : IHttpModule
    {
        /// <summary>
        /// Initializes this HTTP module.
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(EndOfHttpRequest);
        }

        /// <summary>
        /// Releases any resources held by this module. 
        /// </summary>
        public void Dispose()
        {
            /* No resources held... */
        }

        /// <summary>
        /// Is invoked at the end of a HTTP request. Disposes the shared ObjectContext instance. 
        /// </summary>
        private void EndOfHttpRequest(object sender, EventArgs e)
        {
            DisposeObjectContext();
        }

        /// <summary>
        /// Disposes the shared ObjectContext instance.
        /// </summary>
        public static void DisposeObjectContext()
        {
            if (HttpContext.Current == null)
                throw new InvalidOperationException("DisposeObjectContext() can only be used in a HTTP context.");

            string ocKey = "ocm_" + HttpContext.Current.GetHashCode().ToString("x");
            if (HttpContext.Current.Items.Contains(ocKey))
            {
                ObjectContext objectContext = HttpContext.Current.Items[ocKey] as ObjectContext;
                if (objectContext != null)
                    objectContext.Dispose();
                HttpContext.Current.Items.Remove(ocKey);

                System.Diagnostics.Debug.WriteLine("AspNetObjectContextManager: Disposed NorthwindObjectContext");
            }
        }
    }
}
