namespace MyMessages.Api.Models
{
    public class MessageModel
    {
        public int? Id { get; set; }
        public string Text { get; set; }
        public PictureModel Picture { get; set; }
        public FileModel File { get; set; }
        public StickerModel Sticker { get; set; }
        public long? Date { get; set; }
    }
}
