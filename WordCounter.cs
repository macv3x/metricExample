using System.Text.RegularExpressions;

namespace CounterTestApp;

public class WordCounter : IWordCounter
{
    private readonly string _text = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "input.pdf"));
    private Dictionary<string, int> _frequency = [];

    public void CountWords()
    {
        _frequency = new Dictionary<string, int>();
        foreach (Match match in Helper.Words().Matches(_text))
        {
            var word = match.Value;
            if (!_frequency.TryAdd(word, 1))
            {
                _frequency[word]++;
            }
        }
    }
}

public interface IWordCounter
{
    void CountWords();
}

static partial class Helper
{
    [GeneratedRegex(@"\b\w+\b")]
    public static partial Regex Words();
}