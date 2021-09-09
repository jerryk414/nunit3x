using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nunit3x.Suite
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
        TType GetValue<TType>() where TType : class;

        /// <summary>
        /// Sets the actual value for this property
        /// </summary>
        void SetValue<TType>(TType value) where TType : class;
    }

    public sealed class ConcurrentProperty<TType> : IConcurrentProperty
        where TType: class
    {
        #region Construction

        public ConcurrentProperty(Guid key, TType @default)
        {
            this.Key = key;
            this.Default = @default;
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
            where TValueType : class
        {
            if (typeof(TValueType) != typeof(TType))
            {
                throw new Exception($"Concurrent property '{ this.Key }' type '{ typeof(TType) }' does not match requested type '{ typeof(TValueType) }'");
            }

            return _value as TValueType;
        }

        /// <summary>
        /// Gets the actual value for this property
        /// </summary>
        public void SetValue<TValueType>(TValueType value)
            where TValueType : class
        {
            if (typeof(TValueType) != typeof(TType))
            {
                throw new Exception($"Concurrent property '{ this.Key }' type '{ typeof(TType) }' does not match requested type '{ typeof(TValueType) }'");
            }

            _value = value as TType;
        }

        #endregion
    }
}
