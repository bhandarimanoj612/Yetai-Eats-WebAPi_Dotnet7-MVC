namespace Yetai_Eats.Services
{
    public interface IFileService
    {
       Task<string> WriteFile(IFormFile file);
    }
}
