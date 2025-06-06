using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

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

	public Dictionary<String, object> GetMeta() {
		if (this.loaded)
		{
			return this.meta;
		}
		return null;
	}

	public bool HasMeta() {
		return this.meta != null;
	}

	public void SetMeta(Dictionary<String, object> meta) {
	    this.meta = meta;
	}

	public string name;
	public string path;
	public string type;
	private string fileID;
	public bool loaded;
	public UnityEngine.Object data;
	public Dictionary<String, object> meta;
}
