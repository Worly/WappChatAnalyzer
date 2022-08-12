using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WappChatAnalyzer.Domain;
using WappChatAnalyzer.DTOs;

namespace WappChatAnalyzer.Services
{
    public interface IWorkspaceService
    {
        public Workspace AddNew(WorkspaceDTO dto, int ownerId);
        public Workspace Edit(int ownerId, int workspaceId, WorkspaceDTO dto);
        public List<Workspace> GetMy(int ownerId);
        public List<Workspace> GetShared(User user);
        public Workspace GetById(int userId, int workspaceId, bool includeShared);
        public bool TryDelete(int userId, int workspaceId);

        public List<WorkspaceShare> GetWorkspaceShares(int userId, int workspaceId);
        public void ShareWorkspace(User user, int workspaceId, string sharedUserEmail);
        public void UnshareWorkspace(int userId, int workspaceId, string sharedUserEmail);

        public void SetSelectedWorkspace(int userId, int workspaceId);
        public int? GetSelectedWorkspace(int userId);
    }

    public class WorkspaceService : IWorkspaceService
    {
        private MainDbContext context;
        private IMailService mailService;
        private IUserService userService;
        private string appLink;

        public WorkspaceService(MainDbContext context, IMailService mailService, IUserService userService, IConfigurationService configurationService)
        {
            this.context = context;
            this.mailService = mailService;
            this.userService = userService;

            this.appLink = configurationService.Get<string>("AppLink");
        }

        public Workspace AddNew(WorkspaceDTO dto, int ownerId)
        {
            var workspace = new Workspace()
            {
                Name = dto.Name,
                OwnerId = ownerId
            };

            context.Workspaces.Add(workspace);
            context.SaveChanges();

            return workspace;
        }

        public Workspace Edit(int ownerId, int workspaceId, WorkspaceDTO dto)
        {
            var workspace = context.Workspaces.FirstOrDefault(o => o.OwnerId == ownerId && o.Id == workspaceId);
            if (workspace == null)
                return null;

            workspace.Name = dto.Name;
            context.SaveChanges();

            return workspace;
        }

        public List<Workspace> GetMy(int ownerId)
        {
            return context.Workspaces.Where(o => o.OwnerId == ownerId).ToList();
        }

        public List<Workspace> GetShared(User user)
        {
            return context.Workspaces.Where(o => o.WorkspaceShares.Any(w => w.SharedUserEmail == user.Email)).ToList();
        }

        public Workspace GetById(int userId, int workspaceId, bool includeShared)
        {
            var query = context.Workspaces.Where(o => o.Id == workspaceId);

            if (includeShared)
            {
                var user = userService.GetById(userId);
                query = query.Where(o => o.OwnerId == userId || o.WorkspaceShares.Any(w => w.SharedUserEmail == user.Email));
            }
            else
            {
                query = query.Where(o => o.OwnerId == userId);
            }

            return query.FirstOrDefault();
        }

        public bool TryDelete(int userId, int workspaceId)
        {
            var workspace = context.Workspaces.FirstOrDefault(o => o.OwnerId == userId && o.Id == workspaceId);
            if (workspace == null)
                return false;

            var user = context.Users.FirstOrDefault(o => o.Id == userId);
            if (user.SelectedWorkspaceId == workspaceId)
                user.SelectedWorkspaceId = null;

            context.Workspaces.Remove(workspace);
            context.SaveChanges();
            return true;
        }

        public List<WorkspaceShare> GetWorkspaceShares(int userId, int workspaceId)
        {
            var workspace = context.Workspaces.Include(o => o.WorkspaceShares).FirstOrDefault(o => o.OwnerId == userId && o.Id == workspaceId);
            if (workspace == null)
                return null;

            return workspace.WorkspaceShares.ToList();
        }

        public void ShareWorkspace(User user, int workspaceId, string sharedUserEmail)
        {
            var workspace = context.Workspaces
                .Include(o => o.WorkspaceShares)
                .FirstOrDefault(o => o.OwnerId == user.Id && o.Id == workspaceId);
            if (workspace == null)
                return;

            if (workspace.WorkspaceShares.Any(w => w.SharedUserEmail == sharedUserEmail))
                return;

            workspace.WorkspaceShares.Add(new WorkspaceShare()
            {
                SharedUserEmail = sharedUserEmail
            });

            context.SaveChanges();

            this.mailService.SendEmailAsync(new MailRequest()
            {
                Subject = "WappChatAnalyzer workspace shared with you",
                ToEmail = sharedUserEmail,
                Body =
                $"Hello,<br>" +
                $"<br>" +
                $"User {user.Username} has shared his workspace with you. " +
                $"If you don't already have an account on WappChatAnalyzer then <a href=\"{appLink}register\">create</a> it to browse this workspace.<br>"
            });

            return;
        }

        public void UnshareWorkspace(int userId, int workspaceId, string sharedUserEmail)
        {
            var workspace = context.Workspaces.Include(o => o.WorkspaceShares).FirstOrDefault(o => o.OwnerId == userId && o.Id == workspaceId);
            if (workspace == null)
                return;

            var workspaceShare = workspace.WorkspaceShares.FirstOrDefault(w => w.SharedUserEmail == sharedUserEmail);
            if (workspaceShare == null)
                return;

            workspace.WorkspaceShares.Remove(workspaceShare);

            context.SaveChanges();
            return;
        }

        public void SetSelectedWorkspace(int userId, int workspaceId)
        {
            var user = userService.GetById(userId);
            user.SelectedWorkspaceId = workspaceId;
            context.SaveChanges();
        }

        public int? GetSelectedWorkspace(int userId)
        {
            var user = userService.GetById(userId);
            return user.SelectedWorkspaceId;
        }
    }
}
