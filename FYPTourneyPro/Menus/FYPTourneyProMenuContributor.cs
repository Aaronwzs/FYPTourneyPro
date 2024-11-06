using FYPTourneyPro.Permissions;
using FYPTourneyPro.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;

namespace FYPTourneyPro.Menus;

public class FYPTourneyProMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private static Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<FYPTourneyProResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                FYPTourneyProMenus.Home,
                l["Menu:Home"],
                "~/",
                icon: "fas fa-home",
                order: 0
            )
        );

        context.Menu.Items.Insert(
           1,
           new ApplicationMenuItem(
               "TodoList",
               "TodoList",
               icon: "fas fa-home"
           ).AddItem(
                new ApplicationMenuItem(
                    FYPTourneyProMenus.TodoItems,
                    "TodoItems",
                    url: "/TodoItems"
                )
            )
       );

        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 5;

        //Administration->Identity
        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 1);

        //Administration->Tenant Management
        administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 2);

        //Administration->Settings
        administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 6);

        context.Menu.AddItem(
            new ApplicationMenuItem(
                "BooksStore",
                l["Menu:FYPTourneyPro"],
                icon: "fa fa-book"
            ).AddItem(
                new ApplicationMenuItem(
                    "BooksStore.Books",
                    l["Menu:Books"],
                    url: "/Books"
                ).RequirePermissions(FYPTourneyProPermissions.Books.Default)
            )
        );

        if (context.Menu.Name == StandardMenus.Main)
        {
            // Add Tournament Management section
            var tournamentMenu = context.Menu.AddItem(
                new ApplicationMenuItem(
                    "TournamentManagement",
                    "Tournament Management",
                    icon: "fa fa-trophy"
                )
            );
            // Add sub-items under Tournament Management
            tournamentMenu.AddItem(
                    new ApplicationMenuItem(
                        "TournamentManagement.ListTournaments",
                        "Tournament",
                        url: "/Tournament/Index"
                    )
                );

            // Add Category Management section
            var categoryMenu = context.Menu.AddItem(
                new ApplicationMenuItem(
                    "CategoryManagement",
                    "Category Management",
                    icon: "fa fa-list"
                )
            );

            categoryMenu.AddItem(
                new ApplicationMenuItem(
                    "CategoryManagement.AddCategory",
                    "Add Category",
                    url: "/Category/Index"
                )
            );

            // Add Player Registration section
            var playerRegistrationMenu = context.Menu.AddItem(
                new ApplicationMenuItem(
                    "PlayerRegistration",
                    "Player Registration",
                    icon: "fa fa-user-plus"
                )
            );

            playerRegistrationMenu.AddItem(
                new ApplicationMenuItem(
                    "PlayerRegistration.Register",
                    "Register Player",
                    url: "/PlayerRegistration/Register"
                )
            );

            // Add CategoryParticipant Management section
            var categoryParticipantMenu = context.Menu.AddItem(
                new ApplicationMenuItem(
                    "CategoryParticipantManagement",
                    "Participant Management",
                    icon: "fa fa-users"
                )
            );

            categoryParticipantMenu.AddItem(
                new ApplicationMenuItem(
                    "CategoryParticipantManagement.ListParticipants",
                    "Participant List",
                    url: "/CategoryParticipant/List"
                )
            );
        }

        return Task.CompletedTask;
        }
    }

