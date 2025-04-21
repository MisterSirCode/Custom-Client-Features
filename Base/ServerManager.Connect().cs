...

this.SendCommand(Command.Identity.Authenticate, new object[] {
    version, this.username, this.authToken, new Dictionary<string, bool> {{ "initial", ConfigurationCommand.initial }}
});

...