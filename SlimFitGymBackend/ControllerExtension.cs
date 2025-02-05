using Microsoft.AspNetCore.Mvc;

namespace SlimFitGymBackend
{
    public static class ControllerExtension
    {
        public static IActionResult Execute(this ControllerBase controller, Func<IActionResult> function)
        {
            try
            {
                return function();
            }
            catch (Exception ex)
            {
#if DEBUG
                return controller.BadRequest(new
                {
                    message = ex.Message
                    //stackTrace = ex.StackTrace
                });
#else
                return controller.BadRequest(new { message = "Váratlan hiba" });
#endif
            }
        }
    }
}