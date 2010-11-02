using JetBrains.CommonControls;
using JetBrains.IDE.TreeBrowser;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Features.Common.TreePsiBrowser;
using JetBrains.TreeModels;
using JetBrains.UI.TreeView;

namespace YouTrackPowerToy.YouTrackPowerToy
{
    public class TreeModelBrowserDescriptorYouTrack : TreeModelBrowserDescriptor
    {
        public TreeModelBrowserDescriptorYouTrack(ISolution solution)
            : base(solution)
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
}