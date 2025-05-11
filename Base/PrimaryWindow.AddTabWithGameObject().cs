public GameObject AddTabWithGameObject(string title, string icon, GameObject gameObject) {
    RectTransform rectTransform = this.AddGameObject(gameObject);
    this.AddTab(title, icon, new TabButton.TargetPanel(this.contentPanel, rectTransform));
    return rectTransform.gameObject;
}