private void OnActiveChanged(bool isActive) {
    foreach (GameObject gameObject in this.gameObjects) {
        if (this.eventName != "console.activated") {
            gameObject.SetActive((!this.opposite) ? isActive : (!isActive));
        }
    }
}