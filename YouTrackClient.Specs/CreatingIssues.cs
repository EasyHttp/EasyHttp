using Machine.Specifications;

namespace YouTrackClient.Specs
{
    [Subject("Creating Issues")]
    public class when_creating_a_new_issue_with_valid_information
    {
        Establish context = () =>
        {
            youTrack = new YouTrack("youtrack.jetbrains.net");
        };

        Because of = () =>
        {
            var issue = new Issue();

            //response = youTrack.CreateIssue(issue);
        };

        It should_action = () =>
        {

        };

        static YouTrack youTrack;
    }
}