﻿#pragma checksum "..\..\..\..\Controls\AdsControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "80741238740C58230EBE2CE479C7029684B34FDB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Atestat.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Atestat {
    
    
    /// <summary>
    /// AdsControl
    /// </summary>
    public partial class AdsControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\..\Controls\AdsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid sideBarMain;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\Controls\AdsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddAd;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\..\Controls\AdsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAds;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\..\Controls\AdsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMainPage;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\..\Controls\AdsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid adsGrid;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Atestat;component/controls/adscontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\AdsControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.sideBarMain = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.btnAddAd = ((System.Windows.Controls.Button)(target));
            
            #line 48 "..\..\..\..\Controls\AdsControl.xaml"
            this.btnAddAd.SizeChanged += new System.Windows.SizeChangedEventHandler(this.Button_SizeChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnAds = ((System.Windows.Controls.Button)(target));
            
            #line 65 "..\..\..\..\Controls\AdsControl.xaml"
            this.btnAds.SizeChanged += new System.Windows.SizeChangedEventHandler(this.Button_SizeChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnMainPage = ((System.Windows.Controls.Button)(target));
            
            #line 85 "..\..\..\..\Controls\AdsControl.xaml"
            this.btnMainPage.SizeChanged += new System.Windows.SizeChangedEventHandler(this.Button_SizeChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.adsGrid = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

