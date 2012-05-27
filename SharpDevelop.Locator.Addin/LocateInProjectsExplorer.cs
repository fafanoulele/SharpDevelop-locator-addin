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
      //ProjectService.ProjectItemAdded
      if (solution != null)  // OpenSolution is null when no solution is opened
      {
                 
        ProjectBrowserPad pad = ProjectBrowserPad.Instance;
        if (pad == null) return;
        
        if(WorkbenchSingleton.Workbench == null) return;
        
        if(WorkbenchSingleton.Workbench.ActiveWorkbenchWindow == null) return;
        
         ITextEditorProvider provider = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ActiveViewContent as ITextEditorProvider;
                 
        if (provider == null)
          return;
        
        //provider.TextEditorControl.Refresh();
        
        string currentFileName = provider.TextEditor.FileName;//En fait c'est parfois pas le Filename en cours...
        //provider.TextEditor.SelectionChanged:
        //provider.TextEditor.KeyPress (for Ctr-l)...
        if(currentFileName == null)
        {
          return;
        }
        
        //AvalonEditViewContent...
        TreeNode tn = pad.ProjectBrowserControl.RootNode;//Parfois c'est la pas solution..;
        
        TreeNode test = FindFileNode(tn.Nodes, currentFileName);//Bug return null...
        
        if (test != null)
				{
					for (TreeNode parent = test.Parent; parent != null; parent = parent.Parent)
					{
						parent.Expand();
					}
					test.TreeView.SelectedNode = test;
				}
        //Or use test.EnsureVisible():
        
          //pad.ProjectBrowserControl.SelectFileAndExpand(currentFileName);
        
        //WorkbenchSingleton.Workbench.ShowPad
      }
      
    }
    
    //En fait Ctr-G il ne prend pas en compte les fichier build.bat (en dehors des projets ?)
    private TreeNode FindFileNode(TreeNodeCollection nodes, string fileName)
		{
			foreach (TreeNode treeNode in nodes)
			{
				FileNode fileNode = treeNode as FileNode;
				if (fileNode != null && FileUtility.IsEqualFileName(fileNode.FileName, fileName))
				{
					return fileNode;
				}
				
				SolutionItemNode itemNode = treeNode as SolutionItemNode;
				if (itemNode != null && FileUtility.IsEqualFileName(itemNode.FileName, fileName))
				{
					return itemNode;
				}
				
				if (treeNode != null && treeNode.Nodes != null && treeNode.Nodes.Count > 0)
				{
				  if(!treeNode.IsExpanded)
					{
					  //(Faire cela au démarrage c.à.d dans le constructeur de l'action => Faire un expand et un Collapse de tout...)
					  //Si on ne fait pas cela, dans le cas où le tree est fermé au démarrage de SharpDevelop alors pas de noeud trouvé...
					  treeNode.Expand();
					  treeNode.Collapse();
					  
					}
				  
					TreeNode result = this.FindFileNode(treeNode.Nodes, fileName);
										
					if (result != null)
					{
						return result;
					}
				}
			}
			return null;
		}
  }
  
}
