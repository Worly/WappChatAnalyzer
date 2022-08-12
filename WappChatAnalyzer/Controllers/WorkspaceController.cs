using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.Auth;
using WappChatAnalyzer.DTOs;
using WappChatAnalyzer.Guards;
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

            return Ok(result.GetDTO());
        }

        [HttpPut("edit/{id}")]
        [Authorize]
        public ActionResult<WorkspaceDTO> Edit(int id, WorkspaceDTO dto)
        {
            var result = this.workspaceService.Edit(HttpContext.CurrentUser().Id, id, dto);
            if (result == null)
                return NotFound();

            return Ok(result.GetDTO());
        }

        [HttpGet("getMy")]
        [Authorize]
        public ActionResult<List<WorkspaceDTO>> GetMy()
        {
            var result = this.workspaceService.GetMy(HttpContext.CurrentUser().Id);

            return Ok(result.Select(o => o.GetDTO()));
        }

        [HttpGet("getShared")]
        [EmailVerified]
        public ActionResult<List<WorkspaceDTO>> GetShared()
        {
            var result = this.workspaceService.GetShared(HttpContext.CurrentUser());

            return Ok(result.Select(o => o.GetDTO()));
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            if (this.workspaceService.TryDelete(HttpContext.CurrentUser().Id, id))
                return Ok();
            else
                return NotFound();
        }

        [HttpPut("selectWorkspace")]
        [Authorize]
        public ActionResult SelectWorkspace([FromBody] int workspaceId)
        {
            var workspace = workspaceService.GetById(HttpContext.CurrentUser().Id, workspaceId, true);
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

        [HttpGet("getWorkspaceShares/{id}")]
        [Authorize]
        public ActionResult<List<WorkspaceShareDTO>> GetWorkspaceShares(int id)
        {
            var workspaceShares = workspaceService.GetWorkspaceShares(HttpContext.CurrentUser().Id, id);
            if (workspaceShares == null)
                return BadRequest();

            return Ok(workspaceShares.Select(o => o.GetDTO()));
        }

        [HttpPost("shareWorkspace/{id}")]
        [Authorize]
        public ActionResult<List<WorkspaceShareDTO>> ShareWorkspace(int id, [FromBody] WorkspaceShareDTO share)
        {
            workspaceService.ShareWorkspace(HttpContext.CurrentUser(), id, share.SharedUserEmail);

            var workspaceShares = workspaceService.GetWorkspaceShares(HttpContext.CurrentUser().Id, id);
            if (workspaceShares == null)
                return BadRequest();

            return Ok(workspaceShares.Select(o => o.GetDTO()));
        }

        [HttpPost("unshareWorkspace/{id}")]
        [Authorize]
        public ActionResult<List<WorkspaceShareDTO>> UnshareWorkspace(int id, [FromBody] WorkspaceShareDTO share)
        {
            workspaceService.UnshareWorkspace(HttpContext.CurrentUser().Id, id, share.SharedUserEmail);

            var workspaceShares = workspaceService.GetWorkspaceShares(HttpContext.CurrentUser().Id, id);
            if (workspaceShares == null)
                return BadRequest();

            return Ok(workspaceShares.Select(o => o.GetDTO()));
        }
    }
}
