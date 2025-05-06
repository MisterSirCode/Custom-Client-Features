private void OnSpriteChanged(object spriteObj) {
    string text = (string)spriteObj;
    if (text != null && text.Length > 0) {
        if (text.Contains("crow-5")) {
            this.image.sprite = ExternalSpriteLoader.GetSprite("crow-5");
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