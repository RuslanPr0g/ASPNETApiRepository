using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using YTScrapper.Infrastructure.Config;

namespace YTScrapper.Infrastructure.Services
{
    public class DriverInitializer
    {
        private readonly SeleniumConfig _options;

        public DriverInitializer(IOptions<SeleniumConfig> options)
        {
            _options = options.Value;
        }

        public Task<RemoteWebDriver> Initialize()
        {
            var seleniumLocation = _options.SeleniumLocation;
            var uri = new Uri(seleniumLocation);
            var firefoxOptions = new FirefoxOptions();
            var driver = new RemoteWebDriver(uri, firefoxOptions.ToCapabilities());
            return Task.FromResult(driver);
        }
    }
}
