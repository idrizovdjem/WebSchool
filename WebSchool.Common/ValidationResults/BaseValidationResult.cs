using System.Collections.Generic;

namespace WebSchool.Common.ValidationResults
{
    public abstract class BaseValidationResult
    {
        protected BaseValidationResult()
        {
            IsValid = true;
            Errors = new Dictionary<string, ICollection<string>>();
        }
        
        public bool IsValid { get; set; }

        public Dictionary<string, ICollection<string>> Errors { get; set; }

        public void AddErrorMessage(string key, string message)
        {
            IsValid = false;

            if (Errors.ContainsKey(key) == false)
            {
                Errors.Add(key, new List<string>());
            }

            Errors[key].Add(message);
        }
    }
}
