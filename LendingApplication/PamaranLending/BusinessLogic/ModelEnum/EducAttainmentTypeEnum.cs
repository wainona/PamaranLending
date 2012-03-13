using System.Linq;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class EducAttainmentType
    {
        private const string ElementaryGraduate = "Elementary Graduate";
        private const string HighSchoolGraduate = "High School Graduate";
        private const string CollegeUndergraduate = "College Undergraduate";
        private const string CollegeGraduate = "College Graduate";
        private const string PostGraduate = "Post-Graduate";

        /// <summary>
        /// Returns the ObjectContext instance that belongs to the current ObjectContextScope.
        /// If currently no ObjectContextScope exists, a local instance of an ObjectContext 
        /// class is returned.
        /// </summary>
        private static FinancialEntities Context
        {
            get
            {
                if (ObjectContextScope<FinancialEntities>.CurrentObjectContext != null)
                    return ObjectContextScope<FinancialEntities>.CurrentObjectContext;
                else
                    return AspNetObjectContextManager<FinancialEntities>.ObjectContext;
            }
        }

        public static EducAttainmentType ElementaryGraduateType
        {
            get
            {
                var type = Context.EducAttainmentTypes.SingleOrDefault(entity => entity.Name == ElementaryGraduate);
                InitialDatabaseValueChecker.ThrowIfNull<EducAttainmentType>(type);

                return type;
            }
        }

        public static EducAttainmentType HighSchoolGraduateType
        {
            get
            {
                var type = Context.EducAttainmentTypes.SingleOrDefault(entity => entity.Name == HighSchoolGraduate);
                InitialDatabaseValueChecker.ThrowIfNull<EducAttainmentType>(type);

                return type;
            }
        }

        public static EducAttainmentType CollegeUndergraduateType
        {
            get
            {
                var type = Context.EducAttainmentTypes.SingleOrDefault(entity => entity.Name == CollegeUndergraduate);
                InitialDatabaseValueChecker.ThrowIfNull<EducAttainmentType>(type);

                return type;
            }
        }

        public static EducAttainmentType CollegeGraduateType
        {
            get
            {
                var type = Context.EducAttainmentTypes.SingleOrDefault(entity => entity.Name == CollegeGraduate);
                InitialDatabaseValueChecker.ThrowIfNull<EducAttainmentType>(type);

                return type;
            }
        }

        public static EducAttainmentType PostGraduateType
        {
            get
            {
                var type = Context.EducAttainmentTypes.SingleOrDefault(entity => entity.Name == PostGraduate);
                InitialDatabaseValueChecker.ThrowIfNull<EducAttainmentType>(type);

                return type;
            }
        }
    }
}