using System.Collections.Generic;

namespace MyMessages.Logics.Dtos
{
    public class MessagesDataDto
    {
        public List<MessageDto> Messages { get; set; }
        public int? NextId { get; set; }
    }
}
