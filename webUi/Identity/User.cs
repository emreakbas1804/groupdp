using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace webUi.Identity
{
    public class User:IdentityUser
    {                                
        public string FirstName { get; set; }
        public string LastName { get; set; }  
    }
    public class Point
    {             
        public int Id { get; set; }     
        public int UserPoint { get; set; }
        public User User { get; set; }    
    }
    
    

    
}