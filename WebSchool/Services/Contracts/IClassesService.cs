using System.Threading.Tasks;

namespace WebSchool.Services.Contracts
{
    public interface IClassesService
    {
        bool IsClassSignatureAvailable(string signature, string schoolId);

        Task CreateClass(string signature, string schoolId);
    }
}
