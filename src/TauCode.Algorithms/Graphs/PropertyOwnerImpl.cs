using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Algorithms.Graphs
{
    internal class PropertyOwnerImpl : IPropertyOwner
    {
        #region Fields

        private readonly Dictionary<string, object> _properties;

        #endregion

        #region Constructor

        internal PropertyOwnerImpl()
        {
            _properties = new Dictionary<string, object>();
        }

        #endregion

        #region IPropertyOwner Members

        public void SetProperty(string propertyName, object propertyValue)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            _properties[propertyName] = propertyValue;
        }

        public object GetProperty(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (!_properties.ContainsKey(propertyName))
            {
                throw new KeyNotFoundException($"Property '{propertyName}' not found.");
            }

            return _properties[propertyName];
        }

        public bool HasProperty(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            return _properties.ContainsKey(propertyName);
        }

        public IReadOnlyList<string> PropertyNames => _properties.Keys.ToList();

        #endregion
    }
}
