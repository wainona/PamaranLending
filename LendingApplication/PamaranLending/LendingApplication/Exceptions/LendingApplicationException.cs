using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LendingApplication
{
    public class LendingApplicationException : Exception
    {
        public LendingApplicationException()
        {

        }

        public LendingApplicationException(string message)
            : base(message)
        {

        }

        public LendingApplicationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }

    public class AccessToDeletedRecordException : LendingApplicationException
    {
        public AccessToDeletedRecordException()
        {

        }

        public AccessToDeletedRecordException(string message)
            : base(message)
        {

        }

        public AccessToDeletedRecordException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}