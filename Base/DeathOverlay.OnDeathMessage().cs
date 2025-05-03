private void OnDeathMessage(string message) {
    base.gameObject.SetActive(true);
		this.label.text = message.Replace("Press spacebar to respawn.", "Press any key to respawn.");
}