using JetBrains.TreeModels;
using YouTrackPowerToy.YouTrackPowerToy;

namespace YouTrackPowerToy
{
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
}