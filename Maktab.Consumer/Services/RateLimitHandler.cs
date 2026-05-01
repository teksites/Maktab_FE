using System;

namespace Maktab.Consumer.Services
{
    /// <summary>
    /// Handles rate limiting with configurable failed attempts threshold and lockout duration.
    /// Prevents brute force attacks by locking out after N failed attempts for M seconds.
    /// </summary>
    public class RateLimitHandler
    {
        private int _failedAttempts = 0;
        private DateTime? _lockoutUntil = null;
        private readonly int _maxAttempts;
        private readonly int _lockoutDurationSeconds;
        private int _remainingSeconds = 0;
        private System.Timers.Timer _countdownTimer;

        /// <summary>
        /// Initializes a new instance of RateLimitHandler
        /// </summary>
        /// <param name="maxAttempts">Maximum failed attempts before lockout (default: 3)</param>
        /// <param name="lockoutDurationSeconds">Duration of lockout in seconds (default: 30)</param>
        public RateLimitHandler(int maxAttempts = 3, int lockoutDurationSeconds = 30)
        {
            _maxAttempts = maxAttempts;
            _lockoutDurationSeconds = lockoutDurationSeconds;
        }

        /// <summary>
        /// Gets the current number of failed attempts
        /// </summary>
        public int FailedAttempts => _failedAttempts;

        /// <summary>
        /// Gets whether the account is currently locked out
        /// </summary>
        public bool IsLockedOut => _lockoutUntil.HasValue && DateTime.UtcNow < _lockoutUntil.Value;

        /// <summary>
        /// Gets the remaining seconds in lockout (0 if not locked out)
        /// </summary>
        public int RemainingSeconds => _remainingSeconds;

        /// <summary>
        /// Gets the remaining attempts before lockout
        /// </summary>
        public int RemainingAttempts => Math.Max(0, _maxAttempts - _failedAttempts);

        /// <summary>
        /// Checks if currently locked out and updates lockout status
        /// </summary>
        public bool CheckLockout()
        {
            if (_lockoutUntil.HasValue && DateTime.UtcNow >= _lockoutUntil.Value)
            {
                // Lockout duration has passed, reset
                Reset();
                return false;
            }

            if (IsLockedOut)
            {
                _remainingSeconds = (int)Math.Ceiling((_lockoutUntil.Value - DateTime.UtcNow).TotalSeconds);
            }

            return IsLockedOut;
        }

        /// <summary>
        /// Records a failed attempt and applies lockout if threshold exceeded
        /// </summary>
        public void RecordFailedAttempt()
        {
            _failedAttempts++;

            if (_failedAttempts >= _maxAttempts)
            {
                _lockoutUntil = DateTime.UtcNow.AddSeconds(_lockoutDurationSeconds);
                _remainingSeconds = _lockoutDurationSeconds;
            }
        }

        /// <summary>
        /// Records a successful attempt and clears all failed attempts
        /// </summary>
        public void RecordSuccess()
        {
            Reset();
        }

        /// <summary>
        /// Resets all failed attempts and lockout state
        /// </summary>
        public void Reset()
        {
            _failedAttempts = 0;
            _lockoutUntil = null;
            _remainingSeconds = 0;
            StopCountdownTimer();
        }

        /// <summary>
        /// Starts the countdown timer for lockout duration
        /// </summary>
        public void StartCountdownTimer(Action onTick = null)
        {
            if (_countdownTimer != null)
            {
                _countdownTimer.Stop();
                _countdownTimer.Dispose();
            }

            _countdownTimer = new System.Timers.Timer(1000); // 1 second interval
            _countdownTimer.Elapsed += (sender, e) =>
            {
                _remainingSeconds--;
                if (_remainingSeconds <= 0)
                {
                    StopCountdownTimer();
                    Reset();
                }
                onTick?.Invoke();
            };
            _countdownTimer.AutoReset = true;
            _countdownTimer.Start();
        }

        /// <summary>
        /// Stops the countdown timer
        /// </summary>
        public void StopCountdownTimer()
        {
            if (_countdownTimer != null)
            {
                _countdownTimer.Stop();
                _countdownTimer.Dispose();
                _countdownTimer = null;
            }
        }

        /// <summary>
        /// Cleans up resources
        /// </summary>
        public void Dispose()
        {
            StopCountdownTimer();
        }
    }
}
