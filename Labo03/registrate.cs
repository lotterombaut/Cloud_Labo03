using System;
using System.Collections.Generic;
using System.Text;

namespace Labo03
{
    public class registrate
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string ZipCode { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public bool IsFirstTimer { get; set; }
        public string RegistrationID { get; internal set; }
    }//end class
}//end namespace
