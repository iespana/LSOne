using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Business Objects")]
[assembly: AssemblyDescription("Site Manager Business Objects")]
#if DEBUG
[assembly: AssemblyProduct("Business Objects - DEBUG version")]
#else
[assembly: AssemblyProduct("Business Objects")]
#endif

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d9944098-f56a-4994-a22a-c93d96e1944f")]

[assembly: InternalsVisibleTo("LSOne.DataLayer.KDSBusinessObjects")]
[assembly: InternalsVisibleTo("LSOne.DataLayer.SqlDataProviders")]
