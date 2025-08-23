using Microsoft.AspNetCore.Components;

namespace Maktab.Consumer.Helpers
{
     /// <summary>
     /// Internal throttler helper.
     /// </summary>
     internal class RenderThrottle
     {
          private bool _pending;
          private readonly Func<Task> _renderRequest;

          public RenderThrottle(Func<Task> renderRequest) => _renderRequest = renderRequest;

          public void RequestRender(int delayMs)
          {
               if (_pending) return;

               _pending = true;
               _ = Task.Delay(delayMs).ContinueWith(async _ =>
               {
                    await _renderRequest();
                    _pending = false;
               });
          }
     }

}
