using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ApplicationItem
    {
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

        public static ApplicationItem GetById(int id)
        {
            return Context.ApplicationItems.SingleOrDefault(entity => entity.ApplicationId == id);
        }

        public static ApplicationItem Get(Application application, ProductFeature productFeature)
        {
            return Context.ApplicationItems.FirstOrDefault(entity => entity.EndDate == null
                && entity.ProductFeatureApplicability.ProductFeature.Id == productFeature.Id
                && entity.ApplicationId == application.Id);
        }

        public static ApplicationItem GetFirstActive(Application application, ProductFeatureCategory category)
        {
            return Context.ApplicationItems.FirstOrDefault(entity => entity.EndDate == null 
                && entity.ProductFeatureApplicability.ProductFeature.ProductFeatCatId == category.Id
                && entity.ApplicationId == application.Id);
        }

        public static IQueryable<ApplicationItem> GetAllActive(Application application, ProductFeatureCategory category)
        {
            return Context.ApplicationItems.Where(entity => entity.EndDate == null
                && entity.ProductFeatureApplicability.ProductFeature.ProductFeatCatId == category.Id
                && entity.ApplicationId == application.Id);
        }

        public static IQueryable<ApplicationItem> All(FinancialProduct financialProduct)
        {
            return Context.ApplicationItems.Where(entity =>
                    entity.ProductFeatureApplicability.FinancialProductId == financialProduct.Id);
        }

        public static ApplicationItem CreateOrUpdate(Application application, ProductFeatureCategory category, int productFeatureApplicabilityId, DateTime today)
        {
            var current = GetFirstActive(application, category);
            bool create = current == null || current.ProdFeatApplicabilityId != productFeatureApplicabilityId;
            if (current != null && current.ProdFeatApplicabilityId != productFeatureApplicabilityId)
                current.EndDate = today;

            if (create)
            {
                ApplicationItem applicationItem = new ApplicationItem();
                applicationItem.Application = application;
                applicationItem.ProdFeatApplicabilityId = productFeatureApplicabilityId;
                applicationItem.EffectiveDate = today;
                return applicationItem;
            }
            else
                return current;
        }
    }
}
