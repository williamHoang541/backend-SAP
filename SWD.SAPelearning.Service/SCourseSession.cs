using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.CourseSessionDTO;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SCourseSession : ICourseSession
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningAPIContext context;

        public SCourseSession(SAPelearningAPIContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<CourseSessionDTO>> GetAllCourseSessionsAsync(GetAllDTO getAllDTO)
        {
            IQueryable<CourseSession> query = context.CourseSessions
                .Include(cs => cs.Course)
                .Include(cs => cs.Instructor)
                .Include(cs => cs.Topic)
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(getAllDTO.FilterOn) && !string.IsNullOrWhiteSpace(getAllDTO.FilterQuery))
            {
                switch (getAllDTO.FilterOn.ToLower())
                {
                    case "courseid":
                        if (int.TryParse(getAllDTO.FilterQuery, out int courseId))
                        {
                            query = query.Where(cs => cs.CourseId == courseId);
                        }
                        break;
                    case "coursename":
                        query = query.Where(cs => cs.Course.CourseName.Contains(getAllDTO.FilterQuery));
                        break;
                    case "instructorid":
                        if (int.TryParse(getAllDTO.FilterQuery, out int instructorId))
                        {
                            query = query.Where(cs => cs.InstructorId == instructorId);
                        }
                        break;
                    case "instructorname":
                        query = query.Where(cs => cs.Instructor.Fullname.Contains(getAllDTO.FilterQuery));
                        break;
                    case "topicid":
                        if (int.TryParse(getAllDTO.FilterQuery, out int topicId))
                        {
                            query = query.Where(cs => cs.TopicId == topicId);
                        }
                        break;
                    case "topicname":
                        query = query.Where(cs => cs.Topic.TopicName.Contains(getAllDTO.FilterQuery));
                        break;
                    case "sessionname":
                        query = query.Where(cs => cs.SessionName.Contains(getAllDTO.FilterQuery));
                        break;
                    case "sessiondate":
                        if (DateTime.TryParse(getAllDTO.FilterQuery, out DateTime sessionDate))
                        {
                            query = query.Where(cs => cs.SessionDate >= sessionDate);
                        }
                        break;
                    case "status":
                        if (bool.TryParse(getAllDTO.FilterQuery, out bool status))
                        {
                            query = query.Where(cs => cs.Status == status);
                        }
                        break;
                    default:
                        break;
                }
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(getAllDTO.SortBy))
            {
                // Handle null IsAscending by defaulting to true if it's not specified
                bool isAscending = getAllDTO.IsAscending ?? true;

                switch (getAllDTO.SortBy.ToLower())
                {
                    case "sessiondate":
                        query = isAscending
                            ? query.OrderBy(s => s.SessionDate) // true for ascending
                            : query.OrderByDescending(s => s.SessionDate); // false for descending
                        break;

                    case "instructorid":
                        query = isAscending
                            ? query.OrderBy(s => s.InstructorId) // true for ascending
                            : query.OrderByDescending(s => s.InstructorId); // false for descending
                        break;

                    case "sessionname":
                        query = isAscending
                            ? query.OrderBy(s => s.SessionName) // true for ascending
                            : query.OrderByDescending(s => s.SessionName); // false for descending
                        break;
                    case "courseid":
                        if (int.TryParse(getAllDTO.FilterQuery, out int courseId))
                        {
                            query = query.Where(cs => cs.CourseId == courseId);
                        }
                        break;
                    case "topicid":
                        if (int.TryParse(getAllDTO.FilterQuery, out int topicId))
                        {
                            query = query.Where(cs => cs.TopicId == topicId);
                        }
                        break;
                    case "status":
                        query = isAscending
                            ? query.OrderBy(s => s.Status) // true for ascending
                            : query.OrderByDescending(s => s.Status); // false for descending
                        break;

                    default:
                        // Default to sorting by SessionDate if the SortBy property is invalid
                        query = isAscending
                            ? query.OrderBy(s => s.SessionDate) // true for ascending
                            : query.OrderByDescending(s => s.SessionDate); // false for descending
                        break;
                }
            }





            // Pagination
            int pageNumber = getAllDTO.PageNumber ?? 1;
            int pageSize = getAllDTO.PageSize ?? 10;
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Fetch and project to DTO
            var courseSessions = await query
                .Select(cs => new CourseSessionDTO
                {
                    CourseId = cs.CourseId,
                    CourseName = cs.Course.CourseName,
                    InstructorId = cs.InstructorId,
                    InstructorName = cs.Instructor.Fullname,
                    TopicId = cs.TopicId,
                    TopicName = cs.Topic.TopicName,
                    SessionName = cs.SessionName,
                    SessionDescription = cs.SessionDescription,
                    SessionDate = cs.SessionDate,
                    Status = cs.Status
                })
                .ToListAsync();

            return courseSessions;
        }



        public async Task<CourseSessionDTO> CreateCourseSession(CourseSessionCreateDTO request)
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
                return await GetCourseSessionById(newSession.Id);
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
