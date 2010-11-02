using System.Windows.Forms;
using JetBrains.ActionManagement;
using JetBrains.IDE;
using JetBrains.IDE.TreeBrowser;
using JetBrains.UI.Application;

namespace YouTrackPowerToy
{
    namespace YouTrackPowerToy
    {
        [ActionHandler]
        public class YouTrackAction : IActionHandler
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
    }
}