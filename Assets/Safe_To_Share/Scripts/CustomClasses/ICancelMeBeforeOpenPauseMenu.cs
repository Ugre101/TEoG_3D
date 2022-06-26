namespace CustomClasses
{
    public interface ICancelMeBeforeOpenPauseMenu
    {
        /// <summary>
        ///     If gameObject is activeSelf then set gameObject SetActive to false and return true
        /// </summary>
        /// <returns></returns>
        public bool BlockIfActive();
    }
}