public void OnPointerClick(PointerEventData eventData) {
    if (GameManager.IsMobile() || GameManager.IsSteamdeck()) {
        ReplaceableSingleton<Player>.main.Respawn();
    }
}