using System.Collections.Generic;

namespace ExperimentsRunner.Core
{
    public interface IExperimentResults
    {
        ExperimentDiscoveryInfo ExperimentInfo { get; set; }

        IDictionary<string, object> Arguments { get; set; }

        IDictionary<string, object> GetAllResults();
    }
}
