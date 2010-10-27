using Machine.Specifications;

namespace YouTrackClient.Specs
{
    [Subject("Authenticating")]
    public class when_provided_valid_username_and_password  
    {
        Establish context = () =>
        {
            youtrack = new YouTrack("youtrack.jetbrains.net");
        };

        Because of = () =>
        {
            youtrack.Login("youtrackapi", "youtrackapi");

        };

        It should_have_property_IsAuthenticated_set_to_true = () =>
        {
            youtrack.IsAuthenticated.ShouldBeTrue();
        };

        static YouTrack youtrack;
    }

    [Subject("Authenticating")]
    public class when_provided_invalid_username_and_password
    {
        Establish context = () =>
        {
            youtrack = new YouTrack("youtrack.jetbrains.net");
        };

        Because of = () =>
        {
            youtrack.Login("youtrackapi", "youtrackapi123");

        };

        It should_have_property_IsAuthenticated_set_to_false = () =>
        {
            youtrack.IsAuthenticated.ShouldBeFalse();
        };

        static YouTrack youtrack;
    }

}