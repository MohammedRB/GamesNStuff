// Platformer Game Kit // https://kybernetik.com.au/platformer // Copyright 2021 Kybernetik //

using System;

namespace PlatformerGameKit
{
    /// <summary>A system for making data available during method calls without directly passing it in as a parameter.</summary>
    /// <remarks>This system is thread-safe.</remarks>
    /// https://kybernetik.com.au/platformer/api/PlatformerGameKit/Context_1
    /// 
    public readonly struct Context<T> : IDisposable
    {
        /************************************************************************************************************************/

        [ThreadStatic]
        private static T _Current;
        public static ref T Current
        {
            get
            {
#if UNITY_EDITOR
                // Since this is a ref property we don't have a setter that runs when it is assigned.
                // So we have to capture the StackTrace whenever it is accessed unless a value has been assigned.
                if (enableStackTrace &&
                    (_CurrentStackTrace == null || Equals(_Current, BoxedDefault)))
                    _CurrentStackTrace = new System.Diagnostics.StackTrace(1, true);
#endif
                return ref _Current;
            }
        }

        /************************************************************************************************************************/

        private readonly T Previous;

        /************************************************************************************************************************/

        /// <summary>Creates a new <see cref="Context{T}"/>.</summary>
        public Context(T value)
        {
            Previous = Current;
            _Current = value;
#if UNITY_EDITOR
            PreviousStackTrace = _CurrentStackTrace;
            if (enableStackTrace)
                _CurrentStackTrace = new System.Diagnostics.StackTrace(1, true);
#endif
        }

        /// <summary>Creates a new <see cref="Context{T}"/>.</summary>
        public static implicit operator Context<T>(T value) => new Context<T>(value);

        /************************************************************************************************************************/

        public void Dispose()
        {
            _Current = Previous;
#if UNITY_EDITOR
            _CurrentStackTrace = PreviousStackTrace;
#endif
        }

        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        public static bool enableStackTrace;

        private static readonly object BoxedDefault = default(T);

        private static System.Diagnostics.StackTrace _CurrentStackTrace;

        private readonly System.Diagnostics.StackTrace PreviousStackTrace;

        /************************************************************************************************************************/

        static Context()
        {
            UnityEditor.EditorApplication.update += () =>
            {
                if (!Equals(Current, BoxedDefault))
                {
                    enableStackTrace = true;

                    var stackTrace = _CurrentStackTrace != null ?
                        $"\n{_CurrentStackTrace}" :
                        $"Null because {nameof(Context<T>)}<{typeof(T).FullName}>.{nameof(enableStackTrace)} was false" +
                        $" when the {nameof(Context<T>)} was created. It is now set to true for future usage, but may" +
                        $" need to be set manually on startup to identify where the first issue occurs.";

                    UnityEngine.Debug.LogError(
                        $"{nameof(Context<T>)}<{typeof(T).FullName}> hasn't been disposed." +
                        $"\n- {nameof(Current)}: {Current}" +
                        $"\n- {nameof(System.Diagnostics.StackTrace)}: {stackTrace}",
                        Current as UnityEngine.Object);

                    _Current = default;
                    _CurrentStackTrace = null;
                }
            };
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}