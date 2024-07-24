using System.Text;

namespace GeneticCars.Evolution;

// Adapted from Waqar Khan https://stackoverflow.com/a/49922533/880990.

public readonly record struct Name(string Raw, string Display) : IComparable, IComparable<Name>
{
    public const char ChSurrogate = 'ä', ShSurrogate = 'ö', ThSurrogate = 'ü';
    public const char UpperChSurrogate = 'Ä', UpperShSurrogate = 'Ö', UpperThSurrogate = 'Ü';
    private static readonly char[] consonants = ['b', 'b', 'b', 'c', 'c', 'c', ChSurrogate, ChSurrogate, 
        'd', 'd', 'd', 'd', 'f', 'f', 'g', 'g', 'g', 'g', 'h', 'h', 'h', 'j', 'k', 'k', 'l', 'l', 'l', 'l', 'l',
        'm', 'm', 'm', 'm', 'n', 'n', 'n', 'n', 'n', 'n', 'p', 'p', 'p', 'p', 'q', 'r', 'r', 'r', 'r', 'r', 'r',
        's', 's', 's', 's', ShSurrogate, ShSurrogate, ShSurrogate, 't', 't', 't', 't', ThSurrogate, ThSurrogate,
        'v', 'v', 'w', 'w', 'x', 'z'];
    private static readonly char[] vowels = ['a', 'a', 'e', 'e', 'e', 'i', 'i', 'i', 'o', 'o', 'u', 'y'];

    public Name(string raw) : this(raw, ToDisplay(raw))
    {
    }

    public Name CombineWith(Name other)
    {
        string n1 = Raw[..(Raw.Length / 2)];
        string n2 = other.Raw[..(Raw.Length / 2)];
        return new(n1 + n2);
    }

    public static Name GenerateRandom(int length)
    {
        char c = Char.ToUpper(consonants[Random.Shared.Next(consonants.Length)]);
        var (raw, display) = (c.ToString(), ToDisplayChar(c));
        c = vowels[Random.Shared.Next(vowels.Length)];
        raw += c; display += c;

        int n = 2;
        while (n < length) {
            c = consonants[Random.Shared.Next(consonants.Length)];
            raw += c; display += ToDisplayChar(c);
            n++;
            c = vowels[Random.Shared.Next(vowels.Length)];
            raw += c; display += c;
            n++;
        }
        return new(raw);
    }

    private static string ToDisplayChar(char raw) => raw switch {
        ChSurrogate => "ch",
        UpperChSurrogate => "Ch",
        ShSurrogate => "sh",
        UpperShSurrogate => "Sh",
        ThSurrogate => "th",
        UpperThSurrogate => "Th",
        _ => raw.ToString()
    };

    public static string ToDisplay(string raw)
    {
        var sb = new StringBuilder(raw.Length + 6);
        for (int i = 0; i < raw.Length; i++) {
            sb.Append(ToDisplayChar(raw[i]));
        }
        return sb.ToString();
    }

    public override string ToString() => Display;

    int IComparable.CompareTo(object? obj)
    {
        if (obj is Name name) {
            return CompareTo(name);
        }
        throw new ArgumentException(nameof(obj) + " is not a " + nameof(Name));
    }

    public int CompareTo(Name other)
    {
        int result = Raw.CompareTo(other.Raw);
        if (result != 0)
            return result;
        return Display.CompareTo(other.Display);
    }
}
