﻿using GlaxArguments;

Argument[] arguments =
[
    new (["-t", "--test"], 0, "Test arg"),
    new (["--test1"], 1, "Test arg with 1 parameter"),
    new (["--test2"], 2, "Test arg with 2 parameters"),
    new (["--test3"], 0, "Test arg")
];
ArgumentFetcher fetcher = new(arguments);
Dictionary<Argument, string[]> fetched = fetcher.Fetch("-h");
foreach(KeyValuePair<Argument, string[]> arg in fetched)
    Console.WriteLine($"{arg.Key.Flags[0]} - {arg.Key.Description}\n\t{string.Join("\n", arg.Value)}");