using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using ExperimentsRunner.Core;
using ExperimentsRunner.Experiments;

namespace ExperimentRunner.App.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Write("Discovering Experiments...");

            var experimentsDiscoveryService = new ExperimentsDiscoveryService();
            var experimentsRunner = new ExperimentsRunner.Core.ExperimentsRunner();

            var experiments = experimentsDiscoveryService.DiscoverExperiments(new[]
            {
                typeof(StringConcatSpeedTest).Assembly,
            });

            Write($"Found {experiments.Count} experiments:");
            for (var i = 0; i < experiments.Count; i++)
                Write($"{i} - {experiments[i].Name} ({experiments[i].Guid}) - {experiments[i].Type.FullName}");

            WriteNewLine();

            Write("Enter experiment number to run:");

            var selectedExperimentNumber = int.Parse(Read());

            var selectedExperiment = experiments[selectedExperimentNumber];

            Write("Enter arguments as requested below:");

            var arguments = new List<object>(selectedExperiment.EntryPointParameterInfo.Count);
            foreach (var param in selectedExperiment.EntryPointParameterInfo)
            {
                Write($"Enter value for parameter: {param.Name} | Parameter is of type: {param.ParameterType.Name}");
                var enteredValueStr = Read();

                var paramTypeConverter = TypeDescriptor.GetConverter(param.ParameterType);
                var enteredValue = paramTypeConverter.ConvertFromString(enteredValueStr);

                arguments.Add(enteredValue);
            }

            WriteNewLine();
            Write($"Running experiment: {selectedExperiment.Name} ({selectedExperiment.Guid}) | {selectedExperiment.Type.FullName}...");

            var experimentRunnerArguments = new List<(ExperimentDiscoveryInfo ExperimentDiscoveryInfo, IList<object> Arguments)>
            {
                (selectedExperiment, arguments)
            };

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var experimentResults = experimentsRunner.RunExperiments(experimentRunnerArguments).FirstOrDefault();

            stopwatch.Stop();

            Write($"Finished running experiments in {stopwatch.ElapsedMilliseconds}ms");

            WriteNewLine();

            Write("Results: ");

            foreach (var resultLine in experimentResults?.GetAllResults() ?? new Dictionary<string, object>())
                Write($"{resultLine.Key}: {resultLine.Value}");

            Write("All done!");
            Read();
        }

        static void Write(string str) => System.Console.WriteLine(str);

        static void WriteNewLine() => Write(string.Empty);

        static string Read() => System.Console.ReadLine();
    }
}
