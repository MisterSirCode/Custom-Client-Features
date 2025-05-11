public RectTransform AddGameObject(GameObject gameObject) {
    RectTransform rectTransform = (RectTransform)gameObject.transform;
    this.contentPanel.AddAndExpandChild(rectTransform);
    return rectTransform;
}