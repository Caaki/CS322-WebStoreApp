using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;
using WebApp.Models.ViewModels;
using WebApp.Utility;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.ProductRepository.GetAll(includedProperties:"Category").ToList();


            return View(objProductList);
        }
        [HttpGet]
        public IActionResult Create()
        {

            IEnumerable<SelectListItem> cl = _unitOfWork.CategoryRepository
                .GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            //ViewBag.CategoryList = CategoryList;

            ProductVM vm = new()
            {
                CategoryList = cl,
                Product = new Product()
            };
            return View(vm);

        }




        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            ProductVM vm = new() {
                CategoryList = _unitOfWork.CategoryRepository.GetAll()
            .Select(u => new SelectListItem()
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
                Product = new Product()
              
            };

            if(id == null || id == 0)
            {

                return View(vm);

            }
            else
            {
                Product? p = _unitOfWork.ProductRepository.Get(u => u.Id == id);
                if (p == null)
                {
                    return NotFound();
                }
                else
                {
                    vm.Product = p;
                    return View(vm);
                }
            }
        }



        [HttpPost]
        public IActionResult Upsert(ProductVM vm, IFormFile? file)
        {
            string a = vm.Product.ImageUrl;

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName= Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(vm.Product.ImageUrl))
                    {
                        var oldImage = Path.Combine(wwwRootPath, vm.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        
                    }

                    vm.Product.ImageUrl = @"\images\product\" + fileName;
                    
                }

                if (vm.Product.Id == 0)
                {
                    _unitOfWork.ProductRepository.Add(vm.Product);
                    TempData["success"] = "Product created successfully";

                }
                else
                {
                    _unitOfWork.ProductRepository.Update(vm.Product);
                    TempData["success"] = "Product updated successfully";
                }

                _unitOfWork.Save();
                                return RedirectToAction("Index");
            }else
            {
                IEnumerable<SelectListItem> cl = _unitOfWork.CategoryRepository
                   .GetAll()
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   });

                ProductVM newVM = new()
                {
                    CategoryList = cl,
                    Product = new Product()
                };

                return View(newVM);
            }

        }





        [HttpPost]
        public IActionResult Create(ProductVM vm)
        {
            Regex rx = new Regex(".*[0-9].*");
            if (vm.Product.Author == null || rx.Matches(vm.Product.Author).Count >= 1)
            {
                ModelState.AddModelError("", "Enter author name in a valid format" + vm.Product.Author);
            }

            if (ModelState.IsValid)
            {
                int categoryId = vm.Product.CategoryId;
                int productId = vm.Product.Id;
                _unitOfWork.ProductRepository.Add(vm.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
            }
            else
            {
                IEnumerable<SelectListItem> cl = _unitOfWork.CategoryRepository
                   .GetAll()
                   .Select(u => new SelectListItem
                   {
                       Text = u.Name,
                       Value = u.Id.ToString()
                   });

                ProductVM newVM = new()
                {
                    CategoryList = cl,
                    Product = new Product()
                };

                return View(newVM);
            }
        }

        /*
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product? product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.ProductRepository.Delete(product);
                _unitOfWork.Save();
                TempData["success"] = "Product deleted";
                return RedirectToAction("Index", "Product");
            }
        }
*/

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product? product = _unitOfWork.ProductRepository.Get(u => u.Id == id);
            IEnumerable<SelectListItem> cl = _unitOfWork.CategoryRepository
            .GetAll()
            .Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            if (product == null)
            {
                return NotFound();
            }

            ProductVM productVM = new ProductVM()
            {
                Product = product,
                CategoryList = cl

            };

            return View(productVM);
        }

        [HttpPost]
        public IActionResult Edit(ProductVM vm)
        {
            if (vm == null)
            {
                return NotFound();
            }

            Regex rx = new Regex(".*[0-9].*");
            if (vm.Product.Author == null || rx.Matches(vm.Product.Author).Count >= 1)
            {
                Console.WriteLine("Ovde je");
                ModelState.AddModelError("", "Enter author name in a valid format" + vm.Product.Author);
            }

            if (ModelState.IsValid)
            {
                int a = vm.Product.Id;
                _unitOfWork.ProductRepository.Update(vm.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            else
            {

                IEnumerable<SelectListItem> cl = _unitOfWork.CategoryRepository
               .GetAll()
               .Select(u => new SelectListItem
               {
                   Text = u.Name,
                   Value = u.Id.ToString()
               });
                vm.CategoryList = cl;


                return View(vm);
            }
      }

        #region API

        [HttpGet]
        public IActionResult GetAll(int id)
        {

            List<Product> objProductList = _unitOfWork.ProductRepository.GetAll(includedProperties:"Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {

            var toBeDeleted = _unitOfWork.ProductRepository.Get(u => u.Id == id);

            if(toBeDeleted == null)
            {
                return Json(new {success =  false, message="Error while deleting"});
            }

            var oldImage = 
                Path.Combine(
                _webHostEnvironment.WebRootPath, toBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);
            }

            _unitOfWork.ProductRepository.Delete(toBeDeleted);
            _unitOfWork.Save();

            return Json(new {success =  true, message="Product deleted successfully"});

        }

        #endregion
    }
}
