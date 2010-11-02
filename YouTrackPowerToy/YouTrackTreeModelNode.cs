using System.Collections.Generic;
using JetBrains.TreeModels;

namespace YouTrackPowerToy
{
    public class YouTrackTreeModelNode : TreeModelNode
    {
        public YouTrackTreeModelNode(TreeModel model, TreeModelNode parent, object dataValue)
            : base(model, parent, dataValue)
        {
        }

        public override IList<TreeModelNode> ChildrenUnsorted
        {
            get { return new List<TreeModelNode>(); }
        }
    }
}