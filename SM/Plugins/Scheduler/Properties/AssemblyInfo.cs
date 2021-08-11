using System.Reflection;
using System.Runtime.InteropServices;
using LSOne.DataLayer.BusinessObjects;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Job Scheduler")]
[assembly: AssemblyDescription("Plugin for Job Replication")]
#if DEBUG
[assembly: AssemblyProduct("Job Scheduler - DEBUG version")]
#else
[assembly: AssemblyProduct("Job Scheduler")]
#endif

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("3d90cb80-b1e9-11df-94e2-0800200c9a66")]

// Disable renaming of namespaces because some internal controls need to be localizable


