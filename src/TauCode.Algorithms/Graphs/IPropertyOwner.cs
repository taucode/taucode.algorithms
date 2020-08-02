using System.Collections.Generic;

namespace TauCode.Algorithms.Graphs
{
    public interface IPropertyOwner
    {
        void SetProperty(string propertyName, object propertyValue);
        object GetProperty(string propertyName);
        bool HasProperty(string propertyName);
        IReadOnlyList<string> PropertyNames { get; }
    }
}
