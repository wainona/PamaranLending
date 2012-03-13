using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstPacific.UIFramework
{
    public abstract class IPageAbleDataSource<T>
    {
        public abstract int Count
        { get; }

        public List<T> SelectAll(int start, int limit, string orderBy)
        {
            return SelectAll(start, limit, entity => orderBy);
        }

        public abstract List<T> SelectAll(int start, int limit, Func<T, string> orderBy);
    }
}