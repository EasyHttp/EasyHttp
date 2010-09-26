using Machine.Specifications;

namespace EasyHttp.Specs.EasyHTTP
{
    [Subject("EasyHttp")]
    public class when_creating_a_new_instance
    {
        Because of = () =>
        {
            _easyHttp = new EasyHttp(); ;
        };

        It should_return_new_instance = () => _easyHttp.ShouldNotBeNull();

        static EasyHttp _easyHttp;
    }
}