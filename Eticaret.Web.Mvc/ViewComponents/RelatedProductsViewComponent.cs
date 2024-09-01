using Eticaret.Dto;
using Eticaret.Web.Mvc.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Eticaret.Web.Mvc.ViewComponents
{
    public class RelatedProductsViewComponent : ViewComponent
    {
        private readonly HttpClient _httpClient;
        public RelatedProductsViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(ApplicationSettings.DATA_API_CLIENT);
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            using (var response = await _httpClient.GetAsync("Home"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    return View();
                }

                List<ProductListDTO> products = await response.Content.ReadFromJsonAsync<List<ProductListDTO>>() ?? new();
                // return View(products.Take(6).ToList());
                return View(products);
            }
        }
    }

}


