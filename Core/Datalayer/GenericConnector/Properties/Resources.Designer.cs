﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LSOne.DataLayer.GenericConnector.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LSOne.DataLayer.GenericConnector.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Dr.
        /// </summary>
        internal static string Dr {
            get {
                return ResourceManager.GetString("Dr", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Loading services.
        /// </summary>
        internal static string LoadingServices {
            get {
                return ResourceManager.GetString("LoadingServices", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Miss.
        /// </summary>
        internal static string Miss {
            get {
                return ResourceManager.GetString("Miss", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mr.
        /// </summary>
        internal static string Mr {
            get {
                return ResourceManager.GetString("Mr", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mrs.
        /// </summary>
        internal static string Mrs {
            get {
                return ResourceManager.GetString("Mrs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ms.
        /// </summary>
        internal static string Ms {
            get {
                return ResourceManager.GetString("Ms", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rev.
        /// </summary>
        internal static string Rev {
            get {
                return ResourceManager.GetString("Rev", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The current time on the POS is {0} but the current time on the central server is {1}. To be able to retrieve data from the central server please reset the current time settings to match the central server..
        /// </summary>
        internal static string ServerTimeException {
            get {
                return ResourceManager.GetString("ServerTimeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Trying to load #1 from directory #2. File not found.
        /// </summary>
        internal static string ServiceNotFound {
            get {
                return ResourceManager.GetString("ServiceNotFound", resourceCulture);
            }
        }
    }
}