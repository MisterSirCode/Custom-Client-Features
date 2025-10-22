private void SetExploredStat() {
    this.SetStat(
        "explored",
        Locale.Text("Explored", null, null, null),
        ((int)(ReplaceableSingleton<Zone>.main.chunksExploredPercent * 100f)).ToString() + "%", "icon-portal-blue"
    );
}
