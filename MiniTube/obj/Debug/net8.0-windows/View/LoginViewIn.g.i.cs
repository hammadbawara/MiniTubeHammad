﻿#pragma checksum "..\..\..\..\View\LoginViewIn.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7B39C95421217659CFFC52712A30990A9C8952F5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MiniTube.View;
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


namespace MiniTube.View {
    
    
    /// <summary>
    /// LoginViewIn
    /// </summary>
    public partial class LoginViewIn : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 29 "..\..\..\..\View\LoginViewIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_minimize;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\View\LoginViewIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_close;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\View\LoginViewIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txt_msg;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\..\View\LoginViewIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_email;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\..\View\LoginViewIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox txt_password;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\..\View\LoginViewIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txt_error;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\..\View\LoginViewIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_login;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.10.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MiniTube;component/view/loginviewin.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\LoginViewIn.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.10.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 6 "..\..\..\..\View\LoginViewIn.xaml"
            ((MiniTube.View.LoginViewIn)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btn_minimize = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\..\View\LoginViewIn.xaml"
            this.btn_minimize.Click += new System.Windows.RoutedEventHandler(this.btn_minimize_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btn_close = ((System.Windows.Controls.Button)(target));
            
            #line 44 "..\..\..\..\View\LoginViewIn.xaml"
            this.btn_close.Click += new System.Windows.RoutedEventHandler(this.btn_close_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txt_msg = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.txt_email = ((System.Windows.Controls.TextBox)(target));
            
            #line 77 "..\..\..\..\View\LoginViewIn.xaml"
            this.txt_email.KeyDown += new System.Windows.Input.KeyEventHandler(this.txt_email_KeyDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txt_password = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 91 "..\..\..\..\View\LoginViewIn.xaml"
            this.txt_password.KeyDown += new System.Windows.Input.KeyEventHandler(this.txt_password_KeyDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.txt_error = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.btn_login = ((System.Windows.Controls.Button)(target));
            
            #line 101 "..\..\..\..\View\LoginViewIn.xaml"
            this.btn_login.Click += new System.Windows.RoutedEventHandler(this.btn_login_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 121 "..\..\..\..\View\LoginViewIn.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.btn_reset_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 128 "..\..\..\..\View\LoginViewIn.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.btn_register_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

