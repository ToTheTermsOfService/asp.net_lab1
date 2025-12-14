using Core.Entities;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace API.Data
{
    public static class ODataModel
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();

            builder.EntitySet<Tenant>("Tenant")
                   .EntityType
                   .HasKey(t => t.Id)
                   .Filter()
                   .Count()
                   .Expand()
                   .Select()
                   .OrderBy()
                   .Page();

            builder.EntitySet<Service>("Service");

            builder.EntityType<Tenant>().Collection
                   .Function("GetByAccount")
                   .Returns<Tenant>()
                   .Parameter<string>("accountNumber");

            return builder.GetEdmModel();
        }
    }
}
