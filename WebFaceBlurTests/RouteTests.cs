using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Web.Routing;
using WebFaceBlur;
using System;
using System.Reflection;

namespace WebFaceBlur.Tests
{
    /// <summary>
    /// Сводное описание для RouteTests
    /// </summary>
    [TestClass]
    public class RouteTests
    {
        [TestMethod]
        public void DefaultTest()
        {
            TestRouteMatch("~/Default", "Default", "Index");
        }

        [TestMethod]
        public void DefaultOpenAnySiteTest()
        {
            TestRouteMatch("~/?path=" + HttpUtility.UrlEncode("https://google.com/test") , "Default", "Index");
        }
        


        [TestMethod]
        public void DefaultOpenEmptyTest()
        {
            TestRouteMatch("~/", "Default", "Index");
        }

        [TestMethod]
        public void AdressOpenEmptyTest()
        {
            TestRouteMatch("~/AdressForm/Post", "AdressForm", "Post");
        }

        [TestMethod]
        public void ImageTest()
        {
            TestRouteMatch("~/?path=" + HttpUtility.UrlEncode("https://blogs.msdn.microsoft.com/wp-content/themes/microsoft/images/ms-logo-gray.svg"), "Default", "Index");
        }


        private HttpContextBase CreateHttpContext(string targetUrl = null, string httpMethod = "GET")
        {
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath)
                .Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);
            
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(
                It.IsAny<string>())).Returns<string>(s => s);
            
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);
            
            return mockContext.Object;
        }
        private void TestRouteMatch(string url, string controller, string action, object routeProperties = null, string httpMethod = "GET")
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            RouteData result
                = routes.GetRouteData(CreateHttpContext(url, httpMethod));
            
            Assert.IsNotNull(result);
            Assert.IsTrue(TestIncomingRouteResult(result, controller,
                action, routeProperties));
        }
        private bool TestIncomingRouteResult(RouteData routeResult, string controller, string action, object propertySet = null)
        {
            Func<object, object, bool> valCompare = (v1, v2) =>
            {
                return StringComparer.InvariantCultureIgnoreCase
                    .Compare(v1, v2) == 0;
            };

            bool result = valCompare(routeResult.Values["controller"], controller)
                && valCompare(routeResult.Values["action"], action);

            if ( propertySet != null )
            {
                PropertyInfo[] propInfo = propertySet.GetType().GetProperties();
                foreach ( PropertyInfo pi in propInfo )
                {
                    if ( !(routeResult.Values.ContainsKey(pi.Name)
                    && valCompare(routeResult.Values[pi.Name],
                    pi.GetValue(propertySet, null))) )
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }
        private void TestRouteFail(string url)
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            
            RouteData result = routes.GetRouteData(CreateHttpContext(url));
            
            Assert.IsTrue(result == null || result.Route == null);
        }
    }
}
