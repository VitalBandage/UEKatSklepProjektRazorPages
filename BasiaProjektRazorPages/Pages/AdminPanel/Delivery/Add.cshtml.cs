using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BasiaProjektRazorPages.DbModels;
using BasiaProjektRazorPages.Helpers;
using BasiaProjektRazorPages.ViewModels;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BasiaProjektRazorPages.Pages.AdminPanel.Delivery
{
    public class AddModel : PageModel
    {
        public string alertClass { get; set; }
        public string alertMessage { get; set; }
        [BindProperty(SupportsGet = true)]
        public string supplierId { get; set; }
        public List<SelectListItem> Suppliers { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Products { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Warehouses { get; set; } = new List<SelectListItem>();
        [BindProperty]
        public Dostawa Delivery { get; set; }
        [BindProperty]
        public List<Produkt_magazyn_dostawa> Products_warehouses_delivery { get; set; }
        public void OnGet()
        {
            this.FillAllSelectListItems();
        }

        public void OnPost()
        {
            this.FillAllSelectListItems();
        }



        #region selectListItems
        public void FillAllSelectListItems()
        {
            this.FillSupplierSelectListItems();
            this.FillWarehouseSelectListItems();
            this.FillProductSelectListItems();
        }

        public void FillSupplierSelectListItems()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {
                    var dbSuppliers = conn.Query<Dostawca>("SELECT * FROM Dostawca");
                    foreach (Dostawca sup in dbSuppliers)
                    {
                        this.Suppliers.Add(new SelectListItem { Text = sup.Nazwa, Value = sup.ID_Dostawcy.ToString() });
                    }
                }
                catch (InvalidOperationException exc) { }
            }
        }
        public void FillProductSelectListItems()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {
                    var dbProducts = conn.Query<Produkt>("SELECT * FROM Produkt");
                    foreach (Produkt prod in dbProducts)
                    {
                        this.Products.Add(new SelectListItem { Text = prod.Nazwa, Value = prod.ID_Produktu.ToString() });
                    }
                }
                catch (InvalidOperationException exc) { }
            }
        }
        public void FillWarehouseSelectListItems()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {
                    var dbWarehouses = conn.Query<Magazyn>("SELECT * FROM Magazyn");
                    foreach (Magazyn war in dbWarehouses)
                    {
                        this.Warehouses.Add(new SelectListItem { Text = war.Nazwa, Value = war.ID_Magazynu.ToString() });
                    }
                }
                catch (InvalidOperationException exc) { }
            }
        }
        #endregion selectListItems
    }
}
