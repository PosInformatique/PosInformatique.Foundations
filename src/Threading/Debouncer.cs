//-----------------------------------------------------------------------
// <copyright file="Debouncer.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Threading
{
    /// <summary>
    /// Debounces asynchronous operations by cancelling the previous invocation and restarting the delay.
    /// </summary>
    public sealed class Debouncer : IDisposable
    {
        private readonly Func<CancellationToken, Task> action;

        private readonly TimeSpan delay;

#if NET9_0_OR_GREATER
        private readonly Lock sync = new Lock();
#else
        private readonly object sync = new object();
#endif

        private Operation? current;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Debouncer"/> class.
        /// </summary>
        /// <param name="action">The asynchronous action, which should observe the supplied cancellation token.</param>
        /// <param name="delay">The delay to wait before executing the action.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="action"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The <paramref name="delay"/> is negative or exceeds 4,294,967,294 milliseconds.
        /// </exception>
        public Debouncer(Func<CancellationToken, Task> action, TimeSpan delay)
        {
            ArgumentNullException.ThrowIfNull(action);

            if (delay < TimeSpan.Zero || delay.TotalMilliseconds > uint.MaxValue - 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(delay),
                    delay,
                    "The delay must be between zero and 4,294,967,294 milliseconds.");
            }

            this.action = action;
            this.delay = delay;
        }

        /// <summary>
        /// Gets a value indicating whether the latest invocation is waiting or executing.
        /// </summary>
        public bool IsInProgress
        {
            get
            {
                lock (this.sync)
                {
                    return this.current is not null;
                }
            }
        }

        /// <summary>
        /// Requests cancellation of the latest invocation and prevents further invocations.
        /// </summary>
        /// <remarks>This method does not wait for the action or cancellation callbacks to complete.</remarks>
        public void Dispose()
        {
            lock (this.sync)
            {
                this.disposed = true;
                this.current?.Cancel();
                this.current = null;
            }
        }

        /// <summary>
        /// Cancels the previous invocation, waits for the configured delay, and executes the action.
        /// </summary>
        /// <returns>
        /// A task completing when this invocation finishes, including cancellation callbacks.
        /// Cancellation requested by a newer invocation or disposal completes normally.
        /// Other exceptions from the action are propagated.
        /// </returns>
        /// <remarks>
        /// Cancellation is cooperative. A previous action that ignores its token may overlap a newer action.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">This instance has been disposed.</exception>
        public async Task InvokeAsync()
        {
            Operation operation;

            lock (this.sync)
            {
                ObjectDisposedException.ThrowIf(this.disposed, this);
                this.current?.Cancel();
                operation = new Operation();
                this.current = operation;
            }

            try
            {
                await Task.Delay(this.delay, operation.Source.Token);
                await this.action(operation.Source.Token);
            }
            catch (OperationCanceledException) when (operation.Source.IsCancellationRequested)
            {
                // A newer invocation or disposal superseded this operation.
            }
            finally
            {
                Task cancellation;
                lock (this.sync)
                {
                    if (this.current == operation)
                    {
                        this.current = null;
                    }

                    cancellation = operation.Cancellation;
                }

                try
                {
                    await cancellation;
                }
                finally
                {
                    operation.Source.Dispose();
                }
            }
        }

        private sealed class Operation
        {
            public CancellationTokenSource Source { get; } = new CancellationTokenSource();

            public Task Cancellation { get; private set; } = Task.CompletedTask;

            public void Cancel()
            {
                this.Cancellation = this.Source.CancelAsync();
            }
        }
    }
}