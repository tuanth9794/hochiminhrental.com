using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreCnice.Connect;
using CoreCnice.Domain;
using Microsoft.AspNetCore.Authorization;

namespace CoreCnice.Areas.Admin.Controllers
{
    //[Authorize]
    [Area("Admin")]
    public class UserAdminController : Controller
    {
        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        // GET: UserAdmin
        public async Task<IActionResult> Index()
        {
            return View(await _context.ESHOP_USER.ToListAsync());
        }

        // GET: UserAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_USER = await _context.ESHOP_USER
                .SingleOrDefaultAsync(m => m.USER_ID == id);
            if (eSHOP_USER == null)
            {
                return NotFound();
            }

            return View(eSHOP_USER);
        }
        public virtual ObjectResult UserList()
        {
            try
            {
                var UserList = _context.ESHOP_USER.Select(p => new ESHOP_USER
                {
                    USER_ID = p.USER_ID,
                    USER_NAME = p.USER_NAME,
                    USER_PUBLISHDATE = p.USER_PUBLISHDATE,
                    USER_UN = p.USER_UN,
                    USER_TYPE = p.USER_TYPE,
                    USER_START_WORK = p.USER_START_WORK

                }).OrderByDescending(p => p.USER_PUBLISHDATE);
                //  return Json(json);
                return new ObjectResult(UserList);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        // GET: UserAdmin/Create
        public IActionResult Create()
        {


            var eshop_group = _context.ESHOP_GROUP.Select(p => new ESHOP_GROUP
            {
                GROUP_ID = p.GROUP_ID,
                GROUP_NAME = p.GROUP_NAME,

            });

            IEnumerable<ESHOP_GROUP> eshop_group_add = eshop_group;

            ESHOP_USER es_cs = new ESHOP_USER();

            es_cs.GroupList = eshop_group_add;

            return View(es_cs);

        }

        // POST: UserAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("USER_ID,USER_NAME,USER_UN,USER_PW,GROUP_ID,USER_TYPE,USER_ACTIVE,SALT,USER_PUBLISHDATE,USER_START_WORK,USER_END_WORK")] ESHOP_USER eSHOP_USER)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eSHOP_USER);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Edit", "Admin/User", new { id = eSHOP_USER.USER_ID });
            }
            return View(eSHOP_USER);
        }

        // GET: UserAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var eshop_group = _context.ESHOP_GROUP.Select(p => new ESHOP_GROUP
            {
                GROUP_ID = p.GROUP_ID,
                GROUP_NAME = p.GROUP_NAME,
            });

            var eSHOP_USER = await _context.ESHOP_USER.SingleOrDefaultAsync(m => m.USER_ID == id);

            if (eSHOP_USER == null)
            {
                return NotFound();
            }

            eSHOP_USER.GroupList = eshop_group;
            return View(eSHOP_USER);
        }

        // POST: UserAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("USER_ID,USER_NAME,USER_UN,USER_PW,GROUP_ID,USER_TYPE,USER_ACTIVE,SALT,USER_PUBLISHDATE,USER_START_WORK,USER_END_WORK")] ESHOP_USER eSHOP_USER)
        {
            if (id != eSHOP_USER.USER_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eSHOP_USER);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_USERExists(eSHOP_USER.USER_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return View(eSHOP_USER);
            }
            return View(eSHOP_USER);
        }

        // GET: UserAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_USER = await _context.ESHOP_USER
                .SingleOrDefaultAsync(m => m.USER_ID == id);
            if (eSHOP_USER == null)
            {
                return NotFound();
            }

            return View(eSHOP_USER);
        }

        // POST: UserAdmin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eSHOP_USER = await _context.ESHOP_USER.SingleOrDefaultAsync(m => m.USER_ID == id);
            _context.ESHOP_USER.Remove(eSHOP_USER);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin/UserAdmin");
        }

        private bool ESHOP_USERExists(int id)
        {
            return _context.ESHOP_USER.Any(e => e.USER_ID == id);
        }

        [HttpPost]
        public  IActionResult LogIn([Bind("USER_UN,USER_PW")] ESHOP_USER eSHOP_USER)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (String.IsNullOrEmpty(eSHOP_USER.USER_UN))
                    {
                        ViewBag.Error = "Vui lòng nhập tài khoản";
                        return View("Admin");
                    }
                    else if (String.IsNullOrEmpty(eSHOP_USER.USER_PW))
                    {
                        ViewBag.Error = "Vui lòng nhập mật khẩu";
                        return View("Admin");
                    }
                    else
                    {
                        ViewBag.Error = "Đăng Nhập Thành Công";
                        return RedirectToAction("/Admin/Categories");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_USERExists(eSHOP_USER.USER_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }               
            }
            return View(eSHOP_USER);
        }
    }
}
