﻿

#pragma checksum "C:\Users\just\Desktop\Window8exercise\hubTemplateExercise\hubTemplateExercise\HubPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "112FC51B0F4703479695EB47FA39D578"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace hubTemplateExercise
{
    partial class HubPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 51 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Hub)(target)).SectionHeaderClick += this.Hub_SectionHeaderClick;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 126 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ItemView_ItemClick;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 109 "..\..\HubPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ItemView_GroupClick;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

