using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class PhoneBookRespository
    {

        private readonly static string filePath = @"D:\data.json";
        public static List<Contact> _contacts;
        private static List<Contact> Contacts
        {
            get
            {
                try
                {
                    if (_contacts == null)
                    {
                        var fileString = System.IO.File.ReadAllText(filePath);
                        _contacts = JsonConvert.DeserializeObject<List<Contact>>(fileString);
                        if (_contacts == null)
                        {
                            _contacts = new List<Contact>();
                        }
                    }
                }
                catch (Exception)
                {
                    _contacts = new List<Contact>();
                }

                return _contacts;
            }

            set { _contacts = value; }
        }

        public PhoneBookRespository()
        {
            if (!System.IO.File.Exists(filePath))
            {
                var file = System.IO.File.Create(filePath);
                file.Close();
            }
        }

        public List<Contact> GetContacts()
        {
            try
            {
                bool hasInvalidId = false;
                foreach (var contact in Contacts)
                {
                    if (contact.Id == null || contact.Id == Guid.Empty)
                    {
                        hasInvalidId = true;
                        contact.Id = Guid.NewGuid();
                    }
                }
                if (hasInvalidId)
                {
                    SaveContact(Contacts);
                }
            }
            catch (Exception)
            {

            }
            return Contacts;
        }

        public Contact GetContactById(string id)
        {
            var contact = Contacts.FirstOrDefault(x => x.Id.ToString() == id.ToString());
            return contact;
        }

        public bool UpdateContact(Contact model)
        {
            var contact = Contacts.FirstOrDefault(x => x.Id.ToString() == model.Id.ToString());
            if (contact == null)
            {
                return false;
            }
            else
            {
                Contacts.Remove(contact);
                SaveContact(model);
                return true;
            }
        }

        public bool SaveContact(List<Contact> model)
        {
            try
            {
                var stringModel = JsonConvert.SerializeObject(model);
                System.IO.File.WriteAllText(filePath, stringModel);
                Contacts = model;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveContact(Contact model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Firstname + model.Lastname) || string.IsNullOrWhiteSpace(model.PhoneNumber))
                { 
                    return false; 
                }
                Contacts.Add(model);
                SaveContact(Contacts);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
