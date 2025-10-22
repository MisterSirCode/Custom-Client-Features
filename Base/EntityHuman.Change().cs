public override void Change(Dictionary<string, object> changedDetails) {
    base.Change(changedDetails);
    Dictionary<string, object> dictionary = changedDetails.GetDictionary("uni");
    if (dictionary != null) {
        this.uniform = dictionary;
    }
    this.isHuman = true;
    this.HideExo();
    this.UpdateAppearance(this.details);
}
