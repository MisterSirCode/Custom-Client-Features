private void StopAnimation()
{
	if (this.animating)
	{
		base.StopCoroutine("Animate");
		base.StopCoroutine("AnimateJiggle");
		this.animating = false;
	}
}
