using System;
using System.Collections.Generic;
using Vanjaro.Common.Engines.UIEngine;
using JEOLUSA.UXManager.Extensions.Menu.RoleAssignment.Factories;

namespace JEOLUSA.UXManager.Extensions.Menu.RoleAssignment.Controllers
{
    public class UIController : UIEngineController
    {
        public override List<IUIData> GetData(string Identifier, Dictionary<string, string> Parameters)
        {
            switch ((AppFactory.Identifier)Enum.Parse(typeof(AppFactory.Identifier), Identifier))
            {
                case AppFactory.Identifier.setting_users:
                    return UserController.GetData(Identifier, Parameters);
                default:
                    break;
            }
            return base.GetData(Identifier, Parameters);
        }
        public override string AccessRoles()
        {
            return AppFactory.GetAccessRoles(UserInfo);
        }
        public override string AllowedAccessRoles(string Identifier)
        {
            return AppFactory.GetAllowedRoles(Identifier);
        }

    }
}