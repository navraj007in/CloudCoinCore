using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudCoinCore;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WindowsFormsApp1
{
    public class Importer : IImporter
    {
        RichTextBox txtLogs;
        public Importer(IFileUtils fileUtils,RichTextBox txtLogs) : base(fileUtils)
        {
            this.txtLogs = txtLogs;
        }

        public override bool importAll()
        {
            var ext = new List<string> { ".jpg", ".stack", ".jpeg" };
            var fnamesRaw = Directory.GetFiles(this.fileUtils.importFolder, "*.*", SearchOption.TopDirectoryOnly).Where(s => ext.Contains(Path.GetExtension(s)));
            string[] fnames = new string[fnamesRaw.Count()];
            for (int i = 0; i < fnamesRaw.Count(); i++)
            {
                fnames[i] = Path.GetFileName(fnamesRaw.ElementAt(i));
            };

            //String[] fnames = new DirectoryInfo(this.fileUtils.importFolder).GetFiles().Select(o => o.Name).ToArray();//Get a list of all in the folder except the directory "imported"

            if (fnames.Length == 0)//   Console.Out.WriteLine("There were no CloudCoins to import. Please place our CloudCoin .jpg and .stack files in your imports" + " folder at " + this.fileUtils.importFolder );
            {
                return false;
            }
            else
            {
                //  Console.ForegroundColor = ConsoleColor.Green;
                //  Console.Out.WriteLine("Importing the following files: ");
                //CoreLogger.Log("Importing the following files: ");
                //  Console.ForegroundColor = ConsoleColor.White;
                for (int i = 0; i < fnames.Length; i++)// Loop through each file. 
                {
                    Console.Out.WriteLine(fnames[i]);
                    //  CoreLogger.Log(fnames[i]);
                    this.importOneFile(fnames[i]);
                } // end for each file name
                return true;
            }//end if no files         }
        }

        public override void moveFile(string fromPath, string tooPath)
        {
            throw new NotImplementedException();
        }

        protected override bool importJPEG(string fileName)
        {
            throw new NotImplementedException();
        }

        protected override bool importOneFile(string fname)
        {
            String extension = "";
            int indx = fname.LastIndexOf('.');//Get file extension
            if (indx > 0)
            {
                extension = fname.Substring(indx + 1);
            }

            extension = extension.ToLower();
            if (extension == "jpeg" || extension == "jpg")//Run if file is a jpeg
            {
                if (!this.importJPEG(fname))
                {
                    if (!File.Exists(this.fileUtils.trashFolder + fname))
                    {
                        File.Move(this.fileUtils.importFolder + fname, this.fileUtils.trashFolder + fname);
                        Console.Out.WriteLine("File moved to trash: " + fname);
                        txtLogs.AppendText("File moved to trash: " + fname + Environment.NewLine);
                    }
                    else
                    {
                        File.Delete(this.fileUtils.importedFolder + fname);
                        File.Move(this.fileUtils.importFolder + fname, this.fileUtils.importedFolder + fname);
                    }

                    return false;//"Failed to load JPEG file");
                }//end if import fails
            }//end if jpeg
            /*
            else if (extension == "chest" || extension == ".chest")//Run if file is a jpeg
            {
                if (!this.importChest(fname))
                {
                    if (!File.Exists(this.fileUtils.trashFolder + fname))
                    {
                        File.Move(this.fileUtils.importFolder + fname, this.fileUtils.trashFolder + fname);
                        Console.Out.WriteLine("File moved to trash: " +  fname);
                    }
                    return false;//"Failed to load JPEG file");
                }//end if import fails
            }//end if jpeg
            */
            else if (!this.importStack(fname))// run if file is a stack
            {
                if (!File.Exists(this.fileUtils.trashFolder + fname))
                {
                    File.Move(this.fileUtils.importFolder + fname, this.fileUtils.trashFolder + fname);
                    Console.Out.WriteLine("File moved to trash: " + fname);
                    txtLogs.AppendText("File moved to trash: " + fname);
                }
                return false;//"Failed to load .stack file");
            }

            if (!File.Exists(this.fileUtils.importedFolder + fname))
            {
                File.Move(this.fileUtils.importFolder + fname, this.fileUtils.importedFolder + fname);
            }
            else
            {
                File.Delete(this.fileUtils.importedFolder + fname);
                File.Move(this.fileUtils.importFolder + fname, this.fileUtils.importedFolder + fname);
            }

            //End if the file is there
            return true;
        }

        protected override bool importStack(string fileName)
        {
            bool isSuccessful = false;
            //  System.out.println("Trying to load: " + importFolder + fileName );
            try
            {
                String incomeJson = fileUtils.importJSON(this.fileUtils.importFolder + fileName);//Load file as JSON .stack or .chest
                Stack tempCoins = null;
                if (seemsValidJSON(incomeJson))
                {
                    try
                    {
                        tempCoins = this.fileUtils.loadManyCloudCoinFromJsonFile(this.fileUtils.importFolder + fileName, incomeJson);
                    }
                    catch (JsonReaderException e)
                    {
                        //Console.WriteLine("Moving corrupted file to trash: " + fileName);
                        Console.WriteLine("Error reading " + fileName + ". Moving to trash.");
                        txtLogs.AppendText("Error reading " + fileName + ". Moving to trash.");
                        Console.WriteLine(e);
                        txtLogs.AppendText(e.ToString());
                        moveFile(this.fileUtils.importFolder + fileName, this.fileUtils.trashFolder + fileName);
                    }//end catch json error
                }
                else
                {
                    //Console.WriteLine("Moving corrupted file to trash: " + fileName);
                    moveFile(this.fileUtils.importFolder + fileName, this.fileUtils.trashFolder + fileName);
                }

                if (tempCoins == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Out.WriteLine("  The following file does not appear to be valid JSON. It will be moved to the Trash Folder: ");
                    Console.Out.WriteLine(fileName);
                    txtLogs.AppendText("  The following file does not appear to be valid JSON. It will be moved to the Trash Folder: ");

                    Console.Out.WriteLine("  Paste the text into http://jsonlint.com/ to check for validity.");
                    Console.ForegroundColor = ConsoleColor.White;
                    return false;//CloudCoin was null so move to trash
                }
                else
                {
                    for (int i = 0; i < tempCoins.cc.Length; i++)
                    {
                        this.fileUtils.writeTo(this.fileUtils.suspectFolder, tempCoins.cc[i]);
                    }//end for each temp Coin
                    return true;
                }//end if no coins. 
            }
            catch (FileNotFoundException ex)
            {
                Console.Out.WriteLine("File not found: " + fileName + ex);
                txtLogs.AppendText("File not found: " + fileName + ex);
            }
            catch (IOException ioex)
            {
                Console.Out.WriteLine("IO Exception:" + fileName + ioex);
                txtLogs.AppendText("File not found: " + fileName + ioex);

            }

            // end try catch
            return isSuccessful;
        }

        protected override bool seemsValidJSON(string json)
        {
            if (json.Count(f => f == '{') != json.Count(f => f == '}'))
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("The stack file did not have a matching number of { }. There were " + json.Count(f => f == '{') + " {, and " + json.Count(f => f == '}') + " }");
                txtLogs.AppendText("The stack file did not have a matching number of { }. There were " + json.Count(f => f == '{') + " {, and " + json.Count(f => f == '}') + " }");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }//Check if number of currly brackets open are the same as closed
            if (json.Count(f => f == '[') != json.Count(f => f == ']'))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("The stack file did not have a matching number of []. There were " + json.Count(f => f == '[') + " [, and " + json.Count(f => f == ']') + " ]");
                txtLogs.AppendText("The stack file did not have a matching number of []. There were " + json.Count(f => f == '[') + " [, and " + json.Count(f => f == ']') + " ]");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }//Check if number of  brackets open are the same as closed
            if (IsOdd(json.Count(f => f == '\"')))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("The stack file did not have a matching number of double quotations");
                txtLogs.AppendText("The stack file did not have a matching number of double quotations");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }//Check if number of
            return true;
        }
    }
}
