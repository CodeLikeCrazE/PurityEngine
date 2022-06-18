using System;

/// <summary>
/// A class that manages errors
/// </summary>
class panic {
    public static Exception exception;
    /// <summary>
    /// Raises an exception
    /// </summary>
    public static void raise(Exception ex) {
        exception = ex;
    }

    /// <summary>
    /// Raises an exception
    /// </summary>
    public static void raise(string message) {
        raise(new Exception(message));
    }

    /// <summary>
    /// Raises an exception
    /// </summary>
    public static void raise(string message, Exception ex) {
        raise(new Exception(message, ex));
    }

    /// <summary>
    /// Kills the current thread if there is an exception
    /// </summary>
    public static void check() {
        if (exception != null) {
            throw exception;
        }
    }
}