using JetBrains.IDE.TreeBrowser;

namespace YouTrackPowerToy.YouTrackPowerToy
{
    public class TreeModelPanelYouTrack : TreeModelBrowserPanel
    {
        public TreeModelPanelYouTrack(TreeModelBrowserDescriptor descriptor)
            : base(descriptor)
        {
        }

        public TreeModelPanelYouTrack(TreeModelBrowserDescriptor descriptor, ITreeModelBrowserPanelPersistance persistance)
            : base(descriptor, persistance)
        {
        }
    }
}