using Microsoft.AspNetCore.Mvc;

namespace Interprocess.Attending.API.Controllers.Attendances;
public class AttendanceController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
