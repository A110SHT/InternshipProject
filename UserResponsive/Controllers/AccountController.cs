using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using ModelEntities;
using RestSharp;

namespace UserResponsive.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICustomerDataController _customer;

        public AccountController(ICustomerDataController Customer)
        {
            _customer = Customer;
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.Name != null)
            {
                return RedirectToAction("Index", "Customer");
            }
            LoginModel loginData = new LoginModel();
            loginData.Email = Request.Cookies["email"];
            loginData.Password = Request.Cookies["password"];
            if (loginData.Email != null && loginData.Password != null)
            {
                return View(loginData);
            }
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel objDetail)
        {
            CustomerModel data = await _customer.GetByEmail(objDetail.Email);
            if (String.Compare(data.Password, objDetail.Password) == 0)
            {

                if (objDetail.RememberMe == true)
                {
                    CookieOptions cookie = new CookieOptions();
                    Response.Cookies.Append("email", objDetail.Email, cookie); 
                    Response.Cookies.Append("password", objDetail.Password, cookie);
                    cookie.Expires = DateTime.Now.AddMinutes(1);
                }
                var userClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, objDetail.Email),
                    new Claim(ClaimTypes.Name, data.CustomerName),
                    new Claim(ClaimTypes.Role,data.Role)
                };
                var user = new ClaimsIdentity(userClaims, "User Identity");
                var userprincipal = new ClaimsPrincipal(new[] { user });               
                await HttpContext.SignInAsync(userprincipal);
                return RedirectToAction("Index", "Customer");
            }
            ModelState.AddModelError("", "Invalid Customer Name or Password");
            return View();
        }
       
        public async Task<ActionResult> LogOut()
        {           
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(LoginModel objUser)
        {
            using (MailMessage mm = new MailMessage("nabinlav@gmail.com", objUser.Email)) 
            {
                CustomerModel data = await _customer.GetByEmail(objUser.Email);
                if (data!=null)
                {
                    mm.Subject = "Password Recovery";
                    mm.Body = "Your Password is: " + data.Password;
                    mm.IsBodyHtml = false;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("nabinlav@gmail.com", "$0ftw@reeng!neer123#");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    ViewBag.Message = "Password Sent Please Check your email";
                }
                else
                {
                    ViewBag.Message = "Email doesnot exist in our database";
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(LoginModel objData)
        {
            if (string.Equals(objData.NewPassword, objData.ConfirmPassword))
            {
                string loggedUser = User.Claims.FirstOrDefault().Value;
                CustomerModel actualPassword = await _customer.GetByEmail(loggedUser);
                if (string.Equals(actualPassword.Password, objData.Password))
                {
                    _customer.Passwordupdate(objData);
                    return RedirectToAction("Index", "Customer");
                }
                ModelState.AddModelError("Password", "Entered Old Password isn't Correct");
                return View();               
            }
            ModelState.AddModelError("ConfirmPassword", "Password Didn't Matched!!!");
            return View();
           
        }
    }
}
