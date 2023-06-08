using FluentAssertions;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Moq;
using Stock.Application.Infrastructure.Services;
using Stock.Common;

namespace Stock.UnitTests.Services
{
    public class HtmlParserServiceTests
    {
        private readonly ILogger<HtmlParserService> _logger = Mock.Of<ILogger<HtmlParserService>>();

        [Fact]
        public async Task InitializeHtmlContent_ShouldReturnFalse_WhenNullValueProvided()
        {
            //Arrange
            var _htmlParserService = new HtmlParserService(_logger, Mock.Of<HtmlDocument>());

            //Act
            var result = await _htmlParserService.InitializeHtmlContent(null);

            //Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task InitializeHtmlContent_ShouldReturnFalse_WhenEmptyValueProvided(string value)
        {
            //Arrange
            var _htmlParserService = new HtmlParserService(_logger, Mock.Of<HtmlDocument>());

            //Act
            var result = await _htmlParserService.InitializeHtmlContent(value);

            //Assert
            result.Should().BeFalse();
        }


        [Theory]
        [InlineData("<html></sshtml>")]
        [InlineData("<< html></html>>")]
        public async Task InitializeHtmlContent_ShouldReturnFalse_WhenInvalidHtmlProvided(string invalidHtml)
        {
            //Arrange
            var _htmlParserService = new HtmlParserService(_logger, Mock.Of<HtmlDocument>());

            //Act
            var result = await _htmlParserService.InitializeHtmlContent(invalidHtml);

            //Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("<html></html>")]
        [InlineData("<html><p>HELLO WORLD</p></html>")]
        [InlineData("<html><body><p>Test</p></body></html>")]
        public async Task InitializeHtmlContent_ShouldReturnTrue_WhenProperHtmlProvied(string invalidHtml)
        {
            //Arrange
            var _htmlParserService = new HtmlParserService(_logger, Mock.Of<HtmlDocument>());

            //Act
            var result = await _htmlParserService.InitializeHtmlContent(invalidHtml);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task FindElements_ShouldReturnElementsForGivenXpath_WhenElementsExistsInHtml()
        {
            //Arrange
            var htmlContentHelper = new HtmlGeneratorHelper();
            htmlContentHelper.InsertAdditionalContent("1", "p");
            htmlContentHelper.InsertAdditionalContent("2", "div");
            htmlContentHelper.InsertAdditionalContent("3", "p");
            var _htmlParserService = new HtmlParserService(_logger, htmlContentHelper.Build());

            //Act
            var result = await _htmlParserService.FindElements("//p");

            //Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task FindElements_ShouldReturnElementsForGivenXpath_WhenElementsWithWithClassExistsInHtml()
        {
            //Arrange
            var htmlContentHelper = new HtmlGeneratorHelper();
            htmlContentHelper.InsertAdditionalContent("1", "test", "p");
            htmlContentHelper.InsertAdditionalContent("2", "test", "div");
            htmlContentHelper.InsertAdditionalContent("3", "p");
            var _htmlParserService = new HtmlParserService(_logger, htmlContentHelper.Build());

            //Act
            var result = await _htmlParserService.FindElements("//p[@class='test']");

            //Assert
            result.Should().HaveCount(1)
                .And.Contain(i => i.HtmlElementType == "p");
        }

        [Fact]
        public async Task FindElements_ShouldReturnEmptyList_WhenXpathIsNull()
        {
            //Arrange
            var htmlContentHelper = new HtmlGeneratorHelper();
            htmlContentHelper.InsertAdditionalContent("1", "test", "p");
            var _htmlParserService = new HtmlParserService(_logger, htmlContentHelper.Build());

            //Act
            var result = await _htmlParserService.FindElements(null);

            //Assert
            result.Should().HaveCount(0);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public async Task FindElements_ShouldReturnEmptyList_WhenXpathIsEmpty(string xpath)
        {
            //Arrange
            var htmlContentHelper = new HtmlGeneratorHelper();
            htmlContentHelper.InsertAdditionalContent("1", "test", "p");
            var _htmlParserService = new HtmlParserService(_logger, htmlContentHelper.Build());

            //Act
            var result = await _htmlParserService.FindElements(xpath);

            //Assert
            result.Should().HaveCount(0);
        }

        [Theory]
        [InlineData("//p")]
        [InlineData("//div")]
        public async Task FindElements_ShouldReturnEmptyList_WhenHtmlDocumentIsUndefiend(string xpath)
        {
            //Arrange
            var _htmlParserService = new HtmlParserService(_logger, null);

            //Act
            var result = await _htmlParserService.FindElements(xpath);

            //Assert
            result.Should().HaveCount(0);
        }

        [Theory]
        [InlineData("//p[@class='test']")]
        [InlineData("//p[@class='test1']")]
        [InlineData("//p[@class='test2']")]
        public async Task FindSingleElement_ShouldReturnInstance_WhenValidXpathProvided(string xpath)
        {
            //Arrange
            var htmlContentHelper = new HtmlGeneratorHelper();
            htmlContentHelper.InsertAdditionalContent("1", "test", "p");
            htmlContentHelper.InsertAdditionalContent("1", "test1", "p");
            htmlContentHelper.InsertAdditionalContent("1", "test2", "p");
            var _htmlParserService = new HtmlParserService(_logger, htmlContentHelper.Build());

            //Act
            var result = await _htmlParserService.FindSingleElement(xpath);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task FindSingleElement_ShouldReturnInstance_WhenMultipleNodesExistsForGivenXPath()
        {
            //Arrange
            var htmlContentHelper = new HtmlGeneratorHelper();
            htmlContentHelper.InsertAdditionalContent("1", "test", "p");
            htmlContentHelper.InsertAdditionalContent("2", "test", "p");
            htmlContentHelper.InsertAdditionalContent("1", "test2", "p");
            var _htmlParserService = new HtmlParserService(_logger, htmlContentHelper.Build());

            //Act
            var result = await _htmlParserService.FindSingleElement("//p[@class='test']");

            //Assert
            result.Should().NotBeNull();
            result.Text.Should().Be("1");
        }
    }
}
