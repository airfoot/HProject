using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HProject.Application
{
    public class ProduceMenu
    {
        public List<HProject.Application.Menu> GetAdminMenu()
        {
            List<HProject.Application.Menu> adminMenu = new List<HProject.Application.Menu>();
            adminMenu.Add(new Menu("申请IT物品", @"/DistributingArticle/ApplyforITArticle"));
            adminMenu.Add(new Menu("我的IT物品申请", @"/DistributingArticle/MyApplication"));
            adminMenu.Add(new Menu("IT物品管理", @"/DistributingArticle/ManagerITArticle"));
            adminMenu.Add(new Menu("IT物品分配", @"/DistributingArticle/AllocateITArticle"));

            return adminMenu;
        }

        public List<HProject.Application.Menu> GetUserMenu()
        {
            List<HProject.Application.Menu> userMenu = new List<HProject.Application.Menu>();
            userMenu.Add(new Menu("申请IT物品", @"/DistributingArticle/ApplyforITArticle"));
            userMenu.Add(new Menu("我的IT物品申请", @"/DistributingArticle/MyApplication"));
            
            return userMenu;
        }
    }

    public class Menu
    {
        public string Name { get; set; }
        public string Hyperlink { get; set; }

        public Menu(string name, string hyperlink)
        {
            this.Name = name;
            this.Hyperlink = hyperlink;
        }
    }
}