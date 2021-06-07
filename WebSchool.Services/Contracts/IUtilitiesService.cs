using WebSchool.Common.ValidationResults;

namespace WebSchool.Services.Contracts
{
    public interface IUtilitiesService
    {
        void MergeErrorMessages(BaseValidationResult source, BaseValidationResult destination);
    }
}
