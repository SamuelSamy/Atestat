﻿#pragma checksum "..\..\..\..\Controls\LoginControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "C09EEC79B3CD66D6259745C11F6434CDC10C48C8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Atestat;
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
    /// LoginControl
    /// </summary>
    public partial class LoginControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\..\Controls\LoginControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridLogin;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\..\Controls\LoginControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMail;
        
        #line default
        #line hidden
        
        
        #line 125 "..\..\..\..\Controls\LoginControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox passBox;
        
        #line default
        #line hidden
        
        
        #line 168 "..\..\..\..\Controls\LoginControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnLogin;
        
        #line default
        #line hidden
        
        
        #line 189 "..\..\..\..\Controls\LoginControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRegister;
        
        #line default
        #line hidden
        
        
        #line 251 "..\..\..\..\Controls\LoginControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnHome;
        
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
            System.Uri resourceLocater = new System.Uri("/Atestat;component/controls/logincontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\LoginControl.xaml"
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
            this.gridLogin = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.txtMail = ((System.Windows.Controls.TextBox)(target));
            
            #line 103 "..\..\..\..\Controls\LoginControl.xaml"
            this.txtMail.SizeChanged += new System.Windows.SizeChangedEventHandler(this.ControlSizeChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.passBox = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 133 "..\..\..\..\Controls\LoginControl.xaml"
            this.passBox.SizeChanged += new System.Windows.SizeChangedEventHandler(this.ControlSizeChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 151 "..\..\..\..\Controls\LoginControl.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.TextBlock_MouseDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnLogin = ((System.Windows.Controls.Button)(target));
            
            #line 180 "..\..\..\..\Controls\LoginControl.xaml"
            this.btnLogin.Click += new System.Windows.RoutedEventHandler(this.btnLogin_Click);
            
            #line default
            #line hidden
            
            #line 181 "..\..\..\..\Controls\LoginControl.xaml"
            this.btnLogin.SizeChanged += new System.Windows.SizeChangedEventHandler(this.ControlSizeChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnRegister = ((System.Windows.Controls.Button)(target));
            
            #line 201 "..\..\..\..\Controls\LoginControl.xaml"
            this.btnRegister.Click += new System.Windows.RoutedEventHandler(this.btnRegister_Click);
            
            #line default
            #line hidden
            
            #line 202 "..\..\..\..\Controls\LoginControl.xaml"
            this.btnRegister.SizeChanged += new System.Windows.SizeChangedEventHandler(this.ControlSizeChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnHome = ((System.Windows.Controls.Button)(target));
            
            #line 261 "..\..\..\..\Controls\LoginControl.xaml"
            this.btnHome.SizeChanged += new System.Windows.SizeChangedEventHandler(this.Button_SizeChanged);
            
            #line default
            #line hidden
            
            #line 261 "..\..\..\..\Controls\LoginControl.xaml"
            this.btnHome.Click += new System.Windows.RoutedEventHandler(this.btnHome_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

