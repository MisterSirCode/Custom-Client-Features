private void Start() {
    this.itemPrefab.SetActive(false);
    this.betaPanelBanner = GameObject.Find("/Canvas/Gui/Beta Panel/Banner/Title");
    GameObject.Find("/Canvas/Gui/Beta Panel/Scroll Rect/Content/Buttons").GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.MinSize;
    this.betaPanelButton = GameObject.Find("/Canvas/Gui/Beta Panel/Scroll Rect/Content/Buttons/Forums");
    this.betaPanelButtonText = GameObject.Find("/Canvas/Gui/Beta Panel/Scroll Rect/Content/Buttons/Forums/Text");
    this.betaPanelContent = GameObject.Find("/Canvas/Gui/Beta Panel/Scroll Rect/Content/Text");
    this.betaPanelBanner.GetComponent<Text>().text = "Information";
    this.betaPanelButtonText.GetComponent<Text>().text = "Loading...";
    this.betaPanelContent.GetComponent<Text>().text = "Loading...";
    this.betaPanelButton.active = false;
    this.RequestNews();
}