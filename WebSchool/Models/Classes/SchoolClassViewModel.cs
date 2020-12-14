using WebSchool.Models.User;
using System.Collections.Generic;

namespace WebSchool.Models.Classes
{
    public class SchoolClassViewModel
    {
        public string Id { get; set; }

        public string Signature { get; set; }

        public ICollection<StudentClassViewModel> Students { get; set; }
    }
}
