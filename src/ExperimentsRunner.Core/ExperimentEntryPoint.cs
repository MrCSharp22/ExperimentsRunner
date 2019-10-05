using System;

namespace ExperimentsRunner.Core
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class ExperimentEntryPointAttribute : Attribute
    {
    }
}
