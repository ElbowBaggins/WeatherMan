using System;
using System.Text;
using Hemogoblin.YWeather;

namespace Hemogoblin.WeatherMan
{

    /// <summary>
    /// Very basic launcher stub for WeatherMan tool.
    /// 
    /// All this does is make a new WeatherMan object for the given location
    /// </summary>
    public class Launcher
    {

        /*
         * "Elijah! You madman! Why are there these three const strings at the top of your Launcher class?"
         * 
         * Because, friend, Console doesn't wrap words, so I have to break it up myself. You can do this
         * algorithmically but linebreaks count as characters and it ruins everything so I have to break it up
         * or it comes out even more fragmented. No thank you.
         */
        private const string InfoMessagePt1 = "WeatherMan is a tool created for demonstrating the capabilities of the YWeather library. " +
                                              "Okay... so, what's that?";
        private const string InfoMessagePt2 = "The YWeather library is a helper library for retrieving weather data " +
                                              "from the Yahoo! Weather service. I went with Yahoo's service because it's relatively well-established " +
                                              "and I didn't have to sign up to use it, or even use any sort of key. It's also pretty good about not just " +
                                              "barfing up nonsense data if you try to tell it to get weather data that it doesn't have. Oh, and their " +
                                              "data is from The Weather Channel, so it's pretty decent.";
        private const string InfoMessagePt3 = "\"Okay, dork, how do I use this thing? I just want to check the weather!\"";
        private const string InfoMessagePt4 = "Well it's just as easy as insulting a superior intellect! " +
                                              "Just type \"WeatherMan\" followed by the name of the place " +
                                              "you want to get the weather for. You can use postal codes or pretty much anything else you can come up with. " +
                                              "It'll get responses for all sorts of things. Do pay attention, though. It will indiscriminately ask the server for " +
                                              "whatever you tell it and if you type something incorrectly then Yahoo's servers might just guess and give you " +
                                              "something totally weird or shoot your grandma. Not my problem. Happy weather-ing, or whatever.";

        /// <summary>
        /// Returns a string with linebreaks every, at most, 77 columns.
        /// Why 77? Easy. Because Windows' 80 column command prompt will wrap
        /// invisibles onto their own lines. So it wraps line breaks. Cool. 
        /// Let's just give them some space.
        /// 
        /// Shamelessly modified from a solution on StackOverflow.
        /// It's probably rather ineffecient but it's to display a snarky 
        /// info message, so whatever.
        /// </summary>
        /// <param name="toMakeWrappable">A "wrappable" string</param>
        /// <returns></returns>
        private static string MakeTextWrappable(string toMakeWrappable)
        {
            const int columns = 77;
            var words = toMakeWrappable.Split(' ');
            var wrappable = new StringBuilder();


            var currentLine = "";
            foreach(var word in words)
            {
                if((currentLine + word).Length > columns)
                {
                    wrappable.AppendLine(currentLine);
                    currentLine = "";
                }
                currentLine += string.Format("{0} ", word);
            }

            if(currentLine.Length > 0)
                wrappable.AppendLine(currentLine);

            return wrappable.ToString().Replace(" \r\n", "\r\n");
        }

        /// <summary>
        /// Main method. Stub that shows usage message and handles exceptions that bubble all the way out.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            if(0 == args.Length)
            {
                Console.WriteLine("Usage: WeatherMan <location>\r\n" +
                                  "Run \"WeatherMan -info\" for more detailed information.");
            }
            else if(1 == args.Length && args[0].Equals("-info"))
            {
                Console.WriteLine(MakeTextWrappable(InfoMessagePt1));
                Console.WriteLine(MakeTextWrappable(InfoMessagePt2));
                Console.WriteLine(MakeTextWrappable(InfoMessagePt3));
                Console.WriteLine(MakeTextWrappable(InfoMessagePt4));
            }
            else
            {
                // Get the whole location the user gave us by...
                // First get an empty string
                var userEnteredLocation = "";

                // Find out how many command line arguments we can see, we'll need to put
                // spaces between them, as the user typed, but not after the last one!
                var count = args.Length;
                // For every argument...
                foreach(var arg in args)
                {
                    // Append it to the user-entered location string...
                    userEnteredLocation += arg;

                    // and if this isn't the last one, add a space. 
                    if(--count > 0)
                    {
                        userEnteredLocation += " ";
                    }
                }

                // Try to start a new WeatherMan, and wait for it to get done. Catch any exceptions that manage to bubble all the way back here.
                try
                {
                    new WeatherMan(userEnteredLocation).Start().Wait();
                }
                catch(WeatherDataException ex)
                {
                    Console.WriteLine("Whoops! It looks like your request couldn't be completed, for whatever reason. The message below should shed some light on why.");
                    Console.WriteLine(ex.Message);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("An internal error seems to have occurred with WeatherMan. Sorry about that.");
                    if(0 == ex.Message.Length)
                    {
                        Console.WriteLine(
                            "Unfortunately, the .NET exception that caused this failure doesn't have an accompanying message. " +
                            "This means there's not really anything I can tell you other than which exception it was.");
                    }
                    else
                    {
                        Console.WriteLine("The exception which caused this failure included the following message: " + ex.Message + "\r\n" +
                                          "Hopefully the message Microsoft included with the .NET framework is helpful. I don't get to control that, sadly.");
                    }
                    Console.WriteLine("The caught exception was: " + ex.GetType());
                }
            }
        }
    }
}
