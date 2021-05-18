using System;

namespace Atestat.Classes
{
    public class Admin
    {
        public string name { set; get; }
        public string mail { set; get; }
        public string phone { set; get; }
        public DateTime registerDate { set; get; }
        public int id { set; get; }

        public Admin(string name, string mail, string phone, DateTime registerDate, int id)
        {
            this.name = name;
            this.mail = mail;
            this.phone = phone;
            this.registerDate = registerDate;
            this.id = id;
        }

        public Admin()
        {

        }
    }
}
