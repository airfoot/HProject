using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HProject.Application
{
    public class ProduceMenu
    {
        public List<Menu> GetAdminMenu()
        {
            List<Menu> adminMenu = new List<Menu>();
            adminMenu.Add(new Menu("申请IT物品", @"/DistributingArticle/ApplyforITArticle"));
            adminMenu.Add(new Menu("我的IT物品申请", @"/DistributingArticle/MyApplication"));
            adminMenu.Add(new Menu("IT物品管理", @"/DistributingArticle/ManagerITArticle"));
            adminMenu.Add(new Menu("IT物品分配", @"/DistributingArticle/AllocateITArticle"));

            return adminMenu;
        }

        public List<Menu> GetUserMenu()
        {
            List<Menu> userMenu = new List<Menu>();
            userMenu.Add(new Menu("申请IT物品", @"/DistributingArticle/ApplyforITArticle"));
            userMenu.Add(new Menu("我的IT物品申请", @"/DistributingArticle/MyApplication"));
            
            return userMenu;
        }
    }

    public class Menu
    {
        string Name { get; set; }
        string Hyperlink { get; set; }

        public Menu(string name, string hyperlink)
        {
            this.Name = name;
            this.Hyperlink = hyperlink;
        }
    }
}