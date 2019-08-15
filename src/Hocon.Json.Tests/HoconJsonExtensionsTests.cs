using System.IO;

using Newtonsoft.Json.Linq;

using Xunit;
using Xunit.Abstractions;

namespace Hocon.Json.Tests
{
    public class HoconJsonExtensionsTests
    {
        private readonly ITestOutputHelper _output;

        public HoconJsonExtensionsTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData("test04")]
        [InlineData("test05")]
        [InlineData("test06")]
        [InlineData("test09")]
        [InlineData("test11")]
        [InlineData("test12")]
        public void ToJsonTest(string name)
        {
            var hoconPath = $@"data\{name}.conf";
            var hoconRoot = Parser.Parse(File.ReadAllText(hoconPath));
            var jToken = hoconRoot.ToJToken();
            Assert.NotNull(jToken);
            var json = jToken!.ToString(Newtonsoft.Json.Formatting.Indented);
            _output.WriteLine(json);

            var jsonPath = $@"data\{name}.json";
            var expected = JToken.Parse(File.ReadAllText(jsonPath));
            Assert.Equal(expected, jToken);
        }
    }
}
