using DotNetNuke.Entities.Users;
using System;
using System.Collections.Generic;
using Vanjaro.Common.Engines.UIEngine.AngularBootstrap;
using Vanjaro.Common.Entities.Apps;
using Vanjaro.Common.Utilities;
using Vanjaro.Core.Entities.Menu;
using JEOLUSA.UXManager.Extensions.Menu.RoleAssignment.Components;
using JEOLUSA.UXManager.Extensions.Menu.RoleAssignment.Factories;
using Vanjaro.UXManager.Library;
using Vanjaro.UXManager.Library.Entities.Interface;
using Vanjaro.UXManager.Library.Entities.Menu;
using Vanjaro.UXManager.Library.Entities.Shortcut;
using static Vanjaro.Common.FrameworkManager;
using static Vanjaro.Core.Managers;

namespace JEOLUSA.UXManager.Extensions.Menu.RoleAssignment
{
    public class RoleAssignment : IMenuItem
    {
        public List<MenuItem> Items
        {
            get
            {
                List<MenuItem> ListItems = new List<MenuItem>();
                ListItems.Add(new MenuItem
                {
                    Text = Localization.Get(ExtensionInfo.FriendlyName, "Text", Constants.LocalResourcesFile, Extension.ShowMissingKeysStatic, Localization.SharedMissingPrefix),
                    ItemGuid = Guid.Parse(ExtensionInfo.GUID),
                    Icon = "fas fa-user-shield",
                    ViewOrder = 900,

                    Hierarchy = new MenuItem
                    {
                        Text = Localization.Get("Site", "Text", Constants.LocalResourcesFile, Extension.ShowMissingKeysStatic, Localization.SharedMissingPrefix),
                        Hierarchy = null,
                        BelowBreakLine = true
                    }
                });
                return ListItems;
            }
        }



        public Guid SettingGuid => Guid.Parse(ExtensionInfo.GUID);

        public string UIPath => "~/DesktopModules/Vanjaro/UXManager/Extensions/Menu/" + ExtensionInfo.Name + "/Views/";

        public string AppCssPath => "~/DesktopModules/Vanjaro/UXManager/Extensions/Menu/" + ExtensionInfo.Name + "/Resources/Stylesheets/app.css";
        public string AppJsPath => "~/DesktopModules/Vanjaro/UXManager/Extensions/Menu/" + ExtensionInfo.Name + "/Resources/Scripts/app.js";

        public string UIEngineAngularBootstrapPath => "~/DesktopModules/Vanjaro/UXManager/Extensions/Menu/" + ExtensionInfo.Name + "/Resources/UIEngine/AngularBootstrap";
        public string[] Dependencies => new string[] {
                    JavaScriptPlugins.ValidationJS.ToString(),
                    AngularPlugins.AutoComplete.ToString(),
                    AngularPlugins.Grid.ToString(),
                    JavaScriptPlugins.BootstrapDatepicker.ToString(),
                    "Bootstrap"
               };


        public AppInformation App => AppFactory.GetAppInformation();

        public List<AngularView> AngularViews => AppFactory.GetViews();

        public string Icon => "fa fa-cog";

        public bool Visibility => Factories.AppFactory.GetAccessRoles(UserController.Instance.GetCurrentUserInfo()).Contains("admin") && AccessManager.HasAccess(Vanjaro.Core.Entities.Enum.Feature.Core_Memberships);

        public string SearchKeywords => Localization.Get("SearchKeywords", "Text", Constants.LocalResourcesFile, Extension.ShowMissingKeysStatic, Localization.SharedMissingPrefix);

        public MenuAction Event => MenuAction.RightOverlay;

        public int SortOrder => 40;

        public int? Width => 900;

        public string AccessRoles(UserInfo userInfo)
        {
            return Factories.AppFactory.GetAccessRoles(userInfo);
        }

        #region IShortcut       
        public ShortcutItem Shortcut => new ShortcutItem()
        {
            Text = Localization.Get("Shortcut_Text", "Text", Constants.LocalResourcesFile, Extension.ShowMissingKeysStatic, Localization.SharedMissingPrefix),
            ViewOrder = 60,
            URL = ExtensionInfo.GUID + "#!/add",
            Title = Localization.Get("Shortcut_Title", "Text", Constants.LocalResourcesFile, Extension.ShowMissingKeysStatic, Localization.SharedMissingPrefix),
            Breakline = true,
            Width = Width,
            Action = Event,
            Icon = "fas fa-user-shield",
            Visibility = Visibility
        };
        #endregion
    }
}