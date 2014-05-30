using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Management_UserRoles : System.Web.UI.Page
{
    Ajancy.Kimia_Ajancy db = null;
    DataLoadOptions dlo = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int personId = 0;
            List<short> userRoles = new List<short>();
            if (int.TryParse(TamperProofString.QueryStringDecode(Request.QueryString["id"]), out personId)) // Edit mode
            {
                switch (Public.ActiveUserRole.RoleID)
                {
                    case (short)Public.Role.ProvinceManager:
                        this.drpRoles.Items.Add(new ListItem("مدیر آژانس", "2"));
                        this.drpRoles.Items.Add(new ListItem("مدیر آژانس های شهرستان", "9"));
                        break;

                    case (short)Public.Role.CityManager:
                        this.drpRoles.Items.Add(new ListItem("مدیر آژانس", "2"));
                        break;

                    case (short)Public.Role.Admin:
                        this.drpRoles.Items.Add(new ListItem("مدیر آژانس", "2"));
                        this.drpRoles.Items.Add(new ListItem("مدیر سیستم", "1"));
                        this.drpRoles.Items.Add(new ListItem("مدیر آژانس های شهرستان", "9"));
                        this.drpRoles.Items.Add(new ListItem("مدیر آژانس های استان", "10"));
                        this.drpRoles.Items.Add(new ListItem("مدیر آموزشگاه های شهرستان", "13"));
                        this.drpRoles.Items.Add(new ListItem("مدیر آموزشگاه های استان", "14"));
                        break;
                }
                this.drpRoles.Items.Insert(0, new ListItem("- انتخاب کنید -", "0"));

                dlo = new DataLoadOptions();
                dlo.LoadWith<Ajancy.Person>(p => p.User);
                dlo.LoadWith<Ajancy.User>(u => u.UsersInRoles);
                dlo.LoadWith<Ajancy.UsersInRole>(ur => ur.AjancyPartners);
                dlo.LoadWith<Ajancy.AjancyPartner>(jp => jp.Ajancy);
                db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
                db.LoadOptions = dlo;
                Ajancy.Person person = db.Persons.FirstOrDefault<Ajancy.Person>(p => p.PersonID == personId);
                this.ViewState["UserID"] = person.User.UserID;
                this.tdTitle.InnerHtml = string.Format("سمت های کاربر : <b>{0} {1}</b>", person.FirstName, person.LastName);

                ListItem item = null;
                foreach (Ajancy.UsersInRole ur in person.User.UsersInRoles)
                {
                    switch ((Public.Role)ur.RoleID)
                    {
                        case Public.Role.AjancyManager:
                            foreach (Ajancy.AjancyPartner partner in ur.AjancyPartners)
                            {
                                item = new ListItem(string.Format("{0} {1}", Public.GetRoleName(ur.RoleID), partner.Ajancy != null ? partner.Ajancy.AjancyName : "---"), string.Format("{0}|{1}", ur.UserRoleID, partner.AjancyPartnerID));
                                item.Selected = partner.LockOutDate == null ? true : false;
                                this.lstRoles.Items.Add(item);
                            }
                            userRoles.Add(ur.RoleID);
                            break;

                        case Public.Role.ProvinceManager:
                        case Public.Role.AcademyProvince:
                            item = new ListItem(string.Format("{0} {1}", Public.GetRoleName(ur.RoleID), person.User.City.Province.Name), string.Format("{0}|0", ur.UserRoleID));
                            item.Selected = ur.LockOutDate == null ? true : false;
                            this.lstRoles.Items.Add(item);
                            userRoles.Add(ur.RoleID);
                            break;

                        case Public.Role.CityManager:
                        case Public.Role.AcademyCity:
                            item = new ListItem(string.Format("{0} {1}", Public.GetRoleName(ur.RoleID), person.User.City.Name), string.Format("{0}|0", ur.UserRoleID));
                            item.Selected = ur.LockOutDate == null ? true : false;
                            this.lstRoles.Items.Add(item);
                            userRoles.Add(ur.RoleID);
                            break;

                        default:
                            item = new ListItem(Public.GetRoleName(ur.RoleID), string.Format("{0}|0", ur.UserRoleID));
                            item.Selected = ur.LockOutDate == null ? true : false;
                            this.lstRoles.Items.Add(item);
                            userRoles.Add(ur.RoleID);
                            break;
                    }
                    item = null;
                }
            }
            else
            {
                DisposeContext();
                Response.Redirect("~/Management/UsersList.aspx");
            }

            foreach (short item in userRoles)
            {
                this.drpRoles.Items.Remove(this.drpRoles.Items.FindByValue(item.ToString()));
            }
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        DisposeContext();
    }

    protected void lstRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
        foreach (ListItem item in this.lstRoles.Items)
        {
            string[] roleParts = item.Value.Split('|');
            if (roleParts[1] == "0")
            {
                Ajancy.UsersInRole userRole = db.UsersInRoles.First<Ajancy.UsersInRole>(ur => ur.UserRoleID == Public.ToInt(roleParts[0]));
                if (item.Selected && userRole.LockOutDate.HasValue) // unlock this role
                {
                    userRole.LockOutDate = null;
                    db.SubmitChanges();
                    break;
                }
                else if (!item.Selected && !userRole.LockOutDate.HasValue) // lock this role
                {
                    userRole.LockOutDate = DateTime.Now;
                    db.SubmitChanges();
                    break;
                }
            }
            else if (roleParts.Length == 2) // Ajancy partners
            {
                Ajancy.AjancyPartner partner = db.AjancyPartners.First<Ajancy.AjancyPartner>(jp => jp.AjancyPartnerID == Public.ToInt(roleParts[1]));
                if (item.Selected && partner.LockOutDate.HasValue) // unlock this role
                {
                    partner.LockOutDate = null;
                    db.SubmitChanges();
                    break;
                }
                else if (!item.Selected && !partner.LockOutDate.HasValue) // lock this role
                {
                    partner.LockOutDate = DateTime.Now;
                    db.SubmitChanges();
                    break;
                }
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (this.Page.IsValid && this.ViewState["UserID"] != null)
        {
            byte selectedRoleId = byte.Parse(this.drpRoles.SelectedValue);
            ListItem item = null;
            db = new Ajancy.Kimia_Ajancy(Public.ConnectionString);
            Ajancy.User user = db.Users.First<Ajancy.User>(u => u.UserID == Public.ToInt(this.ViewState["UserID"]));
            Ajancy.UsersInRole userRole = new Ajancy.UsersInRole { RoleID = selectedRoleId, MembershipDate = DateTime.Now }; ;
            user.UsersInRoles.Add(userRole);

            switch ((Public.Role)selectedRoleId)
            {
                case Public.Role.AjancyManager:
                case Public.Role.AjancySupervisor:
                case Public.Role.AjancySecretary:
                case Public.Role.AjancyPartner:
                    Ajancy.AjancyPartner partner = new Ajancy.AjancyPartner { SubmitDate = DateTime.Now };
                    userRole.AjancyPartners.Add(partner);
                    db.SubmitChanges();
                    item = new ListItem(string.Format("{0} {1}", Public.GetRoleName(userRole.RoleID), "---"), string.Format("{0}|{1}", userRole.UserRoleID, partner.AjancyPartnerID));
                    break;

                case Public.Role.ProvinceManager:
                case Public.Role.AcademyProvince:
                    db.SubmitChanges();
                    item = new ListItem(string.Format("{0} {1}", Public.GetRoleName(selectedRoleId), user.City.Province.Name), string.Format("{0}|0", userRole.UserRoleID));
                    break;

                case Public.Role.CityManager:
                case Public.Role.AcademyCity:
                    db.SubmitChanges();
                    item = new ListItem(string.Format("{0} {1}", Public.GetRoleName(selectedRoleId), user.City.Name), string.Format("{0}|0", userRole.UserRoleID));
                    break;

                default:
                    db.SubmitChanges();
                    item = new ListItem(Public.GetRoleName(selectedRoleId), string.Format("{0}|0", userRole.UserRoleID));
                    break;
            }

            item.Selected = true;
            this.lstRoles.Items.Add(item);
            this.drpRoles.Items.Remove(this.drpRoles.SelectedItem);
            this.lblMessage.Text = "ثبت سمت انجام گردید";
        }
    }

    private void DisposeContext()
    {
        if (db != null)
        {
            db.Dispose();
        }
    }
}