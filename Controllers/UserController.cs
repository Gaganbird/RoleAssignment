using Dnn.PersonaBar.Roles.Components;
using Dnn.PersonaBar.Roles.Services.DTO;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Vanjaro.Common.ASPNET.WebAPI;
using Vanjaro.Common.Engines.UIEngine;
using Vanjaro.Common.Permissions;
using Vanjaro.Core.Components.Attributes;
using Vanjaro.Core.Entities.Enum;
using JEOLUSA.UXManager.Extensions.Menu.RoleAssignment.Entities;
using Vanjaro.UXManager.Library.Common;
using static Vanjaro.Core.Managers;
using static JEOLUSA.UXManager.Extensions.Menu.RoleAssignment.Managers;

namespace JEOLUSA.UXManager.Extensions.Menu.RoleAssignment.Controllers
{
    [ValidateAntiForgeryToken]
    [AuthorizeAccessRoles(AccessRoles = "admin")]
    public class UserController : UIEngineController
    {
        internal static List<IUIData> GetData(string identifier, Dictionary<string, string> parameters)
        {
            UserController rc = new UserController();
            Dictionary<string, IUIData> Settings = new Dictionary<string, IUIData>();
            switch (identifier)
            {
                case "setting_users":
                    {
                        Settings.Add("AllUsers", new UIData { Name = "AllUsers", Options = null, Value = "" });
                        break;
                    }
               
            }
            return Settings.Values.ToList();
        }

        [HttpGet]
        public ActionResult GetUsers(int pageIndex, int pageSize)
        {
            ActionResult actionResult = new ActionResult();
            try
            {
                var response = new
                {
                    Results = UserManager.GetAllUser(UserInfo.PortalID, pageIndex, out int totalRecords),
                    TotalResults = totalRecords
                };
                actionResult.Data = response;
            }
            catch (Exception ex)
            {
                actionResult.AddError("GetUsers_Exception", ex.Message);
            }
            return actionResult;
        }

        [HttpPost]
        public ActionResult Save(List<dynamic> Data)
        {
            return UserManager.Update(PortalSettings.PortalId, Data);
        }
        public override string AccessRoles()
        {
            return Factories.AppFactory.GetAccessRoles(UserInfo);
        }
    }
}