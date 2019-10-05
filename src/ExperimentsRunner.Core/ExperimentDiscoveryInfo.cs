using System;
using System.Collections.Generic;
using System.Reflection;

namespace ExperimentsRunner.Core
{
    public sealed class ExperimentDiscoveryInfo
    {
        public Guid Guid { get; }

        public string Name { get; }

        public Type Type { get; }

        public MethodInfo EntryPointMethodInfo { get; }

        public IList<ParameterInfo> EntryPointParameterInfo { get; }

        public ExperimentDiscoveryInfo(Guid guid, 
            string name, 
            Type type,
            MethodInfo entryPointMethodInfo,
            IList<ParameterInfo> entryPointParameterInfo)
        {
            this.Guid = guid;
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Type = type ?? throw new ArgumentNullException(nameof(type));
            this.EntryPointMethodInfo = entryPointMethodInfo ?? throw new ArgumentNullException(nameof(entryPointMethodInfo));
            this.EntryPointParameterInfo = entryPointParameterInfo ?? throw new ArgumentNullException(nameof(entryPointParameterInfo));
        }
    }
}
