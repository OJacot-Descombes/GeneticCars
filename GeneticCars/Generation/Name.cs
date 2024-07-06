using System.Text;

namespace GeneticCars.Generation;

// Adapted from Waqar Khan https://stackoverflow.com/a/49922533/880990.

public readonly record struct Name(string Raw, string Display)
{
    public const char ChSurrogate = 'ä', ShSurrogate = 'ö', ThSurrogate = 'ü';
    public const char UpperChSurrogate = 'Ä', UpperShSurrogate = 'Ö', UpperThSurrogate = 'Ü';

    static readonly char[] consonants = ['b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't',
        'v', 'w', 'x', 'z', ChSurrogate, ShSurrogate, ThSurrogate ];
    static readonly char[] vowels = ['a', 'e', 'i', 'o', 'u', 'y'];

    public Name(string raw) : this(raw, ToDisplay(raw))
    {
    }

    public Name CombineWith(Name other) => new(Raw[..(Raw.Length / 2)] + other.Raw[..(Raw.Length / 2)]);

    public static Name GenerateRandom(int length)
    {
        char c = Char.ToUpper(consonants[Random.Shared.Next(consonants.Length)]);
        var (raw, display) = (c.ToString(), ToDisplay(c));
        c = vowels[Random.Shared.Next(vowels.Length)];
        raw += c; display += c;

        int n = 2;
        while (n < length) {
            c = consonants[Random.Shared.Next(consonants.Length)];
            raw += c; display += ToDisplay(c);
            n++;
            c = vowels[Random.Shared.Next(vowels.Length)];
            raw += c; display += c;
            n++;
        }
        return new(raw, display);
    }

    private static string ToDisplay(char raw) => raw switch {
        ChSurrogate => "ch",
        UpperChSurrogate => "Ch",
        ShSurrogate => "sh",
        UpperShSurrogate => "Sh",
        ThSurrogate => "th",
        UpperThSurrogate => "Th",
        _ => raw.ToString()
    };

    private static string ToDisplay(string raw)
    {
        var sb = new StringBuilder(raw.Length + 6);
        for (int i = 0; i < raw.Length; i++) {
            sb.Append(ToDisplay(raw[i]));
        }
        return sb.ToString();
    }

    public override string ToString() => Display;
}
