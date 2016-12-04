using System;
using System.IO;
using IdSharp.Tagging.ID3v2;

namespace IdSharp.Example.Console.TagConverter
{
    class Program
    {
        private static bool _isRecursive;
        private static string _fileName;
        private static string _directory;
        private static Verbosity _verbosity = Verbosity.Default;
        private static bool _isTest;
        private static bool _upgradeOnly;
        private static ID3v2TagVersion? _newTagVersion;
        private static bool _hasWritten;

        static void Main(string[] args)
        {
            bool success = ParseArguments(args);
            if (!success)
                return;

            DateTime start = DateTime.Now;

            if (_isTest && _verbosity >= Verbosity.Default)
            {
                WriteLine("TEST - NO ACTUAL FILES WILL BE CHANGED");
                WriteLine();
            }

            if (_verbosity >= Verbosity.Default)
            {
                if (_directory != null)
                    WriteLine(string.Format("Directory: \"{0}\"", _directory));
                else
                    WriteLine(string.Format("File: \"{0}\"", _fileName));
                
                string options = "Options: Convert to " + _newTagVersion;
                if (_isTest)
                    options += ", test";
                if (_isRecursive)
                    options += ", recurisve";
                if (_upgradeOnly)
                    options += ", upgrade only";
                
                WriteLine(options);
            }

            string[] files;
            if (_directory != null)
            {
                WriteLine(string.Format("Getting '{0}' files...", _directory), Verbosity.Full);

                files = Directory.GetFiles(_directory, "*.mp3", _isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            else
            {
                files = new[] { _fileName };
            }

            int updated = 0, skipped = 0, failed = 0;
            int id3v22Count = 0, id3v23Count = 0, id3v24Count = 0, noid3v2Count = 0;
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];

                try
                {
                    WriteLine(string.Format("File {0:#,0}/{1:#,0}: '{2}'", i + 1, files.Length, file), Verbosity.Full);

                    ID3v2Tag id3v2 = new ID3v2Tag(file);
                    if (id3v2.Header != null)
                    {
                        switch (id3v2.Header.TagVersion)
                        {
                            case ID3v2TagVersion.ID3v22:
                                id3v22Count++;
                                break;
                            case ID3v2TagVersion.ID3v23:
                                id3v23Count++;
                                break;
                            case ID3v2TagVersion.ID3v24:
                                id3v24Count++;
                                break;
                        }

                        if (id3v2.Header.TagVersion != _newTagVersion.Value)
                        {
                            if (!_upgradeOnly || _newTagVersion.Value > id3v2.Header.TagVersion)
                            {
                                ID3v2TagVersion oldTagVersion = id3v2.Header.TagVersion;

                                if (!_isTest)
                                {
                                    id3v2.Header.TagVersion = _newTagVersion.Value;
                                    id3v2.Save(file);
                                }

                                WriteLine(string.Format("- Converted {0} to {1}", oldTagVersion, _newTagVersion.Value), Verbosity.Full);

                                updated++;
                            }
                            else
                            {
                                WriteLine(string.Format("- Skipped, existing tag {0} > {1}", id3v2.Header.TagVersion, _newTagVersion.Value), Verbosity.Full);

                                skipped++;
                            }
                        }
                        else
                        {
                            WriteLine("- Skipped, ID3v2 tag version equal to requested version", Verbosity.Full);

                            skipped++;
                        }
                    }
                    else
                    {
                        WriteLine("- Skipped, ID3v2 tag does not exist", Verbosity.Full);

                        skipped++;
                        noid3v2Count++;
                    }
                }
                catch (Exception ex)
                {
                    if (_verbosity >= Verbosity.Default)
                    {
                        string header = string.Format("File: {0}", file);
                        WriteLine(header);
                        WriteLine(new string('=', header.Length));
                        WriteLine(ex.ToString());
                        WriteLine();
                    }

                    failed++;
                }
            }

            if (_verbosity >= Verbosity.Default)
            {
                DateTime end = DateTime.Now;
                TimeSpan duration = end - start;

                if (_hasWritten)
                    WriteLine();
                WriteLine(string.Format("ID3v2.2:   {0:#,0}", id3v22Count));
                WriteLine(string.Format("ID3v2.3:   {0:#,0}", id3v23Count));
                WriteLine(string.Format("ID3v2.4:   {0:#,0}", id3v24Count));
                WriteLine(string.Format("No ID3v2:  {0:#,0}", noid3v2Count));
                WriteLine();
                WriteLine(string.Format("Start:     {0:HH:mm:ss}", start));
                WriteLine(string.Format("End:       {0:HH:mm:ss}", end));
                WriteLine(string.Format("Duration:  {0:hh}:{0:mm}:{0:ss}.{0:fff}", duration));
                WriteLine();
                WriteLine(string.Format("Updated:   {0:#,0}", updated));
                WriteLine(string.Format("Skipped:   {0:#,0}", skipped));
                WriteLine(string.Format("Failed:    {0:#,0}", failed));
            }
        }

        private static bool ParseArguments(string[] args)
        {
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].StartsWith("--"))
                        args[i] = args[i].Substring(1);

                    if (args[i] == "-2.3" || args[i] == "-23")
                    {
                        _newTagVersion = ID3v2TagVersion.ID3v23;
                    }
                    else if (args[i] == "-2.4" || args[i] == "-24")
                    {
                        _newTagVersion = ID3v2TagVersion.ID3v24;
                    }
                    else if (args[i] == "-r" || args[i] == "-recursive")
                    {
                        _isRecursive = true;
                    }
                    else if (args[i] == "-v" || args[i] == "-verbose" || args[i] == "-verbosity")
                    {
                        i++;
                        if (args[i] == "0")
                            _verbosity = Verbosity.Silent;
                        else if (args[i] == "1")
                            _verbosity = Verbosity.Default;
                        else if (args[i] == "2")
                            _verbosity = Verbosity.Full;
                        else
                            throw new NotSupportedException(string.Format("'-v {0}' not supported.", args[i]));
                    }
                    else if (args[i] == "-t" || args[i] == "-test")
                    {
                        _isTest = true;
                    }
                    else if (args[i] == "-u" || args[i] == "-up" || args[i] == "-upgrade" || args[i] == "-upgradeonly")
                    {
                        _upgradeOnly = true;
                    }
                    else
                    {
                        if (File.Exists(args[i]))
                        {
                            _fileName = args[i];
                        }
                        else if (Directory.Exists(args[i]))
                        {
                            _directory = args[i];
                        }
                        else
                        {
                            if (args[i].StartsWith("-"))
                                throw new NotSupportedException(string.Format("switch '{0}' not recognized.", args[i]));
                            else
                                throw new NotSupportedException(string.Format("'{0}' not found.", args[i]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is NotSupportedException)
                {
                    WriteLine(string.Format("Error: {0}", ex.Message));
                    WriteLine();
                }

                PrintUsage();
                return false;
            }

            if (_newTagVersion == null)
            {
                //WriteLine("-[2.3|2.4] is required");
                //WriteLine();

                PrintUsage();
                return false;
            }
            else if (_fileName == null && _directory == null)
            {
                _directory = Environment.CurrentDirectory;
                return true;
            }
            else
            {
                return true;
            }
        }

        private static void PrintUsage()
        {
            WriteLine("tagconvert 0.1 - http://cdtag.com/tagconvert");
            WriteLine();
            WriteLine("Usage: tagconvert -[2.3|2.4] [OPTIONS] [FILE|DIRECTORY]");
            WriteLine();
            WriteLine("Options:");
            WriteLine("  -[2.3|2.4]                 Convert to ID3v2.3 or ID3v2.4 (required)");
            WriteLine("  -recursive, -r             Recursive (include subdirectories)");
            WriteLine("  -verbosity #, -v #         Verbosity:");
            WriteLine("                               0 - Silent");
            WriteLine("                               1 - Options + final report + show errors for failed files (default)");
            WriteLine("                               2 - Full report for each file");
            WriteLine("  -test, -t                  Test, only display what would be performed.");
            WriteLine("  -upgrade, -u               Upgrade only. Useful for upgrading 2.2 to 2.3 without changing 2.4 tags.");
            WriteLine();
            WriteLine("Examples:");
            WriteLine("  tagconvert -2.3 -r \"C:\\Music\"");
            WriteLine("  tagconvert -2.3 -r -upgrade \"C:\\Music\"");
            WriteLine("  tagconvert -2.4 -r -test \"C:\\Music\"");
            WriteLine("  tagconvert -2.3 -r -upgrade -test -verbosity 2 \"C:\\Music\"");
            WriteLine("  tagconvert -2.3 \"C:\\Music\\JustThisDirectory\"");
            WriteLine("  tagconvert -2.3 \"C:\\Music\\JustThisDirectory\\JustThisFile.mp3\"");
        }

        public static void WriteLine(string value = null, Verbosity? minimumVerbosity = null)
        {
            if (minimumVerbosity == null || _verbosity >= minimumVerbosity)
            {
                _hasWritten = true;
                System.Console.WriteLine(value);
            }
        }
    }
}
