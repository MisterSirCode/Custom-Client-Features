public class ExternalAtlas {
    public ExternalAtlas(string name, ExternalAsset image, ExternalAsset text) {
        this.name = name;
        this.sprite = image;
        this.atlas = text;
    }
    public string name { get; set; }
    public ExternalAsset sprite { get; }
    public ExternalAsset atlas { get; }
    public override string ToString() => $"Atlas {this.name} (Image {this.sprite.name}, Text {this.atlas.name})";
}