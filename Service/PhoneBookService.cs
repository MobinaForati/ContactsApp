using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Service
{
    public class PhoneBookService
    {
        private PhoneBookRespository _repo;
        public PhoneBookService()
        {
            _repo = new PhoneBookRespository();
        }

        public bool DeleteContact(string Id)
        {
            try
            {
                var contacts = _repo.GetContacts();
                var contactForDelete = contacts.FirstOrDefault(x => x.Id.ToString() == Id);
                contacts.Remove(contactForDelete);
                _repo.SaveContact(contacts);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public List<Contact> GetContacts(bool sort = true)
        {
            if (sort)
            {
                return _repo.GetContacts().OrderBy(x => x.Lastname).ToList();
            }
            return _repo.GetContacts();
        }

        public Contact GetContactById(string Id)
        {
            return _repo.GetContactById(Id);
        }

        public bool SaveContact(Contact model)
        {
            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                return _repo.SaveContact(model);
            }
            else
            {
                return _repo.UpdateContact(model);
            }
        }
    }
}
