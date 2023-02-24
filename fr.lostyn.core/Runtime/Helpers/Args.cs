using System;
using System.Collections.Generic;

public static class Args {
    public class ArgsObject {
        private Dictionary<string, string> parameters = new Dictionary<string, string>();
        
        public int Count => parameters.Count;
        public string this[string key] => parameters[key];
        public Action<string, string> Add => parameters.Add;
        public bool Has(string key) => parameters.ContainsKey(key);
    }

    public static ArgsObject Parse(string input) {
        string[] argv = input.Split(new char[] { ' ' });
        if(argv.Length % 2 != 0) {
            throw new ArgumentException("Arguments can be parse, incorrect number of parameters");
        }

        ArgsObject result = new ArgsObject();
        for (int i = 0; i < argv.Length; i += 2) {
            if (!argv[i].StartsWith("-")) {
                throw new ArgumentException("Missing parameters definition");
            }

            if (argv[i+1].StartsWith("-")) {
                throw new ArgumentException("Missing value for parameter " + argv[i]);
            }

            result.Add(argv[i], argv[i+1]);
        }

        return result;
    }
}