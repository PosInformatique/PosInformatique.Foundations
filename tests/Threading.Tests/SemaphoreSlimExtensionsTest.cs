//-----------------------------------------------------------------------
// <copyright file="SemaphoreSlimExtensionsTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Threading.Tests
{
    public class SemaphoreSlimExtensionsTest
    {
        [Fact]
        public async Task EnterScopeAsync_AcquiresAndReleasesSemaphore()
        {
            using var semaphore = new SemaphoreSlim(1, 1);

            using (await semaphore.EnterScopeAsync(TestContext.Current.CancellationToken))
            {
                semaphore.CurrentCount.Should().Be(0);
            }

            semaphore.CurrentCount.Should().Be(1);

            using (await semaphore.EnterScopeAsync(TestContext.Current.CancellationToken))
            {
                semaphore.CurrentCount.Should().Be(0);
            }
        }

        [Fact]
        public async Task EnterScopeAsync_WaitsUntilSemaphoreIsReleased()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var first = await semaphore.EnterScopeAsync(TestContext.Current.CancellationToken);

            var waiting = semaphore.EnterScopeAsync(TestContext.Current.CancellationToken);

            waiting.IsCompleted.Should().BeFalse();

            first.Dispose();
            using var second = await waiting.WaitAsync(TimeSpan.FromSeconds(5), TestContext.Current.CancellationToken);

            semaphore.CurrentCount.Should().Be(0);
        }

        [Fact]
        public async Task EnterScopeAsync_WithNullSemaphore()
        {
            var act = () => SemaphoreSlimExtensions.EnterScopeAsync(null, TestContext.Current.CancellationToken);

            await act.Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("semaphore");
        }

        [Fact]
        public async Task EnterScopeAsync_WithDisposedSemaphore()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            semaphore.Dispose();

            var act = () => semaphore.EnterScopeAsync(TestContext.Current.CancellationToken);

            await act.Should().ThrowExactlyAsync<ObjectDisposedException>();
        }

        [Fact]
        public async Task EnterScopeAsync_WithCancelledTokenDoesNotAcquireOrRelease()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var cancellation = new CancellationTokenSource();
            await cancellation.CancelAsync();

            var act = () => semaphore.EnterScopeAsync(cancellation.Token);

            await act.Should().ThrowAsync<OperationCanceledException>();
            semaphore.CurrentCount.Should().Be(1);
        }

        [Fact]
        public async Task EnterScopeAsync_CancellingPendingWaitDoesNotRelease()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var first = await semaphore.EnterScopeAsync(TestContext.Current.CancellationToken);
            using var cancellation = new CancellationTokenSource();

            var waiting = semaphore.EnterScopeAsync(cancellation.Token);
            waiting.IsCompleted.Should().BeFalse();

            await cancellation.CancelAsync();

            var act = () => waiting;

            await act.Should().ThrowAsync<OperationCanceledException>();
            semaphore.CurrentCount.Should().Be(0);

            first.Dispose();
            semaphore.CurrentCount.Should().Be(1);
        }

        [Fact]
        public async Task Dispose_ReleasesSemaphoreWhenScopedCodeThrows()
        {
            using var semaphore = new SemaphoreSlim(1, 1);

            var act = async () =>
            {
                using (await semaphore.EnterScopeAsync(TestContext.Current.CancellationToken))
                {
                    await Task.Yield();
                    throw new InvalidOperationException("Scoped action failed");
                }
            };

            await act.Should().ThrowExactlyAsync<InvalidOperationException>()
                .WithMessage("Scoped action failed");
            semaphore.CurrentCount.Should().Be(1);
        }

        [Fact]
        public async Task Dispose_RepeatedCallsReleaseOnlyOnce()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var scope = await semaphore.EnterScopeAsync(TestContext.Current.CancellationToken);

            scope.Dispose();
            scope.Dispose();

            semaphore.CurrentCount.Should().Be(1);
        }

        [Fact]
        public async Task Dispose_ConcurrentCallsReleaseOnlyOnce()
        {
            using var semaphore = new SemaphoreSlim(1, 1);
            using var scope = await semaphore.EnterScopeAsync(TestContext.Current.CancellationToken);

            await Task.WhenAll(
                Task.Run(scope.Dispose, TestContext.Current.CancellationToken),
                Task.Run(scope.Dispose, TestContext.Current.CancellationToken));

            semaphore.CurrentCount.Should().Be(1);
        }
    }
}