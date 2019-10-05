using System.Diagnostics;
using System.Linq;
using ExperimentsRunner.Core;

namespace ExperimentsRunner.Experiments
{
    [Experiment("{A0FEB4F8-4CA5-4E28-BFFE-8FC5F38384E0}", "For VS. ForEach Speed Test")]
    public class ForVsForEachSpeedTest : IExperiment
    {
        [ExperimentEntryPoint]
        public IExperimentResults Fight(int itemsCount)
        {
            var stopwatch = new Stopwatch();
            var result = new ForVsForEachSpeedTestResult();

            var c = 0l;

            stopwatch.Start();
            for (var i = 0; i < itemsCount; i++)
                c++;
            stopwatch.Stop();
            result.ForDuration = stopwatch.ElapsedMilliseconds;

            stopwatch.Reset();

            c = 0l;
            var col = Enumerable.Repeat(1, itemsCount);

            stopwatch.Start();
            foreach (var item in col)
                c += item;
            stopwatch.Stop();

            result.ForEachDuration = stopwatch.ElapsedMilliseconds;

            return result;
        }
    }

    public class ForVsForEachSpeedTestResult : ExperimentResultsBase
    {
        public double ForDuration { get; set; }

        public double ForEachDuration { get; set; }
    }
}
