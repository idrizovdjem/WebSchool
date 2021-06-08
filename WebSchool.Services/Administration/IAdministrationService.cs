using WebSchool.ViewModels.Group;

namespace WebSchool.Services.Administration
{
    public interface IAdministrationService
    {
        GroupSettingsViewModel GetGroupSettings(string groupId);
    }
}
