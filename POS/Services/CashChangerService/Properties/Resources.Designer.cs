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
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
        ///   Looks up a localized string similar to An error state detected.
        /// </summary>
        internal static string AnErrorStateDetected {
            get {
                return ResourceManager.GetString("AnErrorStateDetected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cash guard is closed, select login.
        /// </summary>
        internal static string CashGuardIsClosedSelectLogin {
            get {
                return ResourceManager.GetString("CashGuardIsClosedSelectLogin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error number ({0}).
        /// </summary>
        internal static string ErrorNumber {
            get {
                return ResourceManager.GetString("ErrorNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initializing cash guard.
        /// </summary>
        internal static string InitializingCashGuard {
            get {
                return ResourceManager.GetString("InitializingCashGuard", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} added from cash guard.
        /// </summary>
        internal static string XAddedFromCashGuard {
            get {
                return ResourceManager.GetString("XAddedFromCashGuard", resourceCulture);
            }
        }
    }
}
