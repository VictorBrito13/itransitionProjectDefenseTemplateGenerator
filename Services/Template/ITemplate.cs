namespace ItransitionTemplates.Services.Template
{
    public interface ITemplate {
        Task<Models.Template> AddTemplate(Models.Template template);
    }
}