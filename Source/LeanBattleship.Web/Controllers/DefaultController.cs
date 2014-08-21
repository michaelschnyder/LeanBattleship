using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace LeanBattleship.Web.Controllers
{
    public class DefaultController : ApiController
    {
        [HttpGet]
        [Route("api/version")]
        public IHttpActionResult Version()
        {
            var webAssembly = this.GetType().Assembly;
            var configuration = string.Empty;

            var configurationAttribute = webAssembly.GetCustomAttributes(typeof(AssemblyConfigurationAttribute)).OfType<AssemblyConfigurationAttribute>().ToList();
            if (configurationAttribute.Any())
            {
                configuration = configurationAttribute.First().Configuration;
            }


            return Json(new VersionDto
            {
                Version = webAssembly.GetName().Version.ToString(), 
                Build = webAssembly.GetName().Version.Revision.ToString(CultureInfo.CurrentCulture),
                Configuration = configuration
            });
        }
    }

    public class VersionDto
    {
        public string Version { get; set; }
        public string Build { get; set; }
        public string Configuration { get; set; }
    }
}
