using System.Collections.Generic;
using System.Reflection;

namespace ExperimentsRunner.Core
{
    public interface IExperimentsDiscoveryService
    {
        IList<ExperimentDiscoveryInfo> DiscoverExperiments(Assembly[] assemblies);
    }
}
