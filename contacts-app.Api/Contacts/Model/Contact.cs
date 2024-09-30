using contacts_app.Users.Model;

namespace contacts_app.Contacts.Model
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}