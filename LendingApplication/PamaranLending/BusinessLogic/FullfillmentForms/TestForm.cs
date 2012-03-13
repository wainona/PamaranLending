using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstPacific.UIFramework;

namespace BusinessLogic.FullfillmentForms
{
    public class Model2 : BusinessObjectModel
    {
        public int ProductFeatureApplicabilityId { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }

        public int FeatureId { get; set; }

        public Model2(int featureId, decimal rate)
        {
            this.ProductFeatureApplicabilityId = -1;
            ProductFeature feature = ProductFeature.GetById(featureId);
            this.Description = feature.Description;
            this.Rate = rate;
            this.FeatureId = featureId;
        }
    }

    public class TestForm : FullfillmentForm<FinancialEntities>
    {

        private List<Model2> InterestRates;

        public string Name { get; set; }

        public DateTime IntroductionDate { get; set; }

        public DateTime? SalesDiscontinuationDate { get; set; }

        public TestForm()
        {
            InterestRates = new List<Model2>();
        }

        public override void Retrieve(int id)
        {
            
        }

        public override void PrepareForSave()
        {
            FinancialProduct product = new FinancialProduct();
            product.Name = this.Name;
            product.IntroductionDate = this.IntroductionDate;
            product.SalesDiscontinuationDate = this.SalesDiscontinuationDate;
        }
    }
}
