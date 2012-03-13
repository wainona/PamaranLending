using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ProductFeatureApplicability
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

        public static ProductFeatureApplicability GetById(int id)
        {
            return Context.ProductFeatureApplicabilities.SingleOrDefault(entity => entity.Id == id);
        }

        public static List<ProductFeatureApplicability> GetAllActive(ProductFeatureCategory category, FinancialProduct financialProduct)
        {
            return financialProduct.ProductFeatureApplicabilities.Where(entity => entity.EndDate == null && entity.ProductFeature.ProductFeatCatId == category.Id).ToList();
        }

        public static IEnumerable<ProductFeatureApplicability> GetAllActive(ProductFeature feature, FinancialProduct financialProduct)
        {
            return financialProduct.ProductFeatureApplicabilities.Where(entity => entity.EndDate == null && entity.ProductFeatureId == feature.Id);
        }

        public static ProductFeatureApplicability GetActive(ProductFeature feature, FinancialProduct financialProduct)
        {
            return financialProduct.ProductFeatureApplicabilities.FirstOrDefault(entity => entity.EndDate == null && entity.ProductFeatureId == feature.Id);
        }

        public static ProductFeatureApplicability Create(ProductFeature feature, FinancialProduct financialProduct, DateTime today)
        {
            ProductFeatureApplicability applicability = new ProductFeatureApplicability();
            applicability.FinancialProduct = financialProduct;
            applicability.EffectiveDate = today;
            applicability.ProductFeature = feature;

            Context.ProductFeatureApplicabilities.AddObject(applicability);
            return applicability;
        }

        public static ProductFeatureApplicability Create(ProductFeature feature, FinancialProduct financialProduct, decimal? value, DateTime today)
        {
            ProductFeatureApplicability applicability = new ProductFeatureApplicability();
            applicability.FinancialProduct = financialProduct;
            applicability.EffectiveDate = today;
            applicability.ProductFeature = feature;
            applicability.Value = value;

            Context.ProductFeatureApplicabilities.AddObject(applicability);
            return applicability;
        }

        public static ProductFeatureApplicability CreateOrUpdate(ProductFeature feature, FinancialProduct financialProduct, DateTime today)
        {
            ProductFeatureApplicability current = GetActive(feature, financialProduct);
            if (current != null)
                current.EndDate = today;

            ProductFeatureApplicability applicability = new ProductFeatureApplicability();
            applicability.FinancialProduct = financialProduct;
            applicability.EffectiveDate = today;
            applicability.ProductFeature = feature;

            Context.ProductFeatureApplicabilities.AddObject(applicability);
            return applicability;
        }

        public static ProductFeatureApplicability CreateOrUpdate(ProductFeature feature, FinancialProduct financialProduct, decimal? value , DateTime today)
        {
            ProductFeatureApplicability current = GetActive(feature, financialProduct);
            if (current != null && current.Value == value)
                return current;

            bool createNew = (current != null && current.Value != value) || current == null;
            if (current != null && current.Value != value)
                current.EndDate = today;

            if (createNew)
            {
                ProductFeatureApplicability applicability = new ProductFeatureApplicability();
                applicability.FinancialProduct = financialProduct;
                applicability.EffectiveDate = today;
                applicability.ProductFeature = feature;
                applicability.Value = value;

                Context.ProductFeatureApplicabilities.AddObject(applicability);
                return applicability;
            }

            return current;
        }

        public static ProductFeatureApplicability EndActiveAndCreateNew(ProductFeatureApplicability current, DateTime today)
        {
            if (current != null)
                current.EndDate = today;

            ProductFeatureApplicability applicability = new ProductFeatureApplicability();
            applicability.FinancialProduct = current.FinancialProduct;
            applicability.EffectiveDate = today;
            applicability.ProductFeature = current.ProductFeature;

            Context.ProductFeatureApplicabilities.AddObject(applicability);
            return applicability;
        }

        public static ProductFeatureApplicability UpdateFeatureExistence(ProductFeature feature, FinancialProduct financialProduct, bool shouldExist, DateTime today)
        {
            ProductFeatureApplicability current = GetActive(feature, financialProduct);
            //no changes
            if (shouldExist && current != null)
                return current;

            if (shouldExist && current == null)
            {
                ProductFeatureApplicability applicability = new ProductFeatureApplicability();
                applicability.FinancialProduct = financialProduct;
                applicability.EffectiveDate = today;
                applicability.ProductFeature = feature;

                Context.ProductFeatureApplicabilities.AddObject(applicability);
                return applicability;
            }

            if (current != null && shouldExist == false)
            {
                current.EndDate = today;
                return current;
            }

            return null;
        }
        
        public static ProductFeatureApplicability RetrieveFeature(ProductFeatureCategory featureCategory, LoanApplication loanApplication)
        {
            var app = Context.ApplicationItems.SingleOrDefault(entity =>
                entity.ProductFeatureApplicability.ProductFeature.ProductFeatureCategory.Id == featureCategory.Id
                && entity.ApplicationId == loanApplication.ApplicationId && entity.EndDate == null);

            return app.ProductFeatureApplicability;
        }
    }
}