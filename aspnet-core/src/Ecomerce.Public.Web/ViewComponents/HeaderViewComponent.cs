using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ecommerce.Public.Catalogs.ProductCategories;
using System.Collections.Generic;

namespace Ecomerce.Public.Web.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IProductCategoriesAppService _productCategoryAppService;
        public HeaderViewComponent(IProductCategoriesAppService productCategoryAppService)
        {
            _productCategoryAppService = productCategoryAppService;
        }

        /* public class job
         {
             public job(string name, int jobNumber)
             {
                 this.name = name;
                 this.jobNumber = jobNumber;
             }

             public string name { get; set; }
             public int jobNumber { get; set; }
         }*/
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            /*List<job> jobs = new List<job>();
            
            for(int i = 0; i < 10; i++)
            {
                jobs.Add(new job("job " +i, i));
            }
            IEnumerable<job> jobs2 = jobs;
            return View(jobs2);*/


            var model = await _productCategoryAppService.GetListAllAsync();
            return View(model);

        }
    }
}
