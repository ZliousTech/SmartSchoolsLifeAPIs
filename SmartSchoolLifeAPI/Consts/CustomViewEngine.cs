using System.Web.Mvc;

public class CustomViewEngine : RazorViewEngine
{
    public CustomViewEngine()
    {
        ViewLocationFormats = new[]
        {
            "~/Core/Views/{1}/{0}.cshtml",
            "~/Core/Views/Shared/{0}.cshtml"
        };

        PartialViewLocationFormats = new[]
        {
            "~/Core/Views/{1}/{0}.cshtml",
            "~/Core/Views/Shared/{0}.cshtml"
        };
    }
}
