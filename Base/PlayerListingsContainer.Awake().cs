private void Awake() {
    this._transform = (RectTransform)base.transform;
    Messenger.AddListener<string>("playerDidFollow", new Callback<string>(this.OnPlayerDidFollow));
    Messenger.AddListener<string>("playerDidUnfollow", new Callback<string>(this.OnPlayerDidUnfollow));
    transform.parent.gameObject.AddComponent<ScrollSpeedSetting>();
}
