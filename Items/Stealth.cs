using System;

namespace Items {
    public class Stealth : Consumable {
        public Stealth(Item item) : base(item) { }

        protected override bool UseInternal(object useData) {
            if (this.item.firingInterval > 0f) {
                Player player = ReplaceableSingleton<Player>.main;
                if (player != null) {
                    player.StartCoroutine("MomentaryAccessoryFollowUp", new object[] { this.item, this.item.firingDuration });
                }
            }
            return true;
        }

        protected override bool ShouldRemoveInventory() {
            return this.item.category == "consumables";
        }
        
        public static bool UseTheBest() {
            Player player = ReplaceableSingleton<Player>.main;
            if (player == null) {
                return false;
            }
            Item item = player.ActivateMomentaryAccessory(Item.Action.Stealth);
            if (item != null) {
                new Stealth(item).Use(null);
                return true;
            }
            return false;
        }
    }
}
