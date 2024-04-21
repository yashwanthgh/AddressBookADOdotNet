using AddressBookADOdotNet;

public class Program
{
    public static void Main(string[] args)
    {
        int choice;

        do
        {
            Console.WriteLine("\nAddress Book Menu:");
            Console.WriteLine("1. Add New Contact");
            Console.WriteLine("2. Edit Contact");
            Console.WriteLine("3. Delete Contact");
            Console.WriteLine("4. Get Contact of");
            Console.WriteLine("5. Get All Contacts");
            Console.WriteLine("6. Search Contacts by City");
            Console.WriteLine("7. Search Contacts by State");
            Console.WriteLine("8. Exit");
            Console.WriteLine("Enter your choice: ");

            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 7)
            {
                Console.WriteLine("Invalid choice. Please enter a number between 1 and 7: ");
            }

            switch (choice)
            {
                case 1:
                    AddressBookManager.AddContact();
                    break;
                case 2:
                    Console.Write("Enter the name of the contact to edit: ");
                    string? editName = Console.ReadLine();
                    AddressBookManager.EditContact(editName);
                    break;
                case 3:
                    Console.Write("Enter the name of the contact to delete: ");
                    string deleteName = Console.ReadLine();
                    AddressBookManager.DeleteContact(deleteName);
                    break;
                case 4:
                    Console.WriteLine("Enter the name of the contact to display: ");
                    string? displayContact = Console.ReadLine();
                    AddressBookManager.GetContact(displayContact);
                    break;
                case 5:
                    Console.WriteLine("All Contacts:");
                    AddressBookManager.GetAllContacts();
                    break;
                case 6:
                    Console.Write("Enter the city to search contacts: ");
                    string searchCity = Console.ReadLine();
                    Console.WriteLine($"Contacts in {searchCity}:");
                    AddressBookManager.GetByCity(searchCity);
                    break;
                case 7:
                    Console.Write("Enter the state to search contacts: ");
                    string searchState = Console.ReadLine();
                    Console.WriteLine($"Contacts in {searchState}:");
                    AddressBookManager.GetByState(searchState);
                    break;
                default:
                    Console.WriteLine("Exiting Address Book...");
                    break;
            }

        } while (choice != 7); 
    }

}