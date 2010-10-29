using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.Application;
using JetBrains.CommonControls;
using JetBrains.ComponentModel;
using JetBrains.IDE;
using JetBrains.IDE.TreeBrowser;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Features.Common.TreePsiBrowser;
using JetBrains.TreeModels;
using JetBrains.UI.Application;
using JetBrains.UI.Controls;
using JetBrains.UI.RichText;
using JetBrains.UI.TreeView;

namespace YouTrackPowerToy
{
    [ActionHandler]
    public class YouTrackAction: IActionHandler
    {
        public const string YouTrackBrowserWindowID = "YouTrackBrowserWindowID";

        public bool Update(IDataContext context, ActionPresentation presentation, DelegateUpdate nextUpdate)
        {
            // It's always allowed. We don't need a solution present
            return context.CheckAllNotNull(DataConstants.SOLUTION);
        }

        public void Execute(IDataContext context, DelegateExecute nextExecute)
        {
            var solution = context.GetData(DataConstants.SOLUTION);

        
            if (solution == null)
            {
                // Do something...shit! 
                
            }
            else
            {
                using (var searchBox = new SearchBox())
                {
                    if (searchBox.ShowDialog(UIApplicationShell.Instance.MainWindow) == DialogResult.OK)
                    {
                        var browserDescriptor = new TreeModelBrowserDescriptorYouTrack(solution);
                        var browserPanel = new TreeModelPanelYouTrack(browserDescriptor);
                        var browser = TreeModelBrowser.GetInstance(solution);

                        browser.Show(YouTrackBrowserWindowID, browserDescriptor, browserPanel);
                    }
                }
                
            }
        }
    }

    public class TreeModelPanelYouTrack : TreeModelBrowserPanel
    {
        public TreeModelPanelYouTrack(TreeModelBrowserDescriptor descriptor) : base(descriptor)
        {
        }

        public TreeModelPanelYouTrack(TreeModelBrowserDescriptor descriptor, ITreeModelBrowserPanelPersistance persistance) : base(descriptor, persistance)
        {
        }
    }

    public class TreeModelBrowserDescriptorYouTrack : TreeModelBrowserDescriptor
    {
        public TreeModelBrowserDescriptorYouTrack(ISolution solution) : base(solution)
        {
        }

        public override string Title
        {
            get { return "YouTrack Search Results"; }
        }

        public override TreeModel Model
        {
            get { return new YouTrackTreeModel(); }
        }

        public override StructuredPresenter<TreeModelNode, IPresentableItem> Presenter
        {
            get { return new TreeModelBrowserPresenter(); }
        }
    }

    public class YouTrackTreeModel : TreeModel
    {
        protected override void PerformUpdate()
        {
            
        }

        protected override TreeModelNode CreateNode(TreeModelNode parent, object value)
        {
            return new YouTrackTreeModelNode(null, parent, value);
        }
    }

    public class YouTrackTreeModelNode : TreeModelNode
    {
        public YouTrackTreeModelNode(TreeModel model, TreeModelNode parent, object dataValue) : base(model, parent, dataValue)
        {
        }

        public override IList<TreeModelNode> ChildrenUnsorted
        {
            get { return new List<TreeModelNode>(); }
        }
    }

    [SolutionComponentImplementation(ProgramConfigurations.VS_ADDIN)]
    public class WTF : ISolutionComponent
    {
        ISolution solution;

        public WTF(ISolution solution)
        {
            this.solution = solution;
        }

        public void AfterSolutionOpened()
        {
            var emptyLabel = new RichTextLabel { BackColor = SystemColors.Control };
            emptyLabel.RichTextBlock.Add(new RichText("YouTrack Search Results", new TextStyle(FontStyle.Bold)));
            var browser1 = TreeModelBrowser.GetInstance(solution);
            browser1.RegisterBrowserWindow(YouTrackAction.YouTrackBrowserWindowID, emptyLabel);
        }

        public void BeforeSolutionClosed()
        {

        }

        public void Dispose()
        {

        }

        public void Init()
        {

        }
    }
}