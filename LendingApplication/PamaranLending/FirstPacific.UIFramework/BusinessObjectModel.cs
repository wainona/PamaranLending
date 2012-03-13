using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace FirstPacific.UIFramework
{
    [Serializable]
    public abstract class BusinessObjectModel
    {
        public string RandomKey { get; private set; }

        public bool IsNew { get; protected set; }

        public bool ToBeDeleted { get; private set; }

        public bool IsEdited { get; private set; }

        public void MarkDeleted()
        {
            this.ToBeDeleted = true;
        }

        public void MarkEdited()
        {
            if (this.IsNew == false)
                this.IsEdited = true;
        }

        public BusinessObjectModel()
        {
            this.IsNew = true;
            this.IsEdited = false;
            this.ToBeDeleted = false;
            this.RandomKey = Guid.NewGuid().ToString().GetHashCode().ToString("x");
        }

        public override bool Equals(object obj)
        {
            if (obj is BusinessObjectModel)
            {
                BusinessObjectModel other = obj as BusinessObjectModel;
                return this.RandomKey == other.RandomKey;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.RandomKey.GetHashCode();
        }
    }
}
