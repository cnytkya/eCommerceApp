using AutoMapper;
using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Application.Interface.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eCommerceApp.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, ICategoryService categoryService, IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]//CRUD=>Read
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        [HttpGet] //CRUD --> Create(Get)
        public async Task<IActionResult> Create()
        {
            //Alt kategori dropdown'ı için verileri alalım
            var subcategories = await _categoryService.GetAllCategoriesAsync();
            var subcategoryList = new List<SelectListItem>();

            foreach (var category in subcategories)
            {
                if (category.SubCategories != null && category.SubCategories.Any())
                {
                    var categoryGroup = new SelectListGroup { Name = category.Name };
                    foreach (var subcategory in category.SubCategories)
                    {
                        subcategoryList.Add(new SelectListItem
                        {
                            Text = category.Name,
                            Value = subcategory.Id.ToString(),
                            Group = categoryGroup
                        });
                    }
                }
            }
            ViewBag.Subcategories = new SelectList(subcategoryList, "Value", "Text", "Group.Name", null);
            return View();
        }

        [HttpPost] //CRUD --> Create(Post)
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Lütfen tüm zorunlu alanaları doğru doldurun";
                var subcategories = await _categoryService.GetAllCategoriesAsync();
                var subcategoryList = new List<SelectListItem>();
                foreach (var category in subcategories)
                {
                    if (category.SubCategories != null && category.SubCategories.Any())
                    {
                        var categoryGroup = new SelectListGroup { Name = category.Name };
                        foreach (var subcategory in category.SubCategories)
                        {
                            subcategoryList.Add(new SelectListItem
                            {
                                Text = category.Name,
                                Value = subcategory.Id.ToString(),
                                Group = categoryGroup
                            });
                        }
                    }
                }
                ViewBag.Subcategories = new SelectList(subcategoryList, "Value", "Text", "Group.Name", null);
                return View(createProductDto);
            }
            var (succedeed,errors) = await _productService.CreateProductAsync(createProductDto);
            if (succedeed)
            {
                TempData["SuccessMessage"] = "Ürün başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = "Ürün oluşturulken bir hata oluştu";
            return View(createProductDto);
        }
    }
}
