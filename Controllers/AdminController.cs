using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Preora.dbcon;
using Preora.Filters;
using Preora.Models;

namespace SofttrixAccounts.Controllers
{
    [SessionTimeout]
    public class AdminController : Controller
    {
        Preora_DBEntities db = new Preora_DBEntities();
        // GET: Admin
        public ActionResult Index()
        {
            var saleC = db.tbl_SaleBill.Count();
            var PurchaeC = db.tbl_PurchaseBill.Count();
            var UsersC = db.Mst_Users.Where(x => x.is_deleted == false).Count();
            var model = new DashboardModel();
            {
                model.SaleCount = saleC;
                model.PurchaseCount = PurchaeC;
                model.UserCount = UsersC;
                return View(model);
            }
        }
        public ActionResult ShopMaster()
        {
            var list = (from u in db.Mst_Shops
                        select new ShopModel
                        {
                            id = u.id,
                            Shop = u.Shop
                        }).ToList();

            return View(list);
        }
        [HttpGet]
        public ActionResult AddShop(int c_id = 0)
        {
            if (c_id != 0)
            {
                var list = (from u in db.Mst_Shops.Where(x => x.id == c_id)
                            select new ShopModel
                            {
                                id = u.id,
                                Shop = u.Shop
                            }).FirstOrDefault();
                return View(list);
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddShop(ShopModel model)
        {
            var updt = db.Mst_Shops.Where(x => x.id == model.id).FirstOrDefault();
            updt.Shop = model.Shop;
            db.SaveChanges();
            return RedirectToAction("ShopMaster", "Admin");
        }
        public ActionResult UsersMaster()
        {
            UserModel model = new UserModel();
            model.Userlist = (from u in db.Mst_Users.Where(x => x.is_deleted == false)
                              join v in db.Mst_Shops on u.shopid equals v.id
                                select new Users
                                {
                                    id = u.id,
                                    username = u.username,
                                    Uname = u.Uname,
                                    password = u.password,
                                    rolename = u.rolename,
                                    Shopname = v.Shop
                                }).ToList();
            
            return View(model);
        }
        [HttpGet]
        public ActionResult AddUser(int c_id = 0)
        {
            ViewBag.shop = new SelectList(db.Mst_Shops.ToList(), "id", "Shop");
            if (c_id != 0)
            {
                UserModel model = new UserModel();
                model.Addusers = (from u in db.Mst_Users.Where(x => x.id == c_id)
                                  join v in db.Mst_Shops on u.shopid equals v.id
                                  select new Users
                                  {
                                      id = u.id,
                                      shopid = (int)u.shopid,
                                      username = u.username,
                                      Uname = u.Uname,
                                      password = u.password,
                                      rolename = u.rolename,
                                      Shopname = v.Shop
                                  }).FirstOrDefault();
                model.Menuslist1 = (from u in db.tbl_Menus.Where(x => x.mid >= 1 && x.mid <= 7)
                                    select new MenusModel
                                    {
                                        mid = u.mid,
                                        menu_name = u.menu_name
                                    }).ToList();
                model.Menuslist2 = (from u in db.tbl_Menus.Where(x => x.mid >= 8 && x.mid <= 11)
                                    select new MenusModel
                                    {
                                        mid = u.mid,
                                        menu_name = u.menu_name
                                    }).ToList();
                model.Menuslist3 = (from u in db.tbl_Menus.Where(x => x.mid >= 12 && x.mid <= 15)
                                    select new MenusModel
                                    {
                                        mid = u.mid,
                                        menu_name = u.menu_name
                                    }).ToList();
                return View(model);
            }
            else
            {
                UserModel model2 = new UserModel();
                model2.Menuslist1 = (from u in db.tbl_Menus.Where(x => x.mid >= 1 && x.mid <= 7)
                                    select new MenusModel
                                    {
                                        mid = u.mid,
                                        menu_name = u.menu_name
                                    }).ToList();
                model2.Menuslist2 = (from u in db.tbl_Menus.Where(x => x.mid >= 8 && x.mid <= 11)
                                    select new MenusModel
                                    {
                                        mid = u.mid,
                                        menu_name = u.menu_name
                                    }).ToList();
                model2.Menuslist3 = (from u in db.tbl_Menus.Where(x => x.mid >= 12 && x.mid <= 15)
                                    select new MenusModel
                                    {
                                        mid = u.mid,
                                        menu_name = u.menu_name
                                    }).ToList();
                return View(model2);
            }
            
        }
        [HttpGet]
        //Get Assign Permission list
        public JsonResult Getassingrole(int id = 0)
        {
            var list = (from u in db.tbl_UserHasMenus.Where(x => x.userid == id)
                        select new MenusModel
                        {
                            mid = (int)u.m_id,
                        }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddUser(UserModel model)
        {
            int[] arr_menuid = model.Addusers.menuid_arr;
            if (model.Addusers.id == 0)
            {
                int maxid = db.Mst_Users.Max(x => (int?)x.id) ?? 0;
                int ids = maxid + 1;
                Mst_Users tbl = new Mst_Users();
                tbl.id = ids;
                tbl.shopid = model.Addusers.shopid;
                tbl.Uname = model.Addusers.Uname;
                tbl.rolename = model.Addusers.rolename;
                tbl.username = model.Addusers.username;
                tbl.password = model.Addusers.password;
                tbl.is_deleted = false;
                db.Mst_Users.Add(tbl);
                db.SaveChanges();

                for (int i=0; i<=arr_menuid.Length - 1; i++)
                {
                    int maxids = db.tbl_UserHasMenus.Max(x => (int?)x.id) ?? 0;
                    int idss = maxids + 1;
                    tbl_UserHasMenus tbl1 = new tbl_UserHasMenus();
                    tbl1.id = idss;
                    tbl1.userid = ids;
                    tbl1.m_id = arr_menuid[i];
                    db.tbl_UserHasMenus.Add(tbl1);
                    db.SaveChanges();
                }
            }
            else
            {
                var updt = db.Mst_Users.Where(x => x.id == model.Addusers.id).FirstOrDefault();
                updt.shopid = model.Addusers.shopid;
                updt.Uname = model.Addusers.Uname;
                updt.rolename = model.Addusers.rolename;
                updt.username = model.Addusers.username;
                updt.password = model.Addusers.password;
                updt.is_deleted = false;
                db.SaveChanges();

                var removerole = db.tbl_UserHasMenus.Where(x => x.userid == model.Addusers.id).ToList();
                if(removerole != null)
                {
                    foreach (var item in removerole)
                    {
                        db.tbl_UserHasMenus.Remove(item);
                        db.SaveChanges();
                    }
                }
                for (int i = 0; i <= arr_menuid.Length - 1; i++)
                {
                    int maxids = db.tbl_UserHasMenus.Max(x => (int?)x.id) ?? 0;
                    int idss = maxids + 1;
                    tbl_UserHasMenus tbl1 = new tbl_UserHasMenus();
                    tbl1.id = idss;
                    tbl1.userid = model.Addusers.id;
                    tbl1.m_id = arr_menuid[i];
                    db.tbl_UserHasMenus.Add(tbl1);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("UsersMaster", "Admin");
        }
        public ActionResult DelUser(int c_id = 0)
        {
            var del = db.Mst_Users.Where(x => x.id == c_id).FirstOrDefault();
            del.is_deleted = true;
            db.SaveChanges();
            return RedirectToAction("UsersMaster", "Admin");
        }
        [HttpPost]
        public ActionResult AddCustomer(SaleEntryModel model)
        {
            int maxid = db.tbl_Customers.Max(x => (int?)x.id) ?? 0;
            int ids = maxid + 1;
            tbl_Customers tbl = new tbl_Customers();
            tbl.id = ids;
            tbl.Name = model.Cust.Name;
            tbl.mobileno = model.Cust.mobileno;
            tbl.whatsapno = model.Cust.whatsapno;
            tbl.Email = model.Cust.Email;
            tbl.DOB = model.Cust.DOB;
            tbl.Gender = model.Cust.Gender;
            tbl.NameNo = model.Cust.Name + " ("+ model.Cust.mobileno +")";
            db.tbl_Customers.Add(tbl);
            db.SaveChanges();
            return RedirectToAction("SaleEntry", "Admin");
        }
        [HttpGet]
        public ActionResult CompanyMaster()
        {
            var list = (from u in db.Mst_Company.Where(x => x.id == 1)
                        select new CompanyModel
                        {
                            id = u.id,
                            CompName = u.CompName,
                            CompAccName = u.CompAccName,
                            CompAccNo = u.CompAccNo,
                            CompAddress = u.CompAddress,
                            CompBankName = u.CompBankName,
                            CompCity = u.CompCity,
                            CompEmail = u.CompEmail,
                            CompGST = u.CompGST,
                            CompIFSC = u.CompIFSC,
                            CompLogo = u.CompLogo,
                            CompPAN = u.CompPAN,
                            CompPhone1 = u.CompPhone1,
                            CompPhone2 = u.CompPhone2,
                            CompPhone3 = u.CompPhone3,
                            CompPhone4 = u.CompPhone4,
                            CompPin = u.CompPin,
                            CompState = u.CompState,
                            CompWebsite = u.CompWebsite,
                            NameOnInvoice = u.NameOnInvoice
                        }).FirstOrDefault();
            return View(list);
        }
        
        [HttpPost]
        public ActionResult CompanyMaster(CompanyModel model, HttpPostedFileBase file)
        {
            var tbl = db.Mst_Company.Where(x => x.id == model.id).FirstOrDefault();
            tbl.CompName = model.CompName;
            tbl.CompAccName = model.CompAccName;
            tbl.CompAccNo = model.CompAccNo;
            tbl.CompAddress = model.CompAddress;
            tbl.CompBankName = model.CompBankName;
            tbl.CompCity = model.CompCity;
            tbl.CompEmail = model.CompEmail;
            tbl.CompGST = model.CompGST;
            tbl.CompIFSC = model.CompIFSC;
            tbl.CompPAN = model.CompPAN;
            tbl.CompPhone1 = model.CompPhone1;
            tbl.CompPhone2 = model.CompPhone2;
            tbl.CompPhone3 = model.CompPhone3;
            tbl.CompPhone4 = model.CompPhone4;
            tbl.CompPin = model.CompPin;
            tbl.CompState = model.CompState;
            tbl.CompWebsite = model.CompWebsite;
            tbl.NameOnInvoice = model.NameOnInvoice;
            if (file != null)
            {
                string ImageName = System.IO.Path.GetFileName(file.FileName);
                string physicalPath = Server.MapPath("~/CompanyImg/" + ImageName);
                file.SaveAs(physicalPath);
                tbl.CompLogo = ImageName;
            }
            if (file == null)
            {
                tbl.CompLogo = model.CompLogo;
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }
        public ActionResult AccountGroup()
        {
            var list = (from u in db.Mst_AccGroup.Where(x => x.is_deleted == false).OrderByDescending(x => x.id)
                        select new AccGroupModel
                        {
                            id = u.id,
                            GroupName = u.GroupName,
                            GroupCode = u.GroupCode,
                            is_deleted = u.is_deleted
                        }).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult AddAccGroup(int c_id = 0)
        {
            if (c_id != 0)
            {
                var list = (from u in db.Mst_AccGroup.Where(x => x.id == c_id)
                            select new AccGroupModel
                            {
                                id = u.id,
                                GroupName = u.GroupName,
                                GroupCode = u.GroupCode,
                                is_deleted = u.is_deleted
                            }).FirstOrDefault();
                return View(list);
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddAccGroup(AccGroupModel model)
        {
            if (model.id == 0)
            {
                int maxid = db.Mst_AccGroup.Max(x => (int?)x.id) ?? 0;
                int ids = maxid + 1;
                Mst_AccGroup tbl = new Mst_AccGroup();
                tbl.id = ids;
                tbl.GroupName = model.GroupName;
                tbl.GroupCode = model.GroupCode;
                tbl.is_deleted = false;
                db.Mst_AccGroup.Add(tbl);
                db.SaveChanges();
            }
            else
            {
                var updt = db.Mst_AccGroup.Where(x => x.id == model.id).FirstOrDefault();
                updt.GroupName = model.GroupName;
                updt.GroupCode = model.GroupCode;
                updt.is_deleted = false;
                db.SaveChanges();
            }
            return RedirectToAction("AccountGroup", "Admin");
        }
        public ActionResult DelAccGroup(int c_id = 0)
        {
            var del = db.Mst_AccGroup.Where(x => x.id == c_id).FirstOrDefault();
            del.is_deleted = true;
            db.SaveChanges();
            return RedirectToAction("AccountGroup", "Admin");
        }
        public ActionResult AccountHead()
        {
            var list = (from u in db.Mst_AccHead.Where(x => x.is_deleted == false).OrderByDescending(x => x.id)
                        join v in db.Mst_AccGroup on u.AccGroupid equals v.id
                        select new AccHeadModel
                        {
                            id = u.id,
                            AccGroupid = u.AccGroupid,
                            AccName = u.AccName,
                            AccCode = u.AccCode,
                            AccAddress = u.AccAddress,
                            AccArea = u.AccArea,
                            AccCity = u.AccCity,
                            AccContactPerson = u.AccContactPerson,
                            AccEmail = u.AccEmail,
                            AccGST = u.AccGST,
                            AccPAN = u.AccPAN,
                            AccPhone = u.AccPhone,
                            AccPincode = u.AccPincode,
                            OpeningAmount = u.OpeningAmount,
                            RegType = u.RegType,
                            AccGroupName = v.GroupName,
                            is_deleted = u.is_deleted
                        }).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult AddAccHead(int c_id = 0)
        {
            ViewBag.accgrp = new SelectList(db.Mst_AccGroup.Where(x=>x.is_deleted == false).ToList(), "id", "GroupName");
            if (c_id != 0)
            {
                var list = (from u in db.Mst_AccHead.Where(x => x.id == c_id)
                            join v in db.Mst_AccGroup on u.AccGroupid equals v.id
                            select new AccHeadModel
                            {
                                id = u.id,
                                AccGroupid = u.AccGroupid,
                                AccName = u.AccName,
                                AccCode = u.AccCode,
                                AccAddress = u.AccAddress,
                                AccArea = u.AccArea,
                                AccCity = u.AccCity,
                                AccContactPerson = u.AccContactPerson,
                                AccEmail = u.AccEmail,
                                AccGST = u.AccGST,
                                AccPAN = u.AccPAN,
                                AccPhone = u.AccPhone,
                                AccPincode = u.AccPincode,
                                OpeningAmount = u.OpeningAmount,
                                RegType = u.RegType,
                                AccGroupName = v.GroupName,
                                is_deleted = u.is_deleted
                            }).FirstOrDefault();
                return View(list);
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddAccHead(AccHeadModel model)
        {
            if (model.id == 0)
            {
                int maxid = db.Mst_AccHead.Max(x => (int?)x.id) ?? 0;
                int ids = maxid + 1;
                Mst_AccHead tbl = new Mst_AccHead();
                tbl.id = ids;
                tbl.AccGroupid = model.AccGroupid;
                tbl.AccName = model.AccName;
                tbl.AccCode = model.AccCode;
                tbl.AccAddress = model.AccAddress;
                tbl.AccArea = model.AccArea;
                tbl.AccCity = model.AccCity;
                tbl.AccContactPerson = model.AccContactPerson;
                tbl.AccEmail = model.AccEmail;
                tbl.AccGST = model.AccGST;
                tbl.AccPAN = model.AccPAN;
                tbl.AccPhone = model.AccPhone;
                tbl.AccPincode = model.AccPincode;
                tbl.OpeningAmount = model.OpeningAmount;
                tbl.RegType = model.RegType;
                tbl.is_deleted = false;
                db.Mst_AccHead.Add(tbl);
                db.SaveChanges();
            }
            else
            {
                var updt = db.Mst_AccHead.Where(x => x.id == model.id).FirstOrDefault();
                updt.AccGroupid = model.AccGroupid;
                updt.AccName = model.AccName;
                updt.AccCode = model.AccCode;
                updt.AccAddress = model.AccAddress;
                updt.AccArea = model.AccArea;
                updt.AccCity = model.AccCity;
                updt.AccContactPerson = model.AccContactPerson;
                updt.AccEmail = model.AccEmail;
                updt.AccGST = model.AccGST;
                updt.AccPAN = model.AccPAN;
                updt.AccPhone = model.AccPhone;
                updt.AccPincode = model.AccPincode;
                updt.OpeningAmount = model.OpeningAmount;
                updt.RegType = model.RegType;
                updt.is_deleted = false;
                db.SaveChanges();
            }
            return RedirectToAction("AccountHead", "Admin");
        }
        public ActionResult DelAccHead(int c_id = 0)
        {
            var del = db.Mst_AccHead.Where(x => x.id == c_id).FirstOrDefault();
            del.is_deleted = true;
            db.SaveChanges();
            return RedirectToAction("AccountHead", "Admin");
        }
        public ActionResult ItemGroup()
        {
            var list = (from u in db.Mst_ItemGroup.Where(x => x.is_deleted == false).OrderByDescending(x => x.id)
                        select new ItemGroupModel
                        {
                            id = u.id,
                            ItemGroupName = u.ItemGroupName,
                            ItemGroupCode = u.ItemGroupCode,
                            is_deleted = u.is_deleted
                        }).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult AddItemGroup(int c_id = 0)
        {
            ViewBag.itmgrp = new SelectList(db.Mst_AccGroup.ToList(), "id", "ItemGroupName");
            if (c_id != 0)
            {
                var list = (from u in db.Mst_ItemGroup.Where(x => x.id == c_id)
                            select new ItemGroupModel
                            {
                                id = u.id,
                                ItemGroupName = u.ItemGroupName,
                                ItemGroupCode = u.ItemGroupCode,
                                is_deleted = u.is_deleted
                            }).FirstOrDefault();
                return View(list);
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddItemGroup(ItemGroupModel model)
        {
            if (model.id == 0)
            {
                int maxid = db.Mst_ItemGroup.Max(x => (int?)x.id) ?? 0;
                int ids = maxid + 1;
                Mst_ItemGroup tbl = new Mst_ItemGroup();
                tbl.id = ids;
                tbl.ItemGroupName = model.ItemGroupName;
                tbl.ItemGroupCode = model.ItemGroupCode;
                tbl.is_deleted = false;
                db.Mst_ItemGroup.Add(tbl);
                db.SaveChanges();
            }
            else
            {
                var updt = db.Mst_ItemGroup.Where(x => x.id == model.id).FirstOrDefault();
                updt.ItemGroupName = model.ItemGroupName;
                updt.ItemGroupCode = model.ItemGroupCode;
                updt.is_deleted = false;
                db.SaveChanges();
            }
            return RedirectToAction("ItemGroup", "Admin");
        }
        public ActionResult DelItemGroup(int c_id = 0)
        {
            var del = db.Mst_ItemGroup.Where(x => x.id == c_id).FirstOrDefault();
            del.is_deleted = true;
            db.SaveChanges();
            return RedirectToAction("ItemGroup", "Admin");
        }
        public ActionResult ItemHead()
        {
            var list = (from u in db.Mst_ItemHead.Where(x => x.is_deleted == false).OrderByDescending(x => x.id)
                        select new ItemHeadModel
                        {
                            id = u.id,
                            itemName = u.itemName,
                            ItemGroup = u.ItemGroup,
                            itemCGST = u.itemCGST,
                            itemCode = u.itemCode,
                            itemIGST = u.itemIGST,
                            itemMRP = u.itemMRP,
                            itemOpenQty = u.itemOpenQty,
                            itemOpenValue = u.itemOpenValue,
                            itemPurchaseTax = u.itemPurchaseTax,
                            itemSaleAs = u.itemSaleAs,
                            itemPurchaseRate = u.itemPurchaseRate,
                            itemSaleRate = u.itemSaleRate,
                            itemSaleTax = u.itemSaleTax,
                            itemSGST = u.itemSGST,
                            ProductCode = u.ProductCode,
                            itemHSN_SAC = u.itemHSN_SAC,
                            itemAvlQty = u.itemAvlQty,
                            itemMeasureUnit = u.itemMeasureUnit,
                            is_deleted = u.is_deleted
                        }).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult AddItemHead(int c_id = 0)
        {
            ViewBag.itmgrp = new SelectList(db.Mst_ItemGroup.ToList(), "id", "ItemGroupName");
            if (c_id != 0)
            {
                var list = (from u in db.Mst_ItemHead.Where(x => x.id == c_id)
                            select new ItemHeadModel
                            {
                                id = u.id,
                                itemName = u.itemName,
                                ItemGroup = u.ItemGroup,
                                itemCGST = u.itemCGST,
                                itemCode = u.itemCode,
                                itemIGST = u.itemIGST,
                                itemMRP = u.itemMRP,
                                itemOpenQty = u.itemOpenQty,
                                itemOpenValue = u.itemOpenValue,
                                itemPurchaseTax = u.itemPurchaseTax,
                                itemSaleAs = u.itemSaleAs,
                                itemPurchaseRate = u.itemPurchaseRate,
                                itemSaleRate = u.itemSaleRate,
                                itemSaleTax = u.itemSaleTax,
                                itemSGST = u.itemSGST,
                                ProductCode = u.ProductCode,
                                itemHSN_SAC = u.itemHSN_SAC,
                                itemAvlQty = u.itemAvlQty,
                                itemMeasureUnit = u.itemMeasureUnit,
                                itemBarcode = u.itemBarcode,
                                is_deleted = u.is_deleted
                            }).FirstOrDefault();
                return View(list);
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddItemHead(ItemHeadModel model)
        {
            if (model.id == 0)
            {
                int maxid = db.Mst_ItemHead.Max(x => (int?)x.id) ?? 0;
                int ids = maxid + 1;
                Mst_ItemHead tbl = new Mst_ItemHead();
                tbl.id = ids;
                tbl.itemName = model.itemName;
                tbl.ItemGroup = model.ItemGroup;
                tbl.itemCGST = model.itemCGST;
                tbl.itemCode = "ITEM000" + ids;
                tbl.itemIGST = model.itemIGST;
                tbl.itemMRP = model.itemMRP;
                tbl.itemMeasureUnit = model.itemMeasureUnit;
                tbl.itemAvlQty = model.itemOpenQty;
                tbl.itemOpenQty = model.itemOpenQty;
                tbl.itemOpenValue = model.itemOpenValue;
                tbl.itemPurchaseTax = true;
                tbl.itemSaleAs = model.itemSaleAs;
                tbl.itemPurchaseRate = model.itemPurchaseRate;
                tbl.itemSaleRate = model.itemSaleRate;
                tbl.itemSaleTax = true;
                tbl.itemSGST = model.itemSGST;
                tbl.ProductCode = model.ProductCode;
                tbl.itemHSN_SAC = model.itemHSN_SAC;
                tbl.itemBarcode = model.itemBarcode;
                tbl.itembarcodeimg = getBarcodeImage(model.itemBarcode, ids, (decimal)model.itemMRP, model.ProductCode);
                //if (model.itemBarcode != null)
                //{
                //}
                //else
                //{
                //    tbl.itemBarcode = generateBarcode();
                //    string v = tbl.itemBarcode;
                //    tbl.itembarcodeimg = getBarcodeImage(v, ids, (decimal)model.itemMRP, model.ProductCode);
                //}
                tbl.is_deleted = false;
                db.Mst_ItemHead.Add(tbl);
                db.SaveChanges();
            }
            else
            {
                var updt = db.Mst_ItemHead.Where(x => x.id == model.id).FirstOrDefault();
                updt.itemName = model.itemName;
                updt.ItemGroup = model.ItemGroup;
                updt.itemCGST = model.itemCGST;
                updt.itemCode = model.itemCode;
                updt.itemIGST = model.itemIGST;
                updt.itemMRP = model.itemMRP;
                updt.itemMeasureUnit = model.itemMeasureUnit;
                updt.itemAvlQty = model.itemOpenQty;
                updt.itemOpenQty = model.itemOpenQty;
                updt.itemOpenValue = model.itemOpenValue;
                updt.itemPurchaseTax = true;
                updt.itemSaleAs = model.itemSaleAs;
                updt.itemSaleRate = model.itemSaleRate;
                updt.itemPurchaseRate = model.itemPurchaseRate;
                updt.itemSaleTax = true;
                updt.itemSGST = model.itemSGST;
                updt.ProductCode = model.ProductCode;
                updt.itemHSN_SAC = model.itemHSN_SAC;
                updt.itemBarcode = model.itemBarcode;
                updt.itembarcodeimg = getBarcodeImage(model.itemBarcode, model.id, (decimal)model.itemMRP, model.ProductCode);
                updt.is_deleted = false;
                db.SaveChanges();
            }
            return RedirectToAction("ItemHead", "Admin");
        }
        public ActionResult DelItemHead(int c_id = 0)
        {
            var del = db.Mst_ItemHead.Where(x => x.id == c_id).FirstOrDefault();
            del.is_deleted = true;
            db.SaveChanges();
            return RedirectToAction("ItemHead", "Admin");
        }
        public string generateBarcode()
        {
            try
            {
                string[] charPool = "1-2-3-4-5-6-7-8-9-0".Split('-');
                StringBuilder rs = new StringBuilder();
                int length = 10;
                Random rnd = new Random();
                while (length-- > 0)
                {
                    int index = (int)(rnd.NextDouble() * charPool.Length);
                    if (charPool[index] != "-")
                    {
                        rs.Append(charPool[index]);
                        charPool[index] = "-";
                    }
                    else
                        length++;
                }
                return rs.ToString();
            }
            catch (Exception ex)
            {
                //ErrorLog.WriteErrorLog("Barcode", ex.ToString(), ex.Message);
            }
            return "";
        }

        public string getBarcodeImage(string barcode, int mxid, decimal mrp, string prodcode)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Bitmap bitMap = new Bitmap(barcode.Length * 50, 100))
                {
                    using (Graphics graphics = Graphics.FromImage(bitMap))
                    {
                        Font oFont = new Font("IDAutomationHC39M", 14);
                        PointF point = new PointF(10f, 60f);
                        PointF point4 = new PointF(6f, 32f);
                        PointF point1 = new PointF(20f, 3f);
                        PointF point2 = new PointF(135f, 10f);
                        SolidBrush whiteBrush = new SolidBrush(Color.White);
                        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                        SolidBrush blackBrush = new SolidBrush(Color.Black);
                        graphics.DrawString("Preora", new Font("Arial", 11, FontStyle.Bold), blackBrush, point1);
                        graphics.DrawString(prodcode, new Font("Arial", 10, FontStyle.Bold), blackBrush, point4);
                        graphics.DrawString("M.R.P.- " + mrp, new Font("Arial", 11, FontStyle.Bold), blackBrush, point);
                        graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point2);
                    }
                    string imgnam = "Barcode000" + mxid + ".jpeg";
                    bitMap.Save(Server.MapPath("~/BarcodeImages") + "/" + imgnam, ImageFormat.Jpeg);
                    return imgnam;
                }
            }
        }

        public ActionResult BarcodeList()
        {
            var list = new BarcodeModel();
            {
                list.ItemHeadList = (from u in db.Mst_ItemHead.Where(x => x.is_deleted == false).OrderByDescending(x => x.id)
                                     select new ItemHeadModel
                                     {
                                         id = u.id,
                                         itemName = u.itemName,
                                         ItemGroup = u.ItemGroup,
                                         itemCode = u.itemCode,
                                         itemMRP = u.itemMRP,
                                         ProductCode = u.ProductCode,
                                         itemBarcode = u.itemBarcode,
                                         itembarcodeimg = u.itembarcodeimg,
                                         is_deleted = u.is_deleted,
                                     }).ToList();
            }
            return View(list);
        }
        public ActionResult BarcodePartial(int pid = 0)
        {
            var list = (from u in db.Mst_ItemHead.Where(x => x.id == pid)
                        select new ItemHeadModel
                        {
                            id = u.id,
                            itemName = u.itemName,
                            itemBarcode = u.itemBarcode,
                            itembarcodeimg = u.itembarcodeimg
                        }).FirstOrDefault();
            return View(list);
        }

        [HttpGet]
        public JsonResult GetBarcode(int pid = 0)
        {
            var data = (from u in db.Mst_ItemHead.Where(x => x.id == pid)
                        select new ItemHeadModel()
                        {
                            itembarcodeimg = u.itembarcodeimg
                        }).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PurchaseEntryList()
        {
            var list = (from u in db.tbl_PurchaseBill.OrderByDescending(x => x.id)
                        join v in db.Mst_AccHead on u.supllierid equals v.id
                        select new PurchaseEntryModel
                        {
                            id = u.id,
                            p_entrycode = u.p_entrycode,
                            supllierid = u.supllierid,
                            billdate = u.billdate,
                            duedate = u.duedate,
                            invoice_no = u.invoice_no,
                            GrossAmt = u.GrossAmt,
                            Discount = u.Discount,
                            AdditionalDisc = u.AdditionalDisc,
                            S_GST = u.S_GST,
                            C_GST = u.C_GST,
                            I_GST = u.I_GST,
                            LoadingCharge = u.LoadingCharge,
                            FinalAmt = u.FinalAmt,
                            PaidAmt = u.PaidAmt,
                            BalanceAmt = u.BalanceAmt,
                            pay_mode = u.pay_mode,
                            supp_name = v.AccName
                        }).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult PurchaseEntryDetail(int c_id = 0)
        {
            var list = db.Mst_Company.Where(x => x.id == 1).FirstOrDefault();
            var model = new PurchaseVM();
            {
                model.BillVM = (from u in db.tbl_PurchaseBill.Where(x=>x.id == c_id)
                                    join v in db.Mst_AccHead on u.supllierid equals v.id
                                    select new PurchaseEntryModel
                                    {
                                        id = u.id,
                                        p_entrycode = u.p_entrycode,
                                        supllierid = u.supllierid,
                                        billdate = u.billdate,
                                        duedate = u.duedate,
                                        invoice_no = u.invoice_no,
                                        GrossAmt = u.GrossAmt,
                                        Discount = u.Discount,
                                        AdditionalDisc = u.AdditionalDisc,
                                        S_GST = u.S_GST,
                                        C_GST = u.C_GST,
                                        I_GST = u.I_GST,
                                        LoadingCharge = u.LoadingCharge,
                                        FinalAmt = u.FinalAmt,
                                        PaidAmt = u.PaidAmt,
                                        BalanceAmt = u.BalanceAmt,
                                        pay_mode = u.pay_mode,
                                        supp_name = v.AccName
                                    }).ToList();
                model.productVM = (from u in db.tbl_PurchaseBill_Products.Where(x => x.purchasebill_id == c_id)
                                      join v in db.Mst_ItemHead on u.itemhead_id equals v.id
                                      select new PurchaseEntryModel
                                      {
                                          id = u.id,
                                          purchasebill_id = u.purchasebill_id,
                                          itemhead_id = u.itemhead_id,
                                          item_rate = u.item_rate,
                                          item_qty = u.item_qty,
                                          gross_amt = u.gross_amt,
                                          disc_amt = u.disc_amt,
                                          sgst_amt = u.sgst_amt,
                                          cgst_amt = u.cgst_amt,
                                          igst_amt = u.igst_amt,
                                          HSN_SAC = u.HSN_SAC,
                                          finalprod_amt = u.finalprod_amt,
                                          prod_name = v.itemName,
                                          measureunit = v.itemMeasureUnit
                                      }).ToList();
                model.CompName = list.CompName;
                model.CompPhone1 = list.CompPhone1;
                model.CompEmail = list.CompEmail;
                model.CompLogo = list.CompLogo;
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult PurchaseEntry()
        {
            ViewBag.AccHead = new SelectList(db.Mst_AccHead.ToList(), "id", "AccName");
            ViewBag.ItemHead = new SelectList(db.Mst_ItemHead.Where(x => x.is_deleted == false).ToList(), "id", "itemName");
            return View();
        }
        [HttpGet]
        public JsonResult Getdetail(int pid = 0)
        {
            var data = (from u in db.Mst_ItemHead.Where(x => x.id == pid)
                        select new ItemHeadModel()
                        {
                            id = u.id,
                            itemPurchaseRate = u.itemPurchaseRate,
                            itemSaleRate = u.itemSaleRate,
                            itemPurchaseTax = u.itemPurchaseTax,
                            itemSaleTax = u.itemSaleTax,
                            itemCGST = u.itemCGST,
                            itemSGST = u.itemSGST,
                            itemIGST = u.itemIGST,
                            itemHSN_SAC = u.itemHSN_SAC,
                            itemMeasureUnit = u.itemMeasureUnit,
                            itemMRP = u.itemMRP
                        }).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
            
        }
        [HttpPost]
        public ActionResult PurchaseEntry(PurchaseEntryModel model)
        {
            int[] arr_pro_id = model.itemhead_id_arr;
            long[] arr_pro_qty = model.item_qty_arr;
            decimal[] arr_price = model.item_rate_arr;
            decimal[] arr_totalprice = model.gross_amt_arr;
            decimal[] arr_disc = model.disc_amt_arr;
            decimal[] arr_sgst = model.sgst_amt_arr;
            decimal[] arr_cgst = model.cgst_amt_arr;
            decimal[] arr_igst = model.igst_amt_arr;
            string[] arr_hsn = model.HSN_SAC_arr;
            decimal[] arr_finamnt = model.finalprod_amt_arr;

            int maxid = db.tbl_PurchaseBill.Max(x => (int?)x.id) ?? 0;
            var billnum = "PBILL000" + (maxid + 1);
            tbl_PurchaseBill tbl1 = new tbl_PurchaseBill();
            tbl1.id = maxid + 1;
            tbl1.p_entrycode = billnum;
            tbl1.supllierid = model.supllierid;
            tbl1.invoice_no = model.invoice_no;
            tbl1.billdate = model.billdate;
            tbl1.duedate = model.duedate;
            tbl1.GrossAmt = model.GrossAmt;
            tbl1.Discount = model.Discount;
            tbl1.AdditionalDisc = model.AdditionalDisc;
            tbl1.S_GST = model.S_GST;
            tbl1.C_GST = model.C_GST;
            tbl1.I_GST = model.I_GST;
            tbl1.LoadingCharge = model.LoadingCharge;
            tbl1.FinalAmt = model.FinalAmt;
            tbl1.PaidAmt = model.PaidAmt;
            tbl1.BalanceAmt = model.BalanceAmt;
            tbl1.pay_mode = model.pay_mode;
            db.tbl_PurchaseBill.Add(tbl1);
            db.SaveChanges();

            for (int i = 0; i <= arr_pro_id.Length - 1; i++)
            {
                int maxprid = db.tbl_PurchaseBill_Products.Max(x => (int?)x.id) ?? 0;
                tbl_PurchaseBill_Products tbl = new tbl_PurchaseBill_Products();
                tbl.id = maxprid + 1;
                tbl.purchasebill_id = maxid + 1;
                tbl.itemhead_id = arr_pro_id[i];
                tbl.item_qty = arr_pro_qty[i];
                tbl.item_rate = arr_price[i];
                tbl.gross_amt = arr_totalprice[i];
                tbl.disc_amt = arr_disc[i];
                tbl.sgst_amt = arr_sgst[i];
                tbl.cgst_amt = arr_cgst[i];
                tbl.igst_amt = arr_igst[i];
                tbl.HSN_SAC = arr_hsn[i];
                tbl.finalprod_amt = arr_finamnt[i];
                db.tbl_PurchaseBill_Products.Add(tbl);
                db.SaveChanges();

                int pid = arr_pro_id[i];
                var updt = db.Mst_ItemHead.Where(x => x.id == pid).FirstOrDefault();
                int totqty = (int)updt.itemAvlQty;
                int newqty = (int)(totqty + arr_pro_qty[i]);
                updt.itemAvlQty = newqty;
                db.SaveChanges();
            }
            return RedirectToAction("PurchaseEntryList", "Admin");
        }

        public ActionResult SaleEntryList()
        {
            string role = Convert.ToString(Session["role"]);
            if (role == "SuperAdmin")
            {
                var list = (from u in db.tbl_SaleBill.OrderByDescending(x => x.id)
                            join z in db.Mst_Users on u.userid equals z.id
                            select new SaleEntryModel
                            {
                                id = u.id,
                                s_entrycode = u.s_entrycode,
                                custname = u.custname,
                                billdate_s = u.billdate_s,
                                custphone = u.custphone,
                                invoiceno = u.invoiceno,
                                GrossAmt_s = u.GrossAmt_s,
                                Discount_s = u.Discount_s,
                                AdditionalDisc_s = u.AdditionalDisc_s,
                                SGST_s = u.SGST_s,
                                CGST_s = u.CGST_s,
                                FinalAmt_s = u.FinalAmt_s,
                                PaidAmt_s = u.PaidAmt_s,
                                BalanceAmt_s = u.BalanceAmt_s,
                                paymode = u.paymode,
                                Usrname = z.Uname
                            }).ToList();
                return View(list);
            }
            else
            {
                int usrid = Convert.ToInt32(Session["uid"]);
                var list = (from u in db.tbl_SaleBill.Where(x => x.userid == usrid).OrderByDescending(x => x.id)
                            join z in db.Mst_Users on u.userid equals z.id
                            select new SaleEntryModel
                            {
                                id = u.id,
                                s_entrycode = u.s_entrycode,
                                custname = u.custname,
                                billdate_s = u.billdate_s,
                                custphone = u.custphone,
                                invoiceno = u.invoiceno,
                                GrossAmt_s = u.GrossAmt_s,
                                Discount_s = u.Discount_s,
                                AdditionalDisc_s = u.AdditionalDisc_s,
                                SGST_s = u.SGST_s,
                                CGST_s = u.CGST_s,
                                FinalAmt_s = u.FinalAmt_s,
                                PaidAmt_s = u.PaidAmt_s,
                                BalanceAmt_s = u.BalanceAmt_s,
                                paymode = u.paymode,
                                Usrname = z.Uname
                            }).ToList();
                return View(list);
            }

        }
        [HttpGet]
        public ActionResult SaleEntryDetail(int c_id = 0)
        {
            var list = db.Mst_Company.Where(x => x.id == 1).FirstOrDefault();
            var model = new SaleVM();
            {
                model.BillVM = (from u in db.tbl_SaleBill.Where(x=>x.id == c_id)
                                join z in db.Mst_Users on u.userid equals z.id
                                select new SaleEntryModel
                                {
                                    id = u.id,
                                    s_entrycode = u.s_entrycode,
                                    custname = u.custname,
                                    billdate_s = u.billdate_s,
                                    custphone = u.custphone,
                                    invoiceno = u.invoiceno,
                                    GrossAmt_s = u.GrossAmt_s,
                                    Discount_s = u.Discount_s,
                                    AdditionalDisc_s = u.AdditionalDisc_s,
                                    SGST_s = u.SGST_s,
                                    CGST_s = u.CGST_s,
                                    FinalAmt_s = u.FinalAmt_s,
                                    PaidAmt_s = u.PaidAmt_s,
                                    BalanceAmt_s = u.BalanceAmt_s,
                                    paymode = u.paymode,
                                    Usrname = z.Uname
                                }).ToList();
                model.productVM = (from u in db.tbl_SaleBill_Products.Where(x => x.salebillid == c_id)
                                   join v in db.Mst_ItemHead on u.itemheadid_s equals v.id
                                   select new SaleEntryModel
                                   {
                                       id = u.id,
                                       salebillid = u.salebillid,
                                       itemheadid_s = u.itemheadid_s,
                                       itemrate_s = u.itemrate_s,
                                       itemqty_s = u.itemqty_s,
                                       gross_amt_s = u.gross_amt_s,
                                       discamount_s = u.discamount_s,
                                       sgstamt_s = u.sgstamt_s,
                                       cgstamt_s = u.cgstamt_s,
                                       igstamt_s = u.igstamt_s,
                                       HSNSAC_s = u.HSNSAC_s,
                                       finalprice_s = u.finalprice_s,
                                       mrpprice = v.itemMRP,
                                       prod_name = v.itemName,
                                       measurunit = v.itemMeasureUnit
                                   }).ToList();
                model.CompName = list.CompName;
                model.CompPhone1 = list.CompPhone1;
                model.CompEmail = list.CompEmail;
                model.CompAdd = list.CompAddress;
                model.CompCity = list.CompCity;
                model.CompPIN = list.CompPin;
                model.CompState = list.CompState;
                model.CompGST = list.CompGST;
                model.CompLogo = list.CompLogo;
                model.CompBank = list.CompBankName;
                model.CompAccName = list.CompAccName;
                model.CompIFSC = list.CompIFSC;
                model.CompAccNo = list.CompAccNo;
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult SaleEntry()
        {
            ViewBag.AccHeads = new SelectList(db.tbl_Customers.ToList(), "id", "NameNo");
            ViewBag.ItemHeads = new SelectList(db.Mst_ItemHead.Where(x => x.is_deleted == false).ToList(), "id", "itemBarcode");
            return View();
        }
        
        [HttpPost]
        public ActionResult SaleEntry(SaleEntryModel model)
        {
            int usrid = Convert.ToInt32(Session["uid"]);
            int[] arr_pro_id = model.S_itemhead_id_arr;
            long[] arr_pro_qty = model.S_item_qty_arr;
            decimal[] arr_price = model.S_item_rate_arr;
            decimal[] arr_mrp = model.S_item_mrp_arr;
            //decimal[] arr_totalprice = model.S_gross_amt_arr;
            decimal[] arr_disc = model.S_disc_amt_arr;
            decimal[] arr_sgst = model.S_sgst_amt_arr;
            decimal[] arr_cgst = model.S_cgst_amt_arr;
            //decimal[] arr_igst = model.S_igst_amt_arr;
            string[] arr_hsn = model.S_HSN_SAC_arr;
            decimal[] arr_finamnt = model.S_finalprod_amt_arr;

            int maxid = db.tbl_SaleBill.Max(x => (int?)x.id) ?? 0;
            var billnum = "SBILL000" + (maxid + 1);
            tbl_SaleBill tbl1 = new tbl_SaleBill();
            tbl1.id = maxid + 1;
            tbl1.userid = usrid;
            tbl1.s_entrycode = billnum;
            tbl1.custname = model.custname;
            tbl1.billdate_s = Convert.ToDateTime(DateTime.Now);
            tbl1.custphone = model.custphone;
            tbl1.invoiceno = model.invoiceno;
            tbl1.GrossAmt_s = model.GrossAmt_s;
            tbl1.Discount_s = model.Discount_s;
            tbl1.AdditionalDisc_s = model.AdditionalDisc_s;
            tbl1.SGST_s = model.SGST_s;
            tbl1.CGST_s = model.CGST_s;
            tbl1.FinalAmt_s = model.FinalAmt_s;
            tbl1.PaidAmt_s = model.PaidAmt_s;
            tbl1.BalanceAmt_s = model.BalanceAmt_s;
            tbl1.paymode = model.paymode;
            db.tbl_SaleBill.Add(tbl1);
            db.SaveChanges();

            for (int i = 0; i <= arr_pro_id.Length - 1; i++)
            {
                int maxprid = db.tbl_SaleBill_Products.Max(x => (int?)x.id) ?? 0;
                tbl_SaleBill_Products tbl = new tbl_SaleBill_Products();
                tbl.id = maxprid + 1;
                tbl.salebillid = maxid + 1;
                tbl.itemheadid_s = arr_pro_id[i];
                tbl.itemqty_s = arr_pro_qty[i];
                tbl.itemrate_s = arr_mrp[i];
                //tbl.gross_amt_s = arr_totalprice[i];
                tbl.discamount_s = arr_disc[i];
                tbl.sgstamt_s = arr_sgst[i];
                tbl.cgstamt_s = arr_cgst[i];
                //tbl.igstamt_s = arr_igst[i];
                tbl.HSNSAC_s = arr_hsn[i];
                tbl.finalprice_s = arr_finamnt[i];
                db.tbl_SaleBill_Products.Add(tbl);
                db.SaveChanges();

                int pid = arr_pro_id[i];
                var updt = db.Mst_ItemHead.Where(x => x.id == pid).FirstOrDefault();
                int totqty = (int)updt.itemAvlQty;
                int newqty = (int)(totqty - arr_pro_qty[i]);
                updt.itemAvlQty = newqty;
                db.SaveChanges();
            }
            return RedirectToAction("SaleEntryDetail", "Admin", new { c_id = maxid + 1 });
        }
        public ActionResult PurchaseReturnList()
        {
            var list = (from u in db.tbl_PurchaseReturnBill.OrderByDescending(x => x.id)
                        join v in db.Mst_AccHead on u.PR_suppid equals v.id
                        select new PurchaseReturnModel
                        {
                            id = u.id,
                            PR_code = u.PR_code,
                            PR_suppid = u.PR_suppid,
                            PR_billdate = u.PR_billdate,
                            PR_duedate = u.PR_duedate,
                            PR_invoiceno = u.PR_invoiceno,
                            PR_GrossAmt = u.PR_GrossAmt,
                            PR_Discount = u.PR_Discount,
                            PR_AdditionalDisc = u.PR_AdditionalDisc,
                            PR_S_GST = u.PR_S_GST,
                            PR_C_GST = u.PR_C_GST,
                            PR_I_GST = u.PR_I_GST,
                            PR_LoadingCharge = u.PR_LoadingCharge,
                            PR_FinalAmt = u.PR_FinalAmt,
                            PR_PaidAmt = u.PR_PaidAmt,
                            PR_BalanceAmt = u.PR_BalanceAmt,
                            PR_pay_mode = u.PR_pay_mode,
                            PR_suppName = v.AccName
                        }).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult PurchaseReturnDetail(int c_id = 0)
        {
            var list = db.Mst_Company.Where(x => x.id == 1).FirstOrDefault();
            var model = new PurchaseReturnVM();
            {
                model.BillVM = (from u in db.tbl_PurchaseReturnBill.Where(x => x.id == c_id)
                                join v in db.Mst_AccHead on u.PR_suppid equals v.id
                                select new PurchaseReturnModel
                                {
                                    id = u.id,
                                    PR_code = u.PR_code,
                                    PR_suppid = u.PR_suppid,
                                    PR_billdate = u.PR_billdate,
                                    PR_duedate = u.PR_duedate,
                                    PR_invoiceno = u.PR_invoiceno,
                                    PR_GrossAmt = u.PR_GrossAmt,
                                    PR_Discount = u.PR_Discount,
                                    PR_AdditionalDisc = u.PR_AdditionalDisc,
                                    PR_S_GST = u.PR_S_GST,
                                    PR_C_GST = u.PR_C_GST,
                                    PR_I_GST = u.PR_I_GST,
                                    PR_LoadingCharge = u.PR_LoadingCharge,
                                    PR_FinalAmt = u.PR_FinalAmt,
                                    PR_PaidAmt = u.PR_PaidAmt,
                                    PR_BalanceAmt = u.PR_BalanceAmt,
                                    PR_pay_mode = u.PR_pay_mode,
                                    PR_suppName = v.AccName
                                }).ToList();
                model.productVM = (from u in db.tbl_PRBill_Products.Where(x => x.PR_purchasebill_id == c_id)
                                   join v in db.Mst_ItemHead on u.PR_itemhead_id equals v.id
                                   select new PurchaseReturnModel
                                   {
                                       id = u.id,
                                       PR_purchasebill_id = u.PR_purchasebill_id,
                                       PR_itemhead_id = u.PR_itemhead_id,
                                       PR_item_rate = u.PR_item_rate,
                                       PR_item_qty = u.PR_item_qty,
                                       PR_gross_amt = u.PR_gross_amt,
                                       PR_disc_amt = u.PR_disc_amt,
                                       PR_sgst_amt = u.PR_sgst_amt,
                                       PR_cgst_amt = u.PR_cgst_amt,
                                       PR_igst_amt = u.PR_igst_amt,
                                       PR_HSN_SAC = u.PR_HSN_SAC,
                                       PR_finalprod_amt = u.PR_finalprod_amt,
                                       PR_prod_name = v.itemName,
                                       PR_measureunit = v.itemMeasureUnit
                                   }).ToList();
                model.CompName = list.CompName;
                model.CompPhone1 = list.CompPhone1;
                model.CompEmail = list.CompEmail;
                model.CompLogo = list.CompLogo;
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult PurchaseReturn()
        {
            ViewBag.AccHeadPR = new SelectList(db.Mst_AccHead.ToList(), "id", "AccName");
            ViewBag.ItemHeadPR = new SelectList(db.Mst_ItemHead.Where(x => x.is_deleted == false).ToList(), "id", "itemName");
            return View();
        }
       
        [HttpPost]
        public ActionResult PurchaseReturn(PurchaseReturnModel model)
        {
            int[] arr_pro_id = model.PR_itemhead_id_arr;
            long[] arr_pro_qty = model.PR_item_qty_arr;
            decimal[] arr_price = model.PR_item_rate_arr;
            decimal[] arr_totalprice = model.PR_gross_amt_arr;
            decimal[] arr_disc = model.PR_disc_amt_arr;
            decimal[] arr_sgst = model.PR_sgst_amt_arr;
            decimal[] arr_cgst = model.PR_cgst_amt_arr;
            decimal[] arr_igst = model.PR_igst_amt_arr;
            string[] arr_hsn = model.PR_HSN_SAC_arr;
            decimal[] arr_finamnt = model.PR_finalprod_amt_arr;

            int maxid = db.tbl_PurchaseReturnBill.Max(x => (int?)x.id) ?? 0;
            var billnum = "PRBILL000" + (maxid + 1);
            tbl_PurchaseReturnBill tbl1 = new tbl_PurchaseReturnBill();
            tbl1.id = maxid + 1;
            tbl1.PR_code = billnum;
            tbl1.PR_suppid = model.PR_suppid;
            tbl1.PR_invoiceno = model.PR_invoiceno;
            tbl1.PR_billdate = model.PR_billdate;
            tbl1.PR_duedate = model.PR_duedate;
            tbl1.PR_GrossAmt = model.PR_GrossAmt;
            tbl1.PR_Discount = model.PR_Discount;
            tbl1.PR_AdditionalDisc = model.PR_AdditionalDisc;
            tbl1.PR_S_GST = model.PR_S_GST;
            tbl1.PR_C_GST = model.PR_C_GST;
            tbl1.PR_I_GST = model.PR_I_GST;
            tbl1.PR_LoadingCharge = model.PR_LoadingCharge;
            tbl1.PR_FinalAmt = model.PR_FinalAmt;
            tbl1.PR_PaidAmt = model.PR_PaidAmt;
            tbl1.PR_BalanceAmt = model.PR_BalanceAmt;
            tbl1.PR_pay_mode = model.PR_pay_mode;
            db.tbl_PurchaseReturnBill.Add(tbl1);
            db.SaveChanges();

            for (int i = 0; i <= arr_pro_id.Length - 1; i++)
            {
                int maxprid = db.tbl_PRBill_Products.Max(x => (int?)x.id) ?? 0;
                tbl_PRBill_Products tbl = new tbl_PRBill_Products();
                tbl.id = maxprid + 1;
                tbl.PR_purchasebill_id = maxid + 1;
                tbl.PR_itemhead_id = arr_pro_id[i];
                tbl.PR_item_qty = arr_pro_qty[i];
                tbl.PR_item_rate = arr_price[i];
                tbl.PR_gross_amt = arr_totalprice[i];
                tbl.PR_disc_amt = arr_disc[i];
                tbl.PR_sgst_amt = arr_sgst[i];
                tbl.PR_cgst_amt = arr_cgst[i];
                tbl.PR_igst_amt = arr_igst[i];
                tbl.PR_HSN_SAC = arr_hsn[i];
                tbl.PR_finalprod_amt = arr_finamnt[i];
                db.tbl_PRBill_Products.Add(tbl);
                db.SaveChanges();

                int pid = arr_pro_id[i];
                var updt = db.Mst_ItemHead.Where(x => x.id == pid).FirstOrDefault();
                int totqty = (int)updt.itemAvlQty;
                int newqty = (int)(totqty - arr_pro_qty[i]);
                updt.itemAvlQty = newqty;
                db.SaveChanges();
            }
            return RedirectToAction("PurchaseReturnList", "Admin");
        }

        public ActionResult SaleReturnList()
        {
            string role = Convert.ToString(Session["role"]);
            if (role == "SuperAdmin")
            {
                var list = (from u in db.tbl_SaleReturnBill.OrderByDescending(x => x.id)
                            join z in db.Mst_Users on u.userid equals z.id
                            select new SaleReturnModel
                            {
                                id = u.id,
                                SR_entrycode = u.SR_entrycode,
                                SR_custname = u.SR_custname,
                                SR_billdate = u.SR_billdate,
                                SR_duedate = u.SR_duedate,
                                SR_invoiceno = u.SR_invoiceno,
                                SR_invoicetype = u.SR_invoicetype,
                                SR_GrossAmt = u.SR_GrossAmt,
                                SR_Discount = u.SR_Discount,
                                SR_AdditionalDisc = u.SR_AdditionalDisc,
                                SR_SGST = u.SR_SGST,
                                SR_CGST = u.SR_CGST,
                                SR_IGST = u.SR_IGST,
                                SR_othercharge = u.SR_othercharge,
                                SR_FinalAmt = u.SR_FinalAmt,
                                SR_PaidAmt = u.SR_PaidAmt,
                                SR_BalanceAmt = u.SR_BalanceAmt,
                                SR_paymode = u.SR_paymode,
                                SRUsrname = z.Uname
                            }).ToList();
                return View(list);
            }
            else
            {
                int usrid = Convert.ToInt32(Session["uid"]);
                var list = (from u in db.tbl_SaleReturnBill.Where(x => x.userid == usrid).OrderByDescending(x => x.id)
                            join z in db.Mst_Users on u.userid equals z.id
                            select new SaleReturnModel
                            {
                                id = u.id,
                                SR_entrycode = u.SR_entrycode,
                                SR_custname = u.SR_custname,
                                SR_billdate = u.SR_billdate,
                                SR_duedate = u.SR_duedate,
                                SR_invoiceno = u.SR_invoiceno,
                                SR_invoicetype = u.SR_invoicetype,
                                SR_GrossAmt = u.SR_GrossAmt,
                                SR_Discount = u.SR_Discount,
                                SR_AdditionalDisc = u.SR_AdditionalDisc,
                                SR_SGST = u.SR_SGST,
                                SR_CGST = u.SR_CGST,
                                SR_IGST = u.SR_IGST,
                                SR_othercharge = u.SR_othercharge,
                                SR_FinalAmt = u.SR_FinalAmt,
                                SR_PaidAmt = u.SR_PaidAmt,
                                SR_BalanceAmt = u.SR_BalanceAmt,
                                SR_paymode = u.SR_paymode,
                                SRUsrname = z.Uname,
                            }).ToList();
                return View(list);
            }
        }
        [HttpGet]
        public ActionResult SaleReturnDetail(int c_id = 0)
        {
            var list = db.Mst_Company.Where(x => x.id == 1).FirstOrDefault();
            var model = new SaleReturnVM();
            {
                model.BillVM = (from u in db.tbl_SaleReturnBill.Where(x => x.id == c_id)
                                select new SaleReturnModel
                                {
                                    id = u.id,
                                    SR_entrycode = u.SR_entrycode,
                                    SR_custname = u.SR_custname,
                                    SR_billdate = u.SR_billdate,
                                    SR_duedate = u.SR_duedate,
                                    SR_invoiceno = u.SR_invoiceno,
                                    SR_invoicetype = u.SR_invoicetype,
                                    SR_GrossAmt = u.SR_GrossAmt,
                                    SR_Discount = u.SR_Discount,
                                    SR_AdditionalDisc = u.SR_AdditionalDisc,
                                    SR_SGST = u.SR_SGST,
                                    SR_CGST = u.SR_CGST,
                                    SR_IGST = u.SR_IGST,
                                    SR_othercharge = u.SR_othercharge,
                                    SR_FinalAmt = u.SR_FinalAmt,
                                    SR_PaidAmt = u.SR_PaidAmt,
                                    SR_BalanceAmt = u.SR_BalanceAmt,
                                    SR_paymode = u.SR_paymode
                                }).ToList();
                model.productVM = (from u in db.tbl_SRBill_Products.Where(x => x.SR_salebillid == c_id)
                                   join v in db.Mst_ItemHead on u.SR_itemheadid equals v.id
                                   select new SaleReturnModel
                                   {
                                       id = u.id,
                                       SR_salebillid = u.SR_salebillid,
                                       SR_itemheadid = u.SR_itemheadid,
                                       SR_itemrate = u.SR_itemrate,
                                       SR_itemqty = u.SR_itemqty,
                                       SR_gross_amt = u.SR_gross_amt,
                                       //SR_discamount = u.SR_discamount,
                                       SR_sgstamt = u.SR_sgstamt,
                                       SR_cgstamt = u.SR_cgstamt,
                                       //SR_igstamt = u.SR_igstamt,
                                       SR_HSNSAC = u.SR_HSNSAC,
                                       SR_finalprice = u.SR_finalprice,
                                       SR_prod_name = v.itemName,
                                       SR_measurunit = v.itemMeasureUnit,
                                       SR_mrp = v.itemMRP
                                   }).ToList();
                model.CompName = list.CompName;
                model.CompPhone1 = list.CompPhone1;
                model.CompEmail = list.CompEmail;
                model.CompLogo = list.CompLogo;
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult SaleReturn()
        {
            ViewBag.AccHeadSR = new SelectList(db.tbl_Customers.ToList(), "id", "NameNo");
            ViewBag.ItemHeadSR = new SelectList(db.Mst_ItemHead.Where(x=>x.is_deleted == false).ToList(), "id", "itemBarcode");
            return View();
        }

        [HttpPost]
        public ActionResult SaleReturn(SaleReturnModel model)
        {
            int usrid = Convert.ToInt32(Session["uid"]);
            int[] arr_pro_id = model.SR_itemhead_id_arr;
            long[] arr_pro_qty = model.SR_item_qty_arr;
            decimal[] arr_price = model.SR_item_rate_arr;
            decimal[] arr_totalprice = model.SR_gross_amt_arr;
            //decimal[] arr_disc = model.SR_disc_amt_arr;
            decimal[] arr_sgst = model.SR_sgst_amt_arr;
            decimal[] arr_cgst = model.SR_cgst_amt_arr;
            //decimal[] arr_igst = model.SR_igst_amt_arr;
            string[] arr_hsn = model.SR_HSN_SAC_arr;
            decimal[] arr_finamnt = model.SR_finalprod_amt_arr;

            int maxid = db.tbl_SaleReturnBill.Max(x => (int?)x.id) ?? 0;
            var billnum = "SRBILL000" + (maxid + 1);
            tbl_SaleReturnBill tbl1 = new tbl_SaleReturnBill();
            tbl1.id = maxid + 1;
            tbl1.userid = usrid;
            tbl1.SR_entrycode = billnum;
            tbl1.SR_custname = model.SR_custname;
            tbl1.SR_billdate = model.SR_billdate;
            tbl1.SR_duedate = model.SR_duedate;
            tbl1.SR_invoicetype = model.SR_invoicetype;
            tbl1.SR_invoiceno = model.SR_invoiceno;
            tbl1.SR_GrossAmt = model.SR_GrossAmt;
            tbl1.SR_Discount = model.SR_Discount;
            tbl1.SR_AdditionalDisc = model.SR_AdditionalDisc;
            tbl1.SR_SGST = model.SR_SGST;
            tbl1.SR_CGST = model.SR_CGST;
            tbl1.SR_IGST = model.SR_IGST;
            tbl1.SR_othercharge = model.SR_othercharge;
            tbl1.SR_FinalAmt = model.SR_FinalAmt;
            tbl1.SR_PaidAmt = model.SR_PaidAmt;
            tbl1.SR_BalanceAmt = model.SR_BalanceAmt;
            tbl1.SR_paymode = model.SR_paymode;
            db.tbl_SaleReturnBill.Add(tbl1);
            db.SaveChanges();

            for (int i = 0; i <= arr_pro_id.Length - 1; i++)
            {
                int maxprid = db.tbl_SRBill_Products.Max(x => (int?)x.id) ?? 0;
                tbl_SRBill_Products tbl = new tbl_SRBill_Products();
                tbl.id = maxprid + 1;
                tbl.SR_salebillid = maxid + 1;
                tbl.SR_itemheadid = arr_pro_id[i];
                tbl.SR_itemqty = arr_pro_qty[i];
                tbl.SR_itemrate = arr_price[i];
                //tbl.SR_gross_amt = arr_totalprice[i];
                //tbl.SR_discamount = arr_disc[i];
                tbl.SR_sgstamt = arr_sgst[i];
                tbl.SR_cgstamt = arr_cgst[i];
                //tbl.SR_igstamt = arr_igst[i];
                tbl.SR_HSNSAC = arr_hsn[i];
                tbl.SR_finalprice = arr_finamnt[i];
                db.tbl_SRBill_Products.Add(tbl);
                db.SaveChanges();

                int pid = arr_pro_id[i];
                var updt = db.Mst_ItemHead.Where(x => x.id == pid).FirstOrDefault();
                int totqty = (int)updt.itemAvlQty;
                int newqty = (int)(totqty + arr_pro_qty[i]);
                updt.itemAvlQty = newqty;
                db.SaveChanges();
            }
            return RedirectToAction("SaleReturnList", "Admin");
        }
        public ActionResult RepairingList()
        {
            string role = Convert.ToString(Session["role"]);
            if (role == "SuperAdmin")
            {
                var list = (from u in db.tbl_Repair.OrderByDescending(x => x.id)
                            join z in db.Mst_Users on u.usrid equals z.id
                            select new RepairModel
                            {
                                id = u.id,
                                usrid = u.usrid,
                                billno = u.billno,
                                repairno = u.repairno,
                                Custmr = u.Custmr,
                                phoneno = u.phoneno,
                                date = u.date,
                                Amount = u.Amount,
                                status = u.status,
                                Usrname = z.Uname
                            }).ToList();
                return View(list);
            }
            else
            {
                int usrid = Convert.ToInt32(Session["uid"]);
                var list = (from u in db.tbl_Repair.Where(x => x.usrid == usrid).OrderByDescending(x => x.id)
                            join z in db.Mst_Users on u.usrid equals z.id
                            select new RepairModel
                            {
                                id = u.id,
                                usrid = u.usrid,
                                billno = u.billno,
                                repairno = u.repairno,
                                Custmr = u.Custmr,
                                phoneno = u.phoneno,
                                date = u.date,
                                Amount = u.Amount,
                                status = u.status,
                                Usrname = z.Uname
                            }).ToList();
                return View(list);
            }

        }
        [HttpGet]
        public ActionResult RepairDetail(int c_id = 0)
        {
            var list = db.Mst_Company.Where(x => x.id == 1).FirstOrDefault();
            var model = new RepairVM();
            {
                model.BillVM = (from u in db.tbl_Repair.Where(x => x.id == c_id)
                                select new RepairModel
                                {
                                    id = u.id,
                                    usrid = u.usrid,
                                    billno = u.billno,
                                    repairno = u.repairno,
                                    Custmr = u.Custmr,
                                    phoneno = u.phoneno,
                                    date = u.date,
                                    Amount = u.Amount,
                                    status = u.status
                                }).ToList();
                model.productVM = (from u in db.tbl_RepairProduct.Where(x => x.repbillid == c_id)
                                   join v in db.Mst_ItemHead on u.productid equals v.id
                                   select new RepairModel
                                   {
                                       id = u.id,
                                       productid = u.productid,
                                       repbillid = u.repbillid,
                                       quanti = u.quanti,
                                       prod_name = v.itemName
                                   }).ToList();
                model.CompName = list.CompName;
                model.CompPhone1 = list.CompPhone1;
                model.CompEmail = list.CompEmail;
                model.CompAdd = list.CompAddress;
                model.CompCity = list.CompCity;
                model.CompPIN = list.CompPin;
                model.CompState = list.CompState;
                model.CompGST = list.CompGST;
                model.CompLogo = list.CompLogo;
                model.CompBank = list.CompBankName;
                model.CompAccName = list.CompAccName;
                model.CompIFSC = list.CompIFSC;
                model.CompAccNo = list.CompAccNo;
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult RepairEntry()
        {
            ViewBag.ItemHeads = new SelectList(db.Mst_ItemHead.Where(x => x.is_deleted == false).ToList(), "id", "itemName");
            return View();
        }

        [HttpPost]
        public ActionResult RepairEntry(RepairModel model)
        {
            int usrid = Convert.ToInt32(Session["uid"]);
            int[] arr_pro_id = model.S_itemhead_id_arr;
            long[] arr_pro_qty = model.S_item_qty_arr;
            decimal[] arr_finamnt = model.S_finalprod_amt_arr;

            int maxid = db.tbl_Repair.Max(x => (int?)x.id) ?? 0;
            var billnum = "RBILL000" + (maxid + 1);
            tbl_Repair tbl1 = new tbl_Repair();
            tbl1.id = maxid + 1;
            tbl1.usrid = usrid;
            tbl1.repairno = billnum;
            tbl1.billno = model.billno;
            tbl1.Custmr = model.Custmr;
            tbl1.date = Convert.ToDateTime(DateTime.Now);
            tbl1.phoneno = model.phoneno;
            tbl1.Amount = model.Amount;
            tbl1.status = "Pending";
            db.tbl_Repair.Add(tbl1);
            db.SaveChanges();

            for (int i = 0; i <= arr_pro_id.Length - 1; i++)
            {
                int maxprid = db.tbl_RepairProduct.Max(x => (int?)x.id) ?? 0;
                tbl_RepairProduct tbl = new tbl_RepairProduct();
                tbl.id = maxprid + 1;
                tbl.repbillid = maxid + 1;
                tbl.productid = arr_pro_id[i];
                tbl.quanti = arr_pro_qty[i];
                db.tbl_RepairProduct.Add(tbl);
                db.SaveChanges();

            }
            return RedirectToAction("RepairDetail", "Admin", new { c_id = maxid + 1 });
        }
        public ActionResult statuschange(int userid = 0)
        {
            var changestatus = db.tbl_Repair.Where(x => x.id == userid).FirstOrDefault();
            if (changestatus != null)
            {
                changestatus.status = "Completed";
                db.SaveChanges();
            }
            return RedirectToAction("RepairingList");
        }
        [HttpGet]
        public ActionResult SaleProdList()
        {
            var list = (from u in db.tbl_SaleBill_Products.OrderByDescending(x => x.id)
                        join v in db.tbl_SaleBill on u.salebillid equals v.id
                        join z in db.Mst_ItemHead on u.itemheadid_s equals z.id
                        select new SaleEntryModel
                        {
                            id = u.id,
                            salebillid = u.salebillid,
                            itemheadid_s = u.itemheadid_s,
                            itemqty_s = u.itemqty_s,
                            billdate_s = v.billdate_s,
                            measurunit = z.itemMeasureUnit,
                            prod_name = z.itemName
                        }).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult SRProdList()
        {
            var list = (from u in db.tbl_SRBill_Products.OrderByDescending(x => x.id)
                        join v in db.tbl_SaleReturnBill on u.SR_salebillid equals v.id
                        join z in db.Mst_ItemHead on u.SR_itemheadid equals z.id
                        select new SaleReturnModel
                        {
                            id = u.id,
                            SR_salebillid = u.SR_salebillid,
                            SR_itemheadid = u.SR_itemheadid,
                            SR_itemqty = u.SR_itemqty,
                            SR_billdate = v.SR_billdate,
                            SR_measurunit = z.itemMeasureUnit,
                            SR_prod_name = z.itemName
                        }).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult PurchaseProdList()
        {
            var list = (from u in db.tbl_PurchaseBill_Products.OrderByDescending(x => x.id)
                        join v in db.tbl_PurchaseBill on u.purchasebill_id equals v.id
                        join z in db.Mst_ItemHead on u.itemhead_id equals z.id
                        select new PurchaseEntryModel
                        {
                            id = u.id,
                            purchasebill_id = u.purchasebill_id,
                            itemhead_id = u.itemhead_id,
                            item_qty = u.item_qty,
                            billdate = v.billdate,
                            measureunit = z.itemMeasureUnit,
                            prod_name = z.itemName
                        }).ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult PRProdList()
        {
            var list = (from u in db.tbl_PRBill_Products.OrderByDescending(x => x.id)
                        join v in db.tbl_PurchaseReturnBill on u.PR_purchasebill_id equals v.id
                        join z in db.Mst_ItemHead on u.PR_itemhead_id equals z.id
                        select new PurchaseReturnModel
                        {
                            id = u.id,
                            PR_purchasebill_id = u.PR_purchasebill_id,
                            PR_itemhead_id = u.PR_itemhead_id,
                            PR_item_qty = u.PR_item_qty,
                            PR_billdate = v.PR_billdate,
                            PR_measureunit = z.itemMeasureUnit,
                            PR_prod_name = z.itemName
                        }).ToList();
            return View(list);
        }
        [HttpPost]
        public FileResult DownloadExcel()
        {
            string path = "/Doc/SampleProducts.xlsx";
            return File(path, "application/vnd.ms-excel", "SampleProducts.xlsx");
        }
        [HttpPost]
        public ActionResult ImportExcel(HttpPostedFileBase excelfile)
        {
            try
            {
                List<string> data = new List<string>();
                if (excelfile != null)
                {
                    // tdata.ExecuteCommand("truncate table OtherCompanyAssets");
                    if (excelfile.ContentType == "application/vnd.ms-excel" || excelfile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        string filename = excelfile.FileName;
                        string targetpath = Server.MapPath("~/Doc/");
                        excelfile.SaveAs(targetpath + filename);
                        string pathToExcelFile = targetpath + filename;
                        var connectionString = "";
                        if (filename.EndsWith(".xls"))
                        {
                            connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                        }
                        else if (filename.EndsWith(".xlsx"))
                        {
                            connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;", pathToExcelFile);
                        }

                        var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                        var ds = new DataSet();
                        adapter.Fill(ds, "ExcelTable");
                        DataTable dtable = ds.Tables["ExcelTable"];
                        //string sheetName = "Sheet1";
                        //var excelFiles = new ExcelQueryFactory(pathToExcelFile);
                        //var artistAlbums = from a in excelFiles.Worksheet<Mst_ItemHead>(sheetName) select a;
                        foreach (DataRow b in dtable.Rows)
                        {
                            int max = db.Mst_ItemHead.Max(x => (int?)x.id) ?? 0;
                            int maxs = max + 1;
                            Mst_ItemHead TU = new Mst_ItemHead();
                            TU.id = maxs;
                            TU.itemName = b.ItemArray[0].ToString();
                            TU.itemCGST = Convert.ToDecimal(b.ItemArray[1]);
                            TU.itemSGST = Convert.ToDecimal(b.ItemArray[2]);
                            TU.itemMRP = Convert.ToDecimal(b.ItemArray[3]);
                            TU.itemMeasureUnit = b.ItemArray[4].ToString();
                            TU.itemOpenQty = Convert.ToInt32(b.ItemArray[5]);
                            TU.itemPurchaseRate = Convert.ToDecimal(b.ItemArray[6]);
                            TU.ProductCode = b.ItemArray[7].ToString();
                            TU.itemHSN_SAC = b.ItemArray[8].ToString();
                            TU.itemBarcode = b.ItemArray[9].ToString();
                            TU.ItemGroup = b.ItemArray[10].ToString();
                            TU.itemCode = "ITEM000" + maxs;
                            TU.itemAvlQty = Convert.ToInt32(b.ItemArray[5]);
                            decimal calc = Convert.ToDecimal(b.ItemArray[3]) * Convert.ToInt32(b.ItemArray[5]);
                            TU.itemOpenValue = calc;
                            TU.itemPurchaseTax = true;
                            TU.itemSaleTax = true;
                            TU.is_deleted = false;
                            TU.itembarcodeimg = getBarcodeImage(b.ItemArray[9].ToString(), maxs, Convert.ToDecimal(b.ItemArray[3]), b.ItemArray[7].ToString());
                            //TU.s_course_id = objmodel.s_course_id;
                            db.Mst_ItemHead.Add(TU);
                            db.SaveChanges();
                        }
                        
                        //deleting excel file from folder
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        RedirectToAction("ItemHead");
                    }
                    else
                    {
                        //alert message for invalid file format
                        data.Add("<ul>");
                        data.Add("<li>Only Excel file format is allowed</li>");
                        data.Add("</ul>");
                        data.ToArray();
                        // return Json(data, JsonRequestBehavior.AllowGet);
                        return RedirectToAction("ContactGroup");
                    }
                }
                else
                {
                    data.Add("<ul>");
                    if (excelfile == null) data.Add("<li>Please choose Excel file</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return RedirectToAction("ItemHead");
                    //   return Json(data, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("ItemHead");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex.Message;

            }
            return RedirectToAction("ItemHead");
        }

    }
}