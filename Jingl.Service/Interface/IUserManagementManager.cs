using Jingl.General.Model.Admin.UserManagement;
using Jingl.General.Model.Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jingl.Service.Interface
{
    public interface IUserManagementManager
    {

        IList<RoleMenuViewModel> BuildRoleMenu(int roleid);
        bool CheckMenuForFoles(int roleid,string ControllerName);
        UserModel CheckValidUser(UserModel model);
        UserModel CheckValidUserExternal(UserModel model);
        UserModel CreateUser(UserModel model);
        bool VerifyCodeUser(UserModel model);
        void SetVerificationCode(UserModel model);
        UserModel UpdateUser(UserModel model);
        UserModel SignUpTypeUpdate(UserModel model);
        UserModel GetUser(UserModel model);
        UserModel GetUserProfiles(int UserId);
        IList<UserModel> GetAllUser();
        IList<UserModel> AdmGetAllUser();
        IList<RoleModel> GetAllRole();
        RoleModel GetRole(RoleModel model);
        RoleModel CreateRoleData(RoleModel model);
        RoleModel UpdateRoleData(RoleModel model);
        RoleAccessMenuModel CreateRoleAccessMenu(RoleAccessMenuModel model);
        void DeleteAccessMenu(int id);
        IList<MenuModel> GetAllMenu();
        IList<RoleAccessMenuModel> GetAllRoleAccess();
        void DeleteUser(int id);
        void DeleteRole(int id);
        IList<UserModel> GetUserByEmail(UserModel model);
        IList<UserModel> GetAllUserForDropdown();




    }
}
