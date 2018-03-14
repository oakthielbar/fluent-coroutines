namespace FluentCoroutines.Extensions
{
    public static class FCBuilderExtensions
    {
        /// <summary>
        /// Creates a finalized FluentCoroutine and executes it.
        /// </summary>
        /// <returns>Finalized form of the FluentCoroutine.</returns>
        /// <remarks>
        /// This method implicitly calls <see cref="FCBuilder.Finalize"/> and returns the result of that call.
        /// In other words, this method allocates memory for a new instance of FluentCoroutine every
        /// time it is called. If you intend to reuse the FluentCoroutine, subsequent calls to Execute
        /// should be made on the returned FluentCoroutine rather than the FCBuilder.
        /// </remarks>
        public static FluentCoroutine Execute(this FCBuilder builder)
        {
            return FluentCoroutine.Finalize(builder).Execute();
        }
    }
}