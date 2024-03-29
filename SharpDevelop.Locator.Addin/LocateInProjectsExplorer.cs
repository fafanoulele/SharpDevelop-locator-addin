﻿/*
 * Created by SharpDevelop.
 * User: Fabien
 * Date: 26/04/2012
 * Time: 22:37
 * 
 */
using System;
using System.Collections.Generic;
using System.Globalization;
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
    private Dictionary<string, TreeNode> cache = new Dictionary<string, TreeNode>();
    
    public LocateInProjectsExplorer()
    {
      Solution solution = ProjectService.OpenSolution;
      
      if (solution != null)  // OpenSolution is null when no solution is opened
      {
        LoadNodesInCache();
      }
      
      ProjectService.SolutionLoaded += delegate(object sender, SolutionEventArgs e) 
      {
        LoadNodesInCache();
      };
      
      ProjectService.ProjectItemAdded += delegate(object sender, ProjectItemEventArgs e) 
      {
        ProjectBrowserPad pad = ProjectBrowserPad.Instance;
        
        if (pad == null) return;
        
        FileNode newNode = pad.ProjectBrowserControl.FindFileNode(e.ProjectItem.FileName);
        
        if (newNode == null) return;
        
        cache.Add(FormatFilepath(e.ProjectItem.FileName), newNode);
      };
      
      ProjectService.ProjectItemRemoved += delegate(object sender, ProjectItemEventArgs e) 
      {
        cache.Remove(FormatFilepath(e.ProjectItem.FileName));
      };
      
    }
    
    private void LoadNodesInCache()
    {
      cache.Clear();
      
      ProjectBrowserPad pad = ProjectBrowserPad.Instance;
      
      if (pad == null) return;

      TreeNode tn = pad.ProjectBrowserControl.RootNode;
        
      PutInCache(tn.Nodes);
    }
    
    /// <summary>
    /// Locate current file in Projects explorer
    /// </summary>
    public override void Run()
    {
      Solution solution = ProjectService.OpenSolution;
      
      if (solution != null)  // OpenSolution is null when no solution is opened
      {
        
        if(cache.Count==0)
        {
          LoadNodesInCache();
        }
        
        ProjectBrowserPad pad = ProjectBrowserPad.Instance;
        if (pad == null) return;
        
        if(WorkbenchSingleton.Workbench == null) return;
        
        if(WorkbenchSingleton.Workbench.ActiveWorkbenchWindow == null) return;
        
        ITextEditorProvider provider = WorkbenchSingleton.Workbench.ActiveWorkbenchWindow.ActiveViewContent as ITextEditorProvider;
                 
        if (provider == null)
          return;
        
        string currentFileName = FormatFilepath(provider.TextEditor.FileName);
        
        if(currentFileName == null)
        {
          return;
        }
                
        TreeNode node;
        
        if(cache.TryGetValue(currentFileName, out node))
        {
          node.EnsureVisible();
          node.TreeView.SelectedNode = node;
        }
        
      }
    }
    
    private void PutInCache(TreeNodeCollection nodes)
		{
			foreach (TreeNode treeNode in nodes)
			{
				FileNode fileNode = treeNode as FileNode;

				if (fileNode != null)
				{
				  cache.Add(FormatFilepath(fileNode.FileName), fileNode);
				}
				
				SolutionItemNode itemNode = treeNode as SolutionItemNode;
				if (itemNode != null)
				{
				  cache.Add(FormatFilepath(itemNode.FileName), itemNode);
				}
				
				if (treeNode != null && treeNode.Nodes != null && treeNode.Nodes.Count > 0)
				{
				  if(!treeNode.IsExpanded)
					{
					  //Bug ? => if treenode has not expanded then sometimes, children nodes have not been found !
					  treeNode.Expand();
					  treeNode.Collapse();
					}
				  //Recursive
					this.PutInCache(treeNode.Nodes);
										
				}
			}

		}
    
    private string FormatFilepath(string filename)
    {
      //volume letter is case insenstive...=> Set volume drive letter in upper case
      
      int volumeIndex = filename.IndexOf(Path.VolumeSeparatorChar);
      if(volumeIndex >= 0)
      {
        string result = filename.Substring(0, volumeIndex).ToUpper(CultureInfo.CurrentCulture);
        
        result += filename.Substring(volumeIndex, filename.Length-1);
        
        return result;
      }
      
      return filename;
    }
  }
  
}
