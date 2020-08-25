using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DVG.CK.OMS.Models;
using DVG.WIS.Business.Account;
using DVG.WIS.PublicModel.CMS;
using DVG.WIS.Utilities;
using DVG.WIS.Entities;
using DVG.WIS.Core.Enums;
using DVG.WIS.Core;
using Microsoft.AspNetCore.Authorization;
using DVG.WIS.Business.Authenticator;

namespace DVG.CK.OMS.Controllers
{
    [CustomAuthorizeAttribute(Policy = "RequireAdministratorRole")]
    public class UsersController : Controller
    {
        private IAccountBo _accountBo;

        public UsersController(IAccountBo accountBo)
        {
            //if (!UserService.IsAdmin) RedirectToAction("PermissionDenied", "Account");

            this._accountBo = accountBo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(UsersSearchModel searchModel)
        {
            ResponseData responseData = new ResponseData();
            var totalRows = 0;
            var listUsers = _accountBo.GetList(searchModel.Keyword, out totalRows, searchModel.UserType, searchModel.PageIndex, searchModel.PageSize);
            if (listUsers != null)
            {
                List<UsersModel> models = listUsers.Select(item => new UsersModel(item)).ToList();
                responseData.Data = models;
                responseData.TotalRow = totalRows;
                responseData.Success = true;
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.Success);
            }

            return Json(responseData);
        }

        [HttpPost]
        public ActionResult GetUserById(int userId)
        {
            ResponseData responseData = new ResponseData();
            WIS.Entities.Account users = _accountBo.GetById(userId);
            if (users != null)
            {
                UsersModel modelCms = new UsersModel(users);
                modelCms.Password = Crypton.Decrypt(modelCms.Password);
                responseData.Data = modelCms;
                responseData.Success = true;
                responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.Success);
            }

            return Json(responseData);
        }

        [HttpPost]
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

                responseData.Success = errorCode == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(errorCode);
                return Json(responseData);
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
                responseData.Success = errorCodes == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(errorCodes);
                return Json(responseData);
            }

            responseData.Success = false;
            responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.UserNameExisted);
            return Json(responseData);
        }

        [HttpPost]
        public ActionResult UpdateStatus(int userId)
        {
            ResponseData responseData = new ResponseData();
            WIS.Entities.Account usersModel = _accountBo.GetById(userId);
            if (usersModel != null)
            {

                int status = usersModel.Status == (int)UserStatusAdmin.Actived
                    ? (int)UserStatusAdmin.Deactived
                    : (int)UserStatusAdmin.Actived;

                usersModel.Status = status;
                ErrorCodes errorCodes = _accountBo.Update(usersModel);

                responseData.Success = errorCodes == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(errorCodes);
                return Json(responseData);
            }


            responseData.Success = false;
            responseData.Message = "";
            return Json(responseData);

        }

        [HttpPost]
        public ActionResult ResetPassWord(int userId, string passWord)
        {
            ResponseData responseData = new ResponseData();
            WIS.Entities.Account usersModel = _accountBo.GetById(userId);
            if (usersModel != null && !string.IsNullOrEmpty(passWord))
            {

                usersModel.Password = Crypton.Encrypt(passWord);
                ErrorCodes errorCodes = _accountBo.Update(usersModel);

                responseData.Success = errorCodes == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(errorCodes);
                return Json(responseData);
            }


            responseData.Success = false;
            responseData.Message = "";
            return Json(responseData);

        }

        [HttpPost]
        public ActionResult GeneratePassWord()
        {
            ResponseData responseData = new ResponseData();
            string strPass = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);

            responseData.Data = strPass;
            responseData.Success = true;
            responseData.Message = "";
            return Json(responseData);
        }

        [HttpPost]
        public JsonResult GetListUserType()
        {
            ResponseData responseData = new ResponseData();
            var usersModel = new UsersModel();

            usersModel.UsersTypeList = EnumHelper.Instance.ConvertEnumToList<UserTypeEnum>().ToList();
            responseData.Data = usersModel;
            responseData.Success = true;
            return Json(responseData);
        }

        [HttpPost]
        public JsonResult UpdateCatePermission(int userId, int userType)
        {
            ResponseData responseData = new ResponseData();

            WIS.Entities.Account usersModel = _accountBo.GetById(userId);
            if (usersModel != null && usersModel.Id > 0)
            {
                usersModel.UserType = userType;

                ErrorCodes errorCode = _accountBo.Update(usersModel);

                responseData.Success = errorCode == ErrorCodes.Success;
                responseData.Message = StringUtils.GetEnumDescription(errorCode);
                return Json(responseData);
            }

            responseData.Success = false;
            responseData.Message = StringUtils.GetEnumDescription(ErrorCodes.UserNameExisted);
            return Json(responseData);
        }
    }
}
