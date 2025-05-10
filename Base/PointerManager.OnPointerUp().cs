public void OnPointerUp() {
    ReplaceableSingleton<Player>.main.CancelMiningBlock();
    ReplaceableSingleton<Player>.main.StopShooting();
}

public void OnPointerUp(Vector2 position) {
    ReplaceableSingleton<Player>.main.CancelMiningBlock();
    ReplaceableSingleton<Player>.main.TryToInteractAtScreenPosition(position);
    ReplaceableSingleton<Player>.main.StopShooting();
}