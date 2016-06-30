using Alcoa.Framework.Common.Entity;
using Alcoa.Framework.Common.Enumerator;
using Microsoft.IdentityModel.Claims;
using System.Collections.Generic;
using System.Linq;

namespace Alcoa.Framework.Common.Presentation.Web
{
    public enum MenuType
    {
        CurrentApplicationMenu,
        MyApplicationsMenu,
        AboutMenu,
    }

    /// <summary>
    /// Class with menu properties to be displayed
    /// </summary>
    public class MenuModel
    {
        public MenuModel()
        {
        }

        public MenuModel(ClaimCollection claims, string appPath)
        {
            _claims = claims;
            _appPath = appPath;
        }

        private ClaimCollection _claims;

        private string _appPath;

        public string Text { get; set; }

        public string Url { get; set; }

        public int Order { get; set; }

        public List<MenuModel> Items { get; set; }

        public List<MenuModel> GetApplicationMenu()
        {
            var appMenu = new List<MenuModel>();

            if (_claims != null)
            {
                var ssoGroupAndServices = _claims
                    .Where(ss =>
                        ss.ClaimType == SsoClaimTypes.SsoGroup ||
                        ss.ClaimType == SsoClaimTypes.SsoGroupCurrentLevel ||
                        ss.ClaimType == SsoClaimTypes.SsoGroupParentLevel ||
                        ss.ClaimType == SsoClaimTypes.SsoGroupSubLevels ||
                        ss.ClaimType == SsoClaimTypes.SsoGroupOrder ||
                        ss.ClaimType == SsoClaimTypes.SsoServiceUrl)
                    .Select(sg => sg)
                    .ToList();

                foreach (var group in ssoGroupAndServices.Where(ss => ss.ClaimType == SsoClaimTypes.SsoGroup).ToList())
                {
                    var rootLevel = ssoGroupAndServices
                        .FirstOrDefault(ss =>
                            ss.ClaimType == SsoClaimTypes.SsoGroupParentLevel &&
                            ss.Value.Replace(group.Value + "|", string.Empty).ToInt() == 0);

                    //If its a root group, call menu build
                    if (rootLevel != null)
                    {
                        var rootMenu = BuildMenuRecursively(ssoGroupAndServices, group);
                        var existentMenu = appMenu.FirstOrDefault(am => am.Text == rootMenu.Text);

                        //If a menu was already added, add just new itens
                        if (existentMenu != null)
                        {
                            existentMenu.Items = existentMenu.Items
                                .Union(rootMenu.Items, new EqualBy<MenuModel>((x, y) => x.Text == y.Text))
                                .OrderBy(em => em.Order)
                                .ToList();
                        }
                        else
                            appMenu.Add(rootMenu);
                    }
                }

                appMenu = appMenu.OrderBy(am => am.Order).ToList();
            }

            return appMenu;
        }

        private MenuModel BuildMenuRecursively(List<Claim> ssoGroupsAndServices, Claim currentGroup)
        {
            var subLevels = ssoGroupsAndServices
                .FirstOrDefault(ss =>
                    ss.ClaimType == SsoClaimTypes.SsoGroupSubLevels &&
                    ss.Value.StartsWith(currentGroup.Value));

            var subItens = new List<MenuModel>();

            if (subLevels != null)
            {
                var subLevelsIds = subLevels
                    .Value.Replace(currentGroup.Value + "|", string.Empty)
                    .Split("|")
                    .Select(ss => ss.ToInt())
                    .ToList();

                foreach (var subLevelId in subLevelsIds)
                {
                    //Find the subgroup id
                    var groupSubLevel = ssoGroupsAndServices
                        .FirstOrDefault(sg =>
                            sg.ClaimType == SsoClaimTypes.SsoGroupCurrentLevel &&
                            sg.Value.Split("|").LastOrDefault().ToInt() == subLevelId);

                    var groupName = groupSubLevel.Value.Remove(groupSubLevel.Value.LastIndexOf('|'));

                    //Find the subgroup name
                    var group = ssoGroupsAndServices
                        .FirstOrDefault(sg =>
                            sg.ClaimType == SsoClaimTypes.SsoGroup &&
                            sg.Value == groupName);

                    subItens.Add(BuildMenuRecursively(ssoGroupsAndServices, group));
                }
            }

            var currentGroupOrder = ssoGroupsAndServices
                .FirstOrDefault(sg =>
                    sg.ClaimType == SsoClaimTypes.SsoGroupOrder &&
                    sg.Value.StartsWith(currentGroup.Value))
                .Value.Split("|").LastOrDefault()
                .ToInt();

            //Order groups and subgroups already added
            subItens = subItens.OrderBy(si => si.Order).ToList();

            //Add Service links for this group level
            subItens.AddRange(ssoGroupsAndServices
                    .Where(sl =>
                        sl.ClaimType == SsoClaimTypes.SsoServiceUrl &&
                        sl.Value.StartsWith(currentGroup.Value))
                    .Select(sn => sn.Value.Replace(currentGroup.Value, string.Empty).Split("|"))
                    .Select(ssf => new MenuModel
                    {
                        Text = ssf[1], //Get Service Name on Index 1
                        Url = _appPath + ssf.LastOrDefault()
                    })
                    .OrderBy(sn => sn.Text));

            //Creates a Menu for a group or a service
            return new MenuModel
            {
                Text = currentGroup.Value.Split("|").LastOrDefault(),
                Url = string.Empty,
                Order = currentGroupOrder,
                Items = subItens,
            };
        }

        public List<MenuModel> GetMyAppsMenu(string appCode, string labelMenuMyApps)
        {
            var myAppMenu = new List<MenuModel>();

            if (_claims != null)
            {
                //Select Application Mnemonic codes excluding the current Application
                var ssoAppMnemonics = _claims
                        .Where(uc => uc.ClaimType == SsoClaimTypes.SsoAppMnemonic)
                        .Select(uc => uc.Value.Split("|"))
                        .Where(uc => uc.LastOrDefault() != appCode)
                        .ToList();

                //Select Application Urls
                var ssoAppUrls = _claims
                        .Where(uc => uc.ClaimType == SsoClaimTypes.SsoAppHomeUrl)
                        .Select(uc => uc)
                        .ToList();

                myAppMenu.Add(new MenuModel
                {
                    Text = labelMenuMyApps,
                    Url = string.Empty,
                    Items = ssoAppMnemonics
                            .Select(app => new MenuModel
                            {
                                Text = app.LastOrDefault(),
                                Url = (ssoAppUrls.FirstOrDefault(sau => sau.Value.StartsWith(app[0])) ?? new Claim(string.Empty, "|")).Value.Split("|").LastOrDefault()
                            })
                            .OrderBy(app => app.Text)
                            .ToList(),
                });
            }

            return myAppMenu;
        }

        public List<MenuModel> GetAboutMenu(string labelMenuHelp, string labelMenuOpenTicket, string labelMenuAbout)
        {
            var aboutMenu = new List<MenuModel>
            {
                new MenuModel
                {
                    Text = labelMenuHelp,
                    Url = string.Empty,
                    Items = new List<MenuModel>
                    {
                        new MenuModel { Text = labelMenuOpenTicket, Url = @"http://remedyweb.alcoa.com/Logins/EndUserInterFace.asp" },
                        new MenuModel { Text = labelMenuAbout, Url = "" },
                    }
                }
            };

            return aboutMenu;
        }

        public List<dynamic> CastToDynamicMenuList(List<MenuModel> menus)
        {
            var dynamicMenu = new List<dynamic>();

            if (menus != null)
                dynamicMenu = BuildMenuRecursivelyAsDynamic(menus);

            return dynamicMenu;
        }

        private List<dynamic> BuildMenuRecursivelyAsDynamic(List<MenuModel> menus)
        {
            var dynamicMenu = new List<dynamic>();

            foreach (var item in menus)
            {
                if (item.Items != null && item.Items.Count > 0)
                {
                    dynamicMenu.Add(new
                    {
                        text = item.Text,
                        url = item.Url,
                        items = BuildMenuRecursivelyAsDynamic(item.Items)
                    });
                }
                else
                    dynamicMenu.Add(new
                    {
                        text = item.Text,
                        url = item.Url,
                        items = item.Items
                    });
            }

            return dynamicMenu;
        }

        public MenuModel FindMenu(string textToSearch, List<MenuModel> menus)
        {
            var menuFound = default(MenuModel);

            if (menus != null)
            {
                menuFound = menus.FirstOrDefault(m => m.Text.Contains(textToSearch) || m.Url.Contains(textToSearch));

                if (menuFound == null)
                {
                    foreach (var item in menus)
                    {
                        if (item.Items != null && item.Items.Count > 0)
                            menuFound = FindMenu(textToSearch, item.Items);

                        if (menuFound != null)
                            break;
                    }
                }
            }

            return menuFound;
        }
    }
}