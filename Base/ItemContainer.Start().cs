protected virtual void Start() {
	LayoutGroup component = base.GetComponent<LayoutGroup>();
	if (component == null) {
		GridLayoutGroup gridLayoutGroup = base.gameObject.AddComponent<GridLayoutGroup>();
		gridLayoutGroup.cellSize = new Vector3(70f, 70f);
		gridLayoutGroup.padding = new RectOffset(5, 5, 5, 5);
		ContentSizeFitter contentSizeFitter = base.gameObject.AddComponent<ContentSizeFitter>();
		contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
	}
	this.scrollRect = base.GetComponentInParent<ScrollRect>();
	if (this.scrollRect != null && base.GetComponentInParent<ScrollSpeedSetting>() == null) {
		this.scrollRect.gameObject.AddComponent<ScrollSpeedSetting>();
	}
}
