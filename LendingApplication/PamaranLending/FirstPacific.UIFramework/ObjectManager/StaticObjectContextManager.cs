using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Objects;
using System.Linq;
using System.Text;

namespace FirstPacific.UIFramework
{
    /// <summary>
    /// Maintains an ObjectContext instance in static field. This instance is then
    /// shared during the lifespan of the AppDomain.
    /// </summary>
    public sealed class StaticObjectContextManager<T> : ObjectContextManager<T> where T : ObjectContext, new()
    {
        private static T _objectContext;
        private static object _lockObject = new object();

        /// <summary>
        /// Returns a shared ObjectContext instance.
        /// </summary>
        public override T ObjectContext
        {
            get
            {
                lock (_lockObject)
                {
                    if (_objectContext == null)
                        _objectContext = new T();
                }

                return _objectContext;
            }
        }
    }
}
