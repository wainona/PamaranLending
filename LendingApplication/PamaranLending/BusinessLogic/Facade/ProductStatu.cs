using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic
{
    public partial class ProductStatu
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

        public static ProductStatu GetActive(FinancialProduct financialProduct)
        {
            return financialProduct.ProductStatus.FirstOrDefault(entity => entity.IsActive);
        }

        public static ProductStatu ChangeStatus(int financialProductId, ProductStatusType statusTo, DateTime today)
        {
            FinancialProduct financialProduct = FinancialProduct.GetById(financialProductId);
            return ChangeStatus(financialProduct, statusTo, today);
        }

        public static bool CanChangeStatusTo(FinancialProduct financialProduct, ProductStatusType statusTo)
        {
            ProductStatu status = GetActive(financialProduct);

            if(status == null)
                return true;

            if (status.ProductStatusType.Id == ProductStatusType.ActiveType.Id)
            {
                return statusTo.Id != ProductStatusType.ActiveType.Id;
            }
            else if (status.ProductStatusType.Id == ProductStatusType.InactiveType.Id)
            {
                return statusTo.Id == ProductStatusType.ActiveType.Id;
            }
            else
            {
                return false;
            }
        }

        public static ProductStatu ChangeStatus(FinancialProduct financialProduct, ProductStatusType statusTo, DateTime today)
        {
            ProductStatu status = GetActive(financialProduct);
            if (CanChangeStatusTo(financialProduct, statusTo))
            {
                return CreateOrUpdateCurrent(financialProduct, statusTo, today);
            }
            return status;            
        }

        public static ProductStatu CreateOrUpdateCurrent(FinancialProduct financialProduct, ProductStatusType productStatusType, DateTime today)
        {
            ProductStatu productStatus = GetActive(financialProduct);
            if (productStatus != null && productStatus.ProductStatusType.Id != productStatusType.Id)
                productStatus.IsActive = false;

            if (productStatus == null || productStatus.ProductStatusType.Id != productStatusType.Id)
            {
                ProductStatu newProductStatus = new ProductStatu();
                newProductStatus.FinancialProduct = financialProduct;
                newProductStatus.TransitionDateTime = today;
                newProductStatus.IsActive = true;
                newProductStatus.ProductStatusType = productStatusType;
                return newProductStatus;
            }
            return productStatus;
        }
    }
}
