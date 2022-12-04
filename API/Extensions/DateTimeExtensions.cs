
namespace API.Extensions
{
    public static class DateTimeExtensions //Extension classes need to be static
    {
        public static int CalculateAge(this DateOnly dob) //Not fool-proof, doesn't account for leap years etc. 
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow); //Todays date
            var age = today.Year - dob.Year;

            if(dob > today.AddYears(-age)) age--; //If they haven't had their b-day yet this year, we're taking a year off. 
       
            return age;

        }
        
    }
}