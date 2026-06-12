using BLL;
using DAL;
using EL;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace IntegrationTests
{
    public class NotificationServiceIntegrationTests : IDisposable
    {
        private readonly WireMockServer _server;

        public NotificationServiceIntegrationTests()
        {
            _server = WireMockServer.Start();
        }

        [Fact]
        public async Task CheckAndShowNotificationAsync_GiveUnreadMatchNotification_ShowNotification()
        {
            // Arrange
            StubNotification("{\"Success\":true,\"Notifications\":[{\"NotificationId\":5,\"UserId\":1,\"Type\":\"MATCH\",\"Title\":\"Novi mathc\",\"Content\":\"Imate novi match\",\"CreatedAt\":\"2026-01-01T10:00:00\",\"IsRead\":false}]}");

            var presenter = A.Fake<INotificationPresenter>();
            var service = CreateDefaultNotificationService(presenter);

            // Act
            await service.CheckAndShowNotificationAsync(1);

            // Assert
            A.CallTo(() => presenter.Show(A<Notification>.That.Matches(n => n.NotificationId == 5 && n.Type == "MATCH" && !n.IsRead), A<Action>._)).MustHaveHappenedOnceExactly();
        }

        private NotificationService CreateDefaultNotificationService(INotificationPresenter presenter)
        {
            var apiClient = new ApiClient(new Uri(_server.Url + "/"));
            var repository = new NotificationRepository(apiClient);

            return new NotificationService(repository, presenter);
        }

        private void StubNotification(string body)
        {
            _server.Given(Request.Create().WithPath("/api/notifications/list.php").UsingGet()).RespondWith(Response.Create().WithStatusCode(200).WithHeader("Content-Type", "application/json").WithBody(body));
        }

        public void Dispose()
        {
            _server.Stop();
            _server.Dispose();
        }
    }
}
