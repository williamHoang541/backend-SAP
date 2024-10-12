using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.TopicAreaDTO;
using SWD.SAPelearning.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SAPelearning.Service
{
    public class STopicArea : ITopicArea
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningContext context;

        public STopicArea(SAPelearningContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }

        public async Task<List<TopicAreaDTO>> GetAllTopicAreasAsync(GetAllDTO getAllDTO)
        {
            try
            {
                // Create the initial query
                IQueryable<TopicArea> query = context.TopicAreas.AsQueryable();

                // Filtering
                if (!string.IsNullOrWhiteSpace(getAllDTO.FilterOn) && !string.IsNullOrWhiteSpace(getAllDTO.FilterQuery))
                {
                    switch (getAllDTO.FilterOn.ToLower())
                    {
                        case "certificateid":
                            if (int.TryParse(getAllDTO.FilterQuery, out int certificateId))
                            {
                                query = query.Where(ta => ta.CertificateId == certificateId);
                            }
                            break;
                        case "topicname":
                            query = query.Where(ta => ta.TopicName.Contains(getAllDTO.FilterQuery));
                            break;
                        case "status":
                            if (bool.TryParse(getAllDTO.FilterQuery, out bool status))
                            {
                                query = query.Where(ta => ta.Status == status);
                            }
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
                        case "id":
                            query = isAscending
                                ? query.OrderBy(ta => ta.Id)
                                : query.OrderByDescending(ta => ta.Id);
                            break;
                        case "topicname":
                            query = isAscending
                                ? query.OrderBy(ta => ta.TopicName)
                                : query.OrderByDescending(ta => ta.TopicName);
                            break;
                        case "certificateid":
                            query = isAscending
                                ? query.OrderBy(ta => ta.CertificateId)
                                : query.OrderByDescending(ta => ta.CertificateId);
                            break;
                        case "status":
                            query = isAscending
                                ? query.OrderBy(ta => ta.Status)
                                : query.OrderByDescending(ta => ta.Status);
                            break;
                        default:
                            query = isAscending
                                ? query.OrderBy(ta => ta.Id) // Default sorting by Id
                                : query.OrderByDescending(ta => ta.Id);
                            break;
                    }
                }

                // Fetching the data and projecting to TopicAreaDTO
                var topicAreas = await query.Select(ta => new TopicAreaDTO
                {
                    Id = ta.Id,
                    CertificateId = ta.CertificateId,
                    TopicName = ta.TopicName,
                    Status = ta.Status
                }).ToListAsync();

                return topicAreas;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving Topic Areas: {ex.Message}");
            }
        }

        public async Task<TopicAreaDTO> CreateTopicArea(TopicAreaCreateDTO request)
        {
            try
            {
                // Validate input
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "TopicAreaDTO cannot be null.");
                }

                // Check if the associated Certificate exists
                var certificate = await context.Certificates.FindAsync(request.CertificateId);
                if (certificate == null)
                {
                    throw new Exception($"Certificate with ID {request.CertificateId} does not exist.");
                }

                // If the Certificate status is false, prevent creating TopicArea
                if (certificate.Status == false)
                {
                    throw new Exception("Cannot create Topic Area because the associated Certificate is inactive.");
                }

                // Check for duplicate TopicName within the same Certificate (case-insensitive)
                var existingTopicArea = await context.TopicAreas
                    .FirstOrDefaultAsync(t => t.CertificateId == request.CertificateId &&
                                               t.TopicName.ToLower() == request.TopicName.ToLower());

                if (existingTopicArea != null)
                {
                    throw new Exception($"A Topic Area with the name '{request.TopicName}' already exists for this Certificate.");
                }

                // Map DTO to Entity
                var topicArea = new TopicArea
                {
                    CertificateId = request.CertificateId.Value, // Ensure this is not null
                    TopicName = request.TopicName,
                    Status = request.Status ?? true // Default status if null
                };

                // Add new Topic Area to the context
                await context.TopicAreas.AddAsync(topicArea);
                await context.SaveChangesAsync();

                // Return the newly created Topic Area as DTO
                return new TopicAreaDTO
                {
                    Id = topicArea.Id,
                    CertificateId = topicArea.CertificateId,
                    TopicName = topicArea.TopicName,
                    Status = topicArea.Status
                    // Optionally, you can include the Id here if you modify TopicAreaDTO to include it
                };
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("There was an error saving the Topic Area to the database.", dbEx);
            }
            catch (Exception ex)
            {
                var errorMessage = $"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
                throw new Exception($"An error occurred while creating the Topic Area: {errorMessage}");
            }
        }



        public async Task<TopicAreaDTO> GetTopicAreaById(int id)
        {
            try
            {
                // Retrieve the TopicArea by its ID
                var topicArea = await context.TopicAreas.FindAsync(id);

                if (topicArea == null)
                {
                    throw new Exception($"No Topic Area found with ID {id}.");
                }

                // Map the entity to the DTO
                return new TopicAreaDTO
                {
                    Id = topicArea.Id,
                    CertificateId = topicArea.CertificateId, // Assuming there's a relationship with Certificate
                    TopicName = topicArea.TopicName,
                    Status = topicArea.Status
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the Topic Area: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteTopicArea(int id)
        {
            try
            {
                // Retrieve the TopicArea by its ID
                var topicArea = await context.TopicAreas
                    .Include(ta => ta.CourseSessions) // Include related CourseSessions
                    .Include(ta => ta.CertificateQuestions) // Include related CertificateQuestions
                    .FirstOrDefaultAsync(ta => ta.Id == id);

                if (topicArea == null)
                {
                    throw new Exception($"No Topic Area found with ID {id}.");
                }

                // Set the status of related CourseSessions to false
                foreach (var courseSession in topicArea.CourseSessions)
                {
                    courseSession.Status = false;
                }

                // Set the status of related CertificateQuestions to false
                foreach (var certQuestion in topicArea.CertificateQuestions)
                {
                    certQuestion.Status = false;
                }

                // Save the changes related to CourseSessions and CertificateQuestions
                await context.SaveChangesAsync();

                // Remove the TopicArea
                context.TopicAreas.Remove(topicArea);
                await context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("There was an error deleting the Topic Area from the database.", dbEx);
            }
            catch (Exception ex)
            {
                var errorMessage = $"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
                throw new Exception($"An error occurred while deleting the Topic Area: {errorMessage}");
            }
        }

        public async Task<TopicAreaDTO> UpdateTopicArea(int id, TopicAreaCreateDTO request)
        {
            try
            {
                // Validate input
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "TopicAreaDTO cannot be null.");
                }

                // Retrieve the existing TopicArea by its ID
                var topicArea = await context.TopicAreas.FindAsync(id);
                if (topicArea == null)
                {
                    throw new Exception($"No Topic Area found with ID {id}.");
                }

                // Check if a duplicate TopicName exists within the same Certificate
                var duplicateTopicArea = await context.TopicAreas
                    .FirstOrDefaultAsync(ta => ta.CertificateId == request.CertificateId &&
                                               ta.TopicName.ToLower() == request.TopicName.ToLower() &&
                                               ta.Id != id);

                if (duplicateTopicArea != null)
                {
                    throw new Exception($"A Topic Area with the name '{request.TopicName}' already exists for this Certificate.");
                }

                // Update the TopicArea's properties
                topicArea.TopicName = request.TopicName;
                topicArea.Status = request.Status;

                // Save the changes to the database
                await context.SaveChangesAsync();

                // Return the updated Topic Area as DTO
                return new TopicAreaDTO
                {Id = topicArea.Id,
                    CertificateId = topicArea.CertificateId,
                    TopicName = topicArea.TopicName,
                    Status = topicArea.Status
                };
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("There was an error updating the Topic Area in the database.", dbEx);
            }
            catch (Exception ex)
            {
                var errorMessage = $"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
                throw new Exception($"An error occurred while updating the Topic Area: {errorMessage}");
            }
        }

    }
}
