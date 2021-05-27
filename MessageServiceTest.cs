using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using xUnitTestProject;
using ZPool.Models;
using ZPool.Services.EFServices;
using ZPool.Services.Interfaces;

namespace xUnitTestProject1
{
    public class MessageServiceTest : ZPoolTestBase
    {
        private IMessageService _messageService;

        public MessageServiceTest()
        {
            _messageService = new EFMessageService(_context);
        }

        [Fact]
        public void CreateNewMessage_Test()
        {
            var message = new Message()
            {
                SenderId = 1,
                ReceiverId = 2,
                MessageBody = "Test",
                SendingDate = DateTime.Now
            };

            _messageService.CreateMessage(message);

            var testMessage = _context.Messages.ToList()[0];

            Assert.NotEmpty(_context.Messages);
            Assert.Equal(1, testMessage.Id);
            Assert.Equal("Test", testMessage.MessageBody);
            Assert.False(testMessage.IsRead);
            Assert.True(_messageService.HasUnreadMessages(2));
        }

        [Fact]
        public void GetMessagesByUser_Test()
        {
            // Arrange
            var message = new Message()
            {
                SenderId = 1,
                ReceiverId = 2,
                MessageBody = "Test",
                SendingDate = DateTime.Now
            };

            var message2 = new Message()
            {
                SenderId = 2,
                ReceiverId = 3,
                MessageBody = "Test",
                SendingDate = DateTime.Now
            };

            _messageService.CreateMessage(message);
            _messageService.CreateMessage(message2);

            // Act
            var messages = _messageService.GetMessagesByUserId(2);

            // Assert
            Assert.NotEmpty(messages);
            Assert.Equal(2, messages.Count());
        }

        [Fact]
        public void SetStatusToRead_Test()
        {
            //Arrange
            var message = new Message()
            {
                SenderId = 1,
                ReceiverId = 2,
                MessageBody = "Test",
                SendingDate = DateTime.Now
            };

            _messageService.CreateMessage(message);

            // Act
            _messageService.SetStatusToRead(message.Id);
            var testMessage = _context.Messages.ToList()[0];

            // Assert
            Assert.NotEmpty(_context.Messages);
            Assert.Equal(1, testMessage.Id);
            Assert.True(testMessage.IsRead);
        }

        

    }
}
