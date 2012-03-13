using System;

namespace BusinessLogic
{
    public class InitialDatabaseValueChecker
    {
        public static void ThrowIfNull<T>(T entity)
        {
            if (entity == null)
                throw new NotSupportedException(string.Format("Entity of type {0} doesn't contain all expected initial values.", typeof(T).Name));
        }

    }
}