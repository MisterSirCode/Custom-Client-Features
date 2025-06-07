using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class ExternalAssetManager {
	public void Initialize() {
		if (ExternalAssetManager.instance != null) {
			return;
		}
		ExternalAssetManager.instance = this;
		this.localPath = Application.dataPath;
		this.assets = new List<ExternalAsset>();
		this.loaded = new List<ExternalAsset>();
	}

	public static ExternalAssetManager GetInstance() {
		return ExternalAssetManager.instance;
	}

	public void CreateAsset(string name, string path, string type) {
		string fullPath = this.localPath + "/External/" + type + "/" + path;
		ExternalAsset item = new ExternalAsset(name, fullPath, path, type);
		this.assets.Add(item);
		if (item.AssetExists()) this.loaded.Add(item);
	}

	public void AutoCreateAssets() {
		string[] dirs = Directory.GetDirectories(this.localPath + "/External/");
		foreach (string dir in dirs) {
			DirectoryInfo info = new DirectoryInfo(dir);
			FileInfo[] files = info.GetFiles();
			foreach (FileInfo file in files) {
				ExternalAsset item = new ExternalAsset(Path.GetFileNameWithoutExtension(file.FullName), file.FullName, file.Name, info.Name);
				//ExternalConsole.Log(info.Name + "/" + file.Name, Path.GetFileNameWithoutExtension(file.FullName) + " (" + info.Name + ")");
				this.loaded.Add(item);
			}
		}
	}

	public List<ExternalAsset> GetAssetsOfType(string type) {
		List<ExternalAsset> results = new List<ExternalAsset>();
		foreach (ExternalAsset asset in this.loaded) {
			if (asset.type == type) results.Add(asset);
		}
		return results;
	}

	public IEnumerator LoadAssetMetadata(ExternalAsset asset) {
		string metaPath = asset.path + ".meta";
		if (!File.Exists(metaPath)) yield break;
		WWW www = new WWW(metaPath);
		yield return www;
		try {
			Dictionary<string, object> result = new Dictionary<string, object>();
			using (StringReader reader = new StringReader(www.text)) {
				string line;
				while ((line = reader.ReadLine()) != null) {
					int sep = line.IndexOf("=");
					if (sep != -1) result.Add(line.Substring(0, sep), line.Substring(sep + 1));
				}
			}
			asset.SetMeta(result);
			www.Dispose();
			yield break;
		} catch (Exception ex) {
			ExternalConsole.HandleException(asset.name + " Metadata Loader", ex);
			yield break;
		}
		yield break;
	}

	public static ExternalAssetManager instance;
	public string localPath;
	public List<ExternalAsset> assets;
	public List<ExternalAsset> loaded;
}