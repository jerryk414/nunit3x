using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit3x.Suite
{
    public interface IConcurrentProperty
    {

        /// <summary>
        /// Gets the <see cref="PropertyKey"/>
        /// </summary>
        Guid Key { get; }

        /// <summary>
        /// Gets the default value for this property
        /// </summary>
        object Default { get; }

        /// <summary>
        /// Gets the actual value for this property
        /// </summary>
        TType GetValue<TType>();

        /// <summary>
        /// Sets the actual value for this property
        /// </summary>
        void SetValue<TType>(TType value);
    }

    public sealed class ConcurrentProperty<TType> : IConcurrentProperty
    {
        #region Construction

        public ConcurrentProperty(Guid key, TType @default)
        {
            this.Key = key;
            this.Default = @default;

            _value = @default;
        }

        #endregion

        #region Fields

        private TType _value;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="PropertyKey"/>
        /// </summary>
        public Guid Key { get; }
        
        /// <summary>
        /// Gets the default value for this property
        /// </summary>
        public TType Default { get; }

        object IConcurrentProperty.Default => this.Default;

        #endregion

        #region Methods

        /// <summary>
        /// Gets the actual value for this property
        /// </summary>
        public TValueType GetValue<TValueType>()
        {
            if (typeof(TValueType) != typeof(TType))
            {
                throw new Exception($"Concurrent property '{ this.Key }' type '{ typeof(TType) }' does not match requested type '{ typeof(TValueType) }'");
            }

            return (TValueType)((object)_value);
        }

        /// <summary>
        /// Gets the actual value for this property
        /// </summary>
        public void SetValue<TValueType>(TValueType value)
        {
            if (typeof(TValueType) != typeof(TType))
            {
                throw new Exception($"Concurrent property '{ this.Key }' type '{ typeof(TType) }' does not match requested type '{ typeof(TValueType) }'");
            }

            _value = (TType)((object)value);
        }

        #endregion
    }
}
