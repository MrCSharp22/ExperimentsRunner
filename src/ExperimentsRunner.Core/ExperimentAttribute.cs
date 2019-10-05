using System;

namespace ExperimentsRunner.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class ExperimentAttribute : Attribute
    {
        public string Guid { get; }

        public string ExperimentName { get; }

        public ExperimentAttribute(string guid,
            string experimentName)
        {
            this.Guid = guid;
            this.ExperimentName = experimentName;
        }
    }
}
