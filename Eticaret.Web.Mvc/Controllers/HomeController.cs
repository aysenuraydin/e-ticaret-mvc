using Eticaret.Dto;
using Eticaret.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eticaret.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("api");
        }
        public async Task<IActionResult> Index()
        {
            using (var response = await _httpClient.GetAsync("Home"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var products = await response.Content.ReadFromJsonAsync<List<ProductListDTO>>();
                    return View(products); ;
                }
                ViewBag.ErrorMessage = $"Error: {response.ReasonPhrase}";
            }
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        public async Task<IActionResult> Listing(int? id, string? q)
        {
            var productList = new List<ProductListDTO>();
            var url = (id == null) ? $"Home" : $"Home/{id}";
            using (var response = await _httpClient.GetAsync(url))
            {
                productList = await response.Content.ReadFromJsonAsync<List<ProductListDTO>>() ?? new();
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = $"Error: {response.ReasonPhrase}";
                }
            }
            if (!string.IsNullOrWhiteSpace(q))
            {
                TempData["search"] = q;
                productList = productList
                                .Where(s => s.Name!.ToLower().Contains(q.ToLower()))
                                .ToList();
            }
            else
            {
                TempData["search"] = "";
            }
            var categoriesList = new List<CategoryListDTO>();
            using (var response = await _httpClient.GetAsync("Categories"))
            {
                categoriesList = await response.Content.ReadFromJsonAsync<List<CategoryListDTO>>() ?? new();
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.ErrorMessage = $"Error: {response.ReasonPhrase}";
                }
            }
            ProductListViewModel productAndSearch = new();
            productAndSearch.ProductList = productList;
            productAndSearch.Categories = categoriesList;
            return View(productAndSearch);
        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            if (TempData["ErrorMessage"] != null) ViewBag.ErrorMessage = TempData["ErrorMessage"];

            using (var response = await _httpClient.GetAsync($"Product/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var product = await response.Content.ReadFromJsonAsync<ProductDetailDTO>();

                    if (TempData["Message"] != null) ViewBag.ErrorMessage = TempData["Message"];

                    return View(product); ;
                }

                ViewBag.ErrorMessage = $"Error: {response.ReasonPhrase}";
            }

            if (TempData["Message"] != null) ViewBag.ErrorMessage = TempData["Message"];

            return View();
        }
        public IActionResult Error()
        {
            var errorMessage = HttpContext.Items["ExceptionMessage"]?.ToString();
            return View(new ErrorViewModel { RequestId = errorMessage });
        }
    }
}

