using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public partial class ProductFeature
    {
        public static ProductFeature GetById(int id)
        {
            return Context.ProductFeatures.SingleOrDefault(entity => entity.Id == id);
        }

        public static ProductFeature GetByName(string name)
        {
            return Context.ProductFeatures.FirstOrDefault(entity => entity.Name == name);
        }

        public static IQueryable<ProductFeature> All(ProductFeatureCategory category)
        {
            return Context.ProductFeatures.Where(entity => entity.ProductFeatCatId == category.Id);
        }
    }
}
