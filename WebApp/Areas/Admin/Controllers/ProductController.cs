using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;
using WebApp.Models.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.ProductRepository.GetAll().ToList();


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



       /* [HttpPost]
        public IActionResult Upsert(ProductVM vm, IFormFile? file)
        {
            Regex rx = new Regex(".*[0-9].*");
            if (vm.Product.Author == null || rx.Matches(vm.Product.Author).Count >= 1)
            {
                ModelState.AddModelError("", "Enter author name in a valid format" + vm.Product.Author);
            }

        }
*/




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
    }
}
