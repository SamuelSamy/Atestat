﻿#pragma checksum "..\..\..\..\Controls\RegisterControl.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "EDF54FFDF0F9A04F0629C0996A78DB78A431172D"
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
    /// RegisterControl
    /// </summary>
    public partial class RegisterControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\..\Controls\RegisterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridLogin;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\..\..\Controls\RegisterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtName;
        
        #line default
        #line hidden
        
        
        #line 151 "..\..\..\..\Controls\RegisterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtMail;
        
        #line default
        #line hidden
        
        
        #line 180 "..\..\..\..\Controls\RegisterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox passBox;
        
        #line default
        #line hidden
        
        
        #line 213 "..\..\..\..\Controls\RegisterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox passBoxConf;
        
        #line default
        #line hidden
        
        
        #line 245 "..\..\..\..\Controls\RegisterControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRegister;
        
        #line default
        #line hidden
        
        
        #line 320 "..\..\..\..\Controls\RegisterControl.xaml"
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
            System.Uri resourceLocater = new System.Uri("/Atestat;component/controls/registercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\RegisterControl.xaml"
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
            this.txtName = ((System.Windows.Controls.TextBox)(target));
            
            #line 127 "..\..\..\..\Controls\RegisterControl.xaml"
            this.txtName.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtName_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtMail = ((System.Windows.Controls.TextBox)(target));
            
            #line 156 "..\..\..\..\Controls\RegisterControl.xaml"
            this.txtMail.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtMail_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.passBox = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 187 "..\..\..\..\Controls\RegisterControl.xaml"
            this.passBox.KeyDown += new System.Windows.Input.KeyEventHandler(this.passBox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.passBoxConf = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 220 "..\..\..\..\Controls\RegisterControl.xaml"
            this.passBoxConf.KeyDown += new System.Windows.Input.KeyEventHandler(this.passBox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 233 "..\..\..\..\Controls\RegisterControl.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.AlreadyHasAccount_MouseDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnRegister = ((System.Windows.Controls.Button)(target));
            
            #line 258 "..\..\..\..\Controls\RegisterControl.xaml"
            this.btnRegister.Click += new System.Windows.RoutedEventHandler(this.btnRegister_Click);
            
            #line default
            #line hidden
            
            #line 258 "..\..\..\..\Controls\RegisterControl.xaml"
            this.btnRegister.SizeChanged += new System.Windows.SizeChangedEventHandler(this.btnRegister_SizeChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnHome = ((System.Windows.Controls.Button)(target));
            
            #line 330 "..\..\..\..\Controls\RegisterControl.xaml"
            this.btnHome.SizeChanged += new System.Windows.SizeChangedEventHandler(this.Button_SizeChanged);
            
            #line default
            #line hidden
            
            #line 330 "..\..\..\..\Controls\RegisterControl.xaml"
            this.btnHome.Click += new System.Windows.RoutedEventHandler(this.btnHome_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
