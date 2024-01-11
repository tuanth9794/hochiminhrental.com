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
using CoreCnice.Areas.Admin.Models;
using Newtonsoft.Json;
using CoreCnice.UtilsCs;
using CoreCnice.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CoreCnice.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AdminCustomerController : Controller
    {
        //private readonly BulSoftCmsConnectContext _context;

        //public AdminCustomerController(BulSoftCmsConnectContext context)
        //{
        //    _context = context;
        //}

        private readonly BulSoftCmsConnectContext _context = new BulSoftCmsConnectContext();

        //Danh sách khách hàng load list kendo
        private readonly IHostingEnvironment he;
        clsFormat cl = new clsFormat();
        int IdUser = 0;
        int groupid = 0;
        int groupTyoe = 0;
        private readonly IHttpContextAccessor httpContextAccessor;
        public AdminCustomerController(IHostingEnvironment e, IHttpContextAccessor httpContextAccessor)
        {
            he = e;
            this.httpContextAccessor = httpContextAccessor;
            if (Utils.CIntDef(httpContextAccessor.HttpContext.User.Identity.Name) == 0)
            {

            }
            else
            {
                groupid = Utils.CIntDef(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value);
                groupTyoe = Utils.CIntDef(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.GroupSid).Value);
                IdUser = Utils.CIntDef(httpContextAccessor.HttpContext.User.Identity.Name);
            }
        }
        public virtual ObjectResult CustomerList()
        {
            try
            {
                if (groupTyoe == 0)
                {
                    var CusList = _context.ESHOP_CUSTOMER.Where(x => x.CUSTOMER_SHOWTYPE != 2 && x.CUSTOMER_HIDEN ==0).Select(p => new ESHOP_CUSTOMER
                    {
                        CUSTOMER_ID = p.CUSTOMER_ID,
                        CUSTOMER_EMAIL = p.CUSTOMER_EMAIL,
                        CUSTOMER_FULLNAME = p.CUSTOMER_FULLNAME,
                        CUSTOMER_ADDRESS = p.CUSTOMER_ADDRESS,
                        CUSTOMER_PHONE1 = p.CUSTOMER_PHONE1,
                        CUSTOMER_PUBLISHDATE = p.CUSTOMER_PUBLISHDATE,
                    }).OrderByDescending(p => p.CUSTOMER_PUBLISHDATE);
                    //  return Json(json);
                    return new ObjectResult(CusList);
                }
                else
                {
                    var CusList = _context.ESHOP_CUSTOMER.Where(x => (x.USER_ID_PHUTRACH == IdUser || x.USER_ID == IdUser) & x.CUSTOMER_SHOWTYPE != 2 && x.CUSTOMER_HIDEN == 0).Select(p => new ESHOP_CUSTOMER
                    {
                        CUSTOMER_ID = p.CUSTOMER_ID,
                        CUSTOMER_FULLNAME = p.CUSTOMER_FULLNAME,
                        CUSTOMER_EMAIL = p.CUSTOMER_EMAIL,                    
                        CUSTOMER_ADDRESS = p.CUSTOMER_ADDRESS,
                        CUSTOMER_PHONE1 = p.CUSTOMER_PHONE1,
                        CUSTOMER_PUBLISHDATE = p.CUSTOMER_PUBLISHDATE,
                    }).OrderByDescending(p => p.CUSTOMER_PUBLISHDATE);
                    //  return Json(json);
                    return new ObjectResult(CusList);
                }

            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message);
            }
        }


        public virtual ObjectResult CustomerListSelle()
        {
            try
            {
                if (groupTyoe == 0)
                {
                    var CusList = _context.ESHOP_CUSTOMER.Where(x => x.CUSTOMER_SHOWTYPE == 2 && x.CUSTOMER_HIDEN == 0).Select(p => new ESHOP_CUSTOMER
                    {
                        CUSTOMER_ID = p.CUSTOMER_ID,
                        CUSTOMER_FULLNAME = p.CUSTOMER_FULLNAME,
                        CUSTOMER_ADDRESS = p.CUSTOMER_ADDRESS,
                        CUSTOMER_EMAIL = p.CUSTOMER_EMAIL,
                        CUSTOMER_PHONE1 = p.CUSTOMER_PHONE1,
                        CUSTOMER_PUBLISHDATE = p.CUSTOMER_PUBLISHDATE,
                    }).OrderByDescending(p => p.CUSTOMER_PUBLISHDATE);
                    //  return Json(json);
                    return new ObjectResult(CusList);
                }
                else
                {
                    var CusList = _context.ESHOP_CUSTOMER.Where(x => x.USER_ID_PHUTRACH == IdUser || x.USER_ID == IdUser && x.CUSTOMER_HIDEN == 0).Select(p => new ESHOP_CUSTOMER
                    {
                        CUSTOMER_ID = p.CUSTOMER_ID,
                        CUSTOMER_EMAIL = p.CUSTOMER_EMAIL,
                        CUSTOMER_FULLNAME = p.CUSTOMER_FULLNAME,
                        CUSTOMER_ADDRESS = p.CUSTOMER_ADDRESS,
                        CUSTOMER_PHONE1 = p.CUSTOMER_PHONE1,
                        CUSTOMER_PUBLISHDATE = p.CUSTOMER_PUBLISHDATE,
                    }).OrderByDescending(p => p.CUSTOMER_PUBLISHDATE);
                    //  return Json(json);
                    return new ObjectResult(CusList);
                }

            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message);
            }
        }

        // GET: Admin/AdminCustomer
        public async Task<IActionResult> Index()
        {
            return View(await _context.ESHOP_CUSTOMER.ToListAsync());
        }

        public async Task<IActionResult> IndexSeller()
        {
            return View(await _context.ESHOP_CUSTOMER.ToListAsync());
        }

        // GET: Admin/AdminCustomer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_CUSTOMER = await _context.ESHOP_CUSTOMER
                .SingleOrDefaultAsync(m => m.CUSTOMER_ID == id);
            if (eSHOP_CUSTOMER == null)
            {
                return NotFound();
            }

            return View(eSHOP_CUSTOMER);
        }

        // GET: Admin/AdminCustomer/Create
        public IActionResult Create()
        {
            List<TreeViewNode> nodes = new List<TreeViewNode>();

            ESHOP_PROPERTIES entities = new ESHOP_PROPERTIES();


            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID == 0))
            {
                nodes.Add(new TreeViewNode { id = item.PROP_ID.ToString(), parent = "#", text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = false, opened = false } });
            }

            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID != 0))
            {
                nodes.Add(new TreeViewNode { id = item.PROP_PARENT_ID.ToString() + "-" + item.PROP_ID.ToString(), parent = item.PROP_PARENT_ID.ToString(), text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = false } });
            }

            //Serialize to JSON string.
            ViewBag.Json = JsonConvert.SerializeObject(nodes);

            List<TypeRoom> OrderStatus = new List<TypeRoom>();
            OrderStatus.Insert(0, new TypeRoom { id = "Separate Room", value = "PN Riêng" });
            OrderStatus.Insert(1, new TypeRoom { id = "Studio", value = "Studio" });
            OrderStatus.Insert(2, new TypeRoom { id = "Duplex", value = "Duplex" });
            OrderStatus.Insert(3, new TypeRoom { id = "Share House", value = "Share House" });
            OrderStatus.Insert(4, new TypeRoom { id = "Whole House", value = "Nhà Nguyên Căn" });
            ViewBag.OrderStatusList = OrderStatus;

            return View();
        }

        public bool checkExitNewsPro(int? id, int? idnews)
        {
            bool fl = false;

            var Procheck = _context.ESHOP_CUSTOMER_PROPERTIES.SingleOrDefault(x => x.CUSTOMER_ID == idnews && x.PROP_ID == id);
            if (Procheck != null)
            {
                fl = true;
            }

            return fl;
        }

        // POST: Admin/AdminCustomer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string selectedItems, [Bind("CUSTOMER_ID,CUSTOMER_FULLNAME,CUSTOMER_UN,CUSTOMER_PW,CUSTOMER_SEX,CUSTOMER_ADDRESS,CUSTOMER_PHONE1,CUSTOMER_NEWSLETTER,CUSTOMER_PUBLISHDATE,CUSTOMER_UPDATE,CUSTOMER_OID,CUSTOMER_SHOWTYPE,CUSTOMER_TOTAL_POINT,CUSTOMER_REMAIN,CUSTOMER_FIELD1,CUSTOMER_IP,CUSTOMER_NGANSACH,CUSTOMER_LOAICANHO,CUSTOMER_THOIGIANTHUE,CUSTOMER_SONGUOIO,CUSTOMER_ACTIVE")] ESHOP_CUSTOMER eSHOP_CUSTOMER)
        {
            if (ModelState.IsValid)
            {
                int groupd_id = Utils.CIntDef(User.Identity.Name);

                eSHOP_CUSTOMER.USER_ID = IdUser;
                eSHOP_CUSTOMER.USER_ID_PHUTRACH = IdUser;
                _context.Add(eSHOP_CUSTOMER);
                await _context.SaveChangesAsync();

                List<TreeViewNode> result = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);

                var listCon = _context.ESHOP_CUSTOMER_PROPERTIES.Where(x => x.CUSTOMER_ID == eSHOP_CUSTOMER.CUSTOMER_ID);
                if (listCon.ToList().Count > 0)
                {
                    _context.ESHOP_CUSTOMER_PROPERTIES.RemoveRange(listCon);
                    await _context.SaveChangesAsync();
                }

                if (result.ToList().Count() > 0)
                {
                    foreach (var itemcat in result.ToList())
                    {
                        int idPro = Utils.CIntDef(itemcat.id);
                        var CatListNew = _context.ESHOP_CUSTOMER_PROPERTIES.Where(m => m.CUSTOMER_ID == eSHOP_CUSTOMER.CUSTOMER_ID && m.PROP_ID == idPro);
                        if (CatListNew.ToList().Count > 0)
                        {
                        }
                        else
                        {
                            ESHOP_CUSTOMER_PROPERTIES nc = new ESHOP_CUSTOMER_PROPERTIES();
                            nc.PROP_ID = idPro;
                            nc.CUSTOMER_ID = eSHOP_CUSTOMER.CUSTOMER_ID;
                            _context.Add(nc);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Edit", "Admin/Customer", new { id = eSHOP_CUSTOMER.CUSTOMER_ID });
            }
            return View(eSHOP_CUSTOMER);
        }

        // GET: Admin/AdminCustomer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_CUSTOMER = await _context.ESHOP_CUSTOMER.SingleOrDefaultAsync(m => m.CUSTOMER_ID == id);
            if (eSHOP_CUSTOMER == null)
            {
                return NotFound();
            }

            List<TreeViewNode> nodes = new List<TreeViewNode>();

            ESHOP_PROPERTIES entities = new ESHOP_PROPERTIES();


            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID == 0))
            {
                nodes.Add(new TreeViewNode { id = item.PROP_ID.ToString(), parent = "#", text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = false, opened = false } });
            }

            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID != 0))
            {
                nodes.Add(new TreeViewNode { id = item.PROP_PARENT_ID.ToString() + "-" + item.PROP_ID.ToString(), parent = item.PROP_PARENT_ID.ToString(), text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = checkExitNewsPro(item.PROP_ID, id) } });
            }

            //Serialize to JSON string.
            ViewBag.Json = JsonConvert.SerializeObject(nodes);

            List<TypeRoom> OrderStatus = new List<TypeRoom>();
            OrderStatus.Insert(0, new TypeRoom { id = "Separate Room", value = "PN Riêng" });
            OrderStatus.Insert(1, new TypeRoom { id = "Studio", value = "Studio" });
            OrderStatus.Insert(2, new TypeRoom { id = "Duplex", value = "Duplex" });
            OrderStatus.Insert(3, new TypeRoom { id = "Share House", value = "Share House" });
            OrderStatus.Insert(4, new TypeRoom { id = "Whole House", value = "Nhà Nguyên Căn" });
            ViewBag.OrderStatusList = OrderStatus;

            return View(eSHOP_CUSTOMER);
        }

        // POST: Admin/AdminCustomer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string selectedItems, int id, [Bind("CUSTOMER_ID,CUSTOMER_FULLNAME,CUSTOMER_UN,CUSTOMER_PW,CUSTOMER_SEX,CUSTOMER_ADDRESS,CUSTOMER_PHONE1,CUSTOMER_NEWSLETTER,CUSTOMER_PUBLISHDATE,CUSTOMER_UPDATE,CUSTOMER_OID,CUSTOMER_SHOWTYPE,CUSTOMER_TOTAL_POINT,CUSTOMER_REMAIN,CUSTOMER_FIELD1,CUSTOMER_IP,CUSTOMER_NGANSACH,CUSTOMER_LOAICANHO,CUSTOMER_THOIGIANTHUE,CUSTOMER_SONGUOIO,CUSTOMER_ACTIVE,CUSTOMER_EMAIL")] ESHOP_CUSTOMER eSHOP_CUSTOMER)
        {
            if (id != eSHOP_CUSTOMER.CUSTOMER_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eSHOP_CUSTOMER);
                    await _context.SaveChangesAsync();

                    if (selectedItems != null)
                    {
                        List<TreeViewNode> result = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);

                        var listCon = _context.ESHOP_CUSTOMER_PROPERTIES.Where(x => x.CUSTOMER_ID == eSHOP_CUSTOMER.CUSTOMER_ID);
                        if (listCon.ToList().Count > 0)
                        {
                            _context.ESHOP_CUSTOMER_PROPERTIES.RemoveRange(listCon);
                            await _context.SaveChangesAsync();
                        }
                 

                    if (result.ToList().Count() > 0)
                    {
                        foreach (var itemcat in result.ToList())
                        {
                            int idPro = Utils.CIntDef(itemcat.id);
                            var CatListNew = _context.ESHOP_CUSTOMER_PROPERTIES.Where(m => m.CUSTOMER_ID == eSHOP_CUSTOMER.CUSTOMER_ID && m.PROP_ID == idPro);
                            if (CatListNew.ToList().Count > 0)
                            {
                            }
                            else
                            {
                                ESHOP_CUSTOMER_PROPERTIES nc = new ESHOP_CUSTOMER_PROPERTIES();
                                nc.PROP_ID = idPro;
                                nc.CUSTOMER_ID = eSHOP_CUSTOMER.CUSTOMER_ID;
                                _context.Add(nc);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_CUSTOMERExists(eSHOP_CUSTOMER.CUSTOMER_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
               
            }

            List<TreeViewNode> nodes = new List<TreeViewNode>();

            ESHOP_PROPERTIES entities = new ESHOP_PROPERTIES();


            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID == 0))
            {
                nodes.Add(new TreeViewNode { id = item.PROP_ID.ToString(), parent = "#", text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = false, opened = false } });
            }

            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID != 0))
            {
                nodes.Add(new TreeViewNode { id = item.PROP_PARENT_ID.ToString() + "-" + item.PROP_ID.ToString(), parent = item.PROP_PARENT_ID.ToString(), text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = checkExitNewsPro(item.PROP_ID, id) } });
            }

            //Serialize to JSON string.
            ViewBag.Json = JsonConvert.SerializeObject(nodes);

            List<TypeRoom> OrderStatus = new List<TypeRoom>();
            OrderStatus.Insert(0, new TypeRoom { id = "Separate Room", value = "PN Riêng" });
            OrderStatus.Insert(1, new TypeRoom { id = "Studio", value = "Studio" });
            OrderStatus.Insert(2, new TypeRoom { id = "Duplex", value = "Duplex" });
            OrderStatus.Insert(3, new TypeRoom { id = "Share House", value = "Share House" });
            OrderStatus.Insert(4, new TypeRoom { id = "Whole House", value = "Nhà Nguyên Căn" });
            ViewBag.OrderStatusList = OrderStatus;
            return View(eSHOP_CUSTOMER);
        }

        // GET: Admin/AdminCustomer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_CUSTOMER = await _context.ESHOP_CUSTOMER
                .SingleOrDefaultAsync(m => m.CUSTOMER_ID == id);
            if (eSHOP_CUSTOMER == null)
            {
                return NotFound();
            }

            return View(eSHOP_CUSTOMER);
        }

        // POST: Admin/AdminCustomer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eSHOP_CUSTOMER = await _context.ESHOP_CUSTOMER.SingleOrDefaultAsync(m => m.CUSTOMER_ID == id);
            _context.ESHOP_CUSTOMER.Remove(eSHOP_CUSTOMER);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ESHOP_CUSTOMERExists(int id)
        {
            return _context.ESHOP_CUSTOMER.Any(e => e.CUSTOMER_ID == id);
        }

        /// <summary>
        /// Danh sachs đơn hàng
        /// </summary>
        /// <returns></returns>
        public virtual ObjectResult OrderItemList(int id)
        {
            try
            {
                var OrderItemListJoin = (from ord in _context.ESHOP_ORDER
                                         join customer in _context.ESHOP_CUSTOMER
                                         on ord.CUSTOMER_ID equals customer.CUSTOMER_ID
                                         where ord.CUSTOMER_ID == id
                                         select new
                                         {
                                             ord.ORDER_ID,
                                             ord.ORDER_CODE,
                                             ord.ORDER_PUBLISHDATE,
                                             ord.ORDER_STATUS,
                                             ord.ORDER_TOTAL_ALL,
                                         });
                //  return Json(json);
                return new ObjectResult(OrderItemListJoin);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public virtual ObjectResult CustomerListRequest(int id)
        {
            try
            {
                if (groupTyoe == 0)
                {
                    if(id != 0)
                    {
                        var ListCustomer_Request = _context.Customer_Request.Where(x => x.CUSTOMER_ID == id);

                        return new ObjectResult(ListCustomer_Request);
                    }    
                    else
                    {
                        var ListCustomer_Request = _context.Customer_Request;

                        return new ObjectResult(ListCustomer_Request);
                    }    
                   
                }
                else
                {
                    var ListCustomer_Request = _context.Customer_Request.Where(x => x.CUSTOMER_ID == id);

                    return new ObjectResult(null);
                }

            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message);
            }
        }

        public IActionResult IndexRequest(int id)
        {
            ViewBag.CusId = id;
            return View();
        }

        public async Task<IActionResult> RequestEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eSHOP_CUSTOMER = await _context.Customer_Request.SingleOrDefaultAsync(m => m.Customer_request_id == id);
            if (eSHOP_CUSTOMER == null)
            {
                return NotFound();
            }

            List<TreeViewNode> nodes = new List<TreeViewNode>();

            ESHOP_PROPERTIES entities = new ESHOP_PROPERTIES();


            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID == 0))
            {
                nodes.Add(new TreeViewNode { id = item.PROP_ID.ToString(), parent = "#", text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = false, opened = false } });
            }

            foreach (var item in _context.ESHOP_PROPERTIES.Where(x => x.PROP_ID != 1 && x.PROP_PARENT_ID != 0))
            {
                nodes.Add(new TreeViewNode { id = item.PROP_PARENT_ID.ToString() + "-" + item.PROP_ID.ToString(), parent = item.PROP_PARENT_ID.ToString(), text = item.PROP_NAME, state = new JsTreeAttribute { id = item.PROP_ID.ToString(), selected = checkExitNewsPro(item.PROP_ID, id) } });
            }

            //Serialize to JSON string.
            ViewBag.Json = JsonConvert.SerializeObject(nodes);         

            return View(eSHOP_CUSTOMER);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestEdit(int id, [Bind("Customer_request_id,CUSTOMER_ID,Customer_Name,Customer_Email,Customer_Phone,Customer_Quoctich,Customer_DuAn,Customer_PN,Customer_PB,Customer_Dt,Customer_Ns,Customer_TimeThue,Customer_Desc,Customer_Check,Customer_Image,Customer_Active,Customer_Request_Order,Customer_Field1,Customer_Field2,Customer_TimeAv")] Customer_Request eSHOP_CUSTOMER)
        {
            if (id != eSHOP_CUSTOMER.CUSTOMER_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eSHOP_CUSTOMER);
                    await _context.SaveChangesAsync();                 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ESHOP_CUSTOMERExists(eSHOP_CUSTOMER.CUSTOMER_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

         
            return View(eSHOP_CUSTOMER);
        }
    }
}
