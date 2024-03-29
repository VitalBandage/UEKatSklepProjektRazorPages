using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using SklepProjektRazorPages.ViewModels;
using System.Data;
using Dapper;

namespace SklepProjektRazorPages.Pages.AdminPanel.Supplier
{
    public class ViewModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string id { get; set; }
        public string alertClass { get; set; }
        public string alertMessage { get; set; }
        public bool supplierNotFound { get; set; } = false;
        public SupplierViewModel supplier { get; set; }
        public List<Dostawa> deliveries { get; set; }

        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                try
                {
                    var dbSupplier = conn.Query<Dostawca>("SELECT TOP 1 * FROM Dostawca WHERE ID_Dostawcy = @id", new { id = this.id });
                    if (dbSupplier.Any() == false)
                        supplierNotFound = true;
                    else
                    {            
                        this.supplier = new SupplierViewModel(dbSupplier.First(), true);
                    }
                }
                catch(InvalidOperationException exc)
                {
                    alertClass = "alert-danger";
                    alertMessage = "Server Error:\n" + exc.Message;
                }
            }

        }
    }
}
