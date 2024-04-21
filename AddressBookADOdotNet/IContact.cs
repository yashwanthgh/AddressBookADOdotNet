using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookADOdotNet
{
    public interface IContact
    {
        string Name { get; }
        string Phonenumber { get; }
        string Email { get; }
        string City { get; }
        string Zip { get; }
        string State { get; }
    }

    public interface IAddressBook
    {
        public void AddContact(IContact contact);
        public void RemoveContact(string name);
        public void EditContact(string name);
        public IEnumerable<IContact> GetContact(string name);
        public IEnumerable<IContact> GetAllContacts();
        public IEnumerable<IContact> GetContactsByState(string state);
        public IEnumerable<IContact> GetContactsByCity(string city);
    }
}
