private void LoadNews(List<object> newsData) {
    this.loadingIndicator.SetActive(false);
    foreach (object obj in newsData) {
        Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
        HomeNewsItem component = this.itemPrefab;
        if (this.items.Count > 0) {
            GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.itemPrefab.gameObject);
            gameObject.transform.SetParent(this.itemPrefab.transform.parent, false);
            component = gameObject.GetComponent<HomeNewsItem>();
        }
        component.Configure(dictionary);
        this.items.Add(component);
    }
    this.container.localPosition = new Vector2(this.container.localPosition.x, 0f);
}