using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.ActionManagement;
using JetBrains.ComponentModel;
using JetBrains.UI.Application.PluginSupport;
using JetBrains.WindowManagement;
using YouTrackPowerToy;

[assembly: AssemblyTitle("ReSharper PowerToys: YouTrack PowerToy")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("Visual Studio 2010")]
[assembly: AssemblyCompany("JetBrains s.r.o.")]
[assembly: AssemblyProduct("ReSharper PowerToys")]
[assembly: AssemblyCopyright("Copyright \u00A9 2006-2010 JetBrains s.r.o. All rights reserved.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: ComVisible(false)]
[assembly: Guid("823c1185-dc5d-48c7-b031-e41e9645947b")]
[assembly: PluginTitle("ReSharper PowerToys: YouTrack")]
[assembly: PluginVendor("JetBrains s.r.o.")]
[assembly: PluginDescription("Search using YouTrack")]
[assembly: ActionsXml("YouTrackPowerToy.Action.xml")]
[assembly: ToolWindowDescriptor(
  ProgramConfigurations.VS_ADDIN,
  Id = YouTrackAction.YouTrackBrowserWindowID,
  Text = "Hierarchy",
  Guid = "66DDC50A-E1B3-47DC-83B8-8F8813B81FF4",
  ToolWindowVisibilityPersistenceScope = ToolWindowVisibilityPersistenceScope.SOLUTION,
  IconResourceID = 305)]