using System;

namespace TodoApi.Dtos
{
    public class MessageToCreateDto
    {
    
         public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }
        public MessageToCreateDto()
        {
            MessageSent = DateTime.Now;
        }
    
    }
}