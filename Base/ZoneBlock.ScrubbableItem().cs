public Item ScrubbableItem()
{
    Item item = this.ItemByLayer(this.ScrubbableLayer());
    if (item != null && !item.entity) {
        return item;
    }
    return null;
}