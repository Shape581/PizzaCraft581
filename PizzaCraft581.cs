using Life;
using ModKit.Helper;
using ModKit.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModKit.Internal;
using _menu = AAMenu.Menu;
using Life.BizSystem;
using Life.Network;
using UnityEngine;
using Life.InventorySystem;
using ModKit.Utils;
using Life.VehicleSystem;
using Life.CharacterSystem;
using Life.FarmSystem;
using UnityEngine.PlayerLoop;

namespace PizzaCraft581
{
    public class PizzaCraft581 : ModKit.ModKit
    {
        public PizzaCraft581(IGameAPI api) : base(api)
        {
            PluginInformations = new PluginInformations(AssemblyHelper.GetName(), "1.1.0", "Shape581");
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            ModKit.Internal.Logger.LogSuccess($"{PluginInformations.SourceName} v{PluginInformations.Version}", "initialisé");
            InsertMenu();
        }


        public void InsertMenu()
        {
            _menu.AddBizTabLine(PluginInformations, new List<Activity.Type> { Activity.Type.Chef }, null, "Cuisiner une pizza", async (ui) =>
            {
                Player player = PanelHelper.ReturnPlayerFromPanel(ui);

                int[] requiredItems = { 1449, 1450, 137, 136, 1505, 1504, 1451 };
                int[] requiredQuantities = { 1, 1, 4, 1, 3, 2, 1 };

                bool hasAllItems = true;

                for (int i = 0; i < requiredItems.Length; i++)
                {
                    int slot = player.setup.inventory.GetItemSlotById(requiredItems[i]);
                    if (slot == -1 || player.setup.inventory.items[slot].number < requiredQuantities[i])
                    {
                        hasAllItems = false;
                        break;
                    }
                }

                if (hasAllItems)
                {
                    player.setup.NetworkisFreezed = true;

                    player.setup.TargetShowCenterText("PREPARATION", "Vous préparez la pâte a pizza...", 10f);
                    InventoryUtils.RemoveFromInventory(player, 1504, 2);
                    InventoryUtils.RemoveFromInventory(player, 136, 1);
                    InventoryUtils.RemoveFromInventory(player, 1449, 1);
                    InventoryUtils.RemoveFromInventory(player, 1450, 1);
                    InventoryUtils.RemoveFromInventory(player, 1451, 1);
                    await Task.Delay(10000);



                    player.setup.TargetShowCenterText("PREPARATION", "Vous appliquer la sauce tomate sur la pizza...", 5f);
                    InventoryUtils.RemoveFromInventory(player, 1505, 3);
                    await Task.Delay(5000);

                    player.setup.TargetShowCenterText("PREPARATION", "Vous appliquer le fromage sur la pizza...", 5f);
                    InventoryUtils.RemoveFromInventory(player, 137, 4);
                    await Task.Delay(5000);

                    player.setup.TargetShowCenterText("PREPARATION", "Vous mettez votre pizza au four...", 10f);
                    await Task.Delay(10000);

                    player.setup.NetworkisFreezed = false;

                    player.Notify("SUCCES", "Vous avez cuisinez avec succes vôtre pizza.", NotificationManager.Type.Success);
                    InventoryUtils.AddItem(player, 140, 1);
                }
                else
                {
                    player.Notify("AVERTISSEMENT", "Vous n'avez pas les ingrédient nécessaire pour cuisiner une pizza.", NotificationManager.Type.Warning);

                    player.SendText("Voici les ingrédient nécessaire pour cuisiner une pizza : \n- 2 Oeufs \n- 1 Bouteilles d'eau \n- 1 Farine \n- 1 Levure chimique \n- 1 Lait \n- 3 Tomates \n-4 Fromages");
                }
            });
        }
    }
}
