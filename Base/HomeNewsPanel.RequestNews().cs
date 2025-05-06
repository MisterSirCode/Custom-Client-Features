private void RequestNews() {
    Action<WWWRequest> action = delegate(WWWRequest req) {
        if (req.response != null) {
            this.LoadNews(req.response.GetList("posts"));
            if (req.response.GetString("beta_title") != null) {
                this.betaPanelBanner.GetComponent<Text>().text = req.response.GetString("beta_title");
                this.betaPanelContent.GetComponent<Text>().text = req.response.GetString("beta_content");
                if (req.response.GetString("beta_button") != null) {
                    this.betaPanelButtonText.GetComponent<Text>().text = req.response.GetString("beta_button");
                    this.betaPanelButton.active = true;
                    this.betaPanelButton.GetComponent<Hyperlink>().url = req.response.GetString("beta_hyperlink");
                }
            }
        }
    };
    Singleton<GameManager>.main.CreateGatewayRequest("clients", new Hashtable(), action, HTTPMethods.Get);
}