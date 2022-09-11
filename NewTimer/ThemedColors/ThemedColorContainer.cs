using System;
using System.Drawing;

namespace NewTimer.ThemedColors
{
    public abstract class ThemedColorContainer<T>
    {
        private Color _lastColor;
        private T _current;

        /// <summary>
        /// Gets the themed color instance of this themed solid brush
        /// </summary>
        public ThemedColor Color { get; }

        /// <summary>
        /// Creates a new instance of a themed color container
        /// </summary>
        /// <param name="color"></param>
        public ThemedColorContainer(ThemedColor color)
        {
            Color = color;
            _current = CreateNewInstance();
        }

        /// <summary>
        /// Gets the currently applicable <see cref="T"/> based on the current color.
        /// </summary>
        public T Current
        {
            get
            {
                if (_lastColor.R != Color.Current.R ||
                    _lastColor.G != Color.Current.G ||
                    _lastColor.B != Color.Current.B ||
                    _lastColor.A != Color.Current.A
                )
                {
                    OnDestroyOldInstance(_current);
                    _current = CreateNewInstance();
                }

                return _current;
            }
        }

        /// <summary>
        /// Gets the currently applicable <see cref="T"/> based on the current color.
        /// </summary>
        public static implicit operator T(ThemedColorContainer<T> a)
        {
            return a.Current;
        }

        /// <summary>
        /// Creates a new instance 
        /// </summary>
        /// <returns></returns>
        protected abstract T CreateNewInstance();

        /// <summary>
        /// Called when an old instance is about to be destroyed
        /// </summary>
        /// <param name="oldInstance"></param>
        protected virtual void OnDestroyOldInstance(T oldInstance)
        {  }
    }
}