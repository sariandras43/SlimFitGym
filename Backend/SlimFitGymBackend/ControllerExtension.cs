using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
                if (ex is UnauthorizedAccessException)
                    return controller.Forbid();
                if (ex is JsonSerializationException)
                    return controller.BadRequest(new { message = "Helytelen érték a JSON-ben." });
#if DEBUG

                return controller.BadRequest(new
                {
                    message = ex.Message,
                    stackTrace = ex.StackTrace
                });
#else

                return controller.BadRequest(new { message = ex.Message });
#endif
            }
        }
    }
}