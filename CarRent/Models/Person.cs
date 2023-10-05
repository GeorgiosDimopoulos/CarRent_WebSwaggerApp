namespace CarRent.Models
{
    public class Person
    {
        public Person(string Id, string Name, int Age, Gender Gender, UserType user)
        {
            this.User = User;
            this.Id = Id;
            this.Gender = Gender;
            this.Name = Name;
            this.Age = Age;

        }

        public string Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public UserType User { get; set; }
    }

    public enum UserType
    {
        Customer,
        Administrator
    }

    public enum Gender
    {
        Male,
        Female,
        NotToDisclose
    }
}
