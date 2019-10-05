using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExperimentsRunner.Core
{
    public class ExperimentsDiscoveryService : IExperimentsDiscoveryService
    {
        public IList<ExperimentDiscoveryInfo> DiscoverExperiments(Assembly[] assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));

            var result = new List<ExperimentDiscoveryInfo>();

            foreach (var assembly in assemblies)
            {
                var experiments = assembly
                    .DefinedTypes
                    .Where(type => typeof(IExperiment).IsAssignableFrom(type)
                                    && type.IsClass
                                    && !type.IsAbstract);

                foreach (var experiment in experiments)
                {
                    var experimentDefAttribute = experiment.GetCustomAttribute<ExperimentAttribute>();

                    if (experimentDefAttribute == null)
                        continue;

                    var experimentEntryPointMethod = experiment
                        .GetMethods()
                        .FirstOrDefault(method => method.GetCustomAttribute<ExperimentEntryPointAttribute>() != null);

                    if (experimentEntryPointMethod == null)
                        continue;

                    if (!typeof(IExperimentResults).IsAssignableFrom(experimentEntryPointMethod.ReturnType))
                        continue; // maybe log something here to let the user know of their silly mistake

                    var entryPointParams = experimentEntryPointMethod
                        .GetParameters()
                        .ToList();

                    result.Add(new ExperimentDiscoveryInfo(Guid.Parse(experimentDefAttribute.Guid),
                        experimentDefAttribute.ExperimentName,
                        experiment.AsType(),
                        experimentEntryPointMethod,
                        entryPointParams));
                }
            }

            return result;
        }
    }
}
