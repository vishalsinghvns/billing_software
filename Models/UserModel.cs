using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Preora.Models
{
    public class UserModel
    {
        public virtual List<Users> Userlist { get; set; }
        public virtual List<MenusModel> Menuslist1 { get; set; }
        public virtual List<MenusModel> Menuslist2 { get; set; }
        public virtual List<MenusModel> Menuslist3 { get; set; }
        public virtual List<SubMenuModel> SubmenuList { get; set; }
        public virtual List<UserHasMenus> UserMenulist { get; set; }
        public virtual Users Addusers { get; set; }

    }
    public class Users
    {
        public int id { get; set; }
        public int shopid { get; set; }
        public string Shopname { get; set; }
        public string Uname { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string rolename { get; set; }
        public Nullable<bool> is_deleted { get; set; }
        public int[] menuid_arr { get; set; }
    }
    public class MenusModel
    {
        public int mid { get; set; }
        public string menu_name { get; set; }
    }
    public class SubMenuModel
    {
        public int sid { get; set; }
        public Nullable<int> menu_id { get; set; }
        public string submenu_name { get; set; }
    }
    public class UserHasMenus
    {
        public int id { get; set; }
        public Nullable<int> userid { get; set; }
        public Nullable<int> m_id { get; set; }
        public Nullable<int> s_id { get; set; }
    }
}