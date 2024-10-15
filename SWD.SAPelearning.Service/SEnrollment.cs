using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.EnrollmentDTO;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SEnrollment : IEnrollment
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningdeployContext context;

        public SEnrollment(SAPelearningdeployContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<EnrollmentDTO>> GetAllEnrollmentsAsync(GetAllDTO getAllDTO)
        {
            IQueryable<Enrollment> query = context.Enrollments
                .Include(e => e.User)  // Assuming you have a User entity
                .Include(e => e.Course) // Assuming you have a Course entity
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(getAllDTO.FilterOn) && !string.IsNullOrWhiteSpace(getAllDTO.FilterQuery))
            {
                switch (getAllDTO.FilterOn.ToLower())
                {
                    case "userid":
                        query = query.Where(e => e.UserId == getAllDTO.FilterQuery);
                        break;
                    case "courseid":
                        if (int.TryParse(getAllDTO.FilterQuery, out int courseId))
                        {
                            query = query.Where(e => e.CourseId == courseId);
                        }
                        break;
                    case "enrollmentdate":
                        if (DateTime.TryParse(getAllDTO.FilterQuery, out DateTime enrollmentDate))
                        {
                            query = query.Where(e => e.EnrollmentDate == enrollmentDate);
                        }
                        break;
                    case "price":
                        if (double.TryParse(getAllDTO.FilterQuery, out double price))
                        {
                            query = query.Where(e => e.Price == price);
                        }
                        break;
                    case "status":
                        query = query.Where(e => e.Status.Equals(getAllDTO.FilterQuery, StringComparison.OrdinalIgnoreCase));
                        break;
                    default:
                        break;
                }
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(getAllDTO.SortBy))
            {
                bool isAscending = getAllDTO.IsAscending ?? true;

                switch (getAllDTO.SortBy.ToLower())
                {
                    case "userid":
                        query = isAscending ? query.OrderBy(e => e.UserId) : query.OrderByDescending(e => e.UserId);
                        break;
                    case "courseid":
                        query = isAscending ? query.OrderBy(e => e.CourseId) : query.OrderByDescending(e => e.CourseId);
                        break;
                    case "enrollmentdate":
                        query = isAscending ? query.OrderBy(e => e.EnrollmentDate) : query.OrderByDescending(e => e.EnrollmentDate);
                        break;
                    case "price":
                        query = isAscending ? query.OrderBy(e => e.Price) : query.OrderByDescending(e => e.Price);
                        break;
                    case "status":
                        query = isAscending ? query.OrderBy(e => e.Status) : query.OrderByDescending(e => e.Status);
                        break;
                    default:
                        query = isAscending ? query.OrderBy(e => e.EnrollmentDate) : query.OrderByDescending(e => e.EnrollmentDate);
                        break;
                }
            }

            // Pagination
            int pageNumber = getAllDTO.PageNumber ?? 1;
            int pageSize = getAllDTO.PageSize ?? 10;
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Fetch and return enrollments
            var enrollments = await query
                .Select(e => new EnrollmentDTO
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    CourseId = e.CourseId,
                    EnrollmentDate = e.EnrollmentDate,
                    Price = e.Price,
                    Status = e.Status
                })
                .ToListAsync();

            return enrollments;
        }


        public async Task<EnrollmentDTO> CreateEnrollmentAsync(string userId, int courseId, double enrollmentPrice)
        {
            try
            {
                // Fetch user and course details
                var user = await context.Users.FindAsync(userId);
                var course = await context.Courses.FindAsync(courseId);
                var currentDate = DateTime.UtcNow;

                // Check if the user exists and is online
                if (user == null || user.IsOnline != true)
                    throw new InvalidOperationException("User is either not found or not online.");

                // Check if the course exists and is active
                if (course == null || course.Status != true)
                    throw new InvalidOperationException("Course is not available for enrollment.");

                // Check if the enrollment date has passed
                if (currentDate > course.EnrollmentDate)
                    throw new InvalidOperationException("Enrollment period has ended.");

                // Check if the user is already enrolled in this course
                if (await context.Enrollments.AnyAsync(e => e.UserId == userId && e.CourseId == courseId))
                    throw new InvalidOperationException("User is already enrolled in this course.");

                // Check the total number of students based on course mode
                if (course.Mode == "Online" && course.TotalStudent >= 60)
                    throw new InvalidOperationException("Online course has reached maximum enrollment capacity (60).");
                else if (course.Mode == "Offline" && course.TotalStudent >= 40)
                    throw new InvalidOperationException("Offline course has reached maximum enrollment capacity (40).");

                // Validate the price to avoid price mismatch
                if (enrollmentPrice != course.Price)
                    throw new InvalidOperationException("Price mismatch.");

                // Map to the Enrollment entity
                var enrollment = new Enrollment
                {
                    UserId = userId,
                    CourseId = courseId,
                    EnrollmentDate = currentDate,
                    Price = enrollmentPrice,
                    Status = "Pending" // Enrollment is pending until payment is completed
                };

                // Add the enrollment to the database
                context.Enrollments.Add(enrollment);
                await context.SaveChangesAsync(); // Save changes to generate the enrollment ID

                // Return the EnrollmentDTO
                return new EnrollmentDTO
                {
                    Id = enrollment.Id,
                    UserId = enrollment.UserId,
                    CourseId = enrollment.CourseId,
                    EnrollmentDate = enrollment.EnrollmentDate.Value,
                    Price = enrollment.Price.Value,
                    Status = enrollment.Status
                };
            }
            catch (Exception ex)
            {
                // Handle any errors during the process
                throw new Exception($"An error occurred while creating the enrollment: {ex.Message}");
            }
        }

        public async Task<bool> ConfirmEnrollmentAsync(int enrollmentId)
        {
            var enrollment = await context.Enrollments.FindAsync(enrollmentId);
            if (enrollment == null)
            {
                throw new KeyNotFoundException("Enrollment not found.");
            }

            if (enrollment.Status != "Pending")
            {
                throw new InvalidOperationException("Enrollment must be in Pending status to confirm.");
            }

            // Update status to Confirmed
            enrollment.Status = "Confirmed";
            
            var payment = await context.Payments
        .FirstOrDefaultAsync(p => p.EnrollmentId == enrollmentId);

            if (payment == null || payment.Status != "completed")
            {
                throw new InvalidOperationException("Payment must be completed to confirm enrollment.");
            }

            // Update status to Confirmed
            enrollment.Status = "Confirmed";
            // Increment total students in the associated course
            var course = await context.Courses.FindAsync(enrollment.CourseId);
            if (course != null)
            {
                course.TotalStudent += 1;
            }

            await context.SaveChangesAsync();

            return true; // Enrollment confirmed successfully
        }

        public async Task<EnrollmentDTO> GetEnrollmentByIdAsync(int enrollmentId)
        {
            var enrollment = await context.Enrollments.FindAsync(enrollmentId);
            if (enrollment == null)
            {
                throw new KeyNotFoundException("Enrollment not found.");
            }

            return new EnrollmentDTO
            {
                Id = enrollment.Id,
                UserId = enrollment.UserId,
                CourseId = enrollment.CourseId,
                EnrollmentDate = enrollment.EnrollmentDate.Value,
                Price = enrollment.Price.Value,
                Status = enrollment.Status
            };
        }

        public async Task<bool> MarkEnrollmentAsConfirmedAsync(int enrollmentId, string userId)
        {
            // Find the enrollment by ID
            var enrollment = await context.Enrollments
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == enrollmentId && e.UserId == userId);

            // Check if enrollment exists and is not already Confirmed
            if (enrollment == null || enrollment.Status == "Confirmed")
            {
                throw new InvalidOperationException("Enrollment not found or has already been confirmed.");
            }

            // Check the course's end time
            var currentDate = DateTime.UtcNow;
            if (enrollment.Course.EndTime <= currentDate)
            {
                throw new InvalidOperationException("Course end time has already passed; cannot confirm enrollment.");
            }

            // Update the status to Confirmed
            enrollment.Status = "Confirmed";
            await context.SaveChangesAsync();

            return true; // Return true if successful
        }



        public async Task<bool> CancelEnrollmentAsync(int enrollmentId)
        {
            var enrollment = await context.Enrollments.FindAsync(enrollmentId);
            if (enrollment == null)
            {
                throw new KeyNotFoundException("Enrollment not found.");
            }

            enrollment.Status = "Canceled"; // Update status to Canceled
            await context.SaveChangesAsync();

            return true; // Enrollment canceled successfully
        }

    }
}

