using Dnn.PersonaBar.Roles.Components;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Vanjaro.UXManager.Library.Common;

namespace JEOLUSA.UXManager.Extensions.Menu.RoleAssignment
{
    public static partial class Managers
    {
        public class UserManager
        {
            public static List<dynamic> GetAllUser(int PortalID, int PageIndex, out int totalRecords)
            {
                List<dynamic> users = new List<dynamic>();
                string query = @"select u.UserID,u.Username,u.Email,u.DisplayName,bs.Institution, l.Text as Country, CASE WHEN urw.userId IS NULL THEN 'False' ELSE 'True' END AS 'Waiting'

--Get Portal Users Only
from UserPortals up 

--Join w/Global Users
left join users u on up.UserId = u.UserID

--Join w/Profile to get Institution
left join (select UserId, PropertyValue as 'Institution' from UserProfile where PropertyDefinitionID = (select PropertyDefinitionID from ProfilePropertyDefinition where Portalid = " + PortalID + @" and PropertyName = 'Institution')) bs on bs.UserID = up.UserId

--Join w/Profile to get Country
left join (select UserId, PropertyValue as 'Country' from UserProfile where PropertyDefinitionID = (select PropertyDefinitionID from ProfilePropertyDefinition where Portalid = " + PortalID + @" and PropertyName = 'Country')) cn on cn.UserID = up.UserId
 
--Join w/UserRoles to get Admin Users
left join UserRoles ura on ura.UserID = up.UserId and ura.RoleID = (select RoleId from roles where portalid = " + PortalID + @" and rolename = 'Administrators')

--Join w/UserRoles to get Employees
left join UserRoles ure on ure.UserID = up.UserId and ure.RoleID = (select RoleId from roles where portalid = " + PortalID + @" and rolename = 'Employee')

--Join w/UserRoles to get Employees
left join UserRoles urd on urd.UserID = up.UserId and urd.RoleID = (select RoleId from roles where portalid = " + PortalID + @" and rolename = 'DELTA')

--Join w/UserRoles to get Employees
left join UserRoles urn on urn.UserID = up.UserId and urn.RoleID = (select RoleId from roles where portalid = " + PortalID + @" and rolename = 'NMR')

--Join w/UserRoles to get REGUSER
left join UserRoles urru on urru.UserID = up.UserId and urru.RoleID = (select RoleId from roles where portalid = " + PortalID + @" and rolename = 'REGUSER')

--Join w/UserRoles to get REGSPEC
left join UserRoles urrs on urrs.UserID = up.UserId and urrs.RoleID = (select RoleId from roles where portalid = " + PortalID + @" and rolename = 'REGSPEC')

--Join w/UserRoles to get Waiting
left join UserRoles urw on urw.UserID = up.UserId and urw.RoleID = (select RoleId from roles where portalid = " + PortalID + @" and rolename = 'Waiting')

--Join w/Lists to get Country
left join Lists l on l.EntryID = Convert(int,cn.Country)

--Filter by PortalID , active users only, non adminstrators, non employees, non delta, non nmr, Is RegUser, Is RegSpec
where up.portalid = " + PortalID + @" and up.IsDeleted = 0 and u.IsDeleted = 0 and ura.UserID IS NULL and ure.UserID IS NULL and urd.UserID IS NULL and urn.UserID IS NULL --and urru.UserID IS NOT NULL and urrs.UserID IS NOT NULL

--Order by Last Registeration Date
order by up.createddate desc

--Pagination
OFFSET " + PageIndex * 13 + @" ROWS FETCH NEXT 13 ROWS ONLY;";

                string querycount = @"select count(u.UserID)

--Get Portal Users Only
from UserPortals up 

--Join w/Global Users
left join users u on up.UserId = u.UserID

--Join w/Profile to get Institution
left join (select UserId, PropertyValue as 'Institution' from UserProfile where PropertyDefinitionID = (select PropertyDefinitionID from ProfilePropertyDefinition where Portalid = " + PortalID + @" and PropertyName = 'Institution')) bs on bs.UserID = up.UserId

--Join w/Profile to get Country
left join (select UserId, PropertyValue as 'Country' from UserProfile where PropertyDefinitionID = (select PropertyDefinitionID from ProfilePropertyDefinition where Portalid = " + PortalID + @" and PropertyName = 'Country')) cn on cn.UserID = up.UserId
 
--Join w/UserRoles to get Admin Users
left join UserRoles ura on ura.UserID = up.UserId and ura.RoleID = (select RoleId from roles where portalid = " + PortalID + @" and rolename = 'Administrators')

--Join w/UserRoles to get Employees
left join UserRoles ure on ure.UserID = up.UserId and ure.RoleID = (select RoleId from roles where portalid = " + PortalID + @" and rolename = 'Employee')

--Join w/UserRoles to get Employees
left join UserRoles urd on urd.UserID = up.UserId and urd.RoleID = (select RoleId from roles where portalid = " + PortalID + @" and rolename = 'DELTA')

--Join w/UserRoles to get Employees
left join UserRoles urn on urn.UserID = up.UserId and urn.RoleID = (select RoleId from roles where portalid = " + PortalID + @" and rolename = 'NMR')

--Join w/UserRoles to get REGUSER
left join UserRoles urru on urru.UserID = up.UserId and urru.RoleID = (select RoleId from roles where portalid = " + PortalID + @" and rolename = 'REGUSER')

--Join w/UserRoles to get REGSPEC
left join UserRoles urrs on urrs.UserID = up.UserId and urrs.RoleID = (select RoleId from roles where portalid = " + PortalID + @" and rolename = 'REGSPEC')

--Join w/UserRoles to get Waiting
left join UserRoles urw on urw.UserID = up.UserId and urw.RoleID = (select RoleId from roles where portalid = " + PortalID + @" and rolename = 'Waiting')

--Join w/Lists to get Country
left join Lists l on l.EntryID = Convert(int,cn.Country)

--Filter by PortalID , active users only, non adminstrators, non employees, non delta, non nmr, Is RegUser, Is RegSpec
where up.portalid = " + PortalID + @" and up.IsDeleted = 0 and u.IsDeleted = 0 and ura.UserID IS NULL and ure.UserID IS NULL and urd.UserID IS NULL and urn.UserID IS NULL --and urru.UserID IS NOT NULL and urrs.UserID IS NOT NULL";

                totalRecords = Vanjaro.Common.Data.Entities.CommonLibraryRepo.GetInstance().Query<int>(querycount).FirstOrDefault();
                users = Vanjaro.Common.Data.Entities.CommonLibraryRepo.GetInstance().Query<dynamic>(query).ToList();
                return users;
            }

            internal static ActionResult Update(int PortalID, dynamic Data)
            {
                ActionResult actionResult = new ActionResult();
                try
                {
                    using (var db = new Vanjaro.Common.Data.Entities.CommonLibraryRepo())
                    {
                        foreach (var item in Data)
                        {
                            UserInfo user = UserController.GetUserById(PortalID, int.Parse(item.UserID.ToString()));
                            RoleController rc = new RoleController();

                            if (item.Delete == "True")
                                UserController.DeleteUser(ref user, true, false);

                            if (item.Delta == "True")
                            {
                                RoleInfo roleinfo = rc.GetRoleByName(PortalID, "Delta");
                                rc.AddUserRole(PortalID, user.UserID, roleinfo.RoleID, Null.NullDate, Null.NullDate);
                            }

                            if (item.NMR == "True")
                            {
                                RoleInfo roleinfo = rc.GetRoleByName(PortalID, "NMR");
                                rc.AddUserRole(PortalID, user.UserID, roleinfo.RoleID, Null.NullDate, Null.NullDate);
                            }

                            if (item.Waiting == "True")
                            {
                                RoleInfo roleinfo = rc.GetRoleByName(PortalID, "Waiting");
                                rc.AddUserRole(PortalID, user.UserID, roleinfo.RoleID, Null.NullDate, Null.NullDate);
                            }
                        }
                    }

                    actionResult.IsSuccess = true;
                    return actionResult;
                }
                catch (Exception ex)
                {
                    actionResult.AddError("", "", ex);
                    return actionResult;
                }
            }

        internal static bool IsAdmin(UserInfo user, PortalSettings portalSettings)
            {
                return user.IsSuperUser || user.IsInRole(portalSettings.AdministratorRoleName);
            }

            internal static bool IsAdmin(PortalSettings portalSettings)
            {
                UserInfo user = UserController.Instance.GetCurrentUserInfo();
                return user.IsSuperUser || user.IsInRole(portalSettings.AdministratorRoleName);
            }
            internal static bool AllowExpired(int userId, int roleId, PortalSettings PortalSettings)
            {
                return userId != PortalSettings.AdministratorId || roleId != PortalSettings.AdministratorRoleId;
            }
        }
    }
}