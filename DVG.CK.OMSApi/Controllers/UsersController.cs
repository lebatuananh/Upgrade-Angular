using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DVG.CK.OMSApi.Filter;
using DVG.WIS.Business.Account;
using DVG.WIS.Business.Authenticator;
using DVG.WIS.Business.Order;
using DVG.WIS.Business.Product;
using DVG.WIS.Core.Enums;
using DVG.WIS.DAL.District;
using DVG.WIS.DAL.Ward;
using DVG.WIS.Entities;
using DVG.WIS.PublicModel;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DVG.CK.OMSApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [CustomizeAuthorizeAttribute(AdminRole)]
    public class UsersController : BaseController
    {
        private IAccountBo _accountBo;

        public UsersController(IAccountBo accountBo, IUserService userService, IHttpContextAccessor httpContextAccessor) : base(userService, httpContextAccessor)
        {
            //if (!UserService.IsAdmin) RedirectToAction("PermissionDenied", "Account");

            this._accountBo = accountBo;
        }

        [Route("search")]
        [HttpPost]
       
        public ActionResult Search(UsersSearchModel searchModel)
        {
            try
            {
                var totalRows = 0;
                var listUsers = _accountBo.GetList(searchModel.Keyword, out totalRows, searchModel.UserType, searchModel.PageIndex, searchModel.PageSize);
                if (listUsers != null)
                {
                    List<UsersModel> models = listUsers.Select(item => new UsersModel(item)).ToList();
                    Msg.Error = false;
                    var pager = new Pager { CurrentPage = searchModel.PageIndex, PageSize = searchModel.PageSize, TotalItem = totalRows };
                
                    Msg.Obj = new { Data = models, Pager = pager };
                }
                else
                {
                    Msg.Obj = new { Data = listUsers, Pager = new Pager() };
                }
            }
            catch (Exception ex)
            {
                Msg.Obj = null;
                Msg.Error = true;
            }
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("getuserbyid")]
        public ActionResult GetUserById(int userId)
        {
            WIS.Entities.Account users = _accountBo.GetById(userId);
            if (users != null)
            {
                UsersModel modelCms = new UsersModel(users);
                modelCms.Password = Crypton.Decrypt(modelCms.Password);
                Msg.Obj = modelCms;
            }

            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("update")]
        public ActionResult Update(UsersModel users)
        {
            ResponseData responseData = new ResponseData();

            WIS.Entities.Account usersModel = _accountBo.GetById(users.UserId);
            if (usersModel != null && usersModel.Id > 0)
            {
                bool isSendMail = !usersModel.Email.Equals(users.Email);
                usersModel.FullName = users.FullName;
                usersModel.Email = users.Email;
                usersModel.Birthday = users.Birthday;
                usersModel.Avatar = users.Avatar;
                usersModel.Address = users.Address;
                usersModel.Gender = users.Gender;
                usersModel.Phone = users.Mobile;
                usersModel.UserType = users.UserType.Value;

                ErrorCodes errorCode = _accountBo.Update(usersModel);

                Msg.Error = errorCode != ErrorCodes.Success;
                Msg.Title = StringUtils.GetEnumDescription(errorCode);
                return AuthorizeJson(Msg);
            }

            WIS.Entities.Account usersByName = _accountBo.GetByUserName(users.UserName);
            if (usersByName == null || usersByName.Id <= 0)
            {
                string strPass = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
                WIS.Entities.Account usersModelTemp = new Account();
                usersModelTemp.UserName = users.UserName;
                usersModelTemp.Password = Crypton.Encrypt(strPass);
                usersModelTemp.FullName = users.FullName;
                usersModelTemp.Email = users.Email;
                usersModelTemp.Birthday = users.Birthday;
                usersModelTemp.Avatar = users.Avatar;
                usersModelTemp.Address = users.Address;
                usersModelTemp.PasswordQuestion = string.Empty;
                usersModelTemp.PasswordAnswer = string.Empty;
                usersModelTemp.CreatedDate = DateTime.Now;
                usersModelTemp.Status = (int)UserStatusAdmin.Actived;
                usersModelTemp.Gender = users.Gender;
                usersModelTemp.Phone = users.Mobile;
                usersModelTemp.UserType = users.UserType.Value;

                ErrorCodes errorCodes = _accountBo.Insert(usersModelTemp);
                Msg.Error = errorCodes != ErrorCodes.Success;
                Msg.Title = StringUtils.GetEnumDescription(errorCodes);
                return AuthorizeJson(Msg);
            }

            Msg.Error = true;
            Msg.Title = StringUtils.GetEnumDescription(ErrorCodes.UserNameExisted);
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("updatestatus")]
        public ActionResult UpdateStatus(int userId)
        {
            WIS.Entities.Account usersModel = _accountBo.GetById(userId);
            if (usersModel != null)
            {

                int status = usersModel.Status == (int)UserStatusAdmin.Actived
                    ? (int)UserStatusAdmin.Deactived
                    : (int)UserStatusAdmin.Actived;

                usersModel.Status = status;
                ErrorCodes errorCodes = _accountBo.Update(usersModel);

                Msg.Error = errorCodes != ErrorCodes.Success;
                Msg.Title = StringUtils.GetEnumDescription(errorCodes);
                return AuthorizeJson(Msg);
            }


            Msg.Error = true;
            Msg.Title = "";
            return AuthorizeJson(Msg);

        }

        [HttpPost]
        [Route("resetpassword")]
        public ActionResult ResetPassWord(int userId, string passWord)
        {
            WIS.Entities.Account usersModel = _accountBo.GetById(userId);
            if (usersModel != null && !string.IsNullOrEmpty(passWord))
            {

                usersModel.Password = Crypton.Encrypt(passWord);
                ErrorCodes errorCodes = _accountBo.Update(usersModel);

                Msg.Error = errorCodes != ErrorCodes.Success;
                Msg.Title = StringUtils.GetEnumDescription(errorCodes);
                return AuthorizeJson(Msg);
            }


            Msg.Error = false;
            Msg.Title = "";
            return AuthorizeJson(Msg);

        }

        [HttpPost]
        [Route("generatepassword")]
        public ActionResult GeneratePassWord()
        {
            string strPass = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);

            Msg.Obj = strPass;
            Msg.Error = false;
            Msg.Title = "";
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("getlistusertype")]
        public JsonResult GetListUserType()
        {
            var usersModel = new UsersModel();

            usersModel.UsersTypeList = EnumHelper.Instance.ConvertEnumToList<UserTypeEnum>().ToList();
            Msg.Obj = usersModel;
            Msg.Error = false;
            return AuthorizeJson(Msg);
        }

        [HttpPost]
        [Route("updatecatepermission")]
        public JsonResult UpdateCatePermission(int userId, int userType)
        {
            WIS.Entities.Account usersModel = _accountBo.GetById(userId);
            if (usersModel != null && usersModel.Id > 0)
            {
                usersModel.UserType = userType;

                ErrorCodes errorCode = _accountBo.Update(usersModel);

                Msg.Error = errorCode != ErrorCodes.Success;
                Msg.Title = StringUtils.GetEnumDescription(errorCode);
                return AuthorizeJson(Msg);
            }

            Msg.Error = false;
            Msg.Title = StringUtils.GetEnumDescription(ErrorCodes.UserNameExisted);
            return AuthorizeJson(Msg);
        }
    }
}
