private void CreateGeographyTexture(int zoneHeight, int[] surfaceArray)
{
    string biome = ReplaceableSingleton<Zone>.main.biome;
    string[] colors;
    if (!(biome == "arctic")) {
        if (!(biome == "hell")) {
            if (!(biome == "brain")) {
                if (!(biome == "desert")) {
                    if (!(biome == "space")) {
                        colors = new string[] { "FFFFFF", "5DB830", "417431", "414E1C", "4E441C", "2A240C", "18150B" };
                    } else {
                        colors = new string[] { "FFFFFF", "EEEEEE", "DDDDDD", "CCCCCC", "BBBBBB", "AAAAAA", "999999" };
                    }
                } else {
                    colors = new string[] { "ECDE93", "B18E58", "7B5822", "614312", "4E350C", "322209", "1E1506" };
                }
            } else {
                colors = new string[] { "FFFFFF", "A19599", "705C6E", "5D4257", "4E3E55", "3B1C36", "2A0B28" };
            }
        } else {
            colors = new string[] { "FFFFFF", "BE8D6F", "905548", "7F3C32", "6F3932", "5F1814", "380E0D" };
        }
    } else {
        colors = new string[] { "FFFFFF", "75A49E", "456B74", "33535F", "2D4F5D", "142E3F", "0B1A25" };
    }
    double[] depths = new double[] { 0.03, 0.05, 0.08, 0.12, 0.17, 0.26, 0.3 };
    int surfaceMin = zoneHeight;
    int surfaceMax = 0;
    foreach (int surface in surfaceArray) {
        if (surface < surfaceMin) {
            surfaceMin = surface;
        }
        if (surface > surfaceMax) {
            surfaceMax = surface;
        }
    }
    this.geographyTexture = new Texture2D(surfaceArray.Length, zoneHeight, TextureFormat.RGBA32, false);
    for (int x = 0; x < this.geographyTexture.width; x++) {
        for (int y = 0; y < this.geographyTexture.height; y++) {
            this.geographyTexture.SetPixel(x, y, Color.clear);
        }
    }
    int surfaceCenter = (int)Math.Floor((double)(surfaceMax - surfaceMin) * 0.5 + (double)surfaceMin);
    double scaleX = (double)this.geographyTexture.width / (double)surfaceArray.Length;
    double scaleY = (double)this.geographyTexture.height / (double)zoneHeight;
    for (int i = 0; i < this.geographyTexture.width; i++) {
        int surface2 = surfaceArray[(int)((double)i / scaleX)];
        int distanceToPeak = surface2 - surfaceMin;
        double y2 = (double)(zoneHeight - surface2) * scaleY;
        double layerHeight = 1.0;
        int start = this.geographyTexture.height - (int)(y2 * layerHeight);
        for (int j = 0; j < depths.Length; j++) {
            double scale = (double)distanceToPeak / (double)zoneHeight / (double)depths.Length * ((j < 2) ? 2.0 : 0.5);
            layerHeight -= depths[j] - scale;
            int end = ((j + 1 >= depths.Length) ? this.geographyTexture.height : (this.geographyTexture.height - (int)(y2 * layerHeight)));
            Color color = GraphicsHelper.HexColor(colors[(j > 0 || surface2 < surfaceCenter) ? j : 1]);
            for (int k = start; k < end; k++) {
                this.geographyTexture.SetPixel(i, k, color);
            }
            start = end;
        }
    }
    this.geographyTexture.Apply();
    this.geographyImage.texture = this.geographyTexture;
    this.geographyImage.color = new Color(1f, 1f, 1f, 0.5f);
    this.geographySizePercent = 1f;
    this.geographyInsetPercent = 0f;
    this.OnRectTransformDimensionsChange();
}