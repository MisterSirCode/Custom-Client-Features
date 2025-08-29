public void UpdatePlayerLight() {
    float intensity = (this.lightAccessory != null && this.lightTurnedOn) ? this.lightAccessory.light : 6f;
    this.light.localScale = new Vector3(intensity * 3f, intensity * 3f, 1f);
}
