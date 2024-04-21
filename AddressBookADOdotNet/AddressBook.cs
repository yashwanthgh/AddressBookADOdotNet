using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookADOdotNet
{
    public class AddressBook : IAddressBook
    {
        private static readonly string _connectionString = @"Server = (localdb)\MSSQLLocalDB; Initial Catalog = AddressBooks; Integrated Security = SSPI";
        private static readonly SqlConnection _connection = new(_connectionString);

        public void AddContact(IContact contact)
        {
            try
            {
                string addContact = "INSERT INTO Contacts VALUES (@Name, @Phonenumber, @Email, @City, @Zip, @State)";
                using SqlCommand command = new SqlCommand(addContact, _connection);
                command.Parameters.AddWithValue("@Name", contact.Name);
                command.Parameters.AddWithValue("@Phonenumber", contact.Phonenumber);
                command.Parameters.AddWithValue("@Email", contact.Email);
                command.Parameters.AddWithValue("@City", contact.City);
                command.Parameters.AddWithValue("@Zip", contact.Zip);
                command.Parameters.AddWithValue("@State", contact.State);

                _connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Contact " + contact.Name + " added!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding the contact: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }

        public void EditContact(string name)
        {
            try
            {
                IContact? existingContact = GetContact(name).FirstOrDefault();

                if (existingContact == null)
                {
                    Console.WriteLine("Contact not found.");
                    return;
                }

                Console.WriteLine("Edit Contact Details:");
                bool updateName = GetUserInputYN("Update Name (y/n)?");
                string newName = updateName ? GetUserInput("Enter New Name: ") : existingContact.Name;

                bool updatePhone = GetUserInputYN("Update Phone Number (y/n)?");
                string newPhone = updatePhone ? GetUserInput("Enter New Phone Number: ") : existingContact.Phonenumber;

                bool updateEmail = GetUserInputYN("Update Email (y/n)?");
                string newEmail = updateEmail ? GetUserInput("Enter New Email: ") : existingContact.Email;

                bool updateCity = GetUserInputYN("Update City (y/n)?");
                string newCity = updateCity ? GetUserInput("Enter New City: ") : existingContact.City;

                bool updateZip = GetUserInputYN("Update Zip (y/n)?");
                string newZip = updateZip ? GetUserInput("Enter New Zip: ") : existingContact.Zip;

                bool updateState = GetUserInputYN("Update State (y/n)?");
                string newState = updateState ? GetUserInput("Enter New State: ") : existingContact.State;

                string editContact = "UPDATE Contacts SET Name = @Name, Phonenumber = @Phonenumber, Email = @Email, City = @City, Zip = @Zip, State = @State WHERE Name = @OldName";
                using SqlCommand command = new SqlCommand(editContact, _connection);
                command.Parameters.AddWithValue("@Name", newName);
                command.Parameters.AddWithValue("@Phonenumber", newPhone);
                command.Parameters.AddWithValue("@Email", newEmail);
                command.Parameters.AddWithValue("@City", newCity);
                command.Parameters.AddWithValue("@Zip", newZip);
                command.Parameters.AddWithValue("@State", newState);
                command.Parameters.AddWithValue("@OldName", name);

                _connection.Open();
                command.ExecuteNonQuery();

                Console.WriteLine("Contact edited successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while editing the contact: " + ex.Message);
            }
            finally
            {
                _connection.Close();
            }
        }

        private static bool GetUserInputYN(string prompt)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine().ToLower();
            return input == "y";
        }

        private static string GetUserInput(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }

        public IEnumerable<IContact> GetAllContacts() 
        {
            List<IContact> contacts = [];
            try 
            {
                string getAllContacts = "SELECT * FROM Contacts"; 
                using SqlCommand command = new(getAllContacts, _connection); 
                _connection.Open();
                SqlDataReader reader = command.ExecuteReader(); 

                while (reader.Read())
                {
                    string retrievedName = reader.GetString(reader.GetOrdinal("Name"));
                    string phoneNumber = reader.GetString(reader.GetOrdinal("Phonenumber"));
                    string email = reader.GetString(reader.GetOrdinal("Email"));
                    string city = reader.GetString(reader.GetOrdinal("City"));
                    string zip = reader.GetString(reader.GetOrdinal("Zip"));
                    string state = reader.GetString(reader.GetOrdinal("State"));

                    Contact contact = new(retrievedName, phoneNumber, email, city, zip, state); 

                    contacts.Add(contact);
                }

                return contacts;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("An error occurred while retrieving contacts: " + ex.Message);
                return contacts;
            }
            finally
            {
                _connection.Close();
            }
        }

        public IEnumerable<IContact> GetContact(string name)
        {
            List<IContact> contacts = []; 

            try
            {
                string getContact = "SELECT * FROM Contacts WHERE Name = @Name";
                using SqlCommand command = new(getContact, _connection);
                command.Parameters.AddWithValue("@Name", name);

                _connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string retrievedName = reader.GetString(reader.GetOrdinal("Name"));
                    string phoneNumber = reader.GetString(reader.GetOrdinal("Phonenumber"));
                    string email = reader.GetString(reader.GetOrdinal("Email"));
                    string city = reader.GetString(reader.GetOrdinal("City"));
                    string zip = reader.GetString(reader.GetOrdinal("Zip"));
                    string state = reader.GetString(reader.GetOrdinal("State"));

                    Contact contact = new(retrievedName, phoneNumber, email, city, zip, state);
                    contacts.Add(contact);
                }

                return contacts;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving contacts: " + ex.Message);
                return contacts;
            }
            finally
            {
                _connection.Close();
            }
        }

        public IEnumerable<IContact> GetContactsByCity(string city)
        {
            List<IContact> contacts = []; 

            try
            {
                string getByCity = "SELECT * FROM Contacts WHERE City = @City";
                using (SqlCommand command = new(getByCity, _connection))
                {
                    command.Parameters.AddWithValue("@City", city);

                    _connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string retrievedName = reader.GetString(reader.GetOrdinal("Name"));
                        string phoneNumber = reader.GetString(reader.GetOrdinal("Phonenumber"));
                        string email = reader.GetString(reader.GetOrdinal("Email"));
                        string retrievedCity = reader.GetString(reader.GetOrdinal("City"));
                        string zip = reader.GetString(reader.GetOrdinal("Zip"));
                        string state = reader.GetString(reader.GetOrdinal("State"));

                        Contact contact = new(retrievedName, phoneNumber, email, retrievedCity, zip, state);
                        contacts.Add(contact);
                    }
                }

                return contacts;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving contacts: " + ex.Message);
                return contacts; 
            }
            finally
            {
                _connection.Close();
            }
        }


        public IEnumerable<IContact> GetContactsByState(string state)
        {
            List<IContact> contacts = [];

            try
            {
                string getByState = "SELECT * FROM Contacts WHERE State = @State";
                using (SqlCommand command = new(getByState, _connection))
                {
                    command.Parameters.AddWithValue("@State", state);

                    _connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string retrievedName = reader.GetString(reader.GetOrdinal("Name"));
                        string phoneNumber = reader.GetString(reader.GetOrdinal("Phonenumber"));
                        string email = reader.GetString(reader.GetOrdinal("Email"));
                        string city = reader.GetString(reader.GetOrdinal("City"));
                        string zip = reader.GetString(reader.GetOrdinal("Zip"));
                        string retrievedState = reader.GetString(reader.GetOrdinal("State"));

                        Contact contact = new(retrievedName, phoneNumber, email, city, zip, retrievedState);
                        contacts.Add(contact);
                    }
                }

                return contacts; 
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving contacts: " + ex.Message);
                return contacts; 
            }
            finally
            {
                _connection.Close();
            }
        }


        public void RemoveContact(string name)
        {
            string deleteContact = "DELETE FROM Contacts WHERE Name = @Name";
            SqlCommand command = new(deleteContact, _connection);

            command.Parameters.AddWithValue("@Name", name);
            _connection.Open();
            command.ExecuteNonQuery();
            Console.WriteLine("Contact deleted successfully.");
        }
    }
}
