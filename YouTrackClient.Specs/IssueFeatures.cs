using System.Collections.Generic;
using Machine.Specifications;

namespace YouTrackClient.Specs
{
    [Subject("Issues")]
    public class when_requesting_list_of_issues_for_project
    {
        Establish context = () =>
        {
            youtrack = new YouTrack("youtrack.jetbrains.net");
        };

        Because of = () =>
        {

            issues = youtrack.GetIssues("DCVR");
        };

        It should_return_list_of_issues_for_that_project = () =>
        {
            issues.ShouldNotBeNull();
        };

        static YouTrack youtrack;
        static IList<Issue> issues;
    }


    [Subject("Issues")]
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