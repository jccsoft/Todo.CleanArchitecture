﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Todo.Application.IntegrationTests {
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
    internal class SqlScripts {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SqlScripts() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Todo.Application.IntegrationTests.SqlScripts", typeof(SqlScripts).Assembly);
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
        ///   Looks up a localized string similar to CREATE TABLE todoitems (
        ///  Id char(36) NOT NULL PRIMARY KEY,
        ///  Title varchar(50) NOT NULL,
        ///  IsCompleted tinyint(1) DEFAULT 0,
        ///  CreatedOnUtc datetime,
        ///  CompletedOnUtc datetime
        ///); 
        ///INSERT INTO todoitems (Id, Title, CreatedOnUtc) VALUES
        ///(&apos;7db5edbf-ddd5-416d-9724-16600672733d&apos;, &apos;Sample 1&apos;, &apos;2024-10-1&apos;),
        ///(&apos;2631a5b0-4779-4a8a-9caf-25792fe37c17&apos;, &apos;Sample 2&apos;, &apos;2024-10-2&apos;),
        ///(&apos;5dcafa62-3569-41c1-bfe5-ac2e7ec1b2b0&apos;, &apos;Sample 3&apos;, &apos;2024-10-3&apos;),
        ///(&apos;27c5536d-f91e-45f6-8a72-1493322efe9b&apos;, &apos;Sample 4&apos;, &apos;2024-10-4 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string InitDb {
            get {
                return ResourceManager.GetString("InitDb", resourceCulture);
            }
        }
    }
}