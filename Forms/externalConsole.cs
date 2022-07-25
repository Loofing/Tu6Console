using loofer.Iw4;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace loofer.Forms
{
    public partial class externalConsole : Form
    {
        /*not even close to be done*/
        List<customCommand> commandList = new List<customCommand>();
        public bool dvarBool;
        public externalConsole()
        {
            InitializeComponent();
            welcomeText();
            /*custom command code is weird but it works fine for now
             * -TODO: 
             * only supports on/off toggles for now
             * maybe pass params through object[] in the struct
             * fix the <Func> params
             * dvar autofills(also add customcommands)
             */
            addCustomCommand("clear", "Clears the console window", clearConsoleWindow, false);
            addCustomCommand("help", "Displays all custom commands", helpfunc, false);

        }
        private void consoleWindow_TextChanged(object sender, EventArgs e)
        {
            consoleWindow.ScrollToCaret();
        }
        private void displayInput(string input) => consoleWindow.AppendText("\n] " + input); 
        private void consoleLine_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(consoleLine.Text) && ConsoleIW4.isConnected())
                {
                    string input = consoleLine.Text;
                    string[] splitString = input.Split();
                    consoleLine.Text = null;
                    displayInput(input);

                    foreach (customCommand a in commandList)
                    {
                        if(splitString[0].ToLower().Equals(a.name.ToLower()))
                        {
                            if (a.parameters)
                            {
                                if (splitString.Length != 2)
                                {
                                    displayInput("^Parameter missmatch! 0 - OFF | 1 - ON ");
                                    return;
                                }
                                int dvar_value;
                                if (int.TryParse(splitString[1], out dvar_value) && dvar_value == 0 || dvar_value == 1)
                                {
                                    dvarBool = Convert.ToBoolean(dvar_value);
                                    a.func();
                                    return;
                                }
                                displayInput("^Parameter missmatch! 0 - OFF | 1 - ON ");
                                return;
                            }
                            else
                            {
                                a.func();
                                return;
                            }
                        }
                    }
                    Iw4Functions.Cbuf(input);
                }
            }
        }
        private void welcomeText()
        {
            consoleWindow.Text = "*----------------------------------------------------*\n";
            consoleWindow.AppendText("|  IW4 Console for Title Update 6                    |\n");
            consoleWindow.AppendText("|  https://www.youtube.com/c/loofin/                 |\n");
            consoleWindow.AppendText("|  https://twitter.com/2loof                         |\n");
            consoleWindow.AppendText("|  Type 'help' to see all custom commands.           |\n");
            consoleWindow.AppendText("*----------------------------------------------------*\n");
        }
        private void addCustomCommand(string commandName, string commandDesc, Func<bool> func, bool par) => commandList.Add(new customCommand(commandName, commandDesc, func, par));
        public struct customCommand
        {
            public string name;
            public string desc;
            public Func<bool> func;
            public bool parameters;

            public customCommand(string name, string desc, Func<bool> func, bool par = false)
            {
                this.name = name;
                this.desc = desc;
                this.func = func;
                this.parameters = par;
            }
        }
        public bool clearConsoleWindow()
        {
            consoleWindow.Clear();
            welcomeText();
            return true;
        }
        public bool helpfunc()
        {
            consoleWindow.AppendText("\n*----------------------------------------------------*\n");

            foreach (customCommand a in commandList)
                consoleWindow.AppendText("-  " + a.name + " | " + a.desc + "\n");

            consoleWindow.AppendText ("*----------------------------------------------------*\n");
            return true;
        }
    }
}
