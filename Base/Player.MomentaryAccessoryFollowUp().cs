public IEnumerator MomentaryAccessoryFollowUp(object[] args) {
    Item item = (Item)args[0];
    float initialDelay = args.Length > 1 ? (float)args[1] : 0f;
    if (initialDelay > 0f) {
        yield return new WaitForSeconds(initialDelay);
    }
    if (item.firingInterval > 0f) {
        Notification.Create($"Your {item.title} has to cool down for {item.firingInterval} seconds.");
        yield return new WaitForSeconds(item.firingInterval);
        Notification.Create($"Your {item.title} is ready for use again!");
    }
}
