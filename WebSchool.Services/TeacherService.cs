﻿using System.Linq;
using WebSchool.Data;
using WebSchool.ViewModels.User;
using System.Collections.Generic;
using WebSchool.Services.Contracts;

namespace WebSchool.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ApplicationDbContext context;
        private readonly IRolesService rolesService;

        public TeacherService(ApplicationDbContext context, IRolesService rolesService)
        {
            this.context = context;
            this.rolesService = rolesService;
        }

        //public TeacherViewModel GetTeacher(string id)
        //{
        //    return this.context.Users
        //        .Where(x => x.Id == id)
        //        .Select(x => new TeacherViewModel()
        //        {
        //            Id = x.Id,
        //            FirstName = x.FirstName,
        //            LastName = x.LastName,
        //            Classes = this.classesService.GetTeacherAssignedClasses(x.Id)
        //        })
        //        .FirstOrDefault();
        //}

        //public ICollection<UsersViewModel> GetTeachers(string schoolId)
        //{
        //    return this.context.Users
        //        .Where(x => x.SchoolId == schoolId)
        //        .Select(x => new UsersViewModel()
        //        {
        //            Id = x.Id,
        //            FirstName = x.FirstName,
        //            LastName = x.LastName,
        //            Email = x.Email,
        //            Role = this.rolesService.GetUserRole(x.Id)
        //        })
        //        .ToList()
        //        .Where(x => x.Role == "Teacher")
        //        .ToList();
        //}
    }
}
