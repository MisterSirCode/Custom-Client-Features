using System;
using System.IO;

public class ExternalAsset
{
	public ExternalAsset(string name, string path, string fileID, string type)
	{
		this.name = name;
		this.path = path;
		this.fileID = fileID;
		this.type = type;
		this.loaded = false;
	}

	public bool AssetExists()
	{
		return File.Exists(this.path);
	}

	public FileInfo GetInfo()
	{
		return new FileInfo(this.path);
	}

	public string GetRectifiedPath()
	{
		return string.Format("file:///{0}", this.GetInfo().FullName.ToString());
	}

	public string name;
	public string path;
	public string type;
	private string fileID;
	public bool loaded;
}
