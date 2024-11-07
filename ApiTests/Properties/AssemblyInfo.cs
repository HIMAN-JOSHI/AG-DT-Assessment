using System.Reflection;

// Assembly metadata
[assembly: AssemblyDescription("API tests for AG-Data assessment.")]
[assembly: AssemblyCopyright("Copyright ©  2024")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Parallel execution settings for MSTest
[assembly: Parallelize(Workers = 4, Scope = ExecutionScope.ClassLevel)]
