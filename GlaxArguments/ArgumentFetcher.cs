namespace GlaxArguments;

public class ArgumentFetcher
{
    public Argument[] ValidArguments;

    private static readonly Argument HelpArg = new(["-h", "--help"], 0, "Print this help text.");

    public ArgumentFetcher(IEnumerable<Argument> arguments)
    {
        Argument[] temp = arguments as Argument[] ?? arguments.ToArray();
        temp = temp.DistinctBy(t => t.Flags).ToArray();
        if(temp.Length != arguments.Count())
            throw new ArgumentException("Can not have duplicate flags.");
        ValidArguments = temp.Concat(new[] { HelpArg }).ToArray();
    }

    public Dictionary<Argument, string[]> Fetch(string[] args)
    {
        Dictionary<Argument, string[]> ret = new();
        for (int i = 0; i < args.Length; i++)
        {
            string flag = args[i];
            if(flag.Length < 1)
                continue;
            if (!(flag.StartsWith('-') && flag.Length == 2 || flag.StartsWith("--"))) 
                throw new ArgumentException($"{flag} is not a flag. Enter -h for a list of all valid flags.");
            if (!ValidArguments.Any(argument => argument.Flags.Contains(flag)))
                throw new ArgumentNullException($"Flag {flag} does not exist.");
            Argument argument = ValidArguments.First(argument => argument.Flags.Contains(flag));
            if (argument.Equals(HelpArg))
            {
                ret.Clear();
                ret.Add(argument, ValidArguments.Select(arg => arg.ToString()).ToArray());
                return ret;
            }
            
            int lastParameterIndex = i + argument.ParameterCount;
            if(lastParameterIndex >= args.Length)
                throw new ArgumentException($"Not enough Parameters provided for flag {flag}. (Expected {argument.ParameterCount})");
            
            List<string> parameters = new();
            for (; i < lastParameterIndex; i++)
            {
                string param = args[i + 1];
                if(param.StartsWith('-'))
                    throw new ArgumentException($"Not enough Parameters provided for flag {flag}. (Expected {argument.ParameterCount})");
                if(param.StartsWith('"') && param.EndsWith('"'))
                    parameters.Add(param.Substring(1, param.Length - 2));
                else
                    parameters.Add(param);
            }
            ret.Add(argument, parameters.ToArray());
        }
        return ret;
    }

    public Dictionary<Argument, string[]> Fetch(string argumentString)
    {
        return Fetch(argumentString.Split(' '));
    }
}