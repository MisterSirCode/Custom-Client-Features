public void UpdateNameLabel() {
    if (this.isPlayer) return;
    if (this.config.named) {
        if (this.nameIcon != null) {
            if (this.nameIcon.Length > 0) {
            }
        }
    }
    base.StartCoroutine(this.UpdateDistance());
}