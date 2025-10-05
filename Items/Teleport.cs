using System;
using System.Collections.Generic;
using System.Linq;

namespace Items {
    public class Teleport : Consumable {
        public Teleport(Item item) : base(item) { }

        private static List<string> allItemNames = null;

        public static Item BestItem() {
            if (allItemNames == null) {
                allItemNames = (
                    from item in Config.main.AllItems()
                    where item.action == Item.Action.Teleport
                    orderby item.power descending
                    select item.name
                ).ToList();
            }

            Player player = ReplaceableSingleton<Player>.main;

            foreach (string id in allItemNames) {
                Item item = Item.Get(id);
                if (player.inventory.Quantity(item) > 0) {
                    return item;
                }
            }

            return null;
        }

        public static bool UseTheBest() {
            Item bestItem = BestItem();

            if (bestItem != null) {
                Messenger.Broadcast<ZoneBlock, object>("showTeleport", null, null);
                return true;
            }

            return false;
        }

        protected override bool UseInternal(object useData) {
            if (useData != null) {
                return false;
            }
            Messenger.Broadcast<ZoneBlock, object>("showTeleport", null, null);
            return false;
        }
    }
}
