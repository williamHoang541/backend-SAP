using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.TopicAreaDTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ITopicArea
    {
        Task<List<TopicAreaDTO>> GetAllTopicAreasAsync(GetAllDTO getAllDTO);
        Task<TopicAreaDTO> GetTopicAreaById(int id);
        Task<TopicAreaDTO> CreateTopicArea(TopicAreaCreateDTO request);
        Task<TopicAreaDTO> UpdateTopicArea(int id, TopicAreaCreateDTO request);
        Task<bool> DeleteTopicArea(int id);
    }
}
