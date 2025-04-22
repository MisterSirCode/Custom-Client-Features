using System;
using System.Collections.Generic;
using UnityEngine;

public class ExternalAssetManager
{
	public void Initialize()
	{
		if (ExternalAssetManager.instance != null) {
			return;
		}
		ExternalAssetManager.instance = this;
		this.localPath = Application.dataPath;
		this.assets = new List<ExternalAsset>();
		this.loaded = new List<ExternalAsset>();
	}

	public static ExternalAssetManager GetInstance()
	{
		return ExternalAssetManager.instance;
	}

	public void CreateAsset(string name, string path, string type)
	{
		ExternalAsset item = new ExternalAsset(name, this.localPath + "/External/" + type + "/" + path, type + "/" + path, type);
		this.assets.Add(item);
		if (item.AssetExists()) this.loaded.Add(item);
	}

	public List<ExternalAsset> GetAssetsOfType(string type)
	{
		List<ExternalAsset> results = new List<ExternalAsset>();
		foreach (ExternalAsset asset in this.loaded) {
			if (asset.type == type) results.Add(asset);
		}
		return results;
	}

	public static ExternalAssetManager instance;
	public string localPath;
	public List<ExternalAsset> assets;
	public List<ExternalAsset> loaded;
}