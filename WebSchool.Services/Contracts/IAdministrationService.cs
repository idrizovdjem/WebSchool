using WebSchool.ViewModels.Group;

namespace WebSchool.Services.Contracts
{
    public interface IAdministrationService
    {
        GroupSettingsViewModel GetGroupSettings(string groupId);

        bool ValidateIfUserIsAdmin(string userId, string groupId);
    }
}
