using System;
using System.Collections.Generic;
using System.Text;

using Jingl.Service.Interface;
using Jingl.Admin.UserManagement.Model.Dao;
using Jingl.General.Model.Admin.ViewModel;
using Jingl.General.Utility;
using Microsoft.Extensions.Configuration;
using Jingl.General.Model.Admin.UserManagement;
using Jingl.UserManagement.Model.Dao;

namespace Jingl.Service.Manager
{
    public class UserManagementManager : IUserManagementManager
    {

        private readonly RolesAccMenuDao RolesAccMenuDao;
        private readonly UserDao UserDao;
        private readonly IConfiguration _config;
        private readonly Logger _logger;
        private readonly RoleDao RoleDao;
        private readonly MenuDao MenuDao;



        public UserManagementManager(IConfiguration _config)
        {
            this.RoleDao = new RoleDao(_config);
            this._config = _config;
            this.RolesAccMenuDao = new RolesAccMenuDao(_config);
            this.UserDao = new UserDao(_config);
            this._logger = new Logger(_config);
            this.MenuDao = new MenuDao(_config);
        }

        public string DestinationLogFolder()
        {
            return _config.GetSection("Logging:DestinationFolder:Service").Value.ToString();
        }


        public IList<RoleMenuViewModel> BuildRoleMenu(int roleid)
        {

            try
            {
                var data = RolesAccMenuDao.BuildRoleMenu(roleid);
                return data;
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "BuildRoleMenu", ex.Message, "Service");
                return null;
            }

        }

        public bool CheckMenuForFoles(int roleid,string ControllerName)
        {

            try
            {
                var data = RolesAccMenuDao.CheckMenuForFoles(roleid, ControllerName);
                return data;
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CheckMenuForFoles", ex.Message, "Service");
                return false;
            }

        }

        public UserModel CheckValidUser(UserModel model)
        {
            try
            {
                var data = UserDao.CheckValidUser(model);
                return data;
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CheckValidUser", ex.Message, "Service");
                return null;
            }

        }

        

        public UserModel CheckValidUserExternal(UserModel model)
        {
            try
            {
                var data = UserDao.CheckValidUserExternal(model);
                return data;
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CheckValidUserExternal", ex.Message, "Service");
                return null;
            }

        }


        public bool VerifyCodeUser(UserModel model)
        {
            try
            {
                var data = UserDao.VerifyUserCode(model);
                return data;
            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "VerifyCodeUser", ex.Message, "Service");
                return false;
            }

        }


        public UserModel UpdateUser(UserModel model)
        {
            var data = new UserModel();

            try
            {
                data = UserDao.UpdateUser(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateUser", ex.Message, "Service");

            }

            return data;

        }

        public UserModel SignUpTypeUpdate(UserModel model)
        {
            var data = new UserModel();

            try
            {
                data = UserDao.SignUpTypeUpdate(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "SignUpTypeUpdate", ex.Message, "Service");

            }

            return data;

        }

        public UserModel GetUser(UserModel model)
        {
            var data = new UserModel();

            try
            {
                data = UserDao.GetUser(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetUser", ex.Message, "Service");

            }

            return data;

        }


        public UserModel CreateUser(UserModel model)
        {
            var data = new UserModel();

            try
            {
               data = UserDao.CreateUser(model);
               

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateUser", ex.Message, "Service");
              
            }

            return data;

        }

        public void SetVerificationCode(UserModel model)
        {
           

            try
            {
               UserDao.SetVerificationCode(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "SetVerificationCode", ex.Message, "Service");

            }

         

        }

        public UserModel GetUserProfiles(int UserId)
        {
            var data = new UserModel();

            try
            {
                data = UserDao.GetUserProfiles(UserId);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetUserProfiles", ex.Message, "Service");

            }

            return data;
        }

        public IList<UserModel> GetAllUser()
        {
            IList<UserModel> data = new List<UserModel>();

            try
            {
                data =  UserDao.GetAllUser();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllUser", ex.Message, "Service");

            }

            return data;
        }

        public IList<UserModel> AdmGetAllUser()
        {
            IList<UserModel> data = new List<UserModel>();

            try
            {
                data = UserDao.AdmGetAllUser();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "AdmGetAllUser", ex.Message, "Service");

            }

            return data;
        }

        public IList<RoleModel> GetAllRole()
        {
            IList<RoleModel> data = new List<RoleModel>();

            try
            {
                data = RoleDao.GetAllRole();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllRole", ex.Message, "Service");

            }

            return data;
        }

        public RoleModel GetRole(RoleModel model)
        {
            var data = new RoleModel();

            try
            {
                data = RoleDao.GetRole(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetRole", ex.Message, "Service");

            }

            return data;
        }

        public RoleModel CreateRoleData(RoleModel model)
        {
            var data = new RoleModel();

            try
            {
                data = RoleDao.CreateRoleData(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateRoleData", ex.Message, "Service");

            }

            return data;
        }

        public RoleModel UpdateRoleData(RoleModel model)
        {
            var data = new RoleModel();

            try
            {
                data = RoleDao.UpdateRoleData(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "UpdateRoleData", ex.Message, "Service");

            }

            return data;
        }

        public RoleAccessMenuModel CreateRoleAccessMenu(RoleAccessMenuModel model)
        {
            var data = new RoleAccessMenuModel();

            try
            {
                data = RolesAccMenuDao.CreateRoleAccessMenu(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "CreateRoleAccessMenu", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteAccessMenu(int id)
        {
           
            try
            {
              RolesAccMenuDao.DeleteAccessMenu(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteAccessMenu", ex.Message, "Service");

            }

           
        }

        public IList<MenuModel> GetAllMenu()
        {
            IList<MenuModel> data = new List<MenuModel>();

            try
            {
                data = MenuDao.GetAllMenu();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllMenu", ex.Message, "Service");

            }

            return data;
        }

        public IList<RoleAccessMenuModel> GetAllRoleAccess()
        {
            IList<RoleAccessMenuModel> data = new List<RoleAccessMenuModel>();

            try
            {
                data = RolesAccMenuDao.GetAllRoleAccess();

            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllRoleAccess", ex.Message, "Service");

            }

            return data;
        }

        public void DeleteUser(int id)
        {
            try
            {
                UserDao.DeleteUser(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteUser", ex.Message, "Service");

            }
        }

        public void DeleteRole(int id)
        {
            try
            {
                RoleDao.DeleteRole(id);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "DeleteRole", ex.Message, "Service");

            }
        }

        public IList<UserModel> GetUserByEmail(UserModel model)
        {
            //var data = new UserModel();

            IList<UserModel> data = new List<UserModel>();

            try
            {
                data = UserDao.GetUserByEmail(model);


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetUserByEmail", ex.Message, "Service");

            }

            return data;
        }

        public IList<UserModel> GetAllUserForDropdown()
        {
            IList<UserModel> data = new List<UserModel>();

            try
            {
                data = UserDao.GetAllUserForDropdown();


            }
            catch (Exception ex)
            {
                _logger.WriteFunctionLog(DestinationLogFolder(), "", "GetAllUserForDropdown", ex.Message, "Service");

            }

            return data;
        }
    }
}
