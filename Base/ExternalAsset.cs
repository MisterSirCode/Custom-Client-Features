using System;
using System.IO;
using UnityEngine;

public class ExternalAsset {
	public ExternalAsset(string name, string path, string fileID, string type) {
		this.name = name;
		this.path = path;
		this.fileID = fileID;
		this.type = type;
		this.loaded = false;
	}

	public bool AssetExists() {
		return File.Exists(this.path);
	}

	public FileInfo GetInfo() {
		return new FileInfo(this.path);
	}

	public string GetRectifiedPath() {
		return string.Format("file:///{0}", this.GetInfo().FullName.ToString());
	}

	public UnityEngine.Object GetData() {
		if (this.loaded) return this.data;
		else return null;
	}

	public void SetData(UnityEngine.Object data) {
		this.data = data;
	}

	public string name;
	public string path;
	public string type;
	private string fileID;
	public bool loaded;
	public UnityEngine.Object data;
}
