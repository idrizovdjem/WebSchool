using WebSchool.Common.ValidationResults;

namespace WebSchool.Services.Common
{
    public class UtilitiesService : IUtilitiesService
    {
        public void MergeErrorMessages(BaseValidationResult source, BaseValidationResult destination)
        {
            foreach (var key in source.Errors.Keys)
            {
                foreach (var message in source.Errors[key])
                {
                    destination.AddErrorMessage(key, message);
                }
            }
        }
    }
}
