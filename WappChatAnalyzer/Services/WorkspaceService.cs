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
        public Workspace GetById(int userId, int workspaceId);
        public bool TryDelete(int userId, int workspaceId);

        public void SetSelectedWorkspace(int userId, int workspaceId);
        public int? GetSelectedWorkspace(int userId);
    }

    public class WorkspaceService : IWorkspaceService
    {
        private MainDbContext context;

        public WorkspaceService(MainDbContext context)
        {
            this.context = context;
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

        public Workspace GetById(int userId, int workspaceId)
        {
            return context.Workspaces.FirstOrDefault(o => o.OwnerId == userId && o.Id == workspaceId);
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

        public void SetSelectedWorkspace(int userId, int workspaceId)
        {
            var user = context.Users.FirstOrDefault(o => o.Id == userId);
            user.SelectedWorkspaceId = workspaceId;
            context.SaveChanges();
        }

        public int? GetSelectedWorkspace(int userId)
        {
            var user = context.Users.FirstOrDefault(o => o.Id == userId);
            return user.SelectedWorkspaceId;
        }
    }
}
