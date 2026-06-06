using BLL;
using DAL;
using EL;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BLLUnitTests
{
    public class NotificationServiceTests
    {
        [Fact]
        public async Task CheckAndShowNotificationAsync_GivenUnreadMatchNotification_ShowNotification()
        {
            // Arrange
            var notificaitonRepository = A.Fake<INotificationRepository>();
            var presenter = A.Fake<INotificationPresenter>();

            var notification = new Notification
            {
                NotificationId = 5,
                Type = "MATCH",
                IsRead = false
            };

            A.CallTo(() => notificaitonRepository.GetNotificationsAsync(1)).Returns(Task.FromResult(new List<Notification> { notification }));

            var service = new NotificationService(notificaitonRepository, presenter);

            // Act
            await service.CheckAndShowNotificationAsync(1);

            // Assert
            A.CallTo(() => presenter.Show(notification, A<Action>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CheckAndShowNotificationAsync_GiveOnlyReadMatchNotification_DoesNoShowNotification()
        {
            // Arrange
            var notificaitonRepository = A.Fake<INotificationRepository>();
            var presenter = A.Fake<INotificationPresenter>();

            A.CallTo(() => notificaitonRepository.GetNotificationsAsync(1)).Returns(Task.FromResult(new List<Notification>
            {
                new Notification
                {
                    NotificationId = 5,
                    Type = "MATCH",
                    IsRead = true
                }
            }));

            var service = new NotificationService(notificaitonRepository, presenter);

            // Act
            await service.CheckAndShowNotificationAsync(1);

            // Assert
            A.CallTo(() => presenter.Show(A<Notification>._, A<Action>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task CheckAndShowNotificationAsync_GiveUnreadNonMatchNotification_DoesNoShowNotification()
        {
            // Arrange
            var notificaitonRepository = A.Fake<INotificationRepository>();
            var presenter = A.Fake<INotificationPresenter>();

            A.CallTo(() => notificaitonRepository.GetNotificationsAsync(1)).Returns(Task.FromResult(new List<Notification>
            {
                new Notification
                {
                    NotificationId = 5,
                    Type = "MESSAGE",
                    IsRead = false
                }
            }));

            var service = new NotificationService(notificaitonRepository, presenter);

            // Act
            await service.CheckAndShowNotificationAsync(1);

            // Assert
            A.CallTo(() => presenter.Show(A<Notification>._, A<Action>._)).MustNotHaveHappened();
        }

        [Fact]
        public async Task MarkLatestMatchAsReadAsync_GivenUnreadMatchNotification_MarksNotificationAsRead()
        {
            // Arrange
            var notificaitonRepository = A.Fake<INotificationRepository>();
            var presenter = A.Fake<INotificationPresenter>();

            A.CallTo(() => notificaitonRepository.GetNotificationsAsync(1)).Returns(Task.FromResult(new List<Notification>
            {
                new Notification
                {
                    NotificationId = 5,
                    Type = "MATCH",
                    IsRead = false
                }
            }));

            var service = new NotificationService(notificaitonRepository, presenter);

            // Act
            await service.MarkLatestMatchAsReadAsync(1);

            // Assert
            A.CallTo(() => notificaitonRepository.MarkAsReadAsync(5)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CheckAndShowNotificationAsync_GivenNullPresenter_DoesNotShowNotification()
        {
            // Arrange
            var notificaitonRepository = A.Fake<INotificationRepository>();

            A.CallTo(() => notificaitonRepository.GetNotificationsAsync(1)).Returns(Task.FromResult(new List<Notification>
            {
                new Notification
                {
                    NotificationId = 5,
                    Type = "MATCH",
                    IsRead = false
                }
            }));

            var service = new NotificationService(notificaitonRepository, null);

            // Act
            var exception = await Record.ExceptionAsync(() => service.CheckAndShowNotificationAsync(1));

            // Assert
            Assert.Null(exception);
        }
    }
}
