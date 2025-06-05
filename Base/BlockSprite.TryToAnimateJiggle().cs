public void TryToAnimateJiggle(Item item) {
    if(!this.animating) {
        base.StartCoroutine("AnimateJiggle", new object[] {item.jiggle, (float)item.blockSize.width, (float)item.blockSize.height});
        this.animating = true;
    }
}
