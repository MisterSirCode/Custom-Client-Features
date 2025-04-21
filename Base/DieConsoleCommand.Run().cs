public override void Run() {
    Command.Send(Command.Identity.Health, new object[] { 0, 0 });
}