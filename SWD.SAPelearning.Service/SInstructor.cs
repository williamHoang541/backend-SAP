using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.Instructor;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Service
{
    public class SInstructor : IInstructor
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningdeployContext context;

        public SInstructor(SAPelearningdeployContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }

        public async Task<List<Instructor>> GetAllInstructor()
        {
            try
            {
                var a = await this.context.Instructors.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<bool> UpdateInstructorByUserId(string userId, UpdateInstructorDTO updateInstructorDTO)
        {
            // Fetch the instructor by UserId
            var instructor = await context.Instructors.FirstOrDefaultAsync(i => i.UserId == userId);

            // Fetch the user in the Usertb table by UserId
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (instructor == null || user == null)
            {
                return false; // Either instructor or user not found
            }

            // Update instructor fields
            instructor.Fullname = updateInstructorDTO.Fullname;
            instructor.Email = updateInstructorDTO.Email;
            instructor.Phonenumber = updateInstructorDTO.Phonenumber;

            // Update user fields
            user.Fullname = updateInstructorDTO.Fullname; // Update Fullname in Users
            user.Email = updateInstructorDTO.Email;       // Update Email in Users
            user.Phonenumber = updateInstructorDTO.Phonenumber; // Update Phonenumber in Users

            // No need to call Update() if the entity is being tracked.
            // This will automatically track changes made to the instructor and user entities.
            await context.SaveChangesAsync(); // Save changes to both entities

            return true;
        }



        public async Task<bool> RemoveInstructor(RemoveIDTO removeIDTO)
        {
            // Check for Instructor by either ID or UserID
            var instructor = await context.Instructors.FirstOrDefaultAsync(i =>
                i.Id == removeIDTO.ID || i.UserId == removeIDTO.UserID);

            if (instructor == null)
            {
                return false; // Instructor not found
            }

            context.Instructors.Remove(instructor);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
