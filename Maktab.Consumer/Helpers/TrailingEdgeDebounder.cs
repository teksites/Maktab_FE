namespace Maktab.Consumer.Helpers
{
     public class TrailingEdgeDebounder : IDisposable
     {
          private CancellationTokenSource _cts = new();
          private readonly Func<CancellationToken, Task> _action;

          /// <param name="action">
          ///   The work to run after silence.
          ///   Receives a CancellationToken so in-flight HTTP calls
          ///   can be cancelled if the user types again.
          /// </param>
          public TrailingEdgeDebounder(Func<CancellationToken, Task> action)
              => _action = action;

          /// <summary>
          /// Call on every keystroke. Cancels any pending timer and
          /// starts a fresh one. The action fires only if no further
          /// call arrives within <paramref name="delayMs"/> ms.
          /// </summary>
          public void Trigger(int delayMs)
          {
               // Cancel the previous pending timer (and any in-flight search)
               _cts.Cancel();
               _cts.Dispose();
               _cts = new CancellationTokenSource();

               var token = _cts.Token;

               _ = Task.Delay(delayMs, token).ContinueWith(
                   async t =>
                   {
                        // Only run if the delay completed — not if it was cancelled
                        if (!t.IsCanceled)
                             await _action(token);
                   },
                   TaskScheduler.Default);
          }

          public void Dispose()
          {
               _cts.Cancel();
               _cts.Dispose();
          }
     }
}
