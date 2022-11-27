using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class AppUser
    {
        //[Key]
        //Git added comment.
       public int Id { get; set; }
       public string UserName { get; set; } 
       
    }
}