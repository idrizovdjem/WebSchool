using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Microsoft.EntityFrameworkCore;

using WebSchool.Data;
using WebSchool.Services.Groups;
using WebSchool.ViewModels.Group;

namespace WebSchool.Tests.ServicesTests
{
    public class GroupsServiceTests
    {
        private readonly ApplicationDbContext context;

        public GroupsServiceTests()
        {
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Test db");
            this.context = new ApplicationDbContext(dbOptions.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [Fact]
        public async Task TestCreateAsyncReturnsGroupId()
        {
            var groupsService = new GroupsService(context, null, null);
            var groupId = await groupsService.CreateAsync("user id", "Group Name");

            var groupName = await context.Groups
                .Select(g => g.Name)
                .FirstOrDefaultAsync();

            Assert.False(string.IsNullOrWhiteSpace(groupId));
            Assert.Equal("Group Name", groupName);
        }

        [Fact]
        public async Task TestChangeNameAsync()
        {
            var groupsService = new GroupsService(context, null, null);
            var groupId = await groupsService.CreateAsync("user id", "Group Name");

            var inputModel = new ChangeGroupNameInputModel()
            {
                Id = groupId,
                Name = "New Name"
            };

            await groupsService.ChangeNameAsync(inputModel);

            var groupName = await context.Groups
                .Select(g => g.Name)
                .FirstOrDefaultAsync();

            Assert.Equal("New Name", groupName);
        }

        [Fact]
        public async Task TestChangeNameAsyncShouldNotMakeChangesIfGroupIdIsInvalid()
        {
            var groupsService = new GroupsService(context, null, null);
            var groupId = await groupsService.CreateAsync("user id", "Group Name");

            var inputModel = new ChangeGroupNameInputModel()
            {
                Id = "invalid",
                Name = "New Name"
            };

            await groupsService.ChangeNameAsync(inputModel);

            var groupName = await context.Groups
                .Select(g => g.Name)
                .FirstOrDefaultAsync();

            Assert.Equal("Group Name", groupName);
        }

        [Fact]
        public async Task TestGetIdByNameShouldReturnCorrectId()
        {
            var groupsService = new GroupsService(context, null, null);
            var groupName = "Group Name";
            var originalGroupId = await groupsService.CreateAsync("user id", groupName);

            var groupId = groupsService.GetIdByName(groupName);
            Assert.Equal(originalGroupId, groupId);
        }

        [Fact]
        public void TestGetIdByNameShouldReturnNullWithInvalidName()
        {
            var groupsService = new GroupsService(context, null, null);
            var groupId = groupsService.GetIdByName("invalid name");
            Assert.Null(groupId);
        }

        [Fact]
        public async Task GetNameShouldReturnCorrectName()
        {
            var groupsService = new GroupsService(context, null, null);
            var originalGroupName = "Group Name";
            var groupId = await groupsService.CreateAsync("user id", originalGroupName);

            var groupName = groupsService.GetName(groupId);
            Assert.Equal(originalGroupName, groupName);
        }

        [Fact]
        public void GetNameShouldReturnNullWhenGroupIdIsInvalid()
        {
            var groupsService = new GroupsService(context, null, null);
            var groupName = groupsService.GetName("invalid group id");
            Assert.Null(groupName);
        }

        [Fact]
        public async Task GetOwnerIdShouldReturnCorrectOwnerId()
        {
            var groupsService = new GroupsService(context, null, null);
            var originalOwnerId = "user id";
            var groupId = await groupsService.CreateAsync(originalOwnerId, "Group Name");

            var ownerId = groupsService.GetOwnerId(groupId);
            Assert.Equal(originalOwnerId, ownerId);
        }

        [Fact]
        public async Task GetOwnerIdShouldReturnNullWhenGroupIdIsInvalid()
        {
            var groupsService = new GroupsService(context, null, null);
            await groupsService.CreateAsync("user id", "Group Name");

            var ownerId = groupsService.GetOwnerId("invalid group id");
            Assert.Null(ownerId);
        }

        [Fact]
        public async Task GroupExistsShouldReturnTrue()
        {
            var groupsService = new GroupsService(context, null, null);
            var groupId = await groupsService.CreateAsync("user id", "Group Name");

            var groupExists = groupsService.GroupExists(groupId);
            Assert.True(groupExists);
        }

        [Fact]
        public void GroupExistsShouldReturnFalse()
        {
            var groupsService = new GroupsService(context, null, null);
            var groupExists = groupsService.GroupExists("invalid group id");
            Assert.False(groupExists);
        }

        [Fact]
        public async Task IsGroupNameAvailableShouldReturnFalseWhenGroupWithNameExists()
        {
            var groupsService = new GroupsService(context, null, null);
            var groupName = "Group Name";
            var groupId = await groupsService.CreateAsync("user id", groupName);

            var isNameAvailable = groupsService.IsGroupNameAvailable(groupName);
            Assert.False(isNameAvailable);
        }

        [Fact]
        public void IsGroupNameAvailableShouldReturnTrueWhenThereIsNoGroupWithName()
        {
            var groupsService = new GroupsService(context, null, null);
            var isNameAvailable = groupsService.IsGroupNameAvailable("some name");
            Assert.True(isNameAvailable);
        }
    }
}
