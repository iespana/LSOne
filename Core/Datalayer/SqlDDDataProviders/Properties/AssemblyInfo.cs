using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Sql DD DataProviders")]
[assembly: AssemblyDescription("LS One SQL DD Data Providers")]
#if DEBUG
[assembly: AssemblyProduct("SQL DD Data Providers - DEBUG version")]
#else
    [assembly: AssemblyProduct("SQL DD Data Providers")]
#endif

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d5b0b404-da5f-4115-9cc6-9cfe86344482")]

[assembly: Obfuscation(Feature = "optimization", Exclude = true)]


