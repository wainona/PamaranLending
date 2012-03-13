using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects.DataClasses;
using FirstPacific.UIFramework;
using System.Configuration;
using System.Collections;
using System.Reflection;
using System.Data.Objects;

namespace FirstPacific.UIFramework
{
    /// <summary>
    /// Abstract base class for all other ObjectContextManager classes. 
    /// </summary>
    public abstract class ObjectContextManager<T> where T : ObjectContext, new()
    {
        /// <summary>
        /// Returns a reference to an ObjectContext instance.
        /// </summary>
        public abstract T ObjectContext
        {
            get;
        }

        //private static ObjectContextManager<T> instance;
        //private static object _lockObject = new object();

        ///// <summary>
        ///// Returns a shared ObjectContext instance.
        ///// </summary>
        //public static ObjectContextManager<T> Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            lock (_lockObject)
        //            {
        //                if (instance == null)
        //                    instance = CreateFromConfig();
        //            }
        //        }

        //        return instance;
        //    }
        //}

        //public static ObjectContextManager<T> CreateFromConfig()
        //{
        //    /* Retrieve ObjectContextManager configuration settings: */
        //    Hashtable ocManagerConfiguration = ConfigurationManager.GetSection("FirstPacific.UIFramework.ObjectContext") as Hashtable;
        //    if (ocManagerConfiguration != null && ocManagerConfiguration.ContainsKey("managerType"))
        //    {
        //        string managerTypeName = ocManagerConfiguration["managerType"] as string;
        //        if (string.IsNullOrEmpty(managerTypeName))
        //            throw new ConfigurationErrorsException("The managerType attribute is empty.");
        //        else
        //            managerTypeName = managerTypeName.Trim().ToLower();

        //        try
        //        {
        //            /* Try to create a type based on it's name: */
        //            Assembly frameworkAssembly = Assembly.GetAssembly(typeof(ObjectContextManager<>));
        //            /* We have to fix the name, because its a generic class: */
        //            Type managerType = frameworkAssembly.GetType(managerTypeName + "`1", true, true);
        //            managerType = managerType.MakeGenericType(typeof(T));

        //            /* Try to create a new instance of the specified ObjectContextManager type: */
        //            return Activator.CreateInstance(managerType) as ObjectContextManager<T>;
        //        }
        //        catch (Exception e)
        //        {
        //            throw new ConfigurationErrorsException("The managerType specified in the configuration is not valid.", e);
        //        }
        //    }
        //    else
        //        throw new ConfigurationErrorsException("A FirstPacific.UIFramework.ObjectContext tag or its managerType attribute is missing in the configuration.");
        //}
    }
}
