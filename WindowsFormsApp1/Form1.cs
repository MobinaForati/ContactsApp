using Models;

using Service;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        PhoneBookService _service = new PhoneBookService();

        string selectedId = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_firstname.Text + txt_lastname) || string.IsNullOrWhiteSpace(txt_phoneNumber.Text))
            {
                MessageBox.Show("فیلد های خالی را پر کنید.");
                return;
            }
            var contact = new Contact();
            contact.Firstname = txt_firstname.Text;
            contact.Lastname = txt_lastname.Text;
            contact.PhoneNumber = txt_phoneNumber.Text;
            contact.Id = string.IsNullOrEmpty(selectedId) ? Guid.Empty : Guid.Parse(selectedId);
            selectedId = "";
            var saveResult = _service.SaveContact(contact);
            if (saveResult)
            {
                var contacts = _service.GetContacts();
                FillGridView(contacts);
            }
            clearForm();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var contacts = _service.GetContacts();
            FillGridView(contacts);
        }

        public void FillGridView(List<Contact> model)
        {
            grd_contacts.Rows.Clear();
            foreach (Contact contact in model)
            {
                grd_contacts.Rows.Add(contact.Id, contact.Firstname, contact.Lastname, contact.PhoneNumber);
            }
        }

        public void clearForm()
        {
            txt_firstname.Text = "";
            txt_lastname.Text = "";
            txt_phoneNumber.Text = "";
        }

        private void grd_contacts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var index = grd_contacts.CurrentRow.Index;
                var id = grd_contacts.Rows[index].Cells[0].Value.ToString();
                var contactForEdit = _service.GetContactById(id);
                selectedId = id;

                txt_firstname.Text = contactForEdit.Firstname;
                txt_lastname.Text = contactForEdit.Lastname;
                txt_phoneNumber.Text = contactForEdit.PhoneNumber;
            }
            catch (Exception)
            {

            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            clearForm();
            selectedId = "";
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedId))
            {
                var deleteResult = _service.DeleteContact(selectedId);
                var contacts = _service.GetContacts();
                FillGridView(contacts);
                clearForm();
                selectedId = "";
            }
        }
    }
}
