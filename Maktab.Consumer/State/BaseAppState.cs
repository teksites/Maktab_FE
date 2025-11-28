namespace Maktab.Consumer.State
{
     public abstract class BaseAppState
     {
          // =============================
          // Event System
          // =============================
          public event Action? OnChange;
          protected void NotifyStateChanged() => OnChange?.Invoke();

          public virtual void ForceReload()
          {
               NotifyStateChanged();
          }
     }
}
