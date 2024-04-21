using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AddressBookADOdotNet
{
    public class Contact(string name, string phonenumber, string email, string city, string zip, string state) : IContact
    {
        public string Name { get; private set; } = name;
        public string Phonenumber { get; private set; } = phonenumber;
        public string Email { get; private set; } = email;
        public string City { get; private set; } = city;
        public string Zip { get; private set; } = zip;
        public string State { get; private set; } = state;
    }

    public class AddressBookManager
    {
        private static readonly AddressBook? _addressBook = new();

        private static string GetValidatedInput(string prompt, string pattern)
        {
            Regex regex = new(pattern);
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (!regex.IsMatch(input))
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            } while (!regex.IsMatch(input));
            return input;
        }
        private static IContact GetUserInput()
        {
            Console.WriteLine("Enter Name:");
            string name = GetValidatedInput("Name: ", @"^[A-Z][a-zA-Z]{2,}$");

            Console.WriteLine("Enter Phone Number:");
            string phoneNumber = GetValidatedInput("Phone Number: ", @"^[0-9]{10}$");

            Console.WriteLine("Enter Email:");
            string email = GetValidatedInput("Email: ", @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

            Console.WriteLine("Enter City:");
            string? city = Console.ReadLine();

            Console.WriteLine("Enter Zip:");
            string? zip = Console.ReadLine();

            Console.WriteLine("Enter State:");
            string? state = Console.ReadLine();

            return new Contact(name, phoneNumber, email, city, zip, state); 
        }

        public static void AddContact()
        {
            IContact contact = GetUserInput();
            _addressBook.AddContact(contact);
        }

        public static void EditContact(string name)
        {
            IEnumerable<IContact> contacts = new AddressBook().GetContact(name);

            if (!contacts.Any())
            {
                Console.WriteLine("Contact not found.");
                return;
            }

            int index = 1;
            foreach (IContact contact in contacts)
            {
                Console.WriteLine($"{index++}. {contact.Name} - {contact.Email} ({contact.Phonenumber})");
            }

            Console.WriteLine("Enter the number of the contact to edit (or 0 to cancel):");
            int selection;
            while (!int.TryParse(Console.ReadLine(), out selection) || selection < 0 || selection > contacts.Count())
            {
                Console.WriteLine("Invalid selection. Please enter a valid number (0 to cancel):");
            }

            if (selection == 0)
            {
                Console.WriteLine("Edit cancelled.");
                return;
            }

            IContact selectedContact = contacts.ElementAt(selection - 1);
            
            _addressBook.EditContact(selectedContact.Name);
        }

        public static void GetContact(string name)
        {
            IEnumerable<IContact> contacts = _addressBook.GetContact(name);
            if (!contacts.Any())
            {
                Console.WriteLine("Contact not found.");
                return;
            }

            Console.WriteLine("Matching Contacts:");
            foreach (IContact contact in contacts)
            {
                Console.WriteLine($"Name: {contact.Name}, Phonenumber: {contact.Phonenumber}, Email: {contact.Email}, City: {contact.City}, Zip: {contact.Zip}, State: {contact.State}");
            }
        }

        public static void GetAllContacts()
        {
            IEnumerable<IContact> contacts = _addressBook.GetAllContacts();
            if (!contacts.Any())
            {
                Console.WriteLine("Contact not found.");
                return;
            }

            Console.WriteLine("Matching Contacts:");
            foreach (IContact contact in contacts)
            {
                Console.WriteLine(contact.Name);
            }
        }

        public static void GetByCity(string city)
        {
            IEnumerable<IContact> contacts = _addressBook.GetContactsByCity(city);
            if (!contacts.Any())
            {
                Console.WriteLine("Contact not found.");
                return;
            }

            Console.WriteLine("Matching Contacts:");
            foreach (IContact contact in contacts)
            {
                Console.WriteLine(contact.Name);
            }
        }

        public static void GetByState(string state)
        {
            IEnumerable<IContact> contacts = _addressBook.GetContactsByState(state);
            if (!contacts.Any())
            {
                Console.WriteLine("Contact not found.");
                return;
            }

            Console.WriteLine("Matching Contacts:");
            foreach (IContact contact in contacts)
            {
                Console.WriteLine(contact.Name);
            }
        }

        public static void DeleteContact(string name)
        {
            IEnumerable<IContact> contacts = _addressBook.GetContact(name);

            if (!contacts.Any())
            {
                Console.WriteLine("Contact not found.");
                return;
            }

            Console.WriteLine("Multiple contacts found with the same name:");
            int index = 1;
            foreach (var contact in contacts)
            {
                Console.WriteLine($"{index++}. {contact.Name} - {contact.Email} ({contact.Phonenumber})");
            }

            Console.WriteLine("Enter the number of the contact to delete: ");
            int selection;
            while (!int.TryParse(Console.ReadLine(), out selection) || selection < 0 || selection > contacts.Count())
            {
                Console.WriteLine("Invalid selection. Please enter a valid number:");
            }

            IContact selectedContact = contacts.ElementAt(selection - 1);

            Console.WriteLine($"Are you sure you want to delete {selectedContact.Name} (y/n)?");
            string confirmation = Console.ReadLine().ToLower();
            if (confirmation != "y")
            {
                Console.WriteLine("Deletion cancelled.");
                return;
            }

            _addressBook.RemoveContact(selectedContact.Name);
        }
    }
}
