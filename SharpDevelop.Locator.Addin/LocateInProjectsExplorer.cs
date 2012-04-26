/*
 * Created by SharpDevelop.
 * User: Fabien
 * Date: 26/04/2012
 * Time: 22:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
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
    /// Starts the command
    /// </summary>
    public override void Run()
    {
      Solution solution = ProjectService.OpenSolution;
      
      if (solution != null)  // OpenSolution is null when no solution is opened
      {
        string fileName = "";
        FileInfo fiSolution = new FileInfo(solution.FileName);
        List<IProject> projects = new List<IProject>(solution.Projects);
        
        foreach (IProject fiProject in projects) 
        {
          foreach(ProjectItem projectItem in fiProject.Items)
          {
            //projectItem.
            fileName = projectItem.FileName;
          }
        }
        
        
        //AbstractProjectBrowserTreeNode;
        
     ClassBrowserPad classBrowser = ICSharpCode.SharpDevelop.Gui.ClassBrowser.ClassBrowserPad.Instance;
     //classBrowser.
			ProjectBrowserPad pad = ProjectBrowserPad.Instance;
			if (pad == null) return;
			
			//node.Visible();
					//OverlayIconManager.EnqueueParents(node);
					foreach(IViewContent	content in	WorkbenchSingleton.Workbench.ViewContentCollection)
					{
					  //content.I
					  //content.PrimaryFile
					}
					
					ITextEditorProvider provider = WorkbenchSingleton.Workbench.ActiveViewContent as ITextEditorProvider;
			
			if (provider == null)
				return;
			
			//pad.SolutionNode.Nodes;
			
			FileNode node = pad.ProjectBrowserControl.FindFileNode(provider.TextEditor.FileName);
			
			//TODO CHeck if node.IsVisible
			//pad.SelectedNode;
			
			if (node == null) return;
			
			//node.Parent.Expand();
			//node.ActivateItem();
			//node.IsExpanded;
			//node.Expanding();
			  //node.Expand();
			  
			  //TODO Sinon faire le contraire pour que le dossier plus proche soit selectionné
//			  TreeNode pNode = node.Parent;
//			  while(pNode !=null)
//			  {
//			    if(!pNode.IsExpanded)
//			    {
//			      pNode.Expand();
//			    }
//			    pNode = pNode.Parent;
//			  }
			  
			  node.EnsureVisible();
   }
    
    }
  }
}
