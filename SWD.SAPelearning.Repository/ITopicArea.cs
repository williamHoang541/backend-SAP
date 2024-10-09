using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ITopicArea
    {
        Task<List<TopicArea>> GetAllTopicArea();
        Task<TopicAreaDTO> GetTopicAreaById(int id);
        Task<TopicAreaDTO> CreateTopicArea(TopicAreaDTO request);
        Task<TopicAreaDTO> UpdateTopicArea(int id, TopicAreaDTO request);
        Task<bool> DeleteTopicArea(int id);
    }
}
