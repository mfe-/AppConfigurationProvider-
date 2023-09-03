namespace AppConfigurationProvider.Unittest
{
    public class AppConfigurationProviderUnitTest
    {
        [Fact]
        public void JsonToDictionary()
        {
            var jsonstring = """
            {
              "Outer" : { 
                "Middle" : { 
                  "Inner": "value1",
                  "HasValue": "true"
                }
              }
            }
            """;

            var dict = AppConfigurationProvider.JsonToDictionary(jsonstring);
            //expected

            var expected = new Dictionary<string, string> {
              {"Outer:Middle:Inner", "value1"},
              {"Outer:Middle:HasValue", "true"}
            };

            Assert.Equal(expected.Keys.ToArray()[0], dict.Keys.ToArray()[0]);
            Assert.Equal(expected.Keys.ToArray()[1], dict.Keys.ToArray()[1]);

            Assert.Equal(expected.Values.ToArray()[0], dict.Values.ToArray()[0]);
            Assert.Equal(expected.Values.ToArray()[1], dict.Values.ToArray()[1]);
        }
    }
}