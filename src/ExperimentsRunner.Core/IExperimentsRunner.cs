using System.Collections.Generic;

namespace ExperimentsRunner.Core
{
    public interface IExperimentsRunner
    {
        IEnumerable<IExperimentResults> RunExperiments(IEnumerable<(ExperimentDiscoveryInfo ExperimentDiscoveryInfo, IList<object> Arguments)> experimentsAndArguments);
    }
}
