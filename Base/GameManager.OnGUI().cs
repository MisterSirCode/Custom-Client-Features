public void OnGUI() {
    if (ExternalConsole.GetInstance() != null && ExternalConsole.GetInstance().enabled) {
        ExternalConsole.GetInstance().Draw();
    }
    if (!GameManager.IsGame()) {
        return;
    }
    Vector3 vector = Input.mousePosition;
    Vector2 pos = ReplaceableSingleton<Zone>.main.ScreenToBlockPosition(new Vector2(vector.x, vector.y));
	Vector2 wpos = ReplaceableSingleton<Zone>.main.ScreenToWorldPosition(new Vector2(vector.x, vector.y));
    try {
        Entity entity = IdentifyConsoleCommand.NearbyEntity(wpos);
        if (entity != null) {
            float cat = 0f;
            GUI.Label(new Rect(vector.x - 80f, -vector.y + (float)Screen.height + 10f, 160f, 30f), 
                new GUIContent(string.Concat(new string[] { "<color=#fff>", entity.config.name, " (", entity.entityId.ToString(), ")</color>" })), IdentifyConsoleCommand.labelStyle);
            cat++;
            GUI.Label(new Rect(vector.x - 80f, -vector.y + (float)Screen.height + 10f + cat * 30f, 160f, 30f), 
                new GUIContent("<color=#fff>Health: " + entity.health.ToString() + " - Alive: " + entity.alive.ToString() + "</color>"), IdentifyConsoleCommand.labelStyle);
            if (entity.config.named) {
                cat++;
                GUI.Label(new Rect(vector.x - 80f, -vector.y + (float)Screen.height + 10f + cat * 30f, 160f, 30f), 
                    new GUIContent("<color=#fff>Config Name: " + entity.name + "</color>"), IdentifyConsoleCommand.labelStyle);
            }
            cat++;
            GUI.Label(new Rect(vector.x - 80f, -vector.y + (float)Screen.height + 10f + cat * 30f, 160f, 30f), 
                new GUIContent("<color=#fff>Humanoid: " + entity.isHuman.ToString() + " - Player: " + entity.isPlayer.ToString() + "</color>"), IdentifyConsoleCommand.labelStyle);
        } else {
            ZoneBlock block = ReplaceableSingleton<Zone>.main.Block((int)pos.x, (int)pos.y);
            if (IdentifyConsoleCommand.isRunning && block != null) {
                Item item = null;
                if (block.frontItem != null && block.frontItem.name != "air")  item = block.frontItem;
                else if (block.liquidItem != null && block.liquidItem.name != "air")  item = block.liquidItem;
                else if (block.backItem != null && block.backItem.name != "air")  item = block.backItem;	
                else if (block.baseItem != null && block.baseItem.name != "air")  item = block.baseItem;	
                if (item != null) {
                    float cat = 0f;
                    GUI.Label(new Rect(vector.x - 80f, -vector.y + (float)Screen.height + 10f, 160f, 30f), 
                        new GUIContent("<color=#fff>" + item.title + " (" + item.name + ")" + "</color>"), IdentifyConsoleCommand.labelStyle);
                    cat++;
                    GUI.Label(new Rect(vector.x - 80f, -vector.y + (float)Screen.height + 10f + (cat * 30f), 160f, 30f), 
                        new GUIContent("<color=#fff>Layer: " + item.layer.ToString() + "</color>"), IdentifyConsoleCommand.labelStyle);
                    if (item.layer.ToString() == "Liquid") {
                        cat++;
                        GUI.Label(new Rect(vector.x - 80f, -vector.y + (float)Screen.height + 10f + cat * 30f, 160f, 30f), 
                            new GUIContent("<color=#fff>Liquid Mod: " + block.liquidMod.ToString() + " / " + item.modMax.ToString() + "</color>"), IdentifyConsoleCommand.labelStyle);
                    }
                    if (item.light > 0f) {
                        cat++;
                        GUI.Label(new Rect(vector.x - 80f, -vector.y + (float)Screen.height + 10f + cat * 30f, 160f, 30f), 
                            new GUIContent("<color=#fff>Light: " + item.light.ToString() + " - Color: " + item.lightColor.ToString() + "</color>"), IdentifyConsoleCommand.labelStyle);
                    }
                }
            }
        }
    } catch (Exception) { }
}