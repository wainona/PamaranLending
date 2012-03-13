using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public class InterestRateModel : BusinessObjectModel
    {
        public int ProductFeatureApplicabilityId { get; private set; }
        public string Description { get; private set; }
        public decimal InterestRate { get; set; }
        public int FeatureID { get; set; }

        public InterestRateModel(int featureID, decimal interestRate)
        {
            this.ProductFeatureApplicabilityId = -1;
            ProductFeature feature = ProductFeature.GetById(featureID);
            this.Description = feature.Description;
            this.InterestRate = interestRate;
            this.FeatureID = featureID;
        }

        public InterestRateModel(ProductFeatureApplicability interestRate)
        {
            this.IsNew = false;
            this.ProductFeatureApplicabilityId = interestRate.Id;
            this.Description = interestRate.ProductFeature.Description;
            this.InterestRate = interestRate.Value ?? 0;
            this.FeatureID = interestRate.ProductFeatureId;
        }

        //public override bool Equals(object obj)
        //{
        //    if (obj is InterestRateModel)
        //    {
        //        InterestRateModel other = obj as InterestRateModel;
        //        return this.InterestRate == other.InterestRate && this.FeatureID == other.FeatureID && this.ProductFeatureApplicabilityId == other.ProductFeatureApplicabilityId;
        //    }
        //    return base.Equals(obj);
        //}

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}
    }

    public partial class FeeModel : BusinessObjectModel
    {
        //<ext:RecordField Name="Name" />
        //<ext:RecordField Name="ChargeAmount" />
        //<ext:RecordField Name="BaseAmount" />
        //<ext:RecordField Name="Rate" />

        public int FeatureID { get; set; }
        public int ProductFeatureApplicabilityId { get; private set; }
        public string Name { get; set; }
        public decimal ChargeAmount { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal Rate { get; set; }

        public FeeModel(int featureID)
        {
            this.ProductFeatureApplicabilityId = -1;
            this.FeatureID = featureID;
            ProductFeature feature = ProductFeature.GetById(featureID);
            this.Name = feature.Name;
        }

        public FeeModel(int featureID, decimal rate, decimal chargeAmount, decimal baseAmount)
            :this(featureID)
        {
            this.Rate = rate;
            this.ChargeAmount = chargeAmount;
            this.BaseAmount = baseAmount;
        }

        public FeeModel(ProductFeatureApplicability featureApplicability)
        {
            this.IsNew = false;
            this.ProductFeatureApplicabilityId = featureApplicability.Id;
            this.Name = featureApplicability.ProductFeature.Name;
            this.FeatureID = featureApplicability.ProductFeatureId;

            if (featureApplicability.Fee != null)
            {
                Fee fee = featureApplicability.Fee;
                this.ChargeAmount = fee.ChargeAmount ?? 0;
                this.BaseAmount = fee.BaseAmount ?? 0;
                this.Rate = fee.PercentageRate ?? 0;
            }
        }

    }

    public class RequiredDocumentModel : BusinessObjectModel
    {
        public string Name { get; set; }

        public int TypeId { get; set; }

        public int ProductRequriedDocumentId { get; set; }

        public RequiredDocumentModel(int requiredDocumentTypeId)
        {
            RequiredDocumentType type = RequiredDocumentType.GetById(requiredDocumentTypeId);
            this.Name = type.Name;
            this.TypeId = type.Id;
            ProductRequriedDocumentId = -1;
        }

        public RequiredDocumentModel(ProductRequiredDocument requiredDocument)
        {
            this.IsNew = false;
            ProductRequriedDocumentId = requiredDocument.Id;
            this.TypeId = requiredDocument.RequiredDocumentType.Id;
            this.Name = requiredDocument.RequiredDocumentType.Name;
        }
    }

    public class FinancialProductForm : FullfillmentForm<FinancialEntities>
    {
        private List<InterestRateModel> InterestRates;
        private List<InterestRateModel> PastDueInterests;
        private List<FeeModel> Fees;
        private List<RequiredDocumentModel> RequiredDocuments;

        public IEnumerable<InterestRateModel> AvailableInterestRates
        {
            get
            {
                return this.InterestRates.Where(model => model.ToBeDeleted == false);
            }
        }

        public IEnumerable<InterestRateModel> AvailablePastDueInterestRates
        {
            get
            {
                return this.PastDueInterests.Where(model => model.ToBeDeleted == false);
            }
        }

        public IEnumerable<FeeModel> AvailableFees
        {
            get
            {
                return this.Fees.Where(model => model.ToBeDeleted == false);
            }
        }

        public IEnumerable<RequiredDocumentModel> AvailableRequiredDocuments
        {
            get
            {
                return this.RequiredDocuments.Where(model => model.ToBeDeleted == false);
            }
        }

        public string Name { get; set; }

        public int FinancialProductId { get; private set; }

        public DateTime IntroductionDate { get; set; }

        public DateTime? SalesDiscontinuationDate { get; set; }

        public string Comment { get; set; }

        public bool CollateralSecured { get; set; }

        public bool CollateralUnsecured { get; set; }

        public bool DiminishingBalanceMethodType { get; set; }

        public bool StraightLineMethodType { get; set; }

        public bool AddonInterestType { get; set; }

        public bool DiscountedInterestType { get; set; }

        public decimal MinimumLoanableAmount { get; set; }

        public decimal MaximumLoanableAmount { get; set; }

        public int MinimumLoanTerm { get; set; }

        public int MaximumLoanTerm { get; set; }
            
        public int LoanTermTimeUnitId { get; set; }

        public string ProductStatus { get; set; }

        public bool NoTermType { get; set; }
        public bool StartToEndOfMonth { get; set; }
        public bool AnyDayToSameDayOfNextMonth { get; set; }

        public FinancialProductForm()
        {
            this.NoTermType = false;
            this.StartToEndOfMonth = false;
            this.AnyDayToSameDayOfNextMonth = false;
            this.FinancialProductId = -1;
            Fees = new List<FeeModel>();
            PastDueInterests = new List<InterestRateModel>();
            InterestRates = new List<InterestRateModel>();
            RequiredDocuments = new List<RequiredDocumentModel>();
        }

        public void AddFee(FeeModel model)
        {
            if (this.Fees.Contains(model) == false)
                this.Fees.Add(model);
        }

        public void RemoveFee(string randomKey)
        {
            FeeModel model = this.Fees.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (model != null)
                RemoveFee(model);
        }

        public void RemoveFee(FeeModel model)
        {
            if (this.Fees.Contains(model) == true)
            {
                if (model.IsNew)
                    Fees.Remove(model);
                else
                    model.MarkDeleted();
            }
        }

        public void AddPastDueInterest(InterestRateModel item)
        {
            if (this.PastDueInterests.Contains(item) == false)
                this.PastDueInterests.Add(item);
        }

        public void RemovePastDueInterest(string randomKey)
        {
            InterestRateModel interestRate = this.PastDueInterests.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (interestRate != null)
                RemovePastDueInterest(interestRate);
        }

        public void RemovePastDueInterest(InterestRateModel item)
        {
            if (this.PastDueInterests.Contains(item) == true)
            {
                if (item.IsNew)
                    PastDueInterests.Remove(item);
                else
                    item.MarkDeleted();
            }
        }

        public void AddInterestRate(InterestRateModel item)
        {
            if (this.InterestRates.Contains(item) == false)
                this.InterestRates.Add(item);
        }

        public void RemoveInterestRate(string randomKey)
        {
            InterestRateModel interestRate = this.InterestRates.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (interestRate != null)
                RemoveInterestRate(interestRate);
        }

        public void RemoveInterestRate(InterestRateModel item)
        {
            if (this.InterestRates.Contains(item) == true)
            {
                if (item.IsNew)
                    InterestRates.Remove(item);
                else
                    item.MarkDeleted();
            }
        }

        public void AddRequiredDocumentType(RequiredDocumentModel item)
        {
            if (this.RequiredDocuments.Contains(item) == false)
                this.RequiredDocuments.Add(item);
        }

        public bool RequiredDocumentTypeContains(int requiredDocumentTypeId)
        {
            int count = this.AvailableRequiredDocuments.Where(entity => entity.TypeId == requiredDocumentTypeId).Count();
            return count > 0;
        }

        public void RemoveRequriedDocument(string randomKey)
        {
            RequiredDocumentModel requiredDoc = this.RequiredDocuments.SingleOrDefault(entity => entity.RandomKey == randomKey);
            if (requiredDoc != null)
                RemoveRequriedDocument(requiredDoc);
        }

        public void RemoveRequriedDocument(RequiredDocumentModel item)
        {
            if (this.RequiredDocuments.Contains(item) == true)
            {
                if (item.IsNew)
                    RequiredDocuments.Remove(item);
                else
                    item.MarkDeleted();
            }
        }

        private FinancialProduct InsertBasicInformation(DateTime today)
        {
            var productCategory = ProductCategory.LoanProductType;

            FinancialProduct financialProduct = new FinancialProduct();
            financialProduct.Name = this.Name;
            financialProduct.IntroductionDate = this.IntroductionDate;
            financialProduct.SalesDiscontinuationDate = this.SalesDiscontinuationDate;
            financialProduct.Comment = this.Comment;

            ProductCategoryClassification productCategoryClassification = new ProductCategoryClassification();
            productCategoryClassification.ProductCategory = productCategory;
            productCategoryClassification.FinancialProduct = financialProduct;
            productCategoryClassification.EffectiveDate = today;

            ProductStatusType productStatusType;
            if (this.IntroductionDate > today)
                productStatusType = ProductStatusType.InactiveType;
            else if (this.IntroductionDate <= today
                && (this.SalesDiscontinuationDate == null
                || this.SalesDiscontinuationDate > today))
                productStatusType = ProductStatusType.ActiveType;
            else
                productStatusType = ProductStatusType.RetiredType;

            //If introduction date <= Current Date and its Sales Discontinuation Date is null or > Current Date, use ‘Active’ status. Else, use ‘Retired’ status.
            ProductStatu productStatus = ProductStatu.CreateOrUpdateCurrent(financialProduct, productStatusType, today);

            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.SecuredType, financialProduct, this.CollateralSecured, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.UnsecuredType, financialProduct, this.CollateralUnsecured, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.DiminishingBalanceMethodType, financialProduct, this.DiminishingBalanceMethodType, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.StraightLineMethodType, financialProduct, this.StraightLineMethodType, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.AddonInterestType, financialProduct, this.AddonInterestType, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.DiscountedInterestType, financialProduct, this.DiscountedInterestType, today);

            ProductFeatureApplicability.CreateOrUpdate(ProductFeature.MinimumLoanableAmountType, financialProduct, this.MinimumLoanableAmount, today);
            ProductFeatureApplicability.CreateOrUpdate(ProductFeature.MaximumLoanableAmountType, financialProduct, this.MaximumLoanableAmount, today);


            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.NoTermType, financialProduct, this.NoTermType, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.StartToEndOfMonthType, financialProduct, this.StartToEndOfMonth, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.AnyDayToSameDayOfNextMonth, financialProduct, this.AnyDayToSameDayOfNextMonth, today);

            var minLoanTermApplicability = ProductFeatureApplicability.CreateOrUpdate(ProductFeature.MinimumLoanTermType, financialProduct, today);
            var maxLoanTermApplicability = ProductFeatureApplicability.CreateOrUpdate(ProductFeature.MaximumLoanTermType, financialProduct, today);

            var uom = UnitOfMeasure.GetByID(this.LoanTermTimeUnitId);
            var minLoanTerm = new LoanTerm();
            minLoanTerm.ProductFeatureApplicability = minLoanTermApplicability;
            minLoanTerm.UnitOfMeasure = uom;// TODO:
            minLoanTerm.LoanTermLength = this.MinimumLoanTerm;

            var maxLoanTerm = new LoanTerm();
            maxLoanTerm.ProductFeatureApplicability = maxLoanTermApplicability;
            maxLoanTerm.UnitOfMeasure = uom;// TODO:
            maxLoanTerm.LoanTermLength = this.MaximumLoanTerm;

            Context.FinancialProducts.AddObject(financialProduct);
            return financialProduct;
        }

        private FinancialProduct UpdateBasicInformation(FinancialProduct financialProduct, DateTime today)
        {
            financialProduct.Name = this.Name;
            financialProduct.IntroductionDate = this.IntroductionDate;
            financialProduct.SalesDiscontinuationDate = this.SalesDiscontinuationDate;
            financialProduct.Comment = this.Comment;

            //If introduction date <= Current Date and its Sales Discontinuation Date is null 
            //or > Current Date, use ‘Active’ status. Else, use ‘Retired’ status.
            ProductStatusType productStatusType;
            if (this.IntroductionDate > today)
                productStatusType = ProductStatusType.InactiveType;
            else if (this.IntroductionDate <= today
                && (this.SalesDiscontinuationDate == null
                || this.SalesDiscontinuationDate > today))
                productStatusType = ProductStatusType.ActiveType;
            else
                productStatusType = ProductStatusType.RetiredType;

            ProductStatu productStatus = ProductStatu.CreateOrUpdateCurrent(financialProduct, productStatusType, today);

            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.SecuredType, financialProduct, this.CollateralSecured, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.UnsecuredType, financialProduct, this.CollateralUnsecured, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.DiminishingBalanceMethodType, financialProduct, this.DiminishingBalanceMethodType, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.StraightLineMethodType, financialProduct, this.StraightLineMethodType, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.AddonInterestType, financialProduct, this.AddonInterestType, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.DiscountedInterestType, financialProduct, this.DiscountedInterestType, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.AddonInterestType, financialProduct, this.AddonInterestType, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.DiscountedInterestType, financialProduct, this.DiscountedInterestType, today);

            ProductFeatureApplicability.CreateOrUpdate(ProductFeature.MinimumLoanableAmountType, financialProduct, this.MinimumLoanableAmount, today);
            ProductFeatureApplicability.CreateOrUpdate(ProductFeature.MaximumLoanableAmountType, financialProduct, this.MaximumLoanableAmount, today);

            var minLoanTermApplicability = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanTermType, financialProduct);
            var maxLoanTermApplicability = ProductFeatureApplicability.GetActive(ProductFeature.MaximumLoanTermType, financialProduct);

            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.NoTermType, financialProduct, this.NoTermType, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.StartToEndOfMonthType, financialProduct, this.StartToEndOfMonth, today);
            ProductFeatureApplicability.UpdateFeatureExistence(ProductFeature.AnyDayToSameDayOfNextMonth, financialProduct, this.AnyDayToSameDayOfNextMonth, today);

            var uom = UnitOfMeasure.GetByID(this.LoanTermTimeUnitId);

            if (minLoanTermApplicability.LoanTerm != null)
            {
                var minLoanTerm = minLoanTermApplicability.LoanTerm;
                if (minLoanTerm.UomId != this.LoanTermTimeUnitId || minLoanTerm.LoanTermLength != this.MinimumLoanTerm)
                {
                    var newMinLoanTermApplicability = 
                        ProductFeatureApplicability.EndActiveAndCreateNew(minLoanTermApplicability, today);

                    minLoanTerm = new LoanTerm();
                    minLoanTerm.ProductFeatureApplicability = newMinLoanTermApplicability;
                    minLoanTerm.UnitOfMeasure = uom;
                    minLoanTerm.LoanTermLength = this.MinimumLoanTerm;
                }
            }

            if (maxLoanTermApplicability.LoanTerm != null)
            {
                var maxLoanTerm = maxLoanTermApplicability.LoanTerm;
                if (maxLoanTerm.UomId != this.LoanTermTimeUnitId || maxLoanTerm.LoanTermLength != this.MaximumLoanTerm)
                {
                    var newMaxLoanTermApplicability = 
                        ProductFeatureApplicability.EndActiveAndCreateNew(maxLoanTermApplicability, today);

                    maxLoanTerm = new LoanTerm();
                    maxLoanTerm.ProductFeatureApplicability = newMaxLoanTermApplicability;
                    maxLoanTerm.UnitOfMeasure = uom;
                    maxLoanTerm.LoanTermLength = this.MaximumLoanTerm;
                }
            }
            
            return financialProduct;
        }

        private void SaveInterestRates(FinancialProduct financialProduct, DateTime today)
        {
            foreach (InterestRateModel interest in InterestRates)
            {
                if (interest.IsNew)
                {
                    ProductFeature feature = ProductFeature.GetById(interest.FeatureID);
                    var featureApplicability = ProductFeatureApplicability.Create(feature, financialProduct, today);
                    featureApplicability.Value = interest.InterestRate;
                    Context.ProductFeatureApplicabilities.AddObject(featureApplicability);
                }
                else if (interest.ToBeDeleted)
                {
                    ProductFeatureApplicability featureApplicability =
                        ProductFeatureApplicability.GetById(interest.ProductFeatureApplicabilityId);
                    featureApplicability.EndDate = today;
                }
            }
        }

        private void SavePastDueInterests(FinancialProduct financialProduct, DateTime today)
        {
            foreach (InterestRateModel interest in PastDueInterests)
            {
                if (interest.IsNew)
                {
                    ProductFeature feature = ProductFeature.GetById(interest.FeatureID);
                    var featureApplicability = ProductFeatureApplicability.Create(feature, financialProduct, today);
                    featureApplicability.Value = interest.InterestRate;
                    Context.ProductFeatureApplicabilities.AddObject(featureApplicability);
                }
                else if (interest.ToBeDeleted)
                {
                    ProductFeatureApplicability featureApplicability =
                        ProductFeatureApplicability.GetById(interest.ProductFeatureApplicabilityId);
                    featureApplicability.EndDate = today;
                }
            }
        }

        private void SaveFees(FinancialProduct financialProduct, DateTime today)
        {
            foreach (FeeModel model in Fees)
            {
                if (model.IsNew)
                {
                    ProductFeature feature = ProductFeature.GetById(model.FeatureID);
                    var featureApplicability = ProductFeatureApplicability.Create(feature, financialProduct, today);
                    
                    Fee fee = new Fee();
                    fee.ChargeAmount = model.ChargeAmount;
                    fee.BaseAmount = model.BaseAmount;
                    fee.PercentageRate = model.Rate;
                    fee.ProductFeatureApplicability = featureApplicability;

                    Context.ProductFeatureApplicabilities.AddObject(featureApplicability);
                }
                else if (model.ToBeDeleted)
                {
                    ProductFeatureApplicability featureApplicability =
                        ProductFeatureApplicability.GetById(model.ProductFeatureApplicabilityId);
                    featureApplicability.EndDate = today;
                }
            }
        }

        private void SaveRequiredDocuments(FinancialProduct financialProduct, DateTime today)
        {
            foreach (RequiredDocumentModel model in RequiredDocuments)
            {
                if (model.IsNew)
                {
                    ProductRequiredDocument requiredDocument =
                        new ProductRequiredDocument();
                    requiredDocument.RequiredDocumentType = RequiredDocumentType.GetById(model.TypeId);
                    requiredDocument.FinancialProduct = financialProduct;
                    requiredDocument.EffectiveDate = today;
                    Context.ProductRequiredDocuments.AddObject(requiredDocument);
                }
                else if (model.ToBeDeleted)
                {
                    ProductRequiredDocument requiredDocument = ProductRequiredDocument.GetById(model.ProductRequriedDocumentId);
                    requiredDocument.EndDate = today;
                }
            }
        }

        private void RetrieveBasicInformation(FinancialProduct financialProduct)
        {
            this.Name = financialProduct.Name;
            this.IntroductionDate = financialProduct.IntroductionDate;
            this.SalesDiscontinuationDate = financialProduct.SalesDiscontinuationDate;
            this.Comment = financialProduct.Comment;

            //retrieve product status
            var productStatus = ProductStatu.GetActive(financialProduct);
            this.ProductStatus = productStatus.ProductStatusType.Name;

            var secured = ProductFeatureApplicability.GetActive(ProductFeature.SecuredType, financialProduct);
            this.CollateralSecured = (secured != null);

            var unsecured = ProductFeatureApplicability.GetActive(ProductFeature.UnsecuredType, financialProduct);
            this.CollateralUnsecured = (unsecured != null);

            var diminish = ProductFeatureApplicability.GetActive(ProductFeature.DiminishingBalanceMethodType, financialProduct);
            this.DiminishingBalanceMethodType = (diminish != null);

            var straight = ProductFeatureApplicability.GetActive(ProductFeature.StraightLineMethodType, financialProduct);
            this.StraightLineMethodType = (straight != null);

            var addon = ProductFeatureApplicability.GetActive(ProductFeature.AddonInterestType, financialProduct);
            this.AddonInterestType = (addon != null);

            var discounted = ProductFeatureApplicability.GetActive(ProductFeature.DiscountedInterestType, financialProduct);
            this.DiscountedInterestType = (discounted != null);

            var noterm = ProductFeatureApplicability.GetActive(ProductFeature.NoTermType, financialProduct);
            this.NoTermType = (noterm != null);

            var startmonth = ProductFeatureApplicability.GetActive(ProductFeature.StartToEndOfMonthType, financialProduct);
            this.StartToEndOfMonth = (startmonth != null);

            var anyday = ProductFeatureApplicability.GetActive(ProductFeature.AnyDayToSameDayOfNextMonth, financialProduct);
            this.AnyDayToSameDayOfNextMonth = (anyday != null);

            var minimumLoanable = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanableAmountType, financialProduct);
            this.MinimumLoanableAmount = (minimumLoanable != null) ? minimumLoanable.Value.Value : 0;

            var maximumLoanable = ProductFeatureApplicability.GetActive(ProductFeature.MaximumLoanableAmountType, financialProduct);
            this.MaximumLoanableAmount = (maximumLoanable != null) ? maximumLoanable.Value.Value : 0;

            var minimumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MinimumLoanTermType, financialProduct);
            this.MinimumLoanTerm = (minimumTerm != null) ? minimumTerm.LoanTerm.LoanTermLength : 0;

            var maximumTerm = ProductFeatureApplicability.GetActive(ProductFeature.MaximumLoanTermType, financialProduct);
            this.MaximumLoanTerm = (maximumTerm != null) ? maximumTerm.LoanTerm.LoanTermLength : 0;

            if (minimumTerm != null)
                this.LoanTermTimeUnitId = minimumTerm.LoanTerm.UomId;
        }

        private void RetrieveInterestRates(FinancialProduct financialProduct)
        {
            var rates = ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.InterestRateType, financialProduct);
            foreach (ProductFeatureApplicability item in rates)
            {
                this.InterestRates.Add(new InterestRateModel(item));
            }
        }

        private void RetrievePastDueInterests(FinancialProduct financialProduct)
        {
            var rates = ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.PastDueInterestRateType, financialProduct);
            foreach (ProductFeatureApplicability item in rates)
            {
                this.PastDueInterests.Add(new InterestRateModel(item));
            }
        }

        private void RetrieveFees(FinancialProduct financialProduct)
        {
            var fees = ProductFeatureApplicability.GetAllActive(ProductFeatureCategory.FeeType, financialProduct);
            foreach (ProductFeatureApplicability fee in fees.Where(entity=>entity.Fee != null))
            {
                this.Fees.Add(new FeeModel(fee));
            }
        }

        private void RetrieveRequiredDocuments(FinancialProduct financialProduct)
        {
            var requiredDocuments = financialProduct.ProductRequiredDocuments.Where(entity => entity.EndDate == null);
            foreach (ProductRequiredDocument item in requiredDocuments)
            {
                this.RequiredDocuments.Add(new RequiredDocumentModel(item));
            }
        }

        public override void Retrieve(int id)
        {
            this.FinancialProductId = id;
            var financialProduct = FinancialProduct.GetById(id);
            RetrieveBasicInformation(financialProduct);
            RetrieveInterestRates(financialProduct);
            RetrievePastDueInterests(financialProduct);
            RetrieveFees(financialProduct);
            RetrieveRequiredDocuments(financialProduct);
        }

        public override void PrepareForSave()
        {
            var today = DateTime.Now;
            
            if (this.FinancialProductId == -1)
            {
                FinancialProduct financialProduct = InsertBasicInformation(today);
                SaveInterestRates(financialProduct, today);
                SavePastDueInterests(financialProduct, today);
                SaveFees(financialProduct, today);
                SaveRequiredDocuments(financialProduct, today);
            }
            else
            {
                var financialProduct = Context.FinancialProducts.SingleOrDefault(entity => entity.Id == this.FinancialProductId);
                UpdateBasicInformation(financialProduct, today);
                SaveInterestRates(financialProduct, today);
                SavePastDueInterests(financialProduct, today);
                SaveFees(financialProduct, today);
                SaveRequiredDocuments(financialProduct, today);
            }
        }
    }
}
