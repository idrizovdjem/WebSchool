using WebSchool.Common.ValidationResults;

namespace WebSchool.Services.Common
{
    public interface IUtilitiesService
    {
        void MergeErrorMessages(BaseValidationResult source, BaseValidationResult destination);
    }
}
