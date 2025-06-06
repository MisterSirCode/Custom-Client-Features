// Token: 0x06003E82 RID: 16002 RVA: 0x0015E930 File Offset: 0x0015CB30
private ToolTip ToolTip(string type) {
    if (this.toolTips.ContainsKey(type)) {
        return this.toolTips[type];
    }
    GameObject gameObject = (GameObject)Resources.Load("Prefabs/Tooltips/" + type);
    if (gameObject != null) {
        GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(gameObject);
        if (type == "Item" || type == "Crafting") {
            gameObject2.GetComponent<UnityEngine.UI.Image>().sprite = ExternalSpriteLoader.GetSprite("panel-tooltip-darker");
        }
        gameObject2.transform.SetParent(base.transform, false);
        ToolTip component = gameObject2.GetComponent<ToolTip>();
        this.toolTips[type] = component;
        component.gameObject.SetActive(false);
        return component;
    }
    return null;
}
