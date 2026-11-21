//-----------------------------------------------------------------------
// <copyright file="DebouncerTest.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Foundations.Threading.Tests
{
    public class DebouncerTest
    {
        [Fact]
        public void Constructor_WithNullAction()
        {
            var act = () => new Debouncer(null, TimeSpan.Zero);

            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("action");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(4294967295)]
        public void Constructor_WithInvalidDelay(double milliseconds)
        {
            var delay = TimeSpan.FromMilliseconds(milliseconds);
            var act = () => new Debouncer(_ => Task.CompletedTask, delay);

            act.Should().ThrowExactly<ArgumentOutOfRangeException>()
                .WithParameterName("delay")
                .WithMessage("The delay must be between zero and 4,294,967,294 milliseconds.*")
                .Which.ActualValue.Should().Be(delay);
        }

        [Fact]
        public async Task Dispose_CancelsPendingInvocation()
        {
            var calls = 0;
            using var debouncer = new Debouncer(
                _ =>
                {
                    calls++;
                    return Task.CompletedTask;
                },
                TimeSpan.FromSeconds(10));

            var invocation = debouncer.InvokeAsync();
            debouncer.Dispose();
            await invocation;

            calls.Should().Be(0);
            debouncer.IsInProgress.Should().BeFalse();
        }

        [Fact]
        public async Task Dispose_CancelsRunningOperation()
        {
            var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            using var debouncer = new Debouncer(
                async token =>
                {
                    started.SetResult();
                    await Task.Delay(Timeout.InfiniteTimeSpan, token);
                },
                TimeSpan.Zero);

            var invocation = debouncer.InvokeAsync();
            await started.Task;
            debouncer.Dispose();
            await invocation;

            debouncer.IsInProgress.Should().BeFalse();
        }

        [Fact]
        public async Task InvokeAsync_CancelsRunningOperation()
        {
            var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var calls = 0;
            using var debouncer = new Debouncer(
                async token =>
                {
                    var call = Interlocked.Increment(ref calls);
                    if (call == 1)
                    {
                        started.SetResult();
                        await Task.Delay(Timeout.InfiniteTimeSpan, token);
                    }
                },
                TimeSpan.Zero);

            var first = debouncer.InvokeAsync();
            await started.Task;
            var second = debouncer.InvokeAsync();
            await Task.WhenAll(first, second);

            calls.Should().Be(2);
            debouncer.IsInProgress.Should().BeFalse();
        }

        [Fact]
        public async Task InvokeAsync_DebouncesPendingInvocations()
        {
            var calls = 0;
            using var debouncer = new Debouncer(
                _ =>
                {
                    calls++;
                    return Task.CompletedTask;
                },
                TimeSpan.FromMilliseconds(200));

            var elapsed = System.Diagnostics.Stopwatch.StartNew();
            var first = debouncer.InvokeAsync();
            var second = debouncer.InvokeAsync();

            debouncer.IsInProgress.Should().BeTrue();
            calls.Should().Be(0);

            await Task.WhenAll(first, second);

            calls.Should().Be(1);
            elapsed.Elapsed.Should().BeGreaterThanOrEqualTo(TimeSpan.FromMilliseconds(150));
            debouncer.IsInProgress.Should().BeFalse();
        }

        [Fact]
        public async Task InvokeAsync_PreviousCompletionDoesNotClearCurrentOperation()
        {
            var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var calls = 0;
            using var debouncer = new Debouncer(
                async token =>
                {
                    Interlocked.Increment(ref calls);
                    started.TrySetResult();
                    await Task.Delay(Timeout.InfiniteTimeSpan, token);
                },
                TimeSpan.Zero);

            var first = debouncer.InvokeAsync();
            await started.Task;
            var second = debouncer.InvokeAsync();
            await first;

            calls.Should().Be(2);
            debouncer.IsInProgress.Should().BeTrue();

            debouncer.Dispose();
            await second;

            debouncer.IsInProgress.Should().BeFalse();
        }

        [Fact]
        public async Task InvokeAsync_PropagatesActionException()
        {
            using var debouncer = new Debouncer(
                _ => Task.FromException(new InvalidOperationException("Action failed")),
                TimeSpan.Zero);

            var invoke = () => debouncer.InvokeAsync();

            await invoke.Should()
                .ThrowExactlyAsync<InvalidOperationException>()
                .WithMessage("Action failed");
            debouncer.IsInProgress.Should().BeFalse();
        }

        [Fact]
        public async Task InvokeAsync_PropagatesUnrelatedCancellation()
        {
            using var debouncer = new Debouncer(
                _ => Task.FromException(new OperationCanceledException("Unrelated cancellation")),
                TimeSpan.Zero);

            var invoke = () => debouncer.InvokeAsync();

            await invoke.Should()
                .ThrowExactlyAsync<OperationCanceledException>()
                .WithMessage("Unrelated cancellation");
            debouncer.IsInProgress.Should().BeFalse();
        }

        [Fact]
        public async Task InvokeAsync_ThrowsAfterDisposal()
        {
            using var debouncer = new Debouncer(_ => Task.CompletedTask, TimeSpan.Zero);
            debouncer.Dispose();

            var invoke = () => debouncer.InvokeAsync();

            await invoke.Should()
                .ThrowExactlyAsync<ObjectDisposedException>()
                .WithMessage("*Debouncer*");
        }
    }
}