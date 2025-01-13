namespace Translate.Services
{
    public class CollectContext
    {
        public string Path { get; set; }

        public List<ViewText> ViewTexts { get; set; } = new List<ViewText>();

        public List<CoderText> CoderTexts { get; set; } = new List<CoderText>();
    }

    public class ViewText
    {
        public string Text { get; set; }

        public string OriginalText { get; set; }
    }

    public class CoderText
    {
        public string OriginalText { get; set; }

        public int Line { get; set; }
    }
}
