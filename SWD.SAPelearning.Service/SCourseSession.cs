using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.CourseSessionDTO;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SCourseSession : ICourseSession
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningContext context;

        public SCourseSession(SAPelearningContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<CourseSession>> GetAllCourseSession()
        {
            try
            {
                var a = await this.context.CourseSessions.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }

        }



        public async Task<CourseSessionDTO> CreateCourseSession(CourseSessionDTO request)
        {
            try
            {
                // Validate the request
                if (request == null)
                    throw new ArgumentNullException(nameof(request), "CourseSessionDTO cannot be null.");

                // Validate Session Date
                if (!request.SessionDate.HasValue)
                    throw new ArgumentException("Session date is required.");

                // Fetch related entities from the database
                var course = await context.Courses.FindAsync(request.CourseId);
                var instructor = await context.Instructors.FindAsync(request.InstructorId);
                var topic = await context.TopicAreas.FindAsync(request.TopicId);

                // Ensure the related entities exist
                if (course == null)
                    throw new Exception($"Course with ID {request.CourseId} not found.");
                if (instructor == null)
                    throw new Exception($"Instructor with ID {request.InstructorId} not found.");
                if (topic == null)
                    throw new Exception($"Topic with ID {request.TopicId} not found.");
                if (request.SessionDate < course.StartTime || request.SessionDate > course.EndTime)
                {
                    throw new ArgumentException($"Session date must be between {course.StartTime} and {course.EndTime}.");
                }
                // Map DTO to CourseSession entity
                var newSession = new CourseSession
                {
                    CourseId = request.CourseId,
                    InstructorId = request.InstructorId,
                    TopicId = request.TopicId,
                    SessionName = request.SessionName,
                    SessionDescription = request.SessionDescription,
                    SessionDate = request.SessionDate,
                    Status = request.Status
                };

                // Add the new session to the database
                context.CourseSessions.Add(newSession);
                await context.SaveChangesAsync();

                // Return the created session as a DTO
                return new CourseSessionDTO
                {
                    CourseId = newSession.CourseId,
                    CourseName = course.CourseName, // Fetch Course Name from the course entity
                    InstructorId = newSession.InstructorId,
                    InstructorName = instructor.Fullname, // Fetch Instructor Name from the instructor entity
                    TopicId = newSession.TopicId,
                    TopicName = topic.TopicName, // Fetch Topic Name from the topic entity
                    SessionName = newSession.SessionName,
                    SessionDescription = newSession.SessionDescription,
                    SessionDate = newSession.SessionDate,
                    Status = newSession.Status
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the course session: {ex.Message}");
            }
        }


        public async Task<CourseSessionDTO> GetCourseSessionById(int id)
        {
            try
            {
                // Fetch course session by ID and include Course and Instructor details
                var session = await context.CourseSessions
                    .Include(cs => cs.Course)
                    .Include(cs => cs.Instructor)
                    .Include(cs => cs.Topic) // Assuming a relationship with Topic
                    .FirstOrDefaultAsync(cs => cs.Id == id);

                if (session == null)
                {
                    throw new Exception($"Course session with ID {id} not found.");
                }

                // Map session data to DTO
                return new CourseSessionDTO
                {
                    CourseId = session.CourseId,
                    CourseName = session.Course.CourseName,
                    InstructorId = session.InstructorId,
                    InstructorName = session.Instructor.Fullname,
                    TopicId = session.TopicId,
                    TopicName = session.Topic?.TopicName,
                    SessionName = session.SessionName,
                    SessionDescription = session.SessionDescription,
                    SessionDate = session.SessionDate,
                    Status = session.Status
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the course session: {ex.Message}");
            }
        }

        public async Task<bool> DeleteCourseSession(int id)
        {
            try
            {
                // Find the course session by ID
                var session = await context.CourseSessions.FindAsync(id);
                if (session == null)
                {
                    throw new Exception($"Course session with ID {id} not found.");
                }

                // Remove the course session
                context.CourseSessions.Remove(session);
                await context.SaveChangesAsync();

                return true; // Return true if deletion was successful
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the course session: {ex.Message}");
            }
        }

        public async Task<CourseSessionDTO> UpdateCourseSession(int id, CourseSessionDTO request)
        {
            try
            {
                // Validate the request
                if (request == null)
                    throw new ArgumentNullException(nameof(request), "CourseSessionDTO cannot be null.");

                // Check if the course session exists
                var existingSession = await context.CourseSessions.FindAsync(id);
                if (existingSession == null)
                {
                    throw new Exception($"Course session with ID {id} not found.");
                }

                // Validate Session Date
                if (!request.SessionDate.HasValue)
                    throw new ArgumentException("Session date is required.");

                // Fetch the related course entity to validate session date
                var course = await context.Courses.FindAsync(request.CourseId);
                if (course == null)
                {
                    throw new Exception($"Course with ID {request.CourseId} not found.");
                }

                // Validate that SessionDate is within Course Start and End Time
                if (request.SessionDate < course.StartTime || request.SessionDate > course.EndTime)
                {
                    throw new ArgumentException($"Session date must be between {course.StartTime} and {course.EndTime}.");
                }

                // Update properties
                existingSession.CourseId = request.CourseId;
                existingSession.InstructorId = request.InstructorId;
                existingSession.TopicId = request.TopicId;
                existingSession.SessionName = request.SessionName;
                existingSession.SessionDescription = request.SessionDescription;
                existingSession.SessionDate = request.SessionDate;
                existingSession.Status = request.Status;

                // Save changes
                await context.SaveChangesAsync();

                // Return the updated session as a DTO
                return new CourseSessionDTO
                {
                    CourseId = existingSession.CourseId,
                    CourseName = course.CourseName, // Fetch Course Name from the course entity
                    InstructorId = existingSession.InstructorId,
                    InstructorName = request.InstructorName, // Assuming the request includes this
                    TopicId = existingSession.TopicId,
                    TopicName = request.TopicName, // Assuming the request includes this
                    SessionName = existingSession.SessionName,
                    SessionDescription = existingSession.SessionDescription,
                    SessionDate = existingSession.SessionDate,
                    Status = existingSession.Status
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the course session: {ex.Message}");
            }
        }
    }
}
