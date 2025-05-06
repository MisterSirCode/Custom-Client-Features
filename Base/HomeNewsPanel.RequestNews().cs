private void RequestNews() {
    Action<WWWRequest> action = delegate(WWWRequest req) {
        if (req.response != null) {
            this.LoadNews(req.response.GetList("posts"));
        }
    };
    Singleton<GameManager>.main.CreateGatewayRequest("clients", new Hashtable(), action, HTTPMethods.Get);
}