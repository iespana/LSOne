﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LSOne.Services.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LSOne.Services.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to None.
        /// </summary>
        internal static string FiscalLocalVersion {
            get {
                return ResourceManager.GetString("FiscalLocalVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Maximum number of invoices already printed (#1). No more invoices can be printed.
        /// </summary>
        internal static string MaximumNumberOfInvoicesPrinted {
            get {
                return ResourceManager.GetString("MaximumNumberOfInvoicesPrinted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Maximum number of receipt copies already printed (#1). No more receipt copies can be printed.
        /// </summary>
        internal static string MaximumNumberOfReceiptsPrinted {
            get {
                return ResourceManager.GetString("MaximumNumberOfReceiptsPrinted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No fiscal log lines found to export..
        /// </summary>
        internal static string NoExportLines {
            get {
                return ResourceManager.GetString("NoExportLines", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #1 fiscal log lines printed.
        /// </summary>
        internal static string PrintXFiscalLogLines {
            get {
                return ResourceManager.GetString("PrintXFiscalLogLines", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #1 fiscal log lines saved to file #2.
        /// </summary>
        internal static string SavedXFiscalLogLines {
            get {
                return ResourceManager.GetString("SavedXFiscalLogLines", resourceCulture);
            }
        }
    }
}