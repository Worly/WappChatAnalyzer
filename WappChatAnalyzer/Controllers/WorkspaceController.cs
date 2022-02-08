using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.Auth;
using WappChatAnalyzer.DTOs;
using WappChatAnalyzer.Services;

namespace WappChatAnalyzer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkspaceController : ControllerBase
    {
        private IWorkspaceService workspaceService;

        public WorkspaceController(IWorkspaceService workspaceService)
        {
            this.workspaceService = workspaceService;
        }

        [HttpPost("addNew")]
        [Authorize]
        public ActionResult<WorkspaceDTO> AddNew(WorkspaceDTO dto)
        {
            var result = this.workspaceService.AddNew(dto, HttpContext.CurrentUser().Id);

            return Ok(result);
        }

        [HttpGet("getMy")]
        [Authorize]
        public ActionResult<List<WorkspaceDTO>> GetMy()
        {
            var result = this.workspaceService.GetMy(HttpContext.CurrentUser().Id);

            return Ok(result.Select(o => o.GetDTO()));
        }

        [HttpPut("selectWorkspace")]
        [Authorize]
        public ActionResult SelectWorkspace([FromBody] int workspaceId)
        {
            var workspace = workspaceService.GetById(HttpContext.CurrentUser().Id, workspaceId);
            if (workspace == null)
                return BadRequest();

            workspaceService.SetSelectedWorkspace(HttpContext.CurrentUser().Id, workspaceId);
            return Ok();
        }

        [HttpGet("getSelectedWorkspace")]
        [Authorize]
        public ActionResult<int?> GetSelectedWorkspace()
        {
            return Ok(workspaceService.GetSelectedWorkspace(HttpContext.CurrentUser().Id));
        }
    }
}
