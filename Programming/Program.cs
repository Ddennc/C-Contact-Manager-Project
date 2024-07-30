using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Programming
{
    class Program
    {
        static List<Contact> contacts = new List<Contact>();
        static string filePath = "contacts.xml";

        static void Main(string[] args)
        {
            LoadContacts();

            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Contact Manager");
                Console.WriteLine("1. Add Contact");
                Console.WriteLine("2. View Contacts");
                Console.WriteLine("3. Delete Contact");
                Console.WriteLine("4. Edit Contact");
                Console.WriteLine("5. Search Contacts");
                Console.WriteLine("6. Sort Contacts");
                Console.WriteLine("7. Save and Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddContact();
                        break;
                    case "2":
                        ViewContacts();
                        break;
                    case "3":
                        DeleteContact();
                        break;
                    case "4":
                        EditContact();
                        break;
                    case "5":
                        SearchContacts();
                        break;
                    case "6":
                        SortContacts();
                        break;
                    case "7":
                        SaveContacts();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
                if (!exit)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        
        /// Adds a new contact to the list.
       
        static void AddContact()
        {
            Console.Clear();
            Console.WriteLine("Add Contact");
            Console.Write("Enter first name: ");
            string firstname = Console.ReadLine();
            while (!IsValidInput(firstname))
            {
                Console.WriteLine("Invalid input. Please enter only letters in Latin in format Xxxxxx.");
                firstname = Console.ReadLine();
            }

            Console.Write("Enter last name: ");
            string lastname = Console.ReadLine();
            while (!IsValidInput(lastname))
            {
                Console.WriteLine("Invalid input. Please enter only letters in Latin in format Xxxxxx.");
                lastname = Console.ReadLine();
            }

            Console.Write("Enter Czech phone number in format +420xxxxxxxxx: ");
            string phoneNumber = Console.ReadLine();
            while (!IsValidPhoneNumber(phoneNumber))
            {
                Console.WriteLine("Invalid input. Please enter only numbers in format +420xxxxxxxxx.");
                phoneNumber = Console.ReadLine();
            }

            Console.Write("Enter email address in format xxxx@xxx.xxxx: ");
            string email = Console.ReadLine();
            while (!IsValidEmail(email))
            {
                Console.WriteLine("Invalid input. Please enter a valid email address in format xxxx@xxx.xxxx.");
                email = Console.ReadLine();
            }

            string index = (contacts.Count).ToString();
            Contact contact = new Contact( firstname, lastname, phoneNumber, email);
            contacts.Add(contact);

            Console.WriteLine("Contact added successfully!");
        }

      
        /// Validates the phone number format.
       
        static bool IsValidPhoneNumber(string phoneNumber)
        {
            Regex regex = new Regex("^\\+420\\d{9}$");
            return regex.IsMatch(phoneNumber);
        }

       
        /// Validates the name format.
        
        static bool IsValidInput(string input)
        {
            Regex regex = new Regex("^[A-Z][a-z]*(?: [A-Z][a-z]*)*$");
            return regex.IsMatch(input);
        }

        
        /// Validates the email format.
        
        static bool IsValidEmail(string email)
        {
            Regex regex = new Regex("^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$");
            return regex.IsMatch(email);
        }

       
        /// Displays all contacts.
       
        static void ViewContacts()
        {
            Console.Clear();
            if (contacts.Count == 0)
            {
                Console.WriteLine("No contacts to view.");
            }
            else
            {
                Console.WriteLine("View Contacts");
                foreach (var contact in contacts)
                {
                    Console.WriteLine(contact);
                }
            }
        }

       
        /// Deletes a contact by index.
       
        static void DeleteContact()
        {
            Console.Clear();
            if (contacts.Count == 0)
            {
                Console.WriteLine("No contacts to delete.");
                return;
            }

            Console.WriteLine("Delete Contact");
            Console.WriteLine("Contacts:");
            for (int i = 0; i < contacts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {contacts[i]}");
            }
            Console.Write("Enter number of contact to delete: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= contacts.Count)
            {
                contacts.RemoveAt(index - 1);
                Console.WriteLine("Contact deleted successfully!");
            }
            else
            {
                Console.WriteLine("Invalid index. Please try again.");
            }
        }

       
        /// Edits a contact by index.
        
        static void EditContact()
        {
            Console.Clear();
            if (contacts.Count == 0)
            {
                Console.WriteLine("No contacts to edit.");
                return;
            }

            Console.WriteLine("Edit Contact");
            Console.WriteLine("Contacts:");
            for (int i = 0; i < contacts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {contacts[i]}");
            }
            Console.Write("Enter number of contact to edit: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= contacts.Count)
            {
                Contact contact = contacts[index - 1];
                Console.WriteLine("Select field to edit:");
                Console.WriteLine("1. First Name");
                Console.WriteLine("2. Last Name");
                Console.WriteLine("3. Phone Number");
                Console.WriteLine("4. Email");
                Console.Write("Enter your choice: ");
                string fieldChoice = Console.ReadLine();

                switch (fieldChoice)
                {
                    case "1":
                        EditField(contact, "first name", value => contact.Firstname = value);
                        break;
                    case "2":
                        EditField(contact, "last name", value => contact.Lastname = value);
                        break;
                    case "3":
                        EditField(contact, "phone number", value => contact.PhoneNumber = value);
                        break;
                    case "4":
                        EditField(contact, "email", value => contact.Email = value);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("Contact edited successfully!");
            }
            else
            {
                Console.WriteLine("Invalid index. Please try again.");
            }
        }

        /// Edits a specific field of a contact.
       
        static void EditField(Contact contact, string fieldName, Action<string> updateAction)
        {
            Console.Write($"Enter new {fieldName} (press Enter to keep '{contact.Firstname}'): ");
            string newValue = Console.ReadLine();

            if (!string.IsNullOrEmpty(newValue))
            {
                while (!IsValidInput(newValue))
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    Console.Write($"Enter new {fieldName} (press Enter to keep '{contact.Firstname}'): ");
                    newValue = Console.ReadLine();
                }
                updateAction(newValue);
            }
        }

        
        /// Searches contacts by a term.
        
        static void SearchContacts()
        {
            Console.Clear();
            Console.WriteLine("Search Contacts");
            Console.Write("Enter search term: ");
            string searchTerm = Console.ReadLine().ToLower();

            var results = contacts.Where(c =>
                c.Firstname.ToLower().Contains(searchTerm) ||
                c.Lastname.ToLower().Contains(searchTerm) ||
                c.PhoneNumber.ToLower().Contains(searchTerm) ||
                c.Email.ToLower().Contains(searchTerm)
            ).ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("No contacts found matching the search term.");
            }
            else
            {
                Console.WriteLine("Search Results:");
                foreach (var contact in results)
                {
                    Console.WriteLine(contact);
                }
            }
        }

       
        /// Sorts contacts by a selected field.
       
        static void SortContacts()
        {
            Console.Clear();
            Console.WriteLine("Sort Contacts");
            Console.WriteLine("1. By First Name");
            Console.WriteLine("2. By Last Name");
            Console.WriteLine("3. By Phone Number");
            Console.WriteLine("4. By Email Address");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    contacts.Sort((x, y) => x.Firstname.CompareTo(y.Firstname));
                    break;
                case "2":
                    contacts.Sort((x, y) => x.Lastname.CompareTo(y.Lastname));
                    break;
                case "3":
                    contacts.Sort((x, y) => x.PhoneNumber.CompareTo(y.PhoneNumber));
                    break;
                case "4":
                    contacts.Sort((x, y) => x.Email.CompareTo(y.Email));
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    return;
            }

            Console.WriteLine("Contacts sorted successfully!");
            ViewContacts();
        }

       
        /// Loads contacts from an XML file.
      
        static void LoadContacts()
        {
            if (!System.IO.File.Exists(filePath))
            {
                new XDocument(new XElement("Contacts")).Save(filePath);
                return;
            }

            try
            {
                XDocument xdoc = XDocument.Load(filePath);
                contacts = xdoc.Root.Elements("Contact").Select(elem => new Contact(
                    elem.Element("Firstname").Value,
                    elem.Element("Lastname").Value,
                    elem.Element("PhoneNumber").Value,
                    elem.Element("Email").Value
                )).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading contacts: " + e.Message);
            }
        }

        
        /// Saves contacts to an XML file.
        
        static void SaveContacts()
        {
            XDocument xdoc = new XDocument(
                new XElement("Contacts",
                    contacts.Select(c =>
                        new XElement("Contact",
                            new XElement("Firstname", c.Firstname),
                            new XElement("Lastname", c.Lastname),
                            new XElement("PhoneNumber", c.PhoneNumber),
                            new XElement("Email", c.Email)
                        )
                    )
                )
            );
            xdoc.Save(filePath);
            Console.WriteLine("Contacts saved successfully!");
        }

      
        /// Contact class representing a single contact.
      
        class Contact
        {
            
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }

            public Contact(string firstname, string lastname, string phoneNumber, string email)
            {
               // Index = index;
                Firstname = firstname;
                Lastname = lastname;
                PhoneNumber = phoneNumber;
                Email = email;
            }

            public override string ToString()
            {
                return $"Name: {Firstname} {Lastname}, Phone: {PhoneNumber}, Email: {Email}";
            }
        }
    }
}



