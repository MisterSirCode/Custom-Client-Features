public void CheckAccessibility(Vector2 block) {
    try {
        if (ReplaceableSingleton<Player>.main.IsPrimaryItemPlaceable()) {
            this.pointerAccessibility = ((!ReplaceableSingleton<Player>.main.CanPlaceBlock(block)) ? PointerManager.Accessibility.None : PointerManager.Accessibility.Affectable);
        }
        else if (ReplaceableSingleton<Player>.main.CanMineBlock(block, true)) {
            bool flag = ReplaceableSingleton<Player>.main.CanMineBlock(block, false);
            this.pointerAccessibility = ((!flag) ? PointerManager.Accessibility.Accessible : PointerManager.Accessibility.Affectable);
        }
        else if (ReplaceableSingleton<Player>.main.primaryItem != null && ReplaceableSingleton<Player>.main.primaryItem.IsGun()) {
            this.pointerAccessibility = PointerManager.Accessibility.Affectable;
        }
        else {
            this.pointerAccessibility = PointerManager.Accessibility.None;
        }
    } catch (Exception) { }
}