using WebSchool.Common.Enumerations;

namespace WebSchool.ViewModels.Assignment
{
    public class AssignmentPreviewViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string GroupName { get; set; }

        public AssignmentStatus Status { get; set; }
    }
}
