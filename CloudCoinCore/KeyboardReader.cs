using System;
using System.Text;
using System.Linq;


namespace Founders
{
    public class KeyboardReader
    {

        public static int INT_MESSAGE = 0;

        public static int DOUBLE_MESSAGE = 1;

        public static int CHAR_MESSAGE = 2;

        public static int STRING_MESSAGE = 3;

        public static int BOOLEAN_MESSAGE = 4;

        public static int LONG_MESSAGE = 5;

        public static int NUM_ERROR_MESSAGES = 6;


        private string prompt = "> ";

        private string[] errorMessages;

        

        public KeyboardReader()
        {
            this.prompt = "> ";
            
        }


        public void setPrompt(string newPrompt)
        {
            this.prompt = newPrompt;
        }




        public string readString()
        {
            char theChar = 'x';
            string result = "";
            bool done = false;
            while (!done)
            {
                theChar = this.nextChar();
                if ((theChar == '\n'))
                {
                    done = true;
                }
                else if ((theChar == '\r'))
                {

                }
                else
                {
                    result = (result + theChar);
                }
            }

            return result;
        }



        public string readString(string[] args)
        {
            string result = this.readString();
            result = result.ToLower();
            while (!args.Any(result.Contains))
            {
                Console.Out.WriteLine("Please enter one of the following: " + ConvertStringArrayToString(args));// "Please enter one of the following: " );
                Console.Out.Write(this.prompt);
                result = this.readString(args);
            }

            return result;
        }




        public int readInt()
        {
            string inputString = "";
            int number = 0;
            bool done = false;
            while (!done)
            {
                try
                {
                    inputString = this.readString();
                    inputString = inputString.Trim();
                    number = Convert.ToInt32(inputString);
                    done = true;
                }
                catch (FormatException e)
                {
                    Console.Out.WriteLine("Input is not an integer. " + this.errorMessages[INT_MESSAGE]);// "Input is not an integer. ";
                    CoreLogger.Log("Input is not an integer. " + this.errorMessages[INT_MESSAGE]);
                    CoreLogger.Log(e.ToString());
                    Console.Out.Write(this.prompt);
                }

            }

            return number;
        }

        public int readInt(int min, int max)
        {
            string inputString = "";
            int number = 0;
            bool done = false;
            while (!done)
            {
                try
                {
                    inputString = this.readString();
                    inputString = inputString.Trim();
                    number = Convert.ToInt32(inputString);
                    if (((number < min) || (number > max)))
                    {
                        Console.Out.WriteLine("Please enter an integer between " + min + " & " + max);//"Please enter an integer between " 
                    }
                    else
                    {
                        done = true;
                    }

                }
                catch (FormatException e)
                {
                    Console.Out.WriteLine("Input is not an integer. Please enter an integer between " + min + " & " + max);//"Input is not an integer. Please enter an integer between " 
                    CoreLogger.Log("Input is not an integer. Please enter an integer between " + min + " & " + max);
                    CoreLogger.Log(e.ToString());
                    Console.Out.Write(this.prompt);
                }

            }

            return number;
        }

        public int readInt(int[] args)
        {
            int result = this.readInt();
            while (!this.checkInArray(result, args))
            {
                Console.Out.WriteLine("Please enter one of the following: " + string.Join(",", args));// "Please enter one of the following: "
                Console.Out.Write(this.prompt);
                result = this.readInt(args);
            }

            return result;
        }

        private bool checkInArray(int currentState, int[] myArray)
        {
            bool found = false;
            for (int i = 0; (!found
                        && (i < myArray.Length)); i++)
            {
                found = (myArray[i] == currentState);
            }

            return found;
        }


        static string ConvertStringArrayToString(string[] array)
        {
            //
            // Concatenate all the elements into a StringBuilder.
            //
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append(' ');
            }
            return builder.ToString();
        }//End convert string array to string


        /**
    * Use System.in.read to read the next character from the
    * STDIN stream.
    */
        private char nextChar()
        {
            int charAsInt = -1;
            try
            {
                charAsInt = Console.Read();

            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
                CoreLogger.Log(e.ToString());
                Console.WriteLine("Fatal error. Exiting program.");// "Fatal error. Exiting program.");
                return (char)charAsInt;
            }

            return (char)charAsInt;
        }//end nextChar
    }
}
