public void OnGUI() {
    if (ExternalConsole.GetInstance() != null && ExternalConsole.GetInstance().enabled) {
        ExternalConsole.GetInstance().Draw();
    }			
    Vector3 vector = Input.mousePosition;
    Vector2 pos = ReplaceableSingleton<Zone>.main.ScreenToBlockPosition(new Vector2(vector.x, vector.y));
    try {
        Item item = ReplaceableSingleton<Zone>.main.AccessibleBlock((int)pos.x, (int)pos.y).MineableItem();
        if (IdentifyConsoleCommand.isRunning && item != null) {
            GUI.Label(new Rect(vector.x - 80f, -vector.y + (float)Screen.height + 60f, 160f, 30f), new GUIContent("<color=#fff>" + item.title + "</color>"), IdentifyConsoleCommand.labelStyle);
            GUI.Label(new Rect(vector.x - 80f, -vector.y + (float)Screen.height + 90f, 160f, 30f), new GUIContent("<color=#fff>" + item.name + "</color>"), IdentifyConsoleCommand.labelStyle);
        }
    } catch (Exception) { }
}