namespace MyMessages.Logics.Dtos
{
    public class MessageDto
    {
        public int? Id { get; set; }
        public string Text { get; set; }
        public PictureDto Picture { get; set; }
        public FileDto File { get; set; }
        public StickerDto Sticker { get; set; }
        public long? Date { get; set; }
    }
}
