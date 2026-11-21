//-----------------------------------------------------------------------
// <copyright file="SemaphoreSlimExtensions.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Threading
{
    /// <summary>
    /// Provides extension methods for the <see cref="SemaphoreSlim"/> class.
    /// </summary>
    public static class SemaphoreSlimExtensions
    {
        /// <summary>
        /// Asynchronously acquires the semaphore and returns a scope that releases it when disposed.
        /// </summary>
        /// <param name="semaphore">The semaphore to acquire.</param>
        /// <param name="cancellationToken">The token used to cancel the wait.</param>
        /// <returns>A task returning a scope that releases the semaphore at most once.</returns>
        /// <remarks>
        /// The scope does not dispose the semaphore. The semaphore must remain alive until the scope is disposed.
        /// Cancellation after acquisition does not release the semaphore; dispose the scope to release it.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="semaphore"/> is <see langword="null"/>.</exception>
        /// <exception cref="OperationCanceledException">The wait was cancelled.</exception>
        /// <exception cref="ObjectDisposedException">The semaphore has been disposed.</exception>
        public static async Task<IDisposable> EnterScopeAsync(this SemaphoreSlim semaphore, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(semaphore);

            await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

            return new SemaphoreScope(semaphore);
        }

        private sealed class SemaphoreScope : IDisposable
        {
            private SemaphoreSlim? semaphore;

            public SemaphoreScope(SemaphoreSlim semaphore)
            {
                this.semaphore = semaphore;
            }

            public void Dispose()
            {
                Interlocked.Exchange(ref this.semaphore, null)?.Release();
            }
        }
    }
}