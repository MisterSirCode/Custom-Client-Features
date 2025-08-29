public Texture2D LoadExternalAtlas(ExternalAtlas atlas, string materialName = "Graphics/WorldSpriteMaterial") {
    if (atlas.name.Contains("_gui")) {
        materialName = "Graphics/GuiSpriteMaterial";
        atlas.name = atlas.name.Replace("_gui", "");
    }
	bool masked = false;
	if (atlas.name.Contains("_masked")) {
        masked = true;
        atlas.name = atlas.name.Replace("_masked", "");
    }
    string name = atlas.name;
	bool flag = name.Contains("classic");
	new GameObject(name).transform.parent = base.transform;
	TextAsset textAsset = atlas.atlas.data as TextAsset;
	string[] array = textAsset.text.Split(new char[] { '\n' });
	int num = Convert.ToInt32(array[1].Substring(2));
	int num2 = Convert.ToInt32(array[2].Substring(2));
	Material material = (Material)global::UnityEngine.Object.Instantiate(Resources.Load(materialName));
	this.materials[name] = material;
	if (masked) {
		material.SetTexture("_GradientTex", this._maskTexture);
	}
	this.materialsMasked[material] = masked;
	Material material2 = null;
	if (false) {
		material2 = (Material)global::UnityEngine.Object.Instantiate(Resources.Load("Graphics/WorldShadowMaterial"));
		this.materials[name + "-shadow"] = material2;
	}
	Texture2D texture2D = ((Sprite)atlas.sprite.data).texture;
    this.textures[name] = texture2D;
    Texture2D texture2D2 = texture2D;
    this.materials[name].mainTexture = texture2D2;
    if (this.materials.ContainsKey(name + "-shadow")) {
        this.materials[name + "-shadow"].mainTexture = texture2D2;
    }
	tk2dSpriteCollectionData tk2dSpriteCollectionData = tk2dSpriteCollectionData.CreateFromTexturePacker(tk2dSpriteCollectionSize.PixelsPerMeter(this.BaseBlockScale()), textAsset.text, texture2D);
	tk2dSpriteCollectionData.material = material;
	tk2dSpriteCollectionData.materials = new Material[] { material };
	tk2dSpriteCollectionData.transform.parent = base.transform;
	tk2dSpriteCollectionData.gameObject.name = name + ".tk2d";
	this.collections[name] = tk2dSpriteCollectionData;
	this.shadowMaterials[tk2dSpriteCollectionData] = material2;
	if (name == "accents") {
		this._accentsCollection = tk2dSpriteCollectionData;
	}
	foreach (tk2dSpriteDefinition tk2dSpriteDefinition in tk2dSpriteCollectionData.spriteDefinitions) {
		string name2 = tk2dSpriteDefinition.name;
		tk2dSpriteDefinition.collection = tk2dSpriteCollectionData;
		tk2dSpriteDefinition.spriteId = tk2dSpriteCollectionData.GetSpriteIdByName(name2);
		tk2dSpriteDefinition.material = material;
		this.sprites[name2] = tk2dSpriteDefinition;
		this.spriteCollections[name2] = tk2dSpriteCollectionData;
		new Vector2(tk2dSpriteDefinition.rect.xMin / (float)num, 1f - tk2dSpriteDefinition.rect.yMin / (float)num2);
		new Vector2(tk2dSpriteDefinition.rect.xMax / (float)num, 1f - tk2dSpriteDefinition.rect.yMax / (float)num2);
		if (flag) {
			string text = name2.Replace("classic/", string.Empty);
			this.classicSprites[text] = tk2dSpriteDefinition;
			this.classicSpriteCollections[text] = tk2dSpriteCollectionData;
		}
		this.spriteCount++;
	}
	return texture2D;
}