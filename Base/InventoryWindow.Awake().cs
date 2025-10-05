using System;
using UnityEngine;
using UnityEngine.UI;

private void Awake() {
    this.primaryWindow = base.GetComponent<PrimaryWindow>();
    this.primaryWindow.windowName = "inventory";
    this.primaryWindow.maximizeIfTiny = true;
    RectTransform rectTransform = this.primaryWindow.AddPanel("Panels/Parchment");
    rectTransform.Find("Scroll Rect").gameObject.AddComponent<ScrollSpeedSetting>();
    ((RectTransform)rectTransform.Find("Scroll Rect/Content").transform).gameObject.AddComponent<InventoryItemContainer>().primaryWindow = this.primaryWindow;
}
