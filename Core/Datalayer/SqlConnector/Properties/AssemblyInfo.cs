using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("LS One Sql Connector")]
[assembly: AssemblyDescription("")]
#if DEBUG
[assembly: AssemblyProduct("SqlConnector - DEBUG version")]
#else
[assembly: AssemblyProduct("SqlConnector")]
#endif

[assembly: InternalsVisibleTo("LSOne.DataLayer.SqlDataProviders")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("ff72823c-dbb7-4d47-923a-1ee16ddb2a57")]
