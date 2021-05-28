namespace WebSchool.Services.Contracts
{
    public interface IAdministrationService
    {
        bool ValidateIfUserIsAdmin(string userId, string groupId);
    }
}
