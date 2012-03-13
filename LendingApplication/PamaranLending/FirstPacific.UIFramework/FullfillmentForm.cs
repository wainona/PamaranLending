using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace FirstPacific.UIFramework
{
    public abstract class FullfillmentForm
    {
        public abstract void Retrieve(int id);

        public abstract void PrepareForSave();
    }

    public abstract class FullfillmentForm<T> :  FullfillmentForm 
        where T : ObjectContext, new()
    {

        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
        public static T Context
        {
            get
            {
                if (ObjectContextScope<T>.CurrentObjectContext != null)
                    return ObjectContextScope<T>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<T>.ObjectContext;
            }
        }
    }
}