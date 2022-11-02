using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CFDIWEB.Services;
using CFDIWEB.Data;
using CFDIWEB.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CFDIWEB.Pages
{
    public class polizaModel : PageModel
    {
        private MyAppDbContext _dbContext;
        private IPoliza _poliza;
        public polizaModel(MyAppDbContext dbContext, IPoliza poliza)
        {
            _dbContext = dbContext;
            _poliza = poliza;

        }
        public void OnGet()
        {
            
            _poliza.CreacionExcel();
        }
    }
}
