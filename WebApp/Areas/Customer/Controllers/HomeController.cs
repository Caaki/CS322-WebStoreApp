using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Encodings.Web;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;

namespace WebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;



        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public IActionResult Test()
        {
            return View();
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.ProductRepository.GetAll(includedProperties:"Category");
            return View(productList);
        }
        public IActionResult Details(int id)
        {

            Product product = _unitOfWork.ProductRepository.Get(u=>u.Id == id,includedProperties:"Category");
            return View(product);
        }
        [Authorize]
        public async Task<IActionResult> OrderAsync(int id, int count,string email)
        {
            Product p = _unitOfWork.ProductRepository.Get(u => u.Id == id);
            if (p == null)
            {
                return NotFound();
            }
            else
            {
                double price = 0;
                if (count < 50) {
                price = p.Price;
                }else if (count >= 100) {
                price=p.Price100;
                }
                else
                {
                    price = p.Price50;
                }

                double totalPrice = p.Price * count;
                string emailBody = $"Order Confirmation\r\n\r\nDear Customer,\r\n\r\n" +
                    $"Thank you for placing an order with us. Your order details are as follows:\r\n\r\n" +
                    $"Product: {p.Title}\r\n" +
                    $"Quantity: {count}\r\n" +
                    $"Price: {price}\r\n\r\n" +
                    $"Total Price: {totalPrice}\r\n\r\n" +
                    $"We have received your order and will process it shortly.\r\n\r\n" +
                    $"Thank you for choosing our services!\r\n";

                await _emailSender.SendEmailAsync(
                email,
                 "Order created",
                     emailBody);

            return RedirectToAction("Index","Home");

            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
