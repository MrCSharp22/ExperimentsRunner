using System.Diagnostics;
using System.Text;
using ExperimentsRunner.Core;

namespace ExperimentsRunner.Experiments
{
    [Experiment("{FC2F92EA-F8D8-4C37-B34A-36E5C2FE1B5E}", nameof(StringConcatSpeedTest))]
    public class StringConcatSpeedTest : IExperiment
    {
        [ExperimentEntryPoint]
        public IExperimentResults Run(string fragment, int repeats)
        {
            var result = new StringConcatSpeedTestResult();
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            var finalString = string.Empty;
            for (var i = 0; i < repeats; i++)
            {
                finalString += fragment;
            }

            stopwatch.Stop();
            result.ManualConcatDuration = stopwatch.ElapsedMilliseconds;

            stopwatch.Reset();
            finalString = string.Empty;

            stopwatch.Start();

            var stringBuilder = new StringBuilder(fragment.Length * repeats);
            for (var i = 0; i < repeats; i++)
            {
                stringBuilder.Append(fragment);
            }

            finalString = stringBuilder.ToString();

            stopwatch.Stop();
            result.StringBuilderConcatDuration = stopwatch.ElapsedMilliseconds;

            return result;
        }
    }

    public class StringConcatSpeedTestResult : ExperimentResultsBase
    {
        public double ManualConcatDuration { get; set; }

        public double StringBuilderConcatDuration { get; set; }
    }
}
