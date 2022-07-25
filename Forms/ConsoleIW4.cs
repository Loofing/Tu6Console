using loofer.Iw4;
using loofer.Utils;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using XDevkit;
using XDRPCLib;

namespace loofer.Forms
{
    public partial class ConsoleIW4 : Form
    {
        public static IXboxManager xbManager = null;
        public static IXboxConsole xbCon = null;
        public static bool activeConnection = false;
        public static uint xboxConnection = 0;
        private string debuggerName = null;
        private string userName = null;

        public static string weaponName;
        public static string hideTagsText;
        public static uint weaponDefPointer;
        public static uint variantDefPointer;
        public static uint tracerPointer;
        public static uint modelPointer;
        public static uint hideTagPointer;
        public static uint[] animationPointer = new uint[3];
        public static int testnum = 0;

        Iw4Structs.weaponVariantDef currentVariantDef = new Iw4Structs.weaponVariantDef();
        Iw4Structs.weaponDef currentWeaponDef = new Iw4Structs.weaponDef();
        Iw4Structs.Tracer currentTracer = new Iw4Structs.Tracer();
        /*
         * TODO: 
         * add ENUMS for stickiness and Tracer
         * game id detection
         * fix hardcoded materials (xasset calls) other languages then english can throw some errors right now
         * finish json preset loader
         */

        public ConsoleIW4()
        {
            InitializeComponent();

            DialogResult result = MessageBox.Show("Howdy there, fella' - you're not connected to any device!\ndo you want to connect now?", "Device connection", MessageBoxButtons.YesNo);

            if (result == System.Windows.Forms.DialogResult.Yes)
                connectConsole();

            AutoCompleteStringCollection weapon_list = new AutoCompleteStringCollection();
            weapon_list.AddRange(Enum.GetNames(typeof(Iw4Structs.weaponIndex)));
            weaponBox.AutoCompleteCustomSource = weapon_list;
            new externalConsole().Show();
        }
        private void displayTracerVariables<T>(ref T referenceStruct)
        {
            tracerGrid.Rows.Clear();
            foreach (var field in typeof(T).GetFields())
            {
                if(Type.Equals(field.FieldType,typeof(UInt32)) || field.FieldType.ToString().Contains("tracerColor"))
                    continue;

                Type valueType = field.FieldType;
                tracerGrid.Rows.Add(valueType.Name, field.Name, field.GetValue(referenceStruct));
            }
            pictureBox1.BackColor = Color.FromArgb((int)(currentTracer.color1.A * 255f), (int)(currentTracer.color1.R * 255f), (int)(currentTracer.color1.G * 255f), (int)(currentTracer.color1.B * 255f));
            pictureBox2.BackColor = Color.FromArgb((int)(currentTracer.color2.A * 255f), (int)(currentTracer.color2.R * 255f), (int)(currentTracer.color2.G * 255f), (int)(currentTracer.color2.B * 255f));
            pictureBox3.BackColor = Color.FromArgb((int)(currentTracer.color3.A * 255f), (int)(currentTracer.color3.R * 255f), (int)(currentTracer.color3.G * 255f), (int)(currentTracer.color3.B * 255f));
            pictureBox4.BackColor = Color.FromArgb((int)(currentTracer.color4.A * 255f), (int)(currentTracer.color4.R * 255f), (int)(currentTracer.color4.G * 255f), (int)(currentTracer.color4.B * 255f));
            pictureBox5.BackColor = Color.FromArgb((int)(currentTracer.color5.A * 255f), (int)(currentTracer.color5.R * 255f), (int)(currentTracer.color5.G * 255f), (int)(currentTracer.color5.B * 255f));
        }
        public void saveTracerVariables()
        {

            foreach (DataGridViewRow r in tracerGrid.Rows)
            {
                FieldInfo currentField = currentTracer.GetType().GetField(r.Cells[1].Value.ToString());

                if (currentField != null)
                    currentField.SetValueDirect(__makeref(currentTracer), Convert.ChangeType(r.Cells[2].Value, currentField.FieldType));
            }
            currentTracer.color1 = new Iw4Structs.tracerColor(iColor(pictureBox1.BackColor.R), iColor(pictureBox1.BackColor.G), iColor(pictureBox1.BackColor.B),iColor(pictureBox1.BackColor.A));
            currentTracer.color2 = new Iw4Structs.tracerColor(iColor(pictureBox2.BackColor.R), iColor(pictureBox2.BackColor.G), iColor(pictureBox2.BackColor.B), iColor(pictureBox2.BackColor.A));
            currentTracer.color3 = new Iw4Structs.tracerColor(iColor(pictureBox3.BackColor.R), iColor(pictureBox3.BackColor.G), iColor(pictureBox3.BackColor.B), iColor(pictureBox3.BackColor.A));
            currentTracer.color4 = new Iw4Structs.tracerColor(iColor(pictureBox4.BackColor.R), iColor(pictureBox4.BackColor.G), iColor(pictureBox4.BackColor.B), iColor(pictureBox4.BackColor.A));
            currentTracer.color5 = new Iw4Structs.tracerColor(iColor(pictureBox5.BackColor.R), iColor(pictureBox5.BackColor.G), iColor(pictureBox5.BackColor.B), iColor(pictureBox5.BackColor.A));
            xbCon.SetMemory(tracerPointer, MemoryManager.toBytes<Iw4Structs.Tracer>(currentTracer));
        }
        private float iColor(int value)
        {
            return (float)value / 255;
        }
        private void displayWeaponVariables<T>(ref T referenceStruct)
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Type.Equals(field.FieldType, typeof(System.UInt32)))
                        continue;

                Type valueType = field.FieldType;
                weaponGrid.Rows.Add(valueType.Name, field.Name, field.GetValue(referenceStruct));

                if (Type.Equals(valueType, typeof(System.Boolean)))
                {
                    DataGridViewCheckBoxCell checkboxCell = new DataGridViewCheckBoxCell()
                    {
                        TrueValue = "True",
                        FalseValue = "False",
                        Value = (bool)field.GetValue(referenceStruct),
                    };
                    weaponGrid.Rows[weaponGrid.Rows.Count - 1].Cells[2] = checkboxCell;
                }
                if (valueType.ToString().Contains("_t"))
                {
                    DataGridViewComboBoxCell comboCell = new DataGridViewComboBoxCell();
                    comboCell.DataSource = Enum.GetNames(valueType).ToList();
                    weaponGrid.Rows[weaponGrid.Rows.Count - 1].Cells[2] = comboCell;
                    weaponGrid.Rows[weaponGrid.Rows.Count - 1].Cells[0].Value = "Enum";
                    comboCell.Value = Enum.GetName(valueType, field.GetValue(referenceStruct));
                }
                if (!isApproved(weaponGrid.Rows[weaponGrid.Rows.Count - 1].Cells[0].Value.ToString(), field.Name))
                    weaponGrid.Rows[weaponGrid.Rows.Count - 1].Visible = false;
            }
        }
        public void saveWeaponVariables()
        {
            if (isConnected() && !string.IsNullOrEmpty(weaponName))
            {
                foreach (DataGridViewRow r in weaponGrid.Rows)
                {
                    var newValue = r.Cells[2].Value;
                    FieldInfo field = currentWeaponDef.GetType().GetField(r.Cells[1].Value.ToString());

                    if (field == null)
                    {
                        field = currentVariantDef.GetType().GetField(r.Cells[1].Value.ToString());

                        if (field != null)
                        {
                            if (field.ToString().Contains("_t"))
                                field.SetValueDirect(__makeref(currentVariantDef), Convert.ChangeType(Enum.Parse(field.FieldType, (string)newValue), Type.GetType(field.FieldType.ToString())));
                            else
                                field.SetValueDirect(__makeref(currentVariantDef), Convert.ChangeType(newValue, field.FieldType));

                            continue;
                        }
                        Console.WriteLine("skipping struct field");
                        continue;
                    }
                    if (field.ToString().Contains("_t"))
                        field.SetValueDirect(__makeref(currentWeaponDef), Convert.ChangeType(Enum.Parse(field.FieldType, (string)newValue), Type.GetType(field.FieldType.ToString())));
                    else
                        field.SetValueDirect(__makeref(currentWeaponDef), Convert.ChangeType(newValue, field.FieldType));
                }
                xbCon.SetMemory(variantDefPointer, MemoryManager.toBytes<Iw4Structs.weaponVariantDef>(currentVariantDef));
                xbCon.SetMemory(weaponDefPointer, MemoryManager.toBytes<Iw4Structs.weaponDef>(currentWeaponDef));
            }
        }
        private void buttInformation_Click(object sender, EventArgs e)
        {
            if (isConnected())
            {
                if (!Utility.isValidWeapon(weaponBox.Text) || string.IsNullOrEmpty(weaponBox.Text))
                {
                    MessageBox.Show("Please enter a valid weapon");
                    return;
                }
                XAnim.dataLoaded = new bool[] { false, false, false };
                weaponGrid.Rows.Clear();
                currentVariantDef = (Iw4Structs.weaponVariantDef)Iw4Functions._GetWeaponVariantDef(weaponName = weaponBox.Text, out variantDefPointer);
                currentWeaponDef = (Iw4Structs.weaponDef)Iw4Functions._GetWeaponDef(weaponDefPointer = currentVariantDef.weaponDef);
                currentTracer = (Iw4Structs.Tracer)Iw4Functions._GetTracer(tracerPointer = currentWeaponDef.tracer);
                modelPointer = currentWeaponDef.gunXModel;
                hideTagPointer = currentVariantDef.hideTags;
                animationPointer = new uint[] { currentVariantDef.szXAnims, currentWeaponDef.szXAnimsR, currentWeaponDef.szXAnimsL };
                displayWeaponVariables(ref currentVariantDef);
                displayWeaponVariables(ref currentWeaponDef);
                displayTracerVariables(ref currentTracer);
                XAnim.displayAnimations(ref animationGrid);
                hideTags.displayHideTags(ref hideBox);
                XModel.displayModels(ref modelGrid, currentWeaponDef.knifeModel,currentWeaponDef.rocketModel,currentWeaponDef.projectileModel);
            }
        }
        private void buttSave_Click(object sender, EventArgs e)
        {
            if (isConnected() && !string.IsNullOrEmpty(weaponName))
            {
                saveWeaponVariables();
                saveTracerVariables();
                XAnim.saveAnimations(ref animationGrid);
                XModel.saveModels(ref modelGrid);
                hideTags.saveHideTags(ref hideBox);
            }
        }
        private void ConsoleIW4_FormClosing(object sender, FormClosingEventArgs e) => Application.Exit();
        private void externalConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<externalConsole>().Any())
            {
                Form consoleWindow = Application.OpenForms["externalConsole"];

                if (consoleWindow.WindowState != FormWindowState.Normal)
                    consoleWindow.WindowState = FormWindowState.Normal;

                consoleWindow.BringToFront();
                return;
            }
            new externalConsole().Show();
        }
        public static bool isConnected()
        {
            if (xbCon is null)
            {
                MessageBox.Show("Not connected to any device!");
                return false;
            }
            return true;
        }
        private bool connectConsole()
        {
            if (!activeConnection)
            {
                xbManager = new XboxManager();
                xbCon = xbManager.OpenConsole(xbManager.DefaultConsole);

                try
                {
                    xboxConnection = xbCon.OpenConnection(null);
                }
                catch (Exception)
                {
                    MessageBox.Show(this, "Could not connect to console: " + xbManager.DefaultConsole);
                    return false;
                }
                if (xbCon.DebugTarget.IsDebuggerConnected(out debuggerName, out userName))
                {
                    activeConnection = true;
                    MessageBox.Show(this, "Connection to " + xbCon.Name + " established!");
                    return true;
                }
                else
                {
                    xbCon.DebugTarget.ConnectAsDebugger("Xbox Toolbox", XboxDebugConnectFlags.Force);
                    if (!xbCon.DebugTarget.IsDebuggerConnected(out debuggerName, out userName))
                    {
                        MessageBox.Show(this, "Attempted to connect to console: " + xbCon.Name + " but failed");
                        return false;
                    }
                    else
                    {
                        activeConnection = true;
                        MessageBox.Show(this, "Connection to " + xbCon.Name + " established!");
                        return true;
                    }
                }
            }
            else if (xbCon.DebugTarget.IsDebuggerConnected(out debuggerName, out userName))
            {
                MessageBox.Show(this, "Connection to " + xbCon.Name + " already established!");
                return true;
            }
            else
            {
                activeConnection = false;
                return connectConsole();
            }
        }
        public static void msgBox(string text)
        {
            MessageBox.Show(text);
        }
        private bool validCheck(string systemType, object new_value, string name)
        {
            if (systemType == "Enum" || systemType == "Boolean")
                return true;

            if (!Utility.IsNumeric(new_value))
            {
                MessageBox.Show("Please enter a numeric value. @" + name);
                return false;
            }

            switch (systemType)
            {
                case "Single":
                    try { Convert.ToSingle(new_value); }
                    catch
                    {
                        MessageBox.Show("Value exceeds range of float\n" + float.MinValue + " ~ " + float.MaxValue + "\n@" + name);
                        return false;
                    }
                    return true;
                case "Int32":
                    try { Convert.ToInt32(new_value); }
                    catch
                    {
                        MessageBox.Show("Value exceeds range of Int32\n" + int.MinValue + " ~ " + int.MaxValue + "\n@" + name);
                        return false;
                    }
                    return true;
                case "Int16":
                    try { Convert.ToInt16(new_value); }
                    catch
                    {
                        MessageBox.Show("Value exceeds range of Int16\n" + short.MinValue + " ~ " + short.MaxValue + "\n@" + name);
                        return false;
                    }
                    return true;
                default:
                    return false;
            }
        }
        private void weaponGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (weaponGrid.IsCurrentCellDirty)
            {
                var fieldName = weaponGrid.Rows[e.RowIndex].Cells[1].Value;
                var newValue = e.FormattedValue;
                var typeName = weaponGrid.Rows[e.RowIndex].Cells[0].Value;

                if (!validCheck(typeName.ToString(), newValue, fieldName.ToString()))
                {
                    weaponGrid.CancelEdit();
                    e.Cancel = true;
                    return;
                }
            }
        }
        private bool isApproved(string typeName, string fieldName)
        {
            bool stringMatch;
            bool result = false;

            if (!string.IsNullOrEmpty(searchBox.Text))
                stringMatch = Utility.iCompare(fieldName, searchBox.Text);
            else
                stringMatch = true;

            switch (typeName)
            {
                case "Enum":
                    result = enumCheck.Checked && stringMatch;
                    break;
                case "Int32":
                    result = int32Check.Checked && stringMatch;
                    break;
                case "Int16":
                    result = int16Check.Checked && stringMatch;
                    break;
                case "Single":
                    result = floatCheck.Checked && stringMatch;
                    break;
                case "Boolean":
                    result = boolCheck.Checked && stringMatch;
                    break;
            }
            return result;
        }
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(weaponName))
                return;

            foreach (DataGridViewRow dataRow in weaponGrid.Rows)
                dataRow.Visible = isApproved(dataRow.Cells[0].Value.ToString(), dataRow.Cells[1].Value.ToString());
        }
        private void defaultButton_Click(object sender, EventArgs e)
        {
            animLabel.Text = defaultButton.Text + " Animations";
            XAnim.displayAnimations(ref animationGrid, 0);
        }
        private void rightButton_Click(object sender, EventArgs e)
        {
            animLabel.Text = rightButton.Text + " Animations";
            XAnim.displayAnimations(ref animationGrid, 1);
        }
        private void leftButton_Click(object sender, EventArgs e)
        {
            animLabel.Text = leftButton.Text + " Animations";
            XAnim.displayAnimations(ref animationGrid, 2);
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorPicker = new ColorDialog())
            {
                if (colorPicker.ShowDialog() == DialogResult.OK)
                    pictureBox1.BackColor = colorPicker.Color;
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorPicker = new ColorDialog())
            {
                if (colorPicker.ShowDialog() == DialogResult.OK)
                    pictureBox2.BackColor = colorPicker.Color;
            }
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorPicker = new ColorDialog())
            {
                if (colorPicker.ShowDialog() == DialogResult.OK)
                    pictureBox3.BackColor = colorPicker.Color;
            }
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorPicker = new ColorDialog())
            { 
                if (colorPicker.ShowDialog() == DialogResult.OK)
                    pictureBox4.BackColor = colorPicker.Color;
            }
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorPicker = new ColorDialog())
            {
                if (colorPicker.ShowDialog() == DialogResult.OK)
                    pictureBox5.BackColor = colorPicker.Color;
            }
        }
        private void tracerGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (tracerGrid.IsCurrentCellDirty)
            {
                var fieldName = tracerGrid.Rows[e.RowIndex].Cells[1].Value;
                var typeName = tracerGrid.Rows[e.RowIndex].Cells[0].Value;
                var newValue = e.FormattedValue;

                if (!validCheck(typeName.ToString(), newValue, fieldName.ToString()))
                {
                    tracerGrid.CancelEdit();
                    e.Cancel = true;
                    return;
                }
            }
        }
        private void hideBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Utility.RemoveWhitespace(hideBox.Text);
        }
        private void loadPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<PresetLoader>().Any())
            {
                Form presetWindow = Application.OpenForms["PresetLoader"];

                if (presetWindow.WindowState != FormWindowState.Normal)
                    presetWindow.WindowState = FormWindowState.Normal;

                presetWindow.BringToFront();
                return;
            }
            new PresetLoader().Show();
        }
        private void peekPokerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + @"\pp/PeekPoker.exe");
        }
        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
