private void OnSpriteChanged(object spriteObj) {
    string text = (string)spriteObj;
    if (text != null && text.Length > 0) {
		if (text.Contains("crow-1")) GameManager.order = "crow-1";
		if (text.Contains("crow-2")) GameManager.order = "crow-2";
		if (text.Contains("crow-3")) GameManager.order = "crow-3";
		if (text.Contains("crow-4")) GameManager.order = "crow-4";
		if (text.Contains("crow-5")) GameManager.order = "crow-5";
        if (text.Contains("crow-5") && ExternalSpriteLoader.HasSprite("crow-5")) {
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