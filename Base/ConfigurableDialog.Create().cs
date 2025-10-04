public static ConfigurableDialog Create(Dictionary<string, object> config, ConfigurableDialog.ActionHandler successHandler = null, ConfigurableDialog.ActionHandler cancelHandler = null) {
    string @string = config.GetString("type");
    string text;
    if (@string != null) {
        if (@string == "android") {
            text = "Prefabs/Dialogs/Configurable Android";
            goto IL_008E;
        }
        if (@string == "loot") {
            text = "Prefabs/Dialogs/Configurable Loot";
            goto IL_008E;
        }
        if (@string == "loot_red") {
            text = "Prefabs/Dialogs/Configurable Loot Red";
            goto IL_008E;
        }
        if (@string == "loot_mech") {
            text = "Prefabs/Dialogs/Configurable Loot Mech";
            goto IL_008E;
        }
    }
    text = "Prefabs/Dialogs/Configurable";
    IL_008E:
    GameObject dialog = (GameObject)global::UnityEngine.Object.Instantiate(Resources.Load(text));

    if(@string == "android") {
        Transform bubble = dialog.transform.Find("Speech Bubble");
        if(bubble != null) {
            if (ExternalSpriteLoader.HasSprite("bubble-speech")) {
                bubble.GetComponent<Image>().sprite = ExternalSpriteLoader.GetSprite("bubble-speech");
            }
        }
    }

    // For some types of dialog try to resize according to preference
    string dialogSizeId =@string ?? "standard";
    GuiWindow guiWindow = dialog.GetComponent<GuiWindow>();
    if (guiWindow != null) {
        guiWindow.windowGroup = dialogSizeId;
        if (dialogSizeId == "android") {
            guiWindow.autoSavePosition = true;
            guiWindow.RetrievePosition();
        }
    }

    Transform scrollRectObj = gameObject.transform.Find("Scroll Container/Scroll Rect");
    if (scrollRectObj != null) {
        scrollRectObj.gameObject.AddComponent<ScrollSpeedSetting>();
    }

    ConfigurableDialog component = dialog.GetComponent<ConfigurableDialog>();
    component.Show(config, successHandler, cancelHandler);
    return component;
}
