﻿#pragma checksum "..\..\..\..\Controls\TutorialControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2D24BBBF35A991699BFC3ED90E569BCF8DAD5092"
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


namespace Atestat.Controls {
    
    
    /// <summary>
    /// TutorialControl
    /// </summary>
    public partial class TutorialControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\..\Controls\TutorialControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.ImageBrush CurrentImage;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\Controls\TutorialControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock WindowName;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\..\Controls\TutorialControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox CurrentDescription;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\..\Controls\TutorialControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPrevPage;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\..\Controls\TutorialControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnNextPage;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\..\..\Controls\TutorialControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBack;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.4.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Atestat;component/controls/tutorialcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\TutorialControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.4.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\..\Controls\TutorialControl.xaml"
            ((Atestat.Controls.TutorialControl)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.CurrentImage = ((System.Windows.Media.ImageBrush)(target));
            return;
            case 3:
            this.WindowName = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.CurrentDescription = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.btnPrevPage = ((System.Windows.Controls.Button)(target));
            
            #line 75 "..\..\..\..\Controls\TutorialControl.xaml"
            this.btnPrevPage.SizeChanged += new System.Windows.SizeChangedEventHandler(this.ControlSizeChanged);
            
            #line default
            #line hidden
            
            #line 75 "..\..\..\..\Controls\TutorialControl.xaml"
            this.btnPrevPage.Click += new System.Windows.RoutedEventHandler(this.btnPrevPage_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnNextPage = ((System.Windows.Controls.Button)(target));
            
            #line 98 "..\..\..\..\Controls\TutorialControl.xaml"
            this.btnNextPage.SizeChanged += new System.Windows.SizeChangedEventHandler(this.ControlSizeChanged);
            
            #line default
            #line hidden
            
            #line 98 "..\..\..\..\Controls\TutorialControl.xaml"
            this.btnNextPage.Click += new System.Windows.RoutedEventHandler(this.btnNextPage_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnBack = ((System.Windows.Controls.Button)(target));
            
            #line 129 "..\..\..\..\Controls\TutorialControl.xaml"
            this.btnBack.SizeChanged += new System.Windows.SizeChangedEventHandler(this.ControlSizeChanged);
            
            #line default
            #line hidden
            
            #line 130 "..\..\..\..\Controls\TutorialControl.xaml"
            this.btnBack.Click += new System.Windows.RoutedEventHandler(this.btnBack_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

