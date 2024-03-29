﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ledger8.Services {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Ledger8.Services.Strings", typeof(Strings).Assembly);
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
        ///   Looks up a localized string similar to The {0} cannot be deleted because it has associated {1}..
        /// </summary>
        internal static string CantDelete {
            get {
                return ResourceManager.GetString("CantDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A {0} with the {1} &apos;{2}&apos; already exists..
        /// </summary>
        internal static string DuplicateA {
            get {
                return ResourceManager.GetString("DuplicateA", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An {0} with the {1} &apos;{2}&apos; already exists..
        /// </summary>
        internal static string DuplicateAn {
            get {
                return ResourceManager.GetString("DuplicateAn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The {0} is invalid..
        /// </summary>
        internal static string Invalid {
            get {
                return ResourceManager.GetString("Invalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Montly Due Dates require that a day be set..
        /// </summary>
        internal static string InvalidDueMonth {
            get {
                return ResourceManager.GetString("InvalidDueMonth", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quarterly, Semi-Annual and Annual due dates require that both a month and a day be set..
        /// </summary>
        internal static string InvalidDueQuarterlySemiAnnual {
            get {
                return ResourceManager.GetString("InvalidDueQuarterlySemiAnnual", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The cryptographic hash provided is missing or is the incorrect length..
        /// </summary>
        internal static string InvalidHash {
            get {
                return ResourceManager.GetString("InvalidHash", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The model is invalid. It is either null, or is missing required information..
        /// </summary>
        internal static string InvalidModel {
            get {
                return ResourceManager.GetString("InvalidModel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The cryptographic salt provided is missing or is the incorrect length..
        /// </summary>
        internal static string InvalidSalt {
            get {
                return ResourceManager.GetString("InvalidSalt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The payment is more than the balance..
        /// </summary>
        internal static string PaymentInvalid {
            get {
                return ResourceManager.GetString("PaymentInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The {0} is required..
        /// </summary>
        internal static string Required {
            get {
                return ResourceManager.GetString("Required", resourceCulture);
            }
        }
    }
}
