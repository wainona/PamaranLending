using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class FinancialProduct
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

        public static FinancialProduct GetById(int id)
        {
            return Context.FinancialProducts.SingleOrDefault(entity => entity.Id == id);
        }

        public static void Delete(int id)
        {
            Delete(GetById(id));
        }

        public static void Delete(FinancialProduct financialProduct)
        {
            var productStatuses = financialProduct.ProductStatus;
            var productFeatures = financialProduct.ProductFeatureApplicabilities;
            var productRequirements = financialProduct.ProductRequiredDocuments;
            var prodCategoryClass = financialProduct.ProductCategoryClassifications;

            foreach (var productStatus in productStatuses.ToList())
            {
                Context.DeleteObject(productStatus);
            }

            foreach (var productFeature in productFeatures.ToList())
            {
                if (productFeature.Fee != null)
                    Context.DeleteObject(productFeature.Fee);

                if (productFeature.LoanTerm != null)
                    Context.DeleteObject(productFeature.LoanTerm);

                Context.ProductFeatureApplicabilities.DeleteObject(productFeature);
            }

            foreach (var document in productRequirements.ToList())
            {
                Context.DeleteObject(document);
            }

            foreach (var category in prodCategoryClass.ToList())
            {
                Context.DeleteObject(category);
            }

            Context.DeleteObject(financialProduct);
        }

        public static void UpdateStatus()
        {
            DateTime today = DateTime.Now.Date;
            DateTime tommorow = today.AddDays(1);
            var financialProducts = Context.FinancialProducts.Where(entity => entity.SalesDiscontinuationDate.HasValue || entity.IntroductionDate >= today && entity.IntroductionDate <= tommorow);
            foreach (var financialProduct in financialProducts)
            {
                DateTime introductionDate = financialProduct.IntroductionDate;
                DateTime? salesDiscontinuationDate = financialProduct.SalesDiscontinuationDate;

                ProductStatusType productStatusType;
                if (introductionDate > today)
                    productStatusType = ProductStatusType.InactiveType;
                else if (introductionDate <= today &&
                    (salesDiscontinuationDate == null || salesDiscontinuationDate.Value > today))
                    productStatusType = ProductStatusType.ActiveType;
                else
                    productStatusType = ProductStatusType.RetiredType;

                ProductStatu productStatus = ProductStatu.CreateOrUpdateCurrent(financialProduct, productStatusType, today);
            }
        }

        public static bool CheckIfProductIsUsed(FinancialProduct product)
        {
            var used = Context.ApplicationItems.Where(entity =>
                entity.ProductFeatureApplicability.FinancialProductId == product.Id);
            if (used.Count() != 0)
                return true;
            else
                return false;
        }
    }
}
