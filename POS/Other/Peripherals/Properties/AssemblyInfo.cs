using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Peripherals")]
[assembly: AssemblyDescription("")]
#if DEBUG
[assembly: AssemblyProduct("Peripherals - DEBUG version")]
#else
[assembly: AssemblyProduct("Peripherals")]
#endif

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d434badf-83c6-4d4f-afc8-706a9da18820")]

[assembly: Obfuscation(Feature = "string encryption", Exclude = true)]
