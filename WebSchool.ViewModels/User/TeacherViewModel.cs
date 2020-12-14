using WebSchool.ViewModels.Classes;
using System.Collections.Generic;

namespace WebSchool.ViewModels.User
{
    public class TeacherViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<TeacherClassViewModel> Classes { get; set; }
    }
}
