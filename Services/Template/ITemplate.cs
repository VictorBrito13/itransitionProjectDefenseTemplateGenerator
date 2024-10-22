using ItransitionTemplates.Models;

namespace ItransitionTemplates.Services.Template
{
    public interface ITemplate {
        Task<Models.Template> AddTemplate(Models.Template template);
        Task<Models.Template[]> GetLatestTemplatesWithAdmins(int page, int limit);
        Task<Models.Template[]> GetTemplatesByUserId(int page, int limit, ulong userId);
        Task<Models.Template> GetTemplateById(ulong templateId);
        Task<int> UpdateTemplate(ulong templateId, Models.Template template);
        Task<Like[]> LikeAction(ulong userId, ulong templateId, string action);
    }
}