using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ITopicArea
    {
        Task<List<TopicArea>> GetAllTopicArea();
    }
}
