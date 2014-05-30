using System;
using System.Linq;
using System.Web.Script.Serialization;

public partial class Management_UserRolesStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Ajancy.Kimia_Ajancy db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            if (Request.QueryString["mode"] != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string treeJSON = null;
                switch (Request.QueryString["mode"])
                {
                    case "0": // Province managers
                        var provinceManagers = from prv in db.Provinces
                                               join u in db.Users on prv.ProvinceID equals u.ProvinceID
                                               join ur in db.UsersInRoles on u.UserID equals ur.UserID
                                               where ur.RoleID == (byte)Public.Role.ProvinceManager
                                               select new
                                               {
                                                   key = ur.UserRoleID,
                                                   title = prv.Name,
                                                   icon = "city.png",
                                                   selected = ur.LockOutDate == null
                                               };

                        treeJSON = serializer.Serialize(provinceManagers);
                        break;

                    case "1": // City managers
                        var cityManagers = from prv in db.Provinces
                                           select new
                                           {
                                               key = 0,
                                               title = prv.Name,
                                               icon = "province.png",
                                               //selected = false,
                                               children = (from cty in db.Cities
                                                           join u in db.Users on cty.CityID equals u.CityID
                                                           join ur in db.UsersInRoles on u.UserID equals ur.UserID
                                                           where ur.RoleID == (byte)Public.Role.CityManager && cty.ProvinceID == prv.ProvinceID
                                                           orderby prv.Name, cty.Name
                                                           select new { key = ur.UserRoleID, title = cty.Name, icon = "city.png", selected = ur.LockOutDate == null })
                                           };

                        treeJSON = serializer.Serialize(cityManagers.Where(q => q.children.Any()));
                        break;

                    case "2": // Academy province managers
                        var academyProvinceManagers = from prv in db.Provinces
                                                      join u in db.Users on prv.ProvinceID equals u.ProvinceID
                                                      join ur in db.UsersInRoles on u.UserID equals ur.UserID
                                                      where ur.RoleID == (byte)Public.Role.AcademyProvince
                                                      select new
                                                      {
                                                          key = ur.UserRoleID,
                                                          title = prv.Name,
                                                          icon = "city.png",
                                                          selected = ur.LockOutDate == null
                                                      };

                        treeJSON = serializer.Serialize(academyProvinceManagers);
                        break;

                    case "3": // City managers
                        var academyCityManagers = from prv in db.Provinces
                                                  select new
                                                  {
                                                      key = 0,
                                                      title = prv.Name,
                                                      icon = "province.png",
                                                      //selected = false,
                                                      children = (from cty in db.Cities
                                                                  join u in db.Users on cty.CityID equals u.CityID
                                                                  join ur in db.UsersInRoles on u.UserID equals ur.UserID
                                                                  where ur.RoleID == (byte)Public.Role.AcademyCity && cty.ProvinceID == prv.ProvinceID
                                                                  orderby prv.Name, cty.Name
                                                                  select new { key = ur.UserRoleID, title = cty.Name, icon = "city.png", selected = ur.LockOutDate == null })
                                                  };

                        treeJSON = serializer.Serialize(academyCityManagers.Where(q => q.children.Any()));
                        break;
                }

                db.Dispose();
                Response.Clear();
                Response.Write(treeJSON);
                Response.End();
            }
            else if (Request.HttpMethod == "POST" && Request.Params["keys"] != null && Request.Params["status"] != null) // Save changes
            {
                string[] userRoleIds = Request.Params["keys"].Split(',');
                if (bool.Parse(Request.Params["status"]))
                {
                    foreach (string userRoleId in userRoleIds)
                    {
                        db.UsersInRoles.First<Ajancy.UsersInRole>(
                            role => role.UserRoleID == int.Parse(userRoleId)).LockOutDate = null;
                    }
                }
                else
                {
                    foreach (string userRoleId in userRoleIds)
                    {
                        db.UsersInRoles.First<Ajancy.UsersInRole>(
                            role => role.UserRoleID == int.Parse(userRoleId)).LockOutDate = DateTime.Now;
                    }
                }

                db.SubmitChanges();
                db.Dispose();
                Response.Clear();
                Response.Write('1');
                Response.End();
            }
        }
    }
}

