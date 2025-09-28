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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _productService = productService;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        [HttpGet]//CRUD=>Read
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }
        public async Task PrepareSubcategoryViewBag(Guid? selectedId = null)//Create ve Edit için ortak metot.
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
                            Text = subcategory.Name,
                            Value = subcategory.Id.ToString(),
                            Group = categoryGroup,
                            Selected = (selectedId.HasValue && subcategory.Id == selectedId.Value)
                        });
                    }
                }
            }
            ViewBag.Subcategories = subcategoryList;
        }

        [HttpGet] //CRUD --> Create(Get)
        public async Task<IActionResult> Create()
        {
            await PrepareSubcategoryViewBag();
            return View();
        }

        [HttpPost] //CRUD --> Create(Post)
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductDto createProductDto, IFormFile imgFile)//Upsert => update ve create(insert) için ortak metot oluşturulabilir)
        {
            if (imgFile == null && imgFile.Length == 0)
            {
                ModelState.AddModelError("imgFile", "Ürün resmi seçmeniz gerekiyor!");
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessafe"] = "Lütfen tüm zorunlu alanları doğru doldurun";
                //Hata durumunda kategorileri dropdown'a tekrar yüklenmesi gerekir.
                await PrepareSubcategoryViewBag(createProductDto.SubcategoryId);
                return View(createProductDto);
            }

            string uniqueFileName = null;
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            try
            {
                if (imgFile != null && imgFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(wwwRootPath, "img", "products");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    string extension = Path.GetExtension(imgFile.FileName);
                    uniqueFileName = Guid.NewGuid().ToString() + extension;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(fileStream);
                    }
                    createProductDto.ImageUrl = Path.Combine("~/", "img", "products", uniqueFileName).Replace("\\", "/");
                }
                var (succeeded, errors) = await _productService.CreateProductAsync(createProductDto, imgFile);

                if (succeeded)
                {
                    TempData["SuccessMessage"] = "Ürün başarıyla oluşturuldu";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Dosya yüklenirken bir hata oluştu." + ex.Message);
            }

            TempData["ErrorMessage"] = "Ürün oluşturulken bir hata oluştu";
            await PrepareSubcategoryViewBag(createProductDto.SubcategoryId);
            return View(createProductDto);
        }

        [HttpGet] //CRUD --> Edit(Get)
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Ürün bulunamadı.";
            }
            var model = _mapper.Map<EditProductDto>(product);
            await PrepareSubcategoryViewBag(model.SubcategoryId);
            return View(model);
        }
        [HttpPost] //CRUD --> Edit(Post)
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProductDto model, IFormFile? imgFile)
        {
            string uniqueFileName = null;
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string oldImgPath = model.ImageUrl;

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Lütfen tüm zorunlu alanları doldurun.";
                await PrepareSubcategoryViewBag(model.SubcategoryId);
                return View(model);
            }
            try
            {
                if (imgFile != null && imgFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(wwwRootPath, "img", "products");
                    string extension = Path.GetExtension(imgFile.FileName);
                    uniqueFileName = Guid.NewGuid().ToString() + extension;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(fileStream);
                    }
                    model.ImageUrl = Path.Combine("~/", "img", "products", uniqueFileName).Replace("\\", "/");
                    if (!string.IsNullOrEmpty(oldImgPath))
                    {
                        string oldImgFileName = Path.GetFileName(oldImgPath);
                        string fileToDeletepath = Path.Combine(wwwRootPath, "img", "products",oldImgFileName);
                        if (System.IO.File.Exists(fileToDeletepath))
                        {
                            System.IO.File.Delete(fileToDeletepath);
                        }
                    }
                }
                var (succeeded, errors) = await _productService.UpdateProductAsync(model);

                if (succeeded)
                {
                    TempData["SuccessMessage"] = "Ürün başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var er in errors)
                {
                    ModelState.AddModelError(string.Empty,er);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Dosya yüklenirken bir hata oluştu." + ex.Message);
            }
            TempData["ErrorMessage"] = "Ürün oluşturulken bir hata oluştu";
            await PrepareSubcategoryViewBag(model.SubcategoryId);
            return View(model);
        }
    }
}
