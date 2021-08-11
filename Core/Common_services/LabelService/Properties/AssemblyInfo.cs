using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Label Service")]
[assembly: AssemblyDescription("Service for printing and queueing label printing")]
#if DEBUG
[assembly: AssemblyProduct("Label Service - DEBUG version")]
#else
[assembly: AssemblyProduct("Label Service")]
#endif

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("729A8C0E-9854-4CE9-9AB4-B3479791BA1E")]
