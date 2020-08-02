using System.Collections.Generic;

namespace TauCode.Algorithms.Graphs
{
    public class PropertyOwnerImpl : IPropertyOwner
    {
        #region IPropertyOwner Members

        public void SetProperty(string propertyName, object propertyValue)
        {
            throw new System.NotImplementedException();
        }

        public object GetProperty(string propertyName)
        {
            throw new System.NotImplementedException();
        }

        public bool HasProperty(string propertyName)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyList<string> PropertyNames { get; }


        #endregion
    }
}
