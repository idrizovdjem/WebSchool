using WebSchool.Models.Classes;
using System.Collections.Generic;

namespace WebSchool.Areas.Admin.Models.User
{
    public class TeacherViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<TeacherClassViewModel> Classes { get; set; }
    }
}
