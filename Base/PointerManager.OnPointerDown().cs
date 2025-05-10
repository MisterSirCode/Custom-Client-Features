public void OnPointerDown(Vector2 position) {
    if (!this.IsPointerBlockedByGui()) {
        if (GameManager.IsSteamdeck()) {
            if (ExternalControllerManager.instance.RightUp()) {
                ReplaceableSingleton<Player>.main.TryToInteractAtScreenPosition(position);
                return;
            }
        } else {
            ReplaceableSingleton<Player>.main.TryToInteractAtScreenPosition(position);
        }
    }
}