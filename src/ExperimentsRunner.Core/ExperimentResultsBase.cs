using System.Collections.Generic;
using System.Linq;

namespace ExperimentsRunner.Core
{
    public abstract class ExperimentResultsBase : IExperimentResults
    {
        public ExperimentDiscoveryInfo ExperimentInfo { get; set; }

        public IDictionary<string, object> Arguments { get; set; }

        public virtual IDictionary<string, object> GetAllResults()
        {
            var typeProps = this.GetType().GetProperties();
            return typeProps.Where(prop => !new[] { nameof(IExperimentResults.Arguments), nameof(ExperimentInfo) }.Contains(prop.Name)).Select(prop =>
            {
                var propName = prop.Name;
                var propVal = prop.GetValue(this);

                return (propName, propVal);
            }).ToDictionary(item => item.propName, item => item.propVal);
        }
    }
}
