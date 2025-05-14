private void RequestNews() {
    Action<WWWRequest> action = delegate(WWWRequest req) {
        if (req.response != null) {
            this.LoadNews(req.response.GetList("posts"));
            if (req.response.GetDictionary("beta") != null) {
                Dictionary<string, object> dict = req.response.GetDictionary("beta");
                this.betaPanelBanner.GetComponent<Text>().text = (string)dict["title"];
                this.betaPanelContent.GetComponent<Text>().text = (string)dict["content"];
                if (dict["button"] != null) {
					Dictionary<string, object> btn = dict["button"] as Dictionary<string, object>;
                    this.betaPanelButtonText.GetComponent<Text>().text = (string)btn["title"];
                    this.betaPanelButton.active = true;
                    this.betaPanelButton.GetComponent<Hyperlink>().url = (string)btn["url"];
                }
            }
        }
    };
    Singleton<GameManager>.main.CreateGatewayRequest("clients", new Hashtable(), action, HTTPMethods.Get);
}