using System;

public class ExternalConsoleModule
{
	public ExternalConsoleModule(string name, string type, bool state)
	{
		this.name = name;
		this.type = type;
		this.state = state;
		this.output = "";
	}

	public ExternalConsoleModule(string name, Action action)
	{
		this.name = name;
		this.type = "button";
		this.state = true;
		this.output = "";
		this.action = action;
	}

	public void Toggle()
	{
		this.state = !this.state;
	}

	public string name;
	public string type;
	public string output;
	public bool state;
	public Action action;
}
