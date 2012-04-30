/*
 * Created by SharpDevelop.
 * User: Fabien
 * Date: 26/04/2012
 * Time: 22:37
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

using ICSharpCode.AvalonEdit;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Editor;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Gui.ClassBrowser;
using ICSharpCode.SharpDevelop.Project;

namespace SharpDevelop.Addin.Locator
{
  /// <summary>
  /// Description of LocateInProjectsExplorer
  /// </summary>
  public class LocateInProjectsExplorer : AbstractMenuCommand
  {
    /// <summary>
    /// Locate current file in Projects explorer
    /// </summary>
    public override void Run()
    {
      Solution solution = ProjectService.OpenSolution;
      if (solution != null)  // OpenSolution is null when no solution is opened
      {
        
        ClassBrowserPad classBrowser = ICSharpCode.SharpDevelop.Gui.ClassBrowser.ClassBrowserPad.Instance;
                
        ProjectBrowserPad pad = ProjectBrowserPad.Instance;
        if (pad == null) return;
        
        ITextEditorProvider provider = WorkbenchSingleton.Workbench.ActiveViewContent as ITextEditorProvider;
        
        if (provider == null)
          return;
        
        string currentFileName = provider.TextEditor.FileName;
        
        if(currentFileName == null)
        {
          return;
        }
        
        pad.ProjectBrowserControl.SelectFileAndExpand(currentFileName);
        
      }
      
    }
  }
  
}
