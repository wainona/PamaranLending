using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    /// <summary>
    /// Defines a scope for a business transaction. At the end of the scope all object changes
    /// can be persisted to the underlying datastore. Instances of this class are supposed to be 
    /// used in a using() statement.
    /// </summary>
    public sealed class UnitOfWorkScope : ObjectContextScope<FinancialEntities>
    {
        /// <summary>
        /// Default constructor. Object changes are not automatically saved at the 
        /// end of the scope.
        /// </summary>
        public UnitOfWorkScope()
            : base()
        { }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="saveAllChangesAtEndOfScope">
        /// A boolean value that indicates whether to automatically save 
        /// all object changes at end of the scope.
        /// </param>
        public UnitOfWorkScope(bool saveAllChangesAtEndOfScope)
            : base(saveAllChangesAtEndOfScope)
        { }
    }
}
