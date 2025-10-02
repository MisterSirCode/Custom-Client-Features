using System;
using UnityEngine;
using UnityEngine.UI;

private void Awake() {
    this.primaryWindow = base.GetComponent<PrimaryWindow>();
    this.primaryWindow.windowName = "inventory";
    this.primaryWindow.maximizeIfTiny = true;
    RectTransform rectTransform = this.primaryWindow.AddPanel("Panels/Parchment");
    rectTransform.Find("Scroll Rect").GetComponent<ScrollRect>().scrollSensitivity = 15f;
    ((RectTransform)rectTransform.Find("Scroll Rect/Content").transform).gameObject.AddComponent<InventoryItemContainer>().primaryWindow = this.primaryWindow;
}
