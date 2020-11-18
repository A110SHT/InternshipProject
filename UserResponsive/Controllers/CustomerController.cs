using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using ModelEntities;

namespace UserResponsive.Controllers
{
  
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerDataController _customer;
        private readonly IWebHostEnvironment WebHostEnvironment;


        public CustomerController(ICustomerDataController customer, IWebHostEnvironment webHostEnvironment)
        {
            _customer = customer;
            WebHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            IList<CustomerModel> data = await _customer.GetAll();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> AddorEdit(Guid objCustomerId)
        {
            var listRoles= await _customer.GetAllRoles();
            List<SelectListItem> listOptions = new List<SelectListItem>();
            for (int i = 0; i < listRoles.Count; i++)
            {
                listOptions.Add(new SelectListItem()
                {
                    Value = listRoles[i].RoleId.ToString(),
                    Text=listRoles[i].Role
                });
            }
            ViewBag.Role = listOptions;
            if (objCustomerId != Guid.Empty)
            {
                CustomerModel data = await _customer.GetById(objCustomerId);
                return View(data);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddorEdit(CustomerModel objCustomer)
        {
            if (ModelState.IsValid)
            { 
                string image = objCustomer.PictureName;
            if (objCustomer.Files != null)
            {
                string[] picArray = new string[1];
                if (image != null)
                {
                    DeletePicture(image);
                }
                string[] docName = UploadDocument(objCustomer);
                string[] nameExtensions = { "jpg", "png", "jpeg" };
                foreach (string item in docName)
                {
                    if (item != null)
                    {
                        string ext = item.Substring(item.LastIndexOf('.') + 1);
                        if (nameExtensions.Any(ext.Contains) == true)
                        {
                            Array.Resize(ref picArray, picArray.Length + 1);
                            picArray[^1] = item;
                        }
                    }

                }
                objCustomer.Pictures = picArray;

                //Removing null value from index 0 in an array
                List<string> list = new List<string>(objCustomer.Pictures);
                list.RemoveAt(0);
                objCustomer.Pictures = list.ToArray();

            }
            else
            {
                objCustomer.Pictures = image.Split(',');
            }
            _customer.AddorUpdateCustomer(objCustomer);
            return RedirectToAction("List");
            }
            return View();
           
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid objCustomerId)
        {
            var data = await _customer.GetById(objCustomerId);
            return View(data);

        }

        [HttpPost]
        public IActionResult Delete(CustomerModel objCustomer)
        {
            DeletePicture(objCustomer.PictureName);
            _customer.DeleteCustomerData(objCustomer);
            return RedirectToAction("List");

        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid objCustomerId,string email)
        {
            if (objCustomerId == Guid.Empty)
            {
                email = User.Claims.FirstOrDefault().Value;
                CustomerModel customer = await _customer.GetByEmail(email);
                var customerDetail=await _customer.GetById(customer.CustomerId);
                return View(customerDetail);
            }
            else
            {
                var data = await _customer.GetById(objCustomerId);
                return View(data);
            }           

        }

      
        private string[] UploadDocument(CustomerModel objName)
        {
            string[] PICName= new string[1];
            if (objName.Files != null)
            {
                string[] nameExtensions = { "jpg", "png", "jpeg" };
                foreach (IFormFile item in objName.Files)
                {
                    string name = (Guid.NewGuid().ToString() + "-" + item.FileName).ToLower();
                    string ext = name.Substring(name.LastIndexOf('.') + 1);
                    if (nameExtensions.Any(ext.Contains) == true)
                    {
                        string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "Photo");
                        if (Directory.Exists(uploadDir)==false)
                        {
                            Directory.CreateDirectory(uploadDir);
                        }
                        string imgPath = Path.Combine(uploadDir, name);
                        item.CopyTo(new FileStream(imgPath, FileMode.Create));
                        Array.Resize(ref PICName, PICName.Length + 1);
                        PICName[^1] = name;
                    }
                }
            }
            return PICName;
        }


        private void DeletePicture(string objPhotoName)
        {
            if (objPhotoName != null)
            {
                string[] imgName = objPhotoName.Split(',');
                foreach (string item in imgName)
                {
                    if (item != null)
                    {
                        var imgPath = Path.Combine(WebHostEnvironment.WebRootPath, "Photo", item);
                        if (System.IO.File.Exists(imgPath))
                        {
                            System.IO.File.Delete(imgPath);
                        }
                    }
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> ShowAllPicture()
        {
            var loggedEmail = User.Claims.FirstOrDefault().Value;
            var CustomerDetails = await _customer.GetByEmail(loggedEmail);
            var DetailsofCustomer = await _customer.GetById(CustomerDetails.CustomerId);
            return View(DetailsofCustomer);
        }
    }
}

