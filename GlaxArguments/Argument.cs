using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GlaxArguments;

public struct Argument
{
    [Required]
    public string[] Flags;
    [Required]
    public string? Description;
    [Required]
    public int ParameterCount;

    public Argument(string[] flags, int parameterCount = 0, string? description = null)
    {
        if (flags.Length < 1)
            throw new ArgumentException( "Missing flags for Argument.");
        foreach(string flag in flags)
            if(flag == "--")
                throw new ArgumentException("Flag can not be named '--'.");
            else if(flag.StartsWith("--") && flag.Length < 4)
                throw new ArgumentException("Flags one character can only have one dash.");
            else if (flag.StartsWith("-") && !flag.StartsWith("--") && flag.Length != 2)
                throw new ArgumentException("Flags with more than one character need to have two dashes.");
        this.Flags = flags;
        if (parameterCount < 0)
            throw new ArgumentException("Argument can not have less that 0 parameters.");
        this.ParameterCount = parameterCount;
        this.Description = description;
    }

    public override bool Equals(object? obj)
    {
        return obj is Argument a && Equals(a);
    }

    public bool Equals(Argument other)
    {
        return Flags.Equals(other.Flags);
    }

    public override int GetHashCode()
    {
        return Flags.GetHashCode();
    }

    public override string ToString()
    {
        string arg = "<arg> ";
        return $"{string.Join('\n', Flags)}\t{new StringBuilder(arg.Length * ParameterCount).Insert(0, arg, ParameterCount)}\t{Description}";
    }
}