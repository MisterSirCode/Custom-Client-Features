private void OnSpriteChanged(object spriteObj) {
    string text = (string)spriteObj;
    if (text != null && text.Length > 0) {
        string clipped = text.Replace("emoji/orders/", "");
        GameManager.order = clipped;
        if (ExternalSpriteLoader.HasSprite(clipped)) {
            this.image.sprite = ExternalSpriteLoader.GetSprite(clipped);
            base.gameObject.SetActive(true);
            return;
        } else {
            if (this.replace != null && this.replace.Length == 2) {
                text = text.Replace(this.replace[0], this.replace[1]);
            }
            this.image.sprite = GameGui.GetSprite(text);
            base.gameObject.SetActive(true);
            return;
        }
    }
    base.gameObject.SetActive(false);
}