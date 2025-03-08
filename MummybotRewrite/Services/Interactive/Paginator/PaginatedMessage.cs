using System.Collections.Generic;

namespace Discord.Addons.Interactive
{
    public class PaginatedMessage
    {
        public SortedList<int, EmbedBuilder> Pages = new SortedList<int, EmbedBuilder>();        
        
        public string Content { get; set; } = "";
        public PaginatedAppearanceOptions Options { get; set; } = PaginatedAppearanceOptions.Default;
    }

    public static class PaginatedMessageExtensions
    {
        public static void Add(this SortedList<int, EmbedBuilder> pages, EmbedBuilder embedBuilder)
        {
            pages.Add(pages.Count + 1, embedBuilder);
        }

        public static void Remove(this SortedList<int, EmbedBuilder> pages, int page)
        {
            pages.Remove(page);
            var newpages = new SortedList<int, EmbedBuilder>();
            foreach (var item in pages)
            {
                newpages.Add(newpages.Count + 1, item.Value);
            }
            pages = newpages;
        }

    }
}
