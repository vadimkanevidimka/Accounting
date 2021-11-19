using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Accounting.Forms;
using Accounting.Models;
using Microsoft.EntityFrameworkCore;

namespace Accounting
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            using (var DataBase = new DataBase_Context())
            {
                DataBase.Database.Migrate();
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ConfigDataTable();
            dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(this.OnCellValueChanged);
            dataGridView2.CellEndEdit += new DataGridViewCellEventHandler(this.OnCellValueChanged);
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            dataGridView2.ContextMenuStrip = contextMenuStrip1;
        }

        private void добавитьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (Учет.SelectedIndex)
            {
                case 0:
                    new AddNoteForm(typeof(AccountingForOriginals_Model)).ShowDialog();
                    break;
                case 1:
                    new AddNoteForm(typeof(AccountingForNotifications_Model)).ShowDialog();
                    break;
                default:
                    break;
            }
            ConfigDataTable();
        }

        private async void ConfigDataTable()
        {
            try
            {
                using (DataTable table1 = new DataTable())
                {
                    DataTable table2 = new DataTable();
                    using (DataBase_Context db = new DataBase_Context())
                    {
                        table1.ConfigDataTable(typeof(AccountingForOriginals_Model));
                        await table1.FillDataAsync(db.Originals_Models.ToList());
                        table2.ConfigDataTable(typeof(AccountingForNotifications_Model));
                        await table2.FillDataAsync(db.Notifications_Models.ToList());
                    }
                    dataGridView1.DataSource = table1;
                    dataGridView2.DataSource = table2;
                    NotesCount.Text = $"Подлинники: {table1.Rows.Count} | Извещения: {table2.Rows.Count}";
                }
            }
            catch (Exception)
            {

            }
        }

        private void удалитьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Delete();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            using (DataBase_Context dataBase_Context = new DataBase_Context())
            {
                TextBox FilterText = sender as TextBox;
                if (FilterText.Text.Length == 0)
                {
                    ConfigDataTable();
                }

                using (DataTable table = new DataTable())
                {
                    switch (Учет.SelectedIndex)
                    {
                        case 0: table.FillDataByFilter(dataBase_Context.Originals_Models.ToList(), FilterText.Text);
                            dataGridView1.DataSource = table;
                            break;
                        case 1: table.FillDataByFilter(dataBase_Context.Notifications_Models.ToList(), FilterText.Text);
                            dataGridView2.DataSource = table;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        
        private void OnCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                object obj = null;
                using (DataBase_Context dataBase_Context = new DataBase_Context())
                {
                    switch (Учет.SelectedIndex)
                    {
                        case 0:
                            obj = dataBase_Context.Originals_Models.Where((c) => c.Id == (long)dataGridView1.Rows[e.RowIndex].Cells[0].Value).First();
                            PropertyInfo property = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)[e.ColumnIndex];
                            property.SetValue(obj, Convert.ChangeType(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value, Type.GetTypeCode(property.PropertyType)));
                            break;
                        case 1:
                            obj = dataBase_Context.Notifications_Models.Where((c) => c.Id == (long)dataGridView2.Rows[e.RowIndex].Cells[0].Value).First();
                            PropertyInfo propertynotif = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)[e.ColumnIndex];
                            propertynotif.SetValue(obj, Convert.ChangeType(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value, Type.GetTypeCode(propertynotif.PropertyType)));
                            break;
                        default:
                            break;
                    }
                    dataBase_Context.Update(obj);
                    dataBase_Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ConfigDataTable();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ConfigDataTable();
            }
        }

        private void Учет_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigDataTable();
        }

        private void Add_Original_Click(object sender, EventArgs e)
        {
            new AddNoteForm(typeof(AccountingForOriginals_Model)).ShowDialog();
            ConfigDataTable();
        }

        private void Add_notification_Click(object sender, EventArgs e)
        {
            new AddNoteForm(typeof(AccountingForNotifications_Model)).ShowDialog();
            ConfigDataTable();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Delete();
            }
        }

        private void Delete()
        {
            using (DataBase_Context db = new DataBase_Context())
            {
                if (Учет.SelectedIndex == 0)
                {
                    var t = from r in db.Originals_Models.ToList()
                            where r.Id == Convert.ToInt32(dataGridView1.SelectedCells[0].Value)
                            select r;
                    db.RemoveRange(t);
                }
                else
                {
                    var t = from r in db.Notifications_Models.ToList()
                            where r.Id == Convert.ToInt32(dataGridView2.SelectedCells[0].Value)
                            select r;
                    db.RemoveRange(t);
                }
                db.SaveChanges();
            }
            ConfigDataTable();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (MessageBox.Show("Вы действительно хотите выйти из программы?", "Выход", MessageBoxButtons.YesNo) == DialogResult.Yes) ? false : true;
        }

        private void редактироватьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (DataBase_Context db = new DataBase_Context())
            {
                if (Учет.SelectedIndex == 0)
                {
                    var t = from r in db.Originals_Models.ToList()
                            where r.Id == Convert.ToInt32(dataGridView1.SelectedCells[0].Value)
                            select r;
                    new EditNoteForm(t.First()).ShowDialog();
                }
                else
                {
                    var t = from r in db.Notifications_Models.ToList()
                            where r.Id == Convert.ToInt32(dataGridView2.SelectedCells[0].Value)
                            select r;
                    new EditNoteForm(t.First()).ShowDialog();
                }
                db.SaveChanges();
            }
            ConfigDataTable();
        }
    }
}
