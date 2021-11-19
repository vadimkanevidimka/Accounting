using Accounting.Models;
using Accounting.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.Forms
{
    public partial class EditNoteForm : Form
    {
        private IEnumerable<PropertyInfo> properties;
        private IAccountingModel Model;
        private List<Control> AddedControls = new List<Control>();
        internal EditNoteForm(IAccountingModel Model)
        {
            if (Model is null) throw new ArgumentNullException(nameof(Model));
            this.Model = Model;
            this.Text += " " + LangHelper.AccountingTypes[Model.GetType().Name].ToLower();
            InitializeComponent();
            this.ControlAdded += EditNoteForm_ControlAdded;
            ConfigPage();
        }

        private void EditNoteForm_ControlAdded(object sender, ControlEventArgs e)
        {
            if(e.Control.GetType() != typeof(Label))
            {
                AddedControls.Add(e.Control);
            }
        }

        private void ConfigPage()
        {
            AddingElements();
        }

        private void AddingElements()
        {
            properties = Model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Skip(1);
            Point pointlable = new Point(10, 30);
            foreach (var item in properties)
            {
                Label label = new Label() { Text = item.Name.Replace("_", " ") + $" ({LangHelper.LangTypeDictionary[item.PropertyType.Name]})", Location = new Point(pointlable.X, pointlable.Y), AutoSize = true };
                this.Controls.Add(label);
                Point point = new Point(pointlable.X + label.Width, pointlable.Y);
                switch (item.PropertyType.Name.ToString())
                {
                    case "String":
                        this.Controls.Add(new TextBox() { Location = point, Name = item.Name, Text = item.GetValue(Model).ToString() });
                        break;
                    case "DateTime":
                        this.Controls.Add(new DateTimePicker() { Location = point, Name = item.Name, Value = Convert.ToDateTime(item.GetValue(Model)) });
                        break;
                    case "Int32":
                        this.Controls.Add(new NumericUpDown() { Location = point, Name = item.Name, Maximum = int.MaxValue, Minimum = 0,  Value = Convert.ToInt32(item.GetValue(Model)) });
                        break;
                    case "Int64":
                        this.Controls.Add(new NumericUpDown() { Location = point, Name = item.Name, Maximum = long.MaxValue, Minimum = 0, Value = Convert.ToInt64(item.GetValue(Model))});
                        break;
                    case "Format":
                        ComboBox listbox = new ComboBox() { Location = point, Name = item.Name };
                        foreach (var format in Enum.GetValues(typeof(Format)))
                        {
                            listbox.Items.Add(format);
                        }
                        listbox.SelectedItem = item.GetValue(Model);
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
                    DB_Context.Notifications_Models.Update((AccountingForNotifications_Model)Data);
                }
                else
                {
                    DB_Context.Originals_Models.Update((AccountingForOriginals_Model)Data);
                }
                DB_Context.SaveChanges();
            }
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            foreach (var Prop in properties)
            {
                foreach (Control item in this.Controls)
                {
                    if (item.Name == Prop.Name)
                    {
                        if (item.GetType() == typeof(TextBox))
                        {
                            Prop.SetValue(Model, item.Text);
                            break;
                        }
                        else if (item.GetType() == typeof(DateTimePicker))
                        {
                            DateTimePicker dateTime = item as DateTimePicker;
                            Prop.SetValue(Model, dateTime.Value);
                            break;
                        }
                        else if (item.GetType() == typeof(NumericUpDown))
                        {
                            NumericUpDown numericUp = item as NumericUpDown;
                            switch (Prop.PropertyType.Name)
                            {
                                case "Int16":
                                    Prop.SetValue(Model, (ushort)numericUp.Value);
                                    break;
                                case "Int32":
                                    Prop.SetValue(Model, (int)numericUp.Value);
                                    break;
                                case "Int64":
                                    Prop.SetValue(Model, (long)numericUp.Value);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }
                        else if (item.GetType() == typeof(ComboBox))
                        {
                            ComboBox format = item as ComboBox;
                            Prop.SetValue(Model, format.SelectedItem);
                            break;
                        }
                    }
                }
            }
            toDataBase(Model);
        }
    }
}
