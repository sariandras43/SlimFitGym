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

                if (ex.Message.Contains("parsing"))
                {
                    return controller.BadRequest(new { message = "Nem JSON formátumú a body." });
                }
                if (ex is UnauthorizedAccessException)
                {
                    return controller.Forbid();
                }
                return controller.BadRequest(new
                {
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                });
#else
                if (ex.Message.Contains("parsing"))
                {
                    return controller.BadRequest(new { message = "Nem JSON formátumú a body." });
                }
                return controller.BadRequest(new { message = ex.Message });
#endif
            }
        }
    }
}