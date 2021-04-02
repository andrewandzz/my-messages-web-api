using System.Collections.Generic;

namespace MyMessages.Api.Models
{
    public class MessagesDataModel
    {
        public List<MessageModel> Messages { get; set; }
        public int? NextId { get; set; }
    }
}
