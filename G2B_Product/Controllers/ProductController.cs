﻿using G2B_Product.App_Data;
using G2B_Product.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace G2B_Product.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        #region Get/Retrieve product data
        // GET: All Products
        [AllowAnonymous]
        public ActionResult ListAll()
        {
            List<ProductViewModel> ListProductVMs = new List<ProductViewModel>();

            G2BPMDataContext context = new G2BPMDataContext();
            List<Product> ListProducts = context.Products.ToList();

            foreach (Product product in ListProducts)
            {
                ProductViewModel pVM = MappingMethod(product);
                ListProductVMs.Add(pVM);
            }

            TempData["ErrorMessageOfDelete"] = "";
            return View(ListProductVMs);
        }

        // GET: Product/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            try
            {
                G2BPMDataContext context = new G2BPMDataContext();
                Product product = context.Products.FirstOrDefault(i => i.Id == id);
                ProductViewModel pVM = MappingMethod(product);

                return View(pVM);
            }
            catch (Exception e)
            {
                return View();
            }
        }
        #endregion

        #region Add/Create product data
        // GET: Product/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(ProductViewModel model, HttpPostedFileBase postedFile)
        {
            try
            {
                G2BPMDataContext context = new G2BPMDataContext();

                Product s = new Product()
                {
                    Title = model.Title,
                    ImageUrl = (postedFile != null) ? ConvertImageStreamToBase64(postedFile.InputStream) : model.ImageUrl,
                    ShortDesc = model.ShortDesc,
                    FullDesc = model.FullDesc
                };
                context.Products.InsertOnSubmit(s);
                context.SubmitChanges();

                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false, error = e.Message });
            }
        }
        #endregion

        #region Edit/Update product data
        // GET: Product/Edit/5
        [Authorize(Roles = "Admin, ContentContributor")]
        public ActionResult Edit(int id)
        {
            try
            {
                G2BPMDataContext context = new G2BPMDataContext();
                Product product = context.Products.FirstOrDefault(i => i.Id == id);
                ProductViewModel pVM = MappingMethod(product);

                return View(pVM);
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // POST: Product/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin, ContentContributor")]
        public ActionResult Edit(ProductViewModel model)
        {
            try
            {
                G2BPMDataContext context = new G2BPMDataContext();
                Product product = context.Products.FirstOrDefault(i => i.Id == model.Id);

                product.Title = model.Title;
                product.ShortDesc = model.ShortDesc;
                product.FullDesc = model.FullDesc;
                product.ImageUrl = model.ImageUrl;

                context.SubmitChanges();

                return Json(new { success = true });
            }
            catch (Exception e)
            {
                return Json(new { success = false, error = e.Message });
            }
        }
        #endregion

        #region Delete/Remove product data
        // POST: Product/Delete/5
        [Authorize(Roles = "Admin, ContentContributor")]
        public ActionResult Delete(int id)
        {
            try
            {
                G2BPMDataContext context = new G2BPMDataContext();
                Product product = context.Products.FirstOrDefault(i => i.Id == id);
                context.Products.DeleteOnSubmit(product);
                context.SubmitChanges();

                TempData["ErrorMessageOfDelete"] = "";
                return RedirectToAction("ListAll");
            }
            catch (Exception e)
            {
                TempData["ErrorMessageOfDelete"] = "Couldn't deleted the product. Error: " + e.Message;
                return RedirectToAction("ListAll");
            }
        }
        #endregion

        #region Private methods
        private ProductViewModel MappingMethod(Product product)
        {
            if (product != null)
            {
                var pVM = new ProductViewModel()
                {
                    Id = product.Id,
                    Title = product.Title,
                    ImageUrl = product.ImageUrl,
                    ShortDesc = product.ShortDesc,
                    FullDesc = product.FullDesc
                };

                return pVM;
            }
            else return null;
        }

        private string ConvertImageStreamToBase64(Stream fs)
        {
            //Stream fs = FileUpload1.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] bytes = br.ReadBytes((Int32)fs.Length);
            string base64String = "data:image/png;base64," + Convert.ToBase64String(bytes, 0, bytes.Length);
            return base64String;
        }
        #endregion
    }
}
