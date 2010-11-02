using System.Drawing;
using JetBrains.ComponentModel;
using JetBrains.IDE.TreeBrowser;
using JetBrains.ProjectModel;
using JetBrains.UI.Controls;
using JetBrains.UI.RichText;
using YouTrackPowerToy.YouTrackPowerToy;

namespace YouTrackPowerToy
{
    [SolutionComponentImplementation(ProgramConfigurations.VS_ADDIN)]
    public class YouTrackSolutionInitialization : ISolutionComponent
    {
        ISolution solution;

        public YouTrackSolutionInitialization(ISolution solution)
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