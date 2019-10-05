using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExperimentsRunner.Core
{
    public class ExperimentsRunner : IExperimentsRunner
    {
        public IEnumerable<IExperimentResults> RunExperiments(IEnumerable<(ExperimentDiscoveryInfo ExperimentDiscoveryInfo, IList<object> Arguments)> experimentsAndArguments)
        {
            foreach (var experimentToRun in experimentsAndArguments ??
                                            new (ExperimentDiscoveryInfo ExperimentDiscoveryInfo, IList<object>Arguments)[0])
            {
                if (!AreArgumentsValid(experimentToRun.Arguments,
                    experimentToRun.ExperimentDiscoveryInfo.EntryPointParameterInfo))
                {
                    throw new InvalidOperationException($"Arguments mismatch in experiment run: " +
                                                        $"{experimentToRun.ExperimentDiscoveryInfo.Name} " +
                                                        $"- {experimentToRun.ExperimentDiscoveryInfo.Guid} " +
                                                        $"- {experimentToRun.ExperimentDiscoveryInfo.Type}");
                }

                var experimentInstance = Activator.CreateInstance(experimentToRun.ExperimentDiscoveryInfo.Type);

                var runResult = (IExperimentResults)experimentToRun
                    .ExperimentDiscoveryInfo
                    .EntryPointMethodInfo
                    .Invoke(experimentInstance,experimentToRun.Arguments.ToArray());

                runResult.Arguments = MapParametersAndIncomingArguments(experimentToRun.Arguments,
                    experimentToRun.ExperimentDiscoveryInfo.EntryPointParameterInfo);

                runResult.ExperimentInfo = experimentToRun.ExperimentDiscoveryInfo;

                yield return runResult;
            }
        }

        private bool AreArgumentsValid(IList<object> incomingArguments,
            IList<ParameterInfo> entryPointParameters)
        {
            var sameCount = incomingArguments.Count == entryPointParameters.Count;
            var sameOrderAndType = true;

            for (var i = 0; i < incomingArguments.Count; i++)
            {
                sameOrderAndType &= incomingArguments[i].GetType() == entryPointParameters[i].ParameterType;
            }

            return sameCount && sameOrderAndType;
        }

        private IDictionary<string, object> MapParametersAndIncomingArguments(IList<object> incomingArguments,
            IList<ParameterInfo> entryPointParameters)
        {
            var mappedParamsAndArgs = new Dictionary<string, object>();
            for (var i = 0; incomingArguments.Count == entryPointParameters.Count && i < incomingArguments.Count; i++)
            {
                var paramName = entryPointParameters[i].Name;
                var paramVal = incomingArguments[i];

                mappedParamsAndArgs.Add(paramName, paramVal);
            }

            return mappedParamsAndArgs;
        }
    }
}
