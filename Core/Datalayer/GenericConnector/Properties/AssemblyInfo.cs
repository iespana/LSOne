using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("LSOne Generic database connector")]
[assembly: AssemblyDescription("")]
#if DEBUG
[assembly: AssemblyProduct("GenericConnector - DEBUG version")]
#else
[assembly: AssemblyProduct("GenericConnector")]
#endif

[assembly: InternalsVisibleTo("LSOne.DataLayer.SqlConnector")]
[assembly: InternalsVisibleTo("LSOne.DataLayer.SqlDataProviders")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("b63b8cfc-6c7c-49bb-9b88-a4d3b393b876")]


