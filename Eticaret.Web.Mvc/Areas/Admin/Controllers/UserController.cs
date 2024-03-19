
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Eticaret.Application.Abstract;
using Eticaret.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eticaret.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userService;
        private readonly IRoleRepository _roleService;
        private readonly ISellerRepository _sellerService;

        public UserController(IUserRepository userService, IRoleRepository roleService, ISellerRepository sellerervice)
        {
            _userService = userService;
            _roleService = roleService;
            _sellerService = sellerervice;
        }

        public IActionResult List()
        {
            var user = _userService.GetDb()
                            .Include(u => u.RoleFk)
                            .Include(u => u.CartItems)
                            .Include(u => u.ProductComments)
                            .Include(u => u.Orders)
                            .OrderBy(u => u.Enabled)
                            .ToList();

            return View(user);
        }
        public IActionResult Approve(int id)
        {
            var user = _userService.GetDb()
                            .Include(u => u.RoleFk)
                            .Include(u => u.CartItems)
                            .Include(u => u.ProductComments)
                            .Include(u => u.Orders)
                            .FirstOrDefault(p => p.Id == id);

            return View(user);
        }

        [HttpPost]
        /*
         [HttpPost]
         public async Task<IActionResult> Approve(User user)
         {
             var seller = new User()
             {
                 Email = user.Email,
                 FirstName = user.FirstName,
                 LastName = user.LastName,
                 Password = user.Password,
                 RoleId = 1,
                 Enabled = user.Enabled
             };
             User? oldUser = _userService.GetDb()
                             .Include(u => u.RoleFk)
                             .Include(u => u.CartItems)
                             .Include(u => u.ProductComments)
                             .Include(u => u.Orders)
                             .FirstOrDefault(p => p.Id == user.Id);
             try
             {
                 if (oldUser!.Id != user.RoleId && user.RoleId == 1)
                 {
                     //     //_userService.Add(seller);
                     //     //_userService.Delete(user);
                     //     //return RedirectToAction(nameof(List));

                     //     _userService.Update(user);
                     //     return RedirectToAction(nameof(List));
                     return RedirectToAction(nameof(Delete));

                 }

                 _userService.Update(user);
                 return RedirectToAction(nameof(List));
             }
             catch
             {
                 ViewBag.Role = new SelectList(await _roleService.GetAllAsync(), "Id", "Name");
                 return View(user);
             }
         } 
         
          */

        [HttpPost]
        public IActionResult Approve(User user)
        {
            try
            {
                _userService.Update(user);
                return RedirectToAction(nameof(List));
            }
            catch
            {
                return View(user);
            }
        }



        public IActionResult Delete(int id)
        {
            var user = _userService.GetDb()
                            .Include(u => u.RoleFk)
                            .Include(u => u.CartItems)
                            .Include(u => u.ProductComments)
                            .Include(u => u.Orders)
                           .FirstOrDefault(p => p.Id == id);

            return View(user);
        }
        [HttpPost]
        public IActionResult Delete(int id, User comment)
        {
            try
            {
                _userService.Delete(comment);
                return RedirectToAction(nameof(List));
            }
            catch
            {
                return View(id);
            }

        }
    }
}
















