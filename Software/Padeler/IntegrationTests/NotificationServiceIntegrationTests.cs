using BLL;
using DAL;
using EL;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            StubNotification("{\"Success\":true,\"Notifications\":[{\"NotificationId\":5,\"UserId\":1,\"Type\":\"MATCH\",\"Title\":\"Novi match\",\"Content\":\"Imate novi match\",\"CreatedAt\":\"2026-01-01T10:00:00\",\"IsRead\":false}]}");

            var presenter = A.Fake<INotificationPresenter>();
            var service = CreateDefaultNotificationService(presenter);

            // Act
            await service.CheckAndShowNotificationAsync(1);

            // Assert
            A.CallTo(() => presenter.Show(A<Notification>.That.Matches(n => n.NotificationId == 5 && n.Type == "MATCH" && !n.IsRead), A<Action>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CheckAndShowNotificationAsync_GiveReadMatchNotification_DoesNotShowNotification()
        {
            // Arrange
            StubNotification("{\"Success\":true,\"Notifications\":[{\"NotificationId\":5,\"UserId\":1,\"Type\":\"MATCH\",\"Title\":\"Novi match\",\"Content\":\"Imate novi match\",\"CreatedAt\":\"2026-01-01T10:00:00\",\"IsRead\":true}]}");

            var presenter = A.Fake<INotificationPresenter>();
            var service = CreateDefaultNotificationService(presenter);

            // Act
            await service.CheckAndShowNotificationAsync(1);

            // Assert
            A.CallTo(() => presenter.Show(A<Notification>._, A<Action>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task CheckAndShowNotificationAsync_GiveUnReadNonMatchNotification_DoesNotShowNotification()
        {
            // Arrange
            StubNotification("{\"Success\":true,\"Notifications\":[{\"NotificationId\":7,\"UserId\":1,\"Type\":\"INFO\",\"Title\":\"Obavijest\",\"Content\":\"Imate novi match\",\"CreatedAt\":\"2026-01-01T10:00:00\",\"IsRead\":false}]}");

            var presenter = A.Fake<INotificationPresenter>();
            var service = CreateDefaultNotificationService(presenter);

            // Act
            await service.CheckAndShowNotificationAsync(1);

            // Assert
            A.CallTo(() => presenter.Show(A<Notification>._, A<Action>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task MarkLatestMatchAsReadAsync_GiveUnreadMatchNotification_CallsMarkReadEndpoint()
        {
            // Arrange
            StubNotification("{\"Success\":true,\"Notifications\":[{\"NotificationId\":7,\"UserId\":1,\"Type\":\"MATCH\",\"Title\":\"Novi match\",\"Content\":\"Imate novi match\",\"CreatedAt\":\"2026-01-01T10:00:00\",\"IsRead\":false}]}");
            StubPost("/api/notifications/mark_read.php", "{\"Success\":true}");

            var presenter = A.Fake<INotificationPresenter>();
            var service = CreateDefaultNotificationService(presenter);

            // Act
            await service.MarkLatestMatchAsReadAsync(1);

            // Assert
            var request = _server.FindLogEntries(Request.Create().WithPath("/api/notifications/mark_read.php").UsingPost());
            Assert.Single(request);
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

        private void StubPost(string path, string body)
        {
            _server.Given(Request.Create().WithPath(path).UsingPost()).RespondWith(Response.Create().WithStatusCode(200).WithHeader("Content-Type", "application/json").WithBody(body));
        }

        public void Dispose()
        {
            _server.Stop();
            _server.Dispose();
        }
    }
}
