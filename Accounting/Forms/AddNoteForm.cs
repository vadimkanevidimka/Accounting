using Accounting.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Accounting.Models;
using System.Reflection;
using System.Windows.Forms;

namespace Accounting
{
    public partial class AddNoteForm : Form
    {
        private IEnumerable<PropertyInfo> properties;
        private List<Control> AddedControls = new List<Control>();
        internal AddNoteForm(Type type)
        {
            InitializeComponent();
            this.ControlAdded += AddNoteForm_ControlAdded;
            this.Text += " " + LangHelper.AccountingTypes[type.Name].ToLower();
            ConfigPage(type);
        }

        private void AddNoteForm_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control.GetType() != typeof(Label))
            {
                AddedControls.Add(e.Control);
            }
        }

        private void ConfigPage(Type type)
        {
            AddingElements(type);
        }

        private void AddingElements(Type type)
        {
            properties = Type.GetType(type.FullName, false, true).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Skip(1);
            Point pointlable = new Point(10, 30);
            foreach (var item in properties)
            {
                Label label = new Label() { Text = item.Name.Replace("_", " ") + $" ({LangHelper.LangTypeDictionary[item.PropertyType.Name]})", Location = new Point(pointlable.X, pointlable.Y), AutoSize = true };
                this.Controls.Add(label);
                Point point = new Point(pointlable.X + label.Width, pointlable.Y);
                switch (item.PropertyType.Name.ToString())
                {
                    case "String":
                        this.Controls.Add(new TextBox() {Location = point, Name = item.Name });
                        break;
                    case "DateTime":
                        this.Controls.Add(new DateTimePicker() { Location = point, Name = item.Name });
                        break;
                    case "Int32":
                        this.Controls.Add(new NumericUpDown() { Location = point, Name = item.Name, Maximum = int.MaxValue, Minimum = 0 });
                        break;
                    case "Int64":
                        this.Controls.Add(new NumericUpDown() { Location = point, Name = item.Name, Maximum = long.MaxValue, Minimum = 0 });
                        break;
                    case "Format":
                        ComboBox listbox = new ComboBox() { Location = point, Name = item.Name };
                        foreach (var format in Enum.GetValues(typeof(Format)))
                        {
                            listbox.Items.Add(format);
                        }
                        this.Controls.Add(listbox);
                        break;
                    default:
                        pointlable.Y = point.Y += 30;
                        break;
                }
                pointlable.Y = point.Y += 30;
            }
            this.Height = pointlable.Y + 100;
            AddedControlsAligment.AlignHorisontalByMaximum(AddedControls);
            this.Width = AddedControlsAligment.WhichControlAddedLargesize(AddedControls) + 30;
            button1.Location = new Point(this.Width / 2 - button1.Width / 2, this.Height - 40 - button1.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            object Data = null;
            foreach (var Prop in properties)
            {
                ConfigReflection(ref Data, Prop);
                foreach (Control item in this.Controls)
                {
                    if (item.Name == Prop.Name)
                    {
                        if (item.GetType() == typeof(TextBox))
                        {
                            Prop.SetValue(Data, item.Text);
                            break;
                        }
                        else if (item.GetType() == typeof(DateTimePicker))
                        {
                            DateTimePicker dateTime = item as DateTimePicker;
                            Prop.SetValue(Data, dateTime.Value);
                            break;
                        }
                        else if (item.GetType() == typeof(NumericUpDown))
                        {
                            NumericUpDown numericUp = item as NumericUpDown;
                            switch (Prop.PropertyType.Name)
                            {
                                case "Int16":
                                    Prop.SetValue(Data, (ushort)numericUp.Value);
                                    break;
                                case "Int32":
                                    Prop.SetValue(Data, (int)numericUp.Value);
                                    break;
                                case "Int64":
                                    Prop.SetValue(Data, (long)numericUp.Value);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }
                        else if (item.GetType() == typeof(ComboBox))
                        {
                            ComboBox format = item as ComboBox;
                            Prop.SetValue(Data, format.SelectedItem);
                            break;
                        }
                    }
                }
            }
            toDataBase(Data);
        }

        private void toDataBase(object Data)
        {
            if (Data is null)
            {
                throw new ArgumentNullException(nameof(Data));
            }
            using (DataBase_Context DB_Context = new DataBase_Context())
            {
                if (Data.GetType() == typeof(AccountingForNotifications_Model))
                {
                    DB_Context.Notifications_Models.Add((AccountingForNotifications_Model)Data);
                }
                else
                {
                    DB_Context.Originals_Models.Add((AccountingForOriginals_Model)Data);
                }
                DB_Context.SaveChanges();
            }
            this.Close();
        }

        private void ConfigReflection(ref object Data, PropertyInfo Prop)
        {
            if (Data is null)
            {
                Data = Prop.DeclaringType == typeof(AccountingForNotifications_Model) ? new AccountingForNotifications_Model() : new AccountingForOriginals_Model();
            }
        }
    }
}
