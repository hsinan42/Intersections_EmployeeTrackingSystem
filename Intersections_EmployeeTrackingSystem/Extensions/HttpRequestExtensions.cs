namespace Intersections_EmployeeTrackingSystem.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            return request.Headers.TryGetValue("X-Requested-With", out var value)
                   && string.Equals(value, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
        }
    }
}
