using WebSchool.Common.ValidationResults;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
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
