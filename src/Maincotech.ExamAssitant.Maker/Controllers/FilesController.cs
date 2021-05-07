using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using JQuery = Maincotech.Web.Components.JQuery;
using Vditor = Maincotech.Web.Components.Vditor;

namespace Maincotech.ExamAssistant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private Vditor.IVditorService _vditorService;
        private JQuery.IJQueryService _jQueryService;

        public FilesController(Vditor.IVditorService vditorService, JQuery.IJQueryService jQueryService)
        {
            _vditorService = vditorService;
            _jQueryService = jQueryService;
        }

        [HttpPost("vditorUpload")]
        public async Task<Vditor.UploadResult> VditorUpload()
        {
            var result = await _vditorService.Upload(Request.Form.Files.ToList());
            return result;
        }

        [HttpPost("jqueryUpload")]
        public async Task<JQuery.UploadResult> JqueryUpload()
        {
            var result = await _jQueryService.Upload(Request.Form.Files.First());
            return result;
        }
    }
}
